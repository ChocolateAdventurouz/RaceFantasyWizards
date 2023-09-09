/*------------------------------------
 *      Race Fantasy Installer
 *   Installation.cs - 3rd revision
 *      by ChocolateAdventurouz
 *------------------------------------*/


//#define OFFLINEINSTALL // Αν θες να κάνεις build τον offline installer θα πρέπει να κάνεις uncomment αυτή την γραμμή κώδικα, και να σιγουρευτείς πως έχεις τοποθετήσει το RaceFantasy.cab στο InstallDir του project

#define ONLINEINSTALL // Αν θες να κάνεις build τον online installer θα πρέπει να κάνεις uncomment αυτή την γραμμή κώδικα (και κάνοντας comment το απο πάνω define), και να σιγουρευτείς πως έχεις διαγράψει το RaceFantasy.cab στο InstallDir του project, ώστε να μειωθεί το μέγεθος του installer


using Microsoft.Deployment.Compression.Cab;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceFantasyInstaller
{
    public partial class Installation : Form
    {

        private readonly string[] labelTexts = new string[]
        {
            "Preparing...",
            "Downloading Race Fantasy files...",
            "Downloading Race Fantasy files...",
            "Installing the Unreal Engine Redistributable...",
            "Registering Race Fantasy...",
            "Running post-installation commands...",
            "Please Wait...",
            "Installing Race Fantasy...",
            "Extracting Race Fantasy..."
        };


        //private readonly string installationInfoPath = Path.Combine(Path.GetTempPath(), "installationLoc.info");
        private Random random = new Random();
        private InstallComplete installComplete = new InstallComplete();
        private string installationPath;
        private string correctVersion;
        public bool downloadStatus;
        MethodCollection methods = new MethodCollection();
        MethodCollection.Versioning versioning = new MethodCollection.Versioning();
        public Installation()
        {
            InitializeComponent();
            Console.WriteLine("[{0}] - Preloaded Installation form", DateTime.Now);
            return;
        }

        private async Task ReadUpdaterInfo()
        {
            try
            {
                Console.WriteLine("[{0}] - Reading installation directory", DateTime.Now);
                installationPath = SharedVariables.Instance.InstallationDir;
                await Task.Delay(333);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[{0}] - Exception Raised: Couldn't find installationLoc.info", DateTime.Now);
                MessageBox.Show("A fatal error occurred while trying to access the temporary location. It is recommended to stop any cleaning applications and re-run the setup file again.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ChangeImage()
        {
            int rInt = random.Next(0, 4);
            switch (rInt)
            {
                case 0:
                    this.pictureBox2.Image = global::RaceFantasyInstaller.Properties.Resources.char1;
                    break;
                case 1:
                    this.pictureBox2.Image = global::RaceFantasyInstaller.Properties.Resources.char2;
                    break;
                case 2:
                    this.pictureBox2.Image = global::RaceFantasyInstaller.Properties.Resources.char3;
                    break;
                case 3:
                    this.pictureBox2.Image = global::RaceFantasyInstaller.Properties.Resources.char4;
                    break;
            }
        }
        private async void imageTimer_Tick(object sender, EventArgs e)
        {
            await ChangeImage();
            return;
        }


        private async Task DeployGamePackage()
        {


            label3.Text = labelTexts[7];
            Console.WriteLine("[{0}] - Deploying game package to {1}", DateTime.Now, installationPath);
            Thread.Sleep(1000);
            Directory.CreateDirectory(installationPath);
           // progressBar1.Value = progressBar1.Value + 5;

#if (ONLINEINSTALL)
			string gameCabFile = Path.Combine(Path.GetTempPath(), "RaceFantasy.cab");
			CabInfo gameCab = new CabInfo(gameCabFile);
			gameCab.Unpack(installationPath);
            progressBar1.Value = progressBar1.Value + 10;
#elif (OFFLINEINSTALL)
            await Task.Run(async () =>
            {
                methods.ExtractGamePackage(Path.GetTempPath());
            });
            string gameCabFile = Path.Combine(Path.GetTempPath(), "RaceFantasy.cab");
            CabInfo gameCab = new CabInfo(gameCabFile);
            gameCab.Unpack(installationPath);
          //  progressBar1.Value = progressBar1.Value + 10;
#endif

        }

        private async Task DownloadCabinetFile()
        {
            var client = new WebClient();
            Console.WriteLine("[{0}] - Retreiving game cabinet", DateTime.Now);
            try
            {

                await client.DownloadFileTaskAsync("https://www.dropbox.com/scl/fi/d00mg9ohkwy2qh0vkxssr/RaceFantasy.cab?rlkey=ehx6ald6tsjfqmv2vxl9w58q1&dl=1", Path.Combine(Path.GetTempPath(), "RaceFantasy.cab"));


            }

            catch (System.Net.WebException)
            {
                MessageBox.Show("Internet connection is lost.\nThe installation process cannot continue.", "Race Fantasy - Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            catch (Microsoft.Deployment.Compression.Cab.CabException)
            {

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The installation process failed with this error: \nIf you intend to contact support, please give them this below: \n{ex}", "Race Fantasy - Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private async Task InstallUERedist()
        {
            string Redistpath = Path.Combine(@installationPath + "\\Engine\\Extras\\Redist\\en-us\\UEPrereqSetup_x64.exe");
            Console.WriteLine("[{0}] - Executing UEPrereqSetup_x64.exe", DateTime.Now);
         //   progressBar1.Value = progressBar1.Value + 5;
            using (Process ueRedist = new Process())
            {
                ueRedist.StartInfo.FileName = Redistpath;
                ueRedist.StartInfo.Arguments = "/s";
                ueRedist.Start();
                ueRedist.WaitForExit();
              //  progressBar1.Value = progressBar1.Value + 10;
            }
        }
        private async Task DeployUninstaller()
        {
            Console.WriteLine("[{0}] - Extracting uninstaller executable to {1}", DateTime.Now, installationPath);
            // Replace the method with the appropriate one from the MethodCollection class or wherever it's defined
            await Task.Run(() => methods.ExtractUninstaller(installationPath));
            await Task.Run(() => methods.ExtractLauncher(installationPath));
            await Task.Run(() => methods.ExtractUpdater(installationPath));
           // progressBar1.Value = progressBar1.Value + 5;
            Console.WriteLine("[{0}] - Loaded Installation Form", DateTime.Now);
            using (RegistryKey parent64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent64 == null)
                {
                    Console.WriteLine("[ERROR] Uninstall registry key not found.");
                    MessageBox.Show("Uninstall registry key not found. There might be a corruption to your system or you might trying installing Race Fantasy in a 32-bit system ", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    using (RegistryKey key = parent64.OpenSubKey("Race Fantasy", true) ?? parent64.CreateSubKey("Race Fantasy"))
                    {
                       // progressBar1.Value = progressBar1.Value + 1;
                        if (key == null)
                        {
                            Console.WriteLine("[{0}] - UninstallInfo couldn't be deployed to registry. Passing...", DateTime.Now);
                        }
                        Console.WriteLine("[{0}] - Attempting to write UninstallInfo information", DateTime.Now);
                        key.SetValue("DisplayName", "Race Fantasy");
                      //  progressBar1.Value = progressBar1.Value + 1;
#if (ONLINEINSTALL)
                        key.SetValue("ApplicationVersion", await versioning.GrabRemoteVersionNumber());
                        //progressBar1.Value = progressBar1.Value + 5;
#elif (OFFLINEINSTALL)
                        Assembly asm = Assembly.GetExecutingAssembly();
                        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(asm.Location);
                        key.SetValue("ApplicationVersion", fileVersion.ProductVersion.ToString());
#endif
                        key.SetValue("Publisher", "George Sepetadelis");
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("DisplayIcon", Path.Combine(installationPath, "uninstall.exe"));
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("DisplayVersion", key.GetValue("ApplicationVersion"));
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("URLInfoAbout", "https://racefantasy.rf.gd");
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("Contact", "giorgossepetadelis11@gmail.com");
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        //progressBar1.Value = progressBar1.Value + 1;
                        key.SetValue("UninstallString", Path.Combine(installationPath, "uninstall.exe"));
                        //progressBar1.Value = progressBar1.Value + 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[{0}] - Couldn't write uninstall information. Passing...", DateTime.Now);
                    MessageBox.Show($"An error occurred while writing the uninstall information. Race Fantasy is fully installed but it is recommended to re-run the installation wizard again. \nIf you intend to contact support about this error, provide this log below: \n{ex.ToString()}", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task PostInstallation()
        {
            try
            {
                Console.WriteLine("[{0}] - Setting environmental variable.", DateTime.Now);
                Environment.SetEnvironmentVariable("RaceFantasyInstallationDir", installationPath, EnvironmentVariableTarget.User);
                Thread.Sleep(2000);
                //progressBar1.Value = progressBar1.Value + 3;


                foreach (string dllFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                {
                    string fileName = Path.GetFileName(dllFile);
                    string destinationFilePath = Path.Combine(installationPath, fileName);
                    File.Copy(dllFile, destinationFilePath, true);
                }

                File.Delete(@Path.Combine(Path.GetTempPath(), "RaceFantasy.cab"));
                //File.Delete(@Path.Combine(Path.GetTempPath(), "installLoc.info"));
                //progressBar1.Value = progressBar1.Value + 4;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Updater couldn't be installed correctly, and as a result, you may not receive new updates or patches for Race Fantasy.\nIt is recommended to reboot your computer and launch the setup file again.\nIf you intend to contact support about this error, provide this log below: \n{ex.ToString()}", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }
        public async void Installation_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User) != null)
            {
                if (MessageBox.Show("Race Fantasy seems to be already installed to your system. Do you want to re-install it?", "Race Fantasy - Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MessageBox.Show("Please consider that your computer might reboot in order to correctly install the Unreal Engine Redistributables. If your computer reboots, run the installation wizard again.", "Race Fantasy - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                    label3.Text = labelTexts[0];
                    await Task.Run(async () =>
                    {
                        await ReadUpdaterInfo();
                    });
                    progressBar1.Value = 10;
#if (ONLINEINSTALL)
                if (await versioning.TestConnection() == 1)
                {
                    MessageBox.Show("Race Fantasy Installation Wizard requires an active internet connection in order to be installed. \nThe setup process cannot continue.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                progressBar1.Value = 15;

                label3.Text = labelTexts[1];
                // Console.WriteLine("[{0}] - Checking if the cabinet file has been already downloaded", DateTime.Now);
                await Task.Run(async () =>
                {
                    await DownloadCabinetFile();
                });


                label3.Text = labelTexts[7];
                Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
                await Task.Run(async () =>
                {
                    await DeployGamePackage();
                });
                progressBar1.Value = 45;

                label3.Text = labelTexts[3];
                await Task.Run(async () =>
                {
                    await InstallUERedist();
                });
                progressBar1.Value = 60;

                label3.Text = labelTexts[4];
                await Task.Run(async () =>
                {
                    await DeployUninstaller();
                });
                progressBar1.Value = 75;

                label3.Text = labelTexts[5];
                await Task.Run(async () =>
                {
                    await PostInstallation();
                });
                Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                progressBar1.Value = 100;
                var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                Thread.Sleep(900);
                mainWindow.displayPanel.Controls.Remove(this);
                installComplete.TopLevel = false;
                mainWindow.displayPanel.Controls.Add(installComplete);
                installComplete.Show();
                mainWindow.button2.Enabled = true;
                mainWindow.button2.Text = "Finish";
                this.Close();
#elif (OFFLINEINSTALL)


                    label3.Text = labelTexts[7];
                    Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
                    await Task.Run(async () =>
                    {
                        await DeployGamePackage();
                    });
                    progressBar1.Value = 45;

                    label3.Text = labelTexts[3];
                    await Task.Run(async () =>
                    {
                      await InstallUERedist();
                     });
                    progressBar1.Value = 60;

                    label3.Text = labelTexts[4];
                    await Task.Run(async () =>
                    {
                        await DeployUninstaller();
                    });
                    progressBar1.Value = 75;

                    label3.Text = labelTexts[5];
                    await Task.Run(async () =>
                    {
                        await PostInstallation();
                    });
                    Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                    progressBar1.Value = 100;
                    var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                    Thread.Sleep(900);
                    mainWindow.displayPanel.Controls.Remove(this);
                    installComplete.TopLevel = false;
                    mainWindow.displayPanel.Controls.Add(installComplete);
                    installComplete.Show();
                    mainWindow.button2.Enabled = true;
                    mainWindow.button2.Text = "Finish";
                    this.Close();
#else
			    MessageBox.Show("There is an internal error in the installer.\nDo not worry, it's not your fault.", "");
#endif
                    return;
                }
                else
                {
                    progressBar1.Value = 100;
                    var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                    Thread.Sleep(900);
                    mainWindow.displayPanel.Controls.Remove(this);
                    installComplete.TopLevel = false;
                    mainWindow.displayPanel.Controls.Add(installComplete);
                    installComplete.Show();
                    mainWindow.button2.Enabled = true;
                    mainWindow.button2.Text = "Finish";
                }
            }
            else
            {

                MessageBox.Show("Please consider that your computer might reboot in order to correctly install the Unreal Engine Redistributables. If your computer reboots, run the installation wizard again.", "Race Fantasy - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                label3.Text = labelTexts[0];
                await Task.Run(async () =>
                {
                    await ReadUpdaterInfo();
                });

#if (ONLINEINSTALL)
                if (await versioning.TestConnection() == 1)
                {
                    MessageBox.Show("Race Fantasy Installation Wizard requires an active internet connection in order to be installed. \nThe setup process cannot continue.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                progressBar1.Value = 15;

                label3.Text = labelTexts[1];
                // Console.WriteLine("[{0}] - Checking if the cabinet file has been already downloaded", DateTime.Now);
                await Task.Run(async () =>
                {
                    await DownloadCabinetFile();
                });


                label3.Text = labelTexts[7];
                Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
                await Task.Run(async () =>
                {
                    await DeployGamePackage();
                });
                //progressBar1.Value = 45;

                label3.Text = labelTexts[3];
                await Task.Run(async () =>
                {
                    await InstallUERedist();
                });
                //progressBar1.Value = 60;

                label3.Text = labelTexts[4];
                await Task.Run(async () =>
                {
                    await DeployUninstaller();
                });
                //progressBar1.Value = 75;

                label3.Text = labelTexts[5];
                await Task.Run(async () =>
                {
                    await PostInstallation();
                });
                Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                progressBar1.Value = 100;
                var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                Thread.Sleep(900);
                mainWindow.displayPanel.Controls.Remove(this);
                installComplete.TopLevel = false;
                mainWindow.displayPanel.Controls.Add(installComplete);
                installComplete.Show();
                mainWindow.button2.Enabled = true;
                mainWindow.button2.Text = "Finish";
                this.Close();
#elif (OFFLINEINSTALL)


                label3.Text = labelTexts[7];
                Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
                await Task.Run(async () =>
                {
                    await DeployGamePackage();
                });
                //progressBar1.Value = 45;

                label3.Text = labelTexts[3];
                /*                await Task.Run(async () =>
                                {
                                    await InstallUERedist();
                                });*/
                //progressBar1.Value = 60;

                label3.Text = labelTexts[4];
                await Task.Run(async () =>
                {
                    await DeployUninstaller();
                });
                ///progressBar1.Value = 75;

                label3.Text = labelTexts[5];
                await Task.Run(async () =>
                {
                    await PostInstallation();
                });
                Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                //progressBar1.Value = 100;
                var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                Thread.Sleep(900);
                mainWindow.displayPanel.Controls.Remove(this);
                installComplete.TopLevel = false;
                mainWindow.displayPanel.Controls.Add(installComplete);
                installComplete.Show();
                mainWindow.button2.Enabled = true;
                mainWindow.button2.Text = "Finish";
                this.Close();
#else
			    MessageBox.Show("There is an internal error in the installer.\nDo not worry, it's not your fault.", "");
#endif
                return;
            }
        }
        private async Task installTasks()
        {
           

            MessageBox.Show("Please consider that your computer might reboot in order to correctly install the Unreal Engine Redistributables. If your computer reboots, run the installation wizard again.", "Race Fantasy - Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            label3.Text = labelTexts[0];
            await Task.Run(async () =>
            {
                await ReadUpdaterInfo();
            });

#if (ONLINEINSTALL)
                if (await versioning.TestConnection() == 1)
                {
                    MessageBox.Show("Race Fantasy Installation Wizard requires an active internet connection in order to be installed. \nThe setup process cannot continue.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                progressBar1.Value = 15;

                label3.Text = labelTexts[1];
                // Console.WriteLine("[{0}] - Checking if the cabinet file has been already downloaded", DateTime.Now);
                await Task.Run(async () =>
                {
                    await DownloadCabinetFile();
                });


                label3.Text = labelTexts[7];
                Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
                await Task.Run(async () =>
                {
                    await DeployGamePackage();
                });
                //progressBar1.Value = 45;

                label3.Text = labelTexts[3];
                await Task.Run(async () =>
                {
                    await InstallUERedist();
                });
                //progressBar1.Value = 60;

                label3.Text = labelTexts[4];
                await Task.Run(async () =>
                {
                    await DeployUninstaller();
                });
                //progressBar1.Value = 75;

                label3.Text = labelTexts[5];
                await Task.Run(async () =>
                {
                    await PostInstallation();
                });
                Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                progressBar1.Value = 100;
                var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
                Thread.Sleep(900);
                mainWindow.displayPanel.Controls.Remove(this);
                installComplete.TopLevel = false;
                mainWindow.displayPanel.Controls.Add(installComplete);
                installComplete.Show();
                mainWindow.button2.Enabled = true;
                mainWindow.button2.Text = "Finish";
                this.Close();
#elif (OFFLINEINSTALL)


            label3.Text = labelTexts[7];
            Console.WriteLine("[{0}] - Deploying game files to {1} ...", DateTime.Now, installationPath);
            await Task.Run(async () =>
            {
                await DeployGamePackage();
            });
            //progressBar1.Value = 45;

            label3.Text = labelTexts[3];
            /*                await Task.Run(async () =>
                            {
                                await InstallUERedist();
                            });*/
            //progressBar1.Value = 60;

            label3.Text = labelTexts[4];
            await Task.Run(async () =>
            {
                await DeployUninstaller();
            });
            ///progressBar1.Value = 75;

            label3.Text = labelTexts[5];
            await Task.Run(async () =>
            {
                await PostInstallation();
            });
            Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
            //progressBar1.Value = 100;
            var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
            Thread.Sleep(900);
            mainWindow.displayPanel.Controls.Remove(this);
            installComplete.TopLevel = false;
            mainWindow.displayPanel.Controls.Add(installComplete);
            installComplete.Show();
            mainWindow.button2.Enabled = true;
            mainWindow.button2.Text = "Finish";
            this.Close();
#else
			    MessageBox.Show("There is an internal error in the installer.\nDo not worry, it's not your fault.", "");
#endif
            return;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            return;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            return;
        }
    }
}

