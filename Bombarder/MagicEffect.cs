using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class MagicEffect
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string? DamageTarget { get; set; }
        public int Damage { get; set; }
        public float DamageRadius { get; set; }

        public bool Continuous { get; set; }
        public int LifeSpan { get; set; }

        public Color Color { get; set; }

        public MagicEffect()
        {
            X = 0;
            Y = 0;

            DamageTarget = "Entities";
            Damage = 100;
            DamageRadius = 25;

            Continuous = false;
            LifeSpan = 100;
            Color = Color.Turquoise;
        }
    }
}
