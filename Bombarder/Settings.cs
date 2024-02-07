using Microsoft.Xna.Framework;
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
        public float GridOpacityMultiplier { get; set; }
        public float GridSizeMultiplier { get; set; }
        public Color GridColor { get; set; }
        public Color BackgroundColor { get; set; }

        public bool ShowDamageRadii { get; set; }
        public bool ShowHitBoxes { get; set; }

        public bool RunEntityAI { get; set; }
        public (int, int) EnemySpawnCountRange { get; set; }

        public Settings()
        {
            CursorSizeMultiplier = 1.66F;

            ShowGrid = true;
            GridOpacityMultiplier = 0.15F;
            GridSizeMultiplier = 1;
            GridColor = Color.White;
            BackgroundColor = Color.Black;

            ShowDamageRadii = false;
            ShowHitBoxes = false;

            RunEntityAI = true;
            EnemySpawnCountRange = (1, 3);
        }
    }
}
