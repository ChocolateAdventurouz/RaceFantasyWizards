using System;
using System.Reflection;
using System.Windows.Forms;

namespace RaceFantasyUpdater
{
    internal static class Program
    {
        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
        {
            var name = new AssemblyName(e.Name);
            if (name.Name == "System.Runtime.CompilerServices.Unsafe")
            {
                return typeof(System.Runtime.CompilerServices.Unsafe).Assembly;
            }
            return null;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            MainActivity mainActivity = new MainActivity();
            mainActivity.Main();
            return;
        }
    }
}
