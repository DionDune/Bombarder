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

        public List<MagicEffectPeice> Peices { get; set; }
        

        public MagicEffect()
        {
            X = 0;
            Y = 0;

            DamageTarget = "Entities";
            Damage = 100;
            DamageRadius = 25;

            Continuous = false;

            Peices = new List<MagicEffectPeice>() { new MagicEffectPeice ()};
        }

        public void EnactLifespan()
        {
            List<MagicEffectPeice> DeadPieces = new List<MagicEffectPeice>();
            foreach (MagicEffectPeice Piece in Peices)
            {
                Piece.LifeSpan--;

                if (Piece.LifeSpan <= 0)
                {
                    DeadPieces.Add(Piece);
                }
            }

            foreach(MagicEffectPeice Piece in DeadPieces)
            {
                Peices.Remove(Piece);
            }
        }
    }

    internal class MagicEffectPeice
    {
        public int LifeSpan { get; set; }
        public Color Color { get; set; }

        public Point Offset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public MagicEffectPeice()
        {
            LifeSpan = 150;
            Color = Color.Turquoise;

            Offset = new Point(-25, -25);
            Width = 50;
            Height = 50;
        }
    }
}
