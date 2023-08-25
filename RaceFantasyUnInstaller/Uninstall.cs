using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceFantasyUnInstaller
{
    public partial class Uninstall : Form
    {
        Random r = new Random();
        UninstallComplete uninstallComplete = new UninstallComplete();
        public Uninstall()
        {
            InitializeComponent();
        }

        private async Task removeUserData()
        {
            try
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) == true)
                {

                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\RaceFantasy", true);
                    return;
                }
                else
                {
                    return;
                }
            }

            catch (Exception ex)
            {
                return;
            }
        }
        private async Task removeKey()
        {
            RegistryKey key = null;
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                Assembly asm = GetType().Assembly;
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {

                    string guidText = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpper();
                    parent.DeleteSubKey(guidText, false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();

                    }
                }
            }

            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
             @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {

                    parent.DeleteSubKey("Race Fantasy", false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();

                    }
                }
            }
            return;
        }
     
        private async Task RemoveShortcuts()
        {
            try
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + Path.DirectorySeparatorChar + "Race Fantasy" + ".lnk");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "Race Fantasy" + ".lnk");
            }
            catch {
                return;
            }
        }

        static async Task DeleteDirectoryRecursively(string targetDirectory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User));
            string[] filesToKeep = { "updater.exe", "uninstall.exe" };

                // Get all files in the directory
                FileInfo[] allFiles = directoryInfo.GetFiles();

                // Filter out files to be deleted
                FileInfo[] filesToDelete = allFiles.Where(file => !filesToKeep.Contains(file.Name)).ToArray();

                foreach (FileInfo file in filesToDelete)
                {
                    // Delete the file
                    file.Delete();
                }

            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
            {
                     subDirectory.Delete(true); // Recursive delete for subdirectories and files
            }
            return;

        }

      /*  private async Task RemoveStuff()
        {
            try
            {

                if (Directory.Exists(@Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User)) == true)
                {
                    foreach (var file in Directory.GetFiles(targetDirectory))
                    {
                        
                        File.Delete(file);
                        Console.WriteLine($"Deleted file: {file}");
                    }

                    foreach (var directory in Directory.GetDirectories(targetDirectory))
                    {
                        DeleteDirectoryRecursively(directory);
                    }

                }
                else
                {
                    return;
                }
            }
            catch (FileNotFoundException)
            {

                MessageBox.Show("Couldn't find Race Fantasy files.\nYou may try removing the files manually", "Race Fantasy - Error");
                return;
            }
        }*/
        private async void Uninstall_LoadAsync(object sender, System.EventArgs e)
        {
            
            var mainwindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
            mainwindow.button2.Enabled = false;
            Thread.Sleep(333);
            if (MessageBox.Show("Do you want to remove user data along with the game?", "Race Fantasy", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                await removeUserData();
                progressBar1.Value = 38;
            }
            else
            {
                progressBar1.Value = 38;
            }
            
            await DeleteDirectoryRecursively(@Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User));
            progressBar1.Value = 78;

            await RemoveShortcuts();
            progressBar1.Value = 87;
            await removeKey();
            progressBar1.Value = 95;
            Environment.SetEnvironmentVariable("RaceFantasyInstallationDir", null, EnvironmentVariableTarget.User);
            progressBar1.Value = 100;

            mainwindow.button2.Text = "Finish";
            mainwindow.button2.Enabled = true;
            mainwindow.displayPanel.Controls.Remove(this);
            uninstallComplete.TopLevel = false;
            mainwindow.displayPanel.Controls.Add(uninstallComplete);
            uninstallComplete.Show();
            return;
        }

        private async Task ChangeImage()
        {
            int rInt = r.Next(0, 3);

            switch (rInt)
            {
                case 0:
                    this.pictureBox2.Image = global::RaceFantasyUnInstaller.Properties.Resources.char1;
                    break;
                case 1:
                    this.pictureBox2.Image = global::RaceFantasyUnInstaller.Properties.Resources.char2;
                    break;
                case 2:
                    this.pictureBox2.Image = global::RaceFantasyUnInstaller.Properties.Resources.char3;
                    break;
                case 3:
                    this.pictureBox2.Image = global::RaceFantasyUnInstaller.Properties.Resources.char4;
                    break;
            }
        }
        private async void imageTimer_Tick(object sender, EventArgs e)
        {
            await ChangeImage();

        }
    }
}
