/* MethodCollection.cs - A toolset of useful functions to call, in order to keep the messy code away
 * Author(s): ChocolateAdventurouz
 * --------WARNING: UNAUTHORIZED ACCESS OR EDITING OF THIS FILE IS NOT PERMITTED IN ANY WAY, UNLESS IT IS ALLOWED BY THE AUTHOR(S)--------
*/

using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceFantasyInstaller
{
    public class MethodCollection
    {
        internal class Versioning : MethodCollection
        {

            public async Task<string> GrabRemoteVersionNumber()
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        string version = client.DownloadString("https://github.com/georgesepetadelis/rf_version/raw/main/version.txt");
                        //var appSettings = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
                        //var version = appSettings.RootElement.GetProperty("versionCode").GetSingle();
                        return version;
                    }
                    catch (System.Net.WebException)
                    {

                        for (int iTry = 0; 0 < 3;)
                        {
                            if (iTry == 3)
                            {
                                return string.Empty;
                            }
                            await GrabRemoteVersionNumber();
                            iTry++;
                        }
                    }

                }
            }


            public string GrabLocalVersionNumber(string localVersion) // have to make a small hack so it can work
            {
                Assembly asm = GetType().Assembly;

                using (RegistryKey parent64 = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
                {
                    if (parent64 == null)
                    {
                        throw new Exception("Uninstall registry key not found.");
                    }
                    try
                    {
                        using (RegistryKey key = parent64.OpenSubKey("Race Fantasy", true) ?? parent64.CreateSubKey("Race Fantasy"))
                        {

                            if (key == null)
                            {
                                return String.Empty;
                            }
                            try
                            {
                                localVersion = key.GetValue("DisplayVersion").ToString();
                            }

                            finally
                            {
                                if (key != null) { key.Close(); }

                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show("An error occurred while reading the version information.  Race Fantasy doesn't seem to be installed, but it is recommended to re-run the installation wizard again. \n Detailed warning" + ex, "Race Fantasy - Warning"); }
                }
                localVersion = localVersion.Replace(",", ".");
                return localVersion.ToString(); //works only with .ToString() even it is declared as a string :/ 
            }
        }

        internal class MD5Toolset
        {
            async Task<String> CalculateMD5(string filename)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                    }
                }
            }
            async Task<String> GetMD5FromHost()
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile("https://firebasestorage.googleapis.com/v0/b/race-fantasy.appspot.com/o/RaceFantasy.cab", Path.Combine(Path.GetTempPath(), "racefantasy.json"));
                    var json = File.ReadAllText(Path.Combine(Path.GetTempPath(), "racefantasy.json"));
                    var appSettings = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
                    string remoteHash = appSettings.RootElement.GetProperty("md5Hash").GetString();
                    return remoteHash.ToString();
                }
            }
        }

        /// <summary>
        /// Checks if the file is downloaded:
        /// 
        /// This process is divided into 2 sections.
        /// 
        /// 1. Compares bytes between the remote file json and the downloaded file
        /// 2. if the comparison is successful, there is going to be a md5 hash check
        /// </summary>
        /// <returns>"true" if the file is valid, "false" if it is not</returns>
        public bool IsFileDownloaded()
        {
            // 1st stage

            string Cabinetfile = @Path.GetTempPath() + "RaceFantasy.cab";
            if (File.Exists(Cabinetfile))
            {
                return true;
                /* Console.WriteLine("FILE OK!");
                 // 2nd stage

                 Console.WriteLine("Local: {0}", CalculateMD5(Cabinetfile));
                 Console.WriteLine("Remote: {0}", GetCabinetMD5());
                 if (CalculateMD5(Cabinetfile).Equals(GetCabinetMD5()))
                 {
                     Console.WriteLine("MD5 OK!");
                     return true;
                 }
                 else
                 {
                     return false;
                 }*/
            }
            else
            {
                return false;
            }
        }

        public string GetPath()
        {
            string path = Environment.GetEnvironmentVariable("RaceFantasyInstallationDir", EnvironmentVariableTarget.User);
            return path;
        }
        public async Task<int> TestConnection()
        {

            const string exampleHost = "example.com";
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply;
                    reply = ping.Send(exampleHost);

                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("[{0}] - Detected an active internet connection.", DateTime.Now.ToString());
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("[{0}] - Couldn't detect an active internet connection.", DateTime.Now.ToString());
                        return 1;
                    }

                }
                catch (System.Net.NetworkInformation.PingException)
                {
                    Console.WriteLine("[{0}] - Couldn't detect an active internet connection.", DateTime.Now.ToString());
                    return 1;
                }

            }
        }

        internal class Host : MethodCollection
        {
            ///<summary>
            /// Get host's architecture.
            /// </summary>
            public string GetArchitecture()
            {

                string x86 = "32-bit";
                string x64 = "64-bit";

                if (System.Environment.Is64BitOperatingSystem == true) { return x64; }
                else { return x86; }
            }


            /// <summary>
            /// Get host's running version of Windows and compares if it is Windows 7 or later.
            /// </summary>
            /// <returns>
            /// true if it is running windows 7 or later
            /// false if it runs an older version of windows such as 2000, XP32/64 Vista32/64 etc
            /// </returns>
            public bool IsWindows7OrLater()
            {

                if (Environment.OSVersion.Version.Major >= 7) { return true; }
                else { return false; }
            }
        }

        /// <summary>
        /// Extracts resources from the namespace
        /// </summary>
        /// <param name="NameSpace">This namespace</param>
        /// <param name="FileName">The file that is needed to export</param>
        /// <param name="OutputDirectory">The directory to export the file</param>
        public async Task ExtractUninstaller(string OutputDirectory)
        {
            File.WriteAllBytes(OutputDirectory + "\\uninstall.exe", Properties.Resources.uninstall);
            return;
        }
        public async Task ExtractUpdater(string OutputDirectory) // Finally, updater is bundled!
        {
            File.WriteAllBytes(OutputDirectory + "\\updater.exe", Properties.Resources.updater);
        }
        public async Task ExtractLauncher(string OutputDirectory) // PlayRaceFantasy.exe
        {
            File.WriteAllBytes(OutputDirectory + "\\PlayRaceFantasy.exe", Properties.Resources.PlayRaceFantasy);
            return;
        }
        public async Task ExtractGamePackage(string OutputDirectory)
        {
            File.WriteAllBytes(OutputDirectory + "\\RaceFantasy.cab", Properties.Resources.RaceFantasy);
            return;
        }
    }
}
