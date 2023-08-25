
using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace RaceFantasyInstaller
{
    public partial class InstallComplete : Form
    {

        //SharedVariables sharedVariables = new SharedVariables();
        public InstallComplete()
        {
            InitializeComponent();
        }


        private void InstallComplete_Load(object sender, EventArgs e)
        {
            //var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());

            return;
        }

        private static void CreateShortcutDesktop()
        {
            string link = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                + Path.DirectorySeparatorChar + "Race Fantasy" + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User) + "\\PlayRaceFantasy.exe";
            shortcut.IconLocation = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User) + "\\RaceFantasy.exe";
            shortcut.WorkingDirectory = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User);
            Thread.Sleep(333);
            shortcut.Save();
        }
        private static void CreateShortcutStartMenu()
        {
            string link = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)
                + Path.DirectorySeparatorChar + "Race Fantasy" + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User) + "\\PlayRaceFantasy.exe";
            shortcut.IconLocation = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User) + "\\RaceFantasy.exe";
            shortcut.WorkingDirectory = @Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User);
            Thread.Sleep(333);
            shortcut.Save();
        }

        private void RemoveShortcutDesktop()
        {

            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Race Fantasy.lnk");
            return;
        }

        private void RemoveShortcutStartMenu()
        {
            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\Race Fantasy.lnk");
            return;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                CreateShortcutDesktop();
                return;
            }
            else
            {
                RemoveShortcutDesktop();
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                CreateShortcutStartMenu();
                return;
            }
            else
            {
                RemoveShortcutStartMenu();
                return;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                SharedVariables.Instance.LaunchRaceFantasy = true;
                return;
            }
            else
            {
                SharedVariables.Instance.LaunchRaceFantasy = false;
                return;
            }
        }
    }
}
