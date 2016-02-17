using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebateEmulator
{
    internal class DebateTeam
    {
        /// <summary>
        /// Number from 0 to 1 indicating relative ability. .5 means you would beat an average team 50% of the time
        /// </summary>
        public double Ability { get; private set; }

        public int WinCount { get; set; }

        public int LossCount { get; set; }

        public DebateTeam(double ability)
        {
            this.Ability = ability;
        }
    }
}
