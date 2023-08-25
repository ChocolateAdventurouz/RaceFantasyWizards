using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace RaceFantasyInstaller
{
    internal static class Program
    {
        static void RunApplication()
        {
            //MessageBox.Show("To beta testers: Before you continue, please take a look at the changelog:\n====== Release Candidate 3 (built in 15/08/2023 at 17:01:57)======\r\n- *New*: \"Launch Race Fantasy\" on InstallComplete form\r\n- *New*: Uninstaller is removing Race Fantasy shortcuts now\r\n- Visual Studio building arch minor changes\r\n- Improved built-in SFX\r\n- Improved version comparison\r\n- Switched to build numbers, instead of version numbers (starting off 10000)\r\n- Piped MethodCollections.cs from updater to uninstaller\r\n- Got rid of installLoc.info (finally)\r\n- Improved LICENSE text\r\n- Changed version scheme in assembly (1.x.x.x -> 1xxxx) as it can cause future problems with updater\r\n- Removed signing of executables\r\n- Removed SignTool from being executed in building events\r\n- Cleaned up useless files/placeholders\r\n- Fix uninstaller buttons glitches\r\n- Fixed cursor events not being applied\r\n- Removed useless code\r\n- Removed any JSON file leftovers\r\n- Remove useless code\r\n- Cleaned up uninstaller's code\r\n- Fixed a bug in uninstaller where it would treat files as directories and then it would throw exception\r\n- Removed version numbers from all executables, except for installer\r\n- Removed downloaded files from updater, after the process is done\r\n- Fixed a bug in updater where it wouldn't update the version number\r\n- Fixed a bug in updater where it would throw an exception about progressbar value\r\n- Fixed/Cleaned up most compiler warnings (from 55 -> 34)", "Race Fantasy - Beta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Application.Run(new MainWindow());

        }

        static void ActLikeSfx()
        {
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Buffers.dll", Properties.Resources.System_Buffers);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Memory.dll", Properties.Resources.System_Memory);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.ValueTuple.dll", Properties.Resources.System_ValueTuple);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Numerics.Vectors.dll", Properties.Resources.System_Numerics_Vectors);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Runtime.CompilerServices.Unsafe.dll", Properties.Resources.System_Runtime_CompilerServices_Unsafe);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Threading.Tasks.Extensions.dll", Properties.Resources.System_Threading_Tasks_Extensions);
            File.WriteAllBytes(Path.GetTempPath() + "\\Microsoft.Bcl.AsyncInterfaces.dll", Properties.Resources.Microsoft_Bcl_AsyncInterfaces);
            File.WriteAllBytes(Path.GetTempPath() + "\\Microsoft.Deployment.Compression.dll", Properties.Resources.Microsoft_Deployment_Compression);
            File.WriteAllBytes(Path.GetTempPath() + "\\Microsoft.Deployment.Compression.Cab.dll", Properties.Resources.Microsoft_Deployment_Compression_Cab);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Text.Encodings.Web.dll", Properties.Resources.System_Text_Encodings_Web);
            File.WriteAllBytes(Path.GetTempPath() + "\\System.Text.Json.dll", Properties.Resources.System_Text_Json);
            File.Copy(Assembly.GetEntryAssembly().Location, Path.GetTempPath() + "\\" + Path.GetFileName(Assembly.GetEntryAssembly().Location), true);
            return;
        }
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            var name = new AssemblyName(e.Name);
            if (name.Name == "System.Runtime.CompilerServices.Unsafe")
            {
                return typeof(System.Runtime.CompilerServices.Unsafe).Assembly;
            }
            return null;
        }
        static void StartExecution()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            MethodCollection methodCollection = new MethodCollection();
            MethodCollection.Host host = new MethodCollection.Host();
            MethodCollection.Versioning versioning = new MethodCollection.Versioning();
            if (host.GetArchitecture() == "64-bit") { RunApplication(); }
            else { MessageBox.Show("Race Fantasy is not supported in 32-bit systems.", "Race Fantasy - Error"); }
            if (host.IsWindows7OrLater() == true) { RunApplication(); }
            else { MessageBox.Show("Race Fantasy is supported in Windows 7 and later versions of Windows. ", "Race Fantasy - Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            // unloading below to avoid SO exception
            methodCollection = null;
            host = null;
            versioning = null;
            return;
        }
        [STAThread]
        static void Main()
        {
            string realFilename = Path.GetFileName(Assembly.GetEntryAssembly().Location);
            if (File.Exists(Path.GetTempPath() + "\\.sfx.is.complete=yes"))
            {
                StartExecution();
            }
            else
            {
                File.Create(Path.GetTempPath() + "\\.sfx.is.complete=yes");
                ActLikeSfx();
                Process.Start(Path.Combine(Path.GetTempPath() + Path.GetFileName(Assembly.GetEntryAssembly().Location)));
                return;
            }
        }

    }
}
