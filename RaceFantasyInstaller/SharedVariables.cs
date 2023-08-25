using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceFantasyInstaller
{
    internal class SharedVariables
    {
        /// <summary>
        /// Used in installComplete.cs in order to detect if the checkbox is checked so afther the installer instance is killed, the launcher will be executed
        /// </summary>
        public bool LaunchRaceFantasy { get; set; }

        /// <summary>
        /// Used in installDestForm.cs in order to write the installation path that the user selected
        /// </summary>
        public string InstallationDir { get; set; }

        private static SharedVariables instance;
        public static SharedVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedVariables();
                }
                return instance;
            }
        }

    }
}
