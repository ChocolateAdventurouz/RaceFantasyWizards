using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

// This source code is based in Linuxide's Main Window handler
namespace RaceFantasyInstaller
{
    public partial class MainWindow : Form
    {
        public string path;
        public static int NextButtonClicked { get; set; }
        public int currentPageNum;
        Welcome welcome = new Welcome();
        installDestForm installDestForm = new installDestForm();
        Installation installation = new Installation();
        SharedVariables sharedVariables = new SharedVariables();

        public MainWindow()
        {
            InitializeComponent();
        }

        private int ExitHandler()
        {
            if (MessageBox.Show("Are you sure that you want to cancel the installation process?", "Race Fantasy - Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                displayPanel.Controls.Clear();
                if (File.Exists(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes"))) ;
                { File.Delete(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes")); }
                Application.Exit();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            welcome.TopLevel = false;
            displayPanel.Controls.Add(welcome);
            welcome.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExitHandler();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentPageNum > 0)
            {
                currentPageNum--;
                button3.Enabled = true;
            }

            if (currentPageNum < 0)
            {
                currentPageNum += 1;
            }

            if (currentPageNum <= 0)
            {

                button3.Enabled = false;
            }

            if (currentPageNum == 0)
            {
                displayPanel.Controls.Remove(installDestForm);
                welcome.TopLevel = false;
                displayPanel.Controls.Add(welcome);
                welcome.Show();
                return;
            }
        }

        public void button2_Click(object sender, EventArgs e)
        {
            currentPageNum++;
            NextButtonClicked = 1;
            int totalPageNum = 4 % currentPageNum;
            if (currentPageNum > 0)
            { //if the current page is greater than 0, enable the back button
                button3.Enabled = true;
            }

            if (totalPageNum == currentPageNum)
            {
                currentPageNum = 0;
            }

            if (currentPageNum == 1)
            {
                displayPanel.Controls.Remove(welcome);
                installDestForm.TopLevel = false;
                displayPanel.Controls.Add(installDestForm);
                installDestForm.Show();
                return;
            }
            if (currentPageNum == 2)
            {
                button3.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                displayPanel.Controls.Remove(installDestForm);
                installation.TopLevel = false;
                displayPanel.Controls.Add(installation);
                installation.Show();
                return;
            }
            if (currentPageNum == 3)
            {
                Application.Exit();
                return;
            }
            NextButtonClicked = 0;
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            return;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    if (ExitHandler() == 0)
                    {
                        e.Cancel = true;
                        displayPanel.Controls.Clear();
                        e.Cancel = false;
                        if (File.Exists(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes"))) ; { File.Delete(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes")); }
                        Application.Exit();
                        return;

                    }
                    else
                    {
                        e.Cancel = true;
                        break;
                    }
            }
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SharedVariables.Instance.LaunchRaceFantasy == true)
            {
                Process.Start(SharedVariables.Instance.InstallationDir + "\\PlayRaceFantasy.exe");
                return;
            }
            /*(if (File.Exists(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes"))) ;
            { File.Delete(Path.Combine(Path.GetTempPath(), ".sfx.is.complete=yes")); }*/
            Process.GetCurrentProcess().Kill();
            return;
        }

        private void displayPanel_Paint_1(object sender, PaintEventArgs e)
        {
            return;
        }
    }
}
