using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Settings
    {
        public bool ShowGrid { get; set; }
        public bool ShowDamageRadii { get; set; }
        public bool RunEntityAI { get; set; }

        public Settings()
        {
            ShowGrid = true;
            ShowDamageRadii = true;
            RunEntityAI = true;
        }
    }
}
