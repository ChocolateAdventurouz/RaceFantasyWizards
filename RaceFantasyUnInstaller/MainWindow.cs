using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RaceFantasyUnInstaller
{
    public partial class MainWindow : Form
    {
        public int currentPageNum;

        Confirmation confirmation = new Confirmation();
        Uninstall uninstall = new Uninstall();
        RaceFantasyInstaller.MethodCollection methodCollection = new RaceFantasyInstaller.MethodCollection();
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public MainWindow()
        {
            InitializeComponent();
            titlebar.MouseDown += PanelTitleBar_MouseDown;
        }
        private void PanelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        static void SelfDelete()
        {
            string executablePath = Process.GetCurrentProcess().MainModule.FileName;
            string updaterexecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater.exe");
            try
            {
                // Make sure the process of the application is closed before deletion
                Process[] processes1 = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(executablePath));
                Process[] processes2 = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(updaterexecutablePath));
                foreach (Process process in processes1)
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                foreach (Process process in processes2)
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }

                // Move the executable to a temporary location
                string tempExePath = Path.Combine(Path.GetTempPath(), "temp.exe");
                File.Move(executablePath, tempExePath);

                // Start a new process to delete the original executable
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C ping localhost -n 1 -w 3000 > Nul & Del \"{tempExePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                File.Move(updaterexecutablePath, tempExePath);
                // Start a new process to delete the original executable
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C ping localhost -n 1 -w 3000 > Nul & Del \"{tempExePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                // Exit the application after initiating the self-deletion
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                // If self-deletion fails, handle the exception (e.g., log it) and exit gracefully.
                Console.WriteLine($"Error occurred during finalizing: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            confirmation.TopLevel = false;
            displayPanel.Controls.Add(confirmation);
            confirmation.Show();
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            currentPageNum++;

            int totalPageNum = 2 % currentPageNum;

            if (totalPageNum == currentPageNum)
            {
                this.Close();
            }

            if (currentPageNum == 1)
            {
                displayPanel.Controls.Remove(confirmation);
                uninstall.TopLevel = false;
                displayPanel.Controls.Add(uninstall);
                uninstall.Show();
                button1.Enabled = false;
                // button2.Enabled = false;
                return;
            }
            if (currentPageNum == 2)
            {
                Application.Exit();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            return;

        }
        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (methodCollection.GetPath() == null)
            {
                SelfDelete();
            }
            return;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (methodCollection.GetPath() == null)
            {
                SelfDelete();
            }
            
            return;
        }

    }
}
