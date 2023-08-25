using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayRaceFantasy
{

    public partial class Banner : Form
    {
        Actions actions = new Actions();
        // Setting rules for picturebox'es
        public bool pbx1_canChangeImage = true;
        public bool pbx2_canChangeImage = true;
        public bool pbx3_canChangeImage = true;
        public bool pbx4_canChangeImage = true;

        public Banner()
        {
            InitializeComponent();
        }

        private void Banner_Load(object sender, EventArgs e){ return;}
        private void pictureBox1_MouseHover(object sender, EventArgs e){ if (pbx1_canChangeImage) { pictureBox1.Image = PlayRaceFantasy.Properties.Resources.launch_hover; }; return; }
        private void pictureBox1_MouseLeave(object sender, EventArgs e){ if (pbx1_canChangeImage) { pictureBox1.Image = PlayRaceFantasy.Properties.Resources.launch_normal; }; return; }
        private void pictureBox1_Click(object sender, EventArgs e){ pbx1_canChangeImage = false; pictureBox1.Image = PlayRaceFantasy.Properties.Resources.launch_pressed; pictureBox1.Enabled = false; actions.LaunchGame(); pictureBox1.Enabled = true; pictureBox1.Image = PlayRaceFantasy.Properties.Resources.launch_normal; pbx1_canChangeImage = true; return; }
        private void pictureBox2_Click(object sender, EventArgs e){ pbx2_canChangeImage = false; pictureBox2.Image = PlayRaceFantasy.Properties.Resources.update_pressed; pictureBox2.Enabled = false; actions.LaunchUpdater(); pictureBox2.Enabled = true; pictureBox2.Image = Properties.Resources.update_normal; pbx2_canChangeImage = true; return; }
        private void pictureBox2_MouseHover(object sender, EventArgs e){ if (pbx2_canChangeImage) { pictureBox2.Image = Properties.Resources.update_hover; } return; }
        private void pictureBox2_MouseLeave(object sender, EventArgs e) { if (pbx2_canChangeImage) { pictureBox2.Image = Properties.Resources.update_normal; } return; }
        private void pictureBox4_Click(object sender, EventArgs e){ Application.Exit(); return; }
        private void pictureBox4_MouseHover(object sender, EventArgs e) { pictureBox4.Image = Properties.Resources.exit_hovered; return; }
        private void pictureBox4_MouseLeave(object sender, EventArgs e) { pictureBox4.Image = Properties.Resources.exit_nomral; return; }
        private async void pictureBox3_Click(object sender, EventArgs e) { pbx3_canChangeImage = false; pbx4_canChangeImage = false; pictureBox3.Image = PlayRaceFantasy.Properties.Resources.repair_pressed; pictureBox3.Enabled = false; await actions.RepairInstallation(); pictureBox3.Enabled = true; pictureBox3.Image = Properties.Resources.repair_normal; pbx3_canChangeImage = true; }
        private void pictureBox3_MouseHover(object sender, EventArgs e){ if (pbx3_canChangeImage) { pictureBox3.Image = Properties.Resources.repair_hover; }; return; }
        private void pictureBox3_MouseLeave(object sender, EventArgs e){ if (pbx3_canChangeImage) { pictureBox3.Image = Properties.Resources.repair_normal; }; return; }

        private static Banner instance;
        public static Banner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Banner();
                }
                return instance;
            }
        }
    }
    internal class Actions
    {
        public int LaunchGame()
        {
            if (File.Exists("RaceFantasy.exe"))
            {
                try
                {
                    Process game = new Process();
                    game.StartInfo.FileName = "RaceFantasy.exe";
                    game.Start();
                    game.WaitForExit();
                    return 0;
                }
                catch (System.ComponentModel.Win32Exception) { MessageBox.Show("Race Fantasy cannot start due to an error. \nTry reinstalling Race Fantasy", "Race Fantasy - Game Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return 1; }
            }
            else { MessageBox.Show("Race Fantasy required files were not found.\nTry reinstalling Race Fantasy", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return 1; }
        }
        public int LaunchUpdater()
        {
            if (File.Exists("updater.exe"))
            {
                try
                {
                    Process updater = new Process();
                    updater.StartInfo.FileName = "updater.exe";
                    updater.StartInfo.Verb = "runAs";
                    updater.StartInfo.UseShellExecute = true;
                    updater.Start();
                    updater.WaitForExit();
                    return 0;
                }
                catch (System.ComponentModel.Win32Exception) { MessageBox.Show("Race Fantasy cannot check for updates due to an error (maybe administrator rights were not given).", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return 1; }
            }
            else
            {
                MessageBox.Show("Updater program wasn't found.\nPlease re-install Race Fantasy", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }
        public async Task<int> RepairInstallation()
        {
            if (MessageBox.Show("Are you sure you want to reinstall Race Fantasy?\nYour current progress will not be lost.", "Race Fantasy - Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    Banner.Instance.pbx1_canChangeImage = false;
                    Banner.Instance.pbx2_canChangeImage = false;
                    Banner.Instance.pbx3_canChangeImage = false;
                    Banner.Instance.pbx4_canChangeImage = false;
                    Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), ("RaceFantasy")));
                    using (WebClient webClient = new WebClient())
                    {
                        await webClient.DownloadFileTaskAsync(new Uri("https://www.dropbox.com/scl/fi/ludv9x2nuy58x9r42r38a/install_racefantasy_online.exe?rlkey=nw11vhjow9d5lohgdjqn5xhnm&dl=1"), Path.Combine(Path.GetTempPath(), "RaceFantasy" + "\\racefantasyinstall.exe"));
                    }

                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(Path.GetTempPath(), "racefantasyinstall.exe"),
                        Verb = "runas", // Run as administrator
                        UseShellExecute = true
                    };
                    Process.Start(processStartInfo);
                    
                    Process.GetCurrentProcess().Kill();
                }
                catch (System.ComponentModel.Win32Exception) { MessageBox.Show("Race Fantasy cannot check for updates due to an error (maybe administrator rights were not given).", "Race Fantasy - Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return 1; }
                
            }
            else
            {
                return 0;
            }
            MessageBox.Show("Feature not available in Release Candidate (RC) builds of this launcher.", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return 0;
        }
    }
}
