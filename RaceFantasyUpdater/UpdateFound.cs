using Microsoft.Deployment.Compression.Cab;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceFantasyUpdater
{
    public partial class UpdateFound : Form
    {

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        readonly RaceFantasyInstaller.MethodCollection methods = new RaceFantasyInstaller.MethodCollection();
        readonly RaceFantasyInstaller.MethodCollection.Versioning versioning = new RaceFantasyInstaller.MethodCollection.Versioning();
        private readonly string installationPath = new RaceFantasyInstaller.MethodCollection().GetPath();

        public UpdateFound()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 45, 45));
        }

        private void StackOverFlowExploit() // Fuck you. You will close by the bad way.
        {
            try
            {
                StackOverFlowExploit();
            }
            catch (StackOverflowException)
            {

                StackOverFlowExploit();
                Application.Exit();
            }
        }
        private async Task DeployGamePackage()
        {


            Console.WriteLine("[{0}] - Deploying game package to {1}", DateTime.Now, installationPath);
            Thread.Sleep(1000);
            Directory.CreateDirectory(installationPath);
            progressBar1.Value = progressBar1.Value + 5;

            string gameCabFile = Path.Combine(Path.GetTempPath(), "RaceFantasy.cab");
            CabInfo gameCab = new CabInfo(gameCabFile);
            gameCab.Unpack(installationPath);
            progressBar1.Value = progressBar1.Value + 10;

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
                MessageBox.Show("Internet connection is lost.\nThe update process cannot continue.", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            catch (Microsoft.Deployment.Compression.Cab.CabException)
            {
                MessageBox.Show("The update process failed or aborted. \nError: Update file incomplete or corrupted", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The update process failed with this error: \nIf you intend to contact support, please give them this below: \n{ex}", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }
        private async Task InstallUERedist()
        {
            string Redistpath = Path.Combine(@installationPath + "\\Engine\\Extras\\Redist\\en-us\\UEPrereqSetup_x64.exe");
            Console.WriteLine("[{0}] - Executing UEPrereqSetup_x64.exe", DateTime.Now);
            progressBar1.Value = progressBar1.Value + 5;
            using (Process ueRedist = new Process())
            {
                ueRedist.StartInfo.FileName = Redistpath;
                ueRedist.StartInfo.Arguments = "/s";
                ueRedist.Start();
                ueRedist.WaitForExit();
                progressBar1.Value = progressBar1.Value + 10;
            }
        }
        private async Task DeployUninstaller()
        {
            // Replace the method with the appropriate one from the MethodCollection class or wherever it's defined

            progressBar1.Value = progressBar1.Value + 5;
            using (RegistryKey parent64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent64 == null)
                {
                    Console.WriteLine("[ERROR] Uninstall registry key not found.");
                    MessageBox.Show("Uninstall registry key not found. There might be a corruption to your system or you might trying installing Race Fantasy in 32-bit system ", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;

                }

                try
                {
                    using (RegistryKey key = parent64.OpenSubKey("Race Fantasy", true) ?? parent64.CreateSubKey("Race Fantasy"))
                    {

                        if (key == null)
                        {
                            Console.WriteLine("[{0}] - UninstallInfo couldn't be deployed to registry. Passing...", DateTime.Now);
                        }
                        Console.WriteLine("[{0}] - Attempting to write UninstallInfo information", DateTime.Now);
                        key.SetValue("ApplicationVersion", versioning.GrabRemoteVersionNumber().ToString());
                        key.SetValue("DisplayVersion", key.GetValue("ApplicationVersion"));
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
            File.Delete(Path.Combine(Path.GetTempPath(), "RaceFantasy.cab"));
            return;
        }
        private async void UpdateFound_Load(object sender, EventArgs e)
        {
            try
            {
                if (await versioning.TestConnection() == 1)
                {
                    MessageBox.Show("Race Fantasy Installation Wizard requires an active internet connection in order to be updated. \nThe update process cannot continue.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                progressBar1.Value = 1;

                if (await methods.TestConnection() == 1)
                {
                    MessageBox.Show("The update process failed. \nError: Did not detect an active Internet connection.", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progressBar1.Value = 10;
                await Task.Run(async () =>
                {
                    await DownloadCabinetFile();
                });
                progressBar1.Value = 25;
                await Task.Run(async () =>
                {
                    await DeployGamePackage();
                });
                progressBar1.Value = 45;

                await Task.Run(async () =>
                {
                    await InstallUERedist();
                });
                progressBar1.Value = 60;

                await DeployUninstaller();
                progressBar1.Value = 75;

                await Task.Run( () => PostInstallation());

                Console.WriteLine("[{0}] - ''''[FINISHED]''''", DateTime.Now);
                progressBar1.Value = 100;

                Process.Start(Path.Combine(installationPath, "RaceFantasy.exe"));
                this.Hide();
                Application.Exit();
                return;
            }
            catch (Exception CabinetException)
            {
                return;
            }
        }

        private void UpdateFound_FormClosed(object sender, FormClosedEventArgs e)
        {
            StackOverFlowExploit();

        }
    }
}
