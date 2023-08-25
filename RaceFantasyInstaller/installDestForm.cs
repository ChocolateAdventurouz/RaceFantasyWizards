using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RaceFantasyInstaller
{
    public partial class installDestForm : Form
    {
        public string path;
        public installDestForm()
        {
            InitializeComponent();

        }

        public void RegisterFileConfig()
        {
            
           /* if (File.Exists(@Path.GetTempPath() + "\\installationLoc.info"))
            {
                File.WriteAllText(@Path.GetTempPath() + "\\installationLoc.info", path.ToString());
            }
            else
            {
                File.WriteAllText(@Path.GetTempPath() + "\\installationLoc.info", path.ToString());
            }
            */
            SharedVariables.Instance.InstallationDir = path.ToString();
        }
        public void installDestForm_Load(object sender, EventArgs e)
        {

            path = textBox1.Text;
            RegisterFileConfig();
            return;
        }

        public void button1_Click(object sender, EventArgs e)

        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog()) // well, it broke again...
            {
                // Set the initial directory (optional)
                folderDialog.SelectedPath = @"C:\";

                // Show the folder dialog and get the result
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    textBox1.Text = folderDialog.SelectedPath;
                    path = folderDialog.SelectedPath.ToString();
                    RegisterFileConfig();
                    return;
                }
                return;
            }
            /*using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = fbd.SelectedPath;
                    path = fbd.SelectedPath.ToString();
                    RegisterFileConfig();
                    return;
                }
            }*/
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var mainWindow = (Application.OpenForms.OfType<MainWindow>().FirstOrDefault() ?? new MainWindow());
            if (textBox1.Text.Contains(":") && textBox1.Text.Contains("\\")) // making sure that there is a directory-style input to avoid bad inputs
            {
                path = textBox1.Text;
                RegisterFileConfig();
            }
            else
            {
                MessageBox.Show("The installation path doesn't seem valid.", "Race Fantasy");
                mainWindow.button2.Enabled = false;
            }
        }
    }
}
