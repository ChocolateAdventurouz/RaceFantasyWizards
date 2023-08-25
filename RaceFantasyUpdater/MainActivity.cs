using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
namespace RaceFantasyUpdater
{
    internal class MainActivity
    {
        RaceFantasyInstaller.MethodCollection methodCollection = new RaceFantasyInstaller.MethodCollection();
        RaceFantasyInstaller.MethodCollection.Versioning versioning = new RaceFantasyInstaller.MethodCollection.Versioning();

        UpdateFound updateFoundForm = new UpdateFound();

        private readonly string stringPlaceHolder;
        private string path;

        static int CompareVersions(string version1, string version2)
        {
            // Implement version comparison logic here
            // Return -1 if version1 < version2
            // Return 0 if version1 == version2
            // Return 1 if version1 > version2-
            Console.WriteLine(string.Compare(version1, version2, StringComparison.Ordinal));
            return string.Compare(version1, version2, StringComparison.Ordinal);
        }
        private void InterConnect()
        {
            if (MessageBox.Show("A new version is available! Do you want to download?", "Race Fantasy - Update Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                UpdateFound updateFound = new UpdateFound();
                updateFound.ShowDialog();
                return;
            }
            else
            {
                return;
            }
        }
        public void Main()
        {
            path = methodCollection.GetPath();

            if (methodCollection.CheckForInternet() == 1)
            {
                Application.Exit();
                return;
            }

            int remoteVersion = versioning.GrabRemoteVersionNumber();
            int localVersion = versioning.GrabLocalVersionNumber(stringPlaceHolder);

            
           // Console.WriteLine(localVersion.ToString() + " local|remote " + remoteVersion.ToString());

            if (remoteVersion > localVersion)
            {
                InterConnect();
                return;
            }
            else
            {
                MessageBox.Show("No updates available.", "Race Fantasy - Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
                Application.Exit();
                return;
            }
        }
    }
}
