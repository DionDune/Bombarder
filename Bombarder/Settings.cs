using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Settings
    {
        public float CursorSizeMultiplier { get; set; }
        public bool ShowGrid { get; set; }
        public bool ShowDamageRadii { get; set; }
        public bool ShowHitBoxes { get; set; }

        public bool RunEntityAI { get; set; }

        public Settings()
        {
            CursorSizeMultiplier = 2;
            ShowGrid = true;
            ShowDamageRadii = true;
            RunEntityAI = true;
            ShowHitBoxes = true;
        }
    }
}
