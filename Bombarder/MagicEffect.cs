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

        public bool RadiusIsCircle { get; set; }
        public float DamageRadius { get; set; }
        public Point RadiusOffset { get; set; }
        public Point RadiusSize { get; set; }

        public int DamageDuration { get; set; }

        public List<MagicEffectPiece> Pieces { get; set; }
        

        public MagicEffect()
        {
            X = 0;
            Y = 0;

            DamageTarget = "Entities";
            Damage = 100;
            RadiusIsCircle = false;
            DamageRadius = 0;
            RadiusOffset = new Point(-24, -24);
            RadiusSize = new Point(24, 24);


            DamageDuration = 150;

            Pieces = new List<MagicEffectPiece>() { new MagicEffectPiece()};
        }

        public void EnactLifespan()
        {
            DamageDuration--;

            List<MagicEffectPiece> DeadPieces = new List<MagicEffectPiece>();
            foreach (MagicEffectPiece Piece in Pieces)
            {
                Piece.LifeSpan--;

                if (Piece.LifeSpan <= 0)
                {
                    DeadPieces.Add(Piece);
                }
            }

            foreach(MagicEffectPiece Piece in DeadPieces)
            {
                Pieces.Remove(Piece);
            }
        }
    }

    internal class MagicEffectPiece
    {
        public int LifeSpan { get; set; }
        public Color Color { get; set; }

        public string BaseShape { get; set; }

        public Point Offset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public MagicEffectPiece()
        {
            LifeSpan = 150;
            Color = Color.Turquoise;

            BaseShape = "Circle";

            Offset = new Point(-25, -25);
            Width = 50;
            Height = 50;
        }
    }
}
