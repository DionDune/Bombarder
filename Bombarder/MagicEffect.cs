using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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

        public bool IsProjectile { get; set; }
        public float Angle { get; set; }
        public float Velocity { get; set; }
        public float VelocityLoss { get; set; }

        public List<MagicEffectPiece> Pieces { get; set; }

        public object MagicObj { get; set; }

        

        public MagicEffect()
        {
            X = 0;
            Y = 0;

            DamageTarget = "Entities";
            Damage = 4;
            DamageDuration = 150;
            RadiusIsCircle = false;
            DamageRadius = 0;
            RadiusOffset = new Point(-24, -24);
            RadiusSize = new Point(24, 24);

            IsProjectile = false;

            Pieces = new List<MagicEffectPiece>() { new MagicEffectPiece() { LifeSpan = DamageDuration } };
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

        public void EnactVelocity()
        {
            float AngleRadians = Angle * (float)(Math.PI / 180);

            X += (int)(Velocity * (float)Math.Cos(AngleRadians));
            Y += (int)(Velocity * (float)Math.Sin(AngleRadians));

            Velocity *= VelocityLoss;

            if (Velocity < 1)
            {
                Velocity = 0;
            }
        }



        public static bool CheckCollision(Point Coord1, Point Coord2, Entity Entity)
        {
            Vector2 HitboxStart = new Vector2(Entity.X + Entity.HitboxOffset.X, Entity.Y + Entity.HitboxOffset.Y);


            // Effect Hitbox is smaller than the Entity Hitbox
            if (Coord2.X - Coord1.X < Entity.HitboxSize.X && 
                Coord2.Y - Coord1.Y < Entity.HitboxSize.Y)
            {
                if (Coord1.X >= HitboxStart.X && Coord1.X <= HitboxStart.X + Entity.HitboxSize.X &&
                    Coord1.Y >= HitboxStart.Y && Coord1.Y <= HitboxStart.Y + Entity.HitboxSize.Y)
                {
                    return true;
                }
                if (Coord2.X >= HitboxStart.X && Coord2.X <= HitboxStart.X + Entity.HitboxSize.X &&
                    Coord1.Y >= HitboxStart.Y && Coord1.Y <= HitboxStart.Y + Entity.HitboxSize.Y)
                {
                    return true;
                }
                if (Coord2.X >= HitboxStart.X && Coord2.X <= HitboxStart.X + Entity.HitboxSize.X &&
                    Coord2.Y >= HitboxStart.Y && Coord2.Y <= HitboxStart.Y + Entity.HitboxSize.Y)
                {
                    return true;
                }
                if (Coord1.X >= HitboxStart.X && Coord1.X <= HitboxStart.X + Entity.HitboxSize.X &&
                    Coord2.Y >= HitboxStart.Y && Coord2.Y <= HitboxStart.Y + Entity.HitboxSize.Y)
                {
                    return true;
                }
            }
            // Effect Entity is smaller than the Effect Hitbox
            else
            {
                if (HitboxStart.X >= Coord1.X && HitboxStart.X <= Coord2.X &&
                    HitboxStart.Y >= Coord1.Y && HitboxStart.Y <= Coord2.Y)
                {
                    return true;
                }
                else if (HitboxStart.X + Entity.HitboxSize.X >= Coord1.X && HitboxStart.X + Entity.HitboxSize.X <= Coord2.X &&
                         HitboxStart.Y >= Coord1.Y && HitboxStart.Y <= Coord2.Y)
                {
                    return true;
                }
                else if (HitboxStart.X + Entity.HitboxSize.X >= Coord1.X && HitboxStart.X + Entity.HitboxSize.X <= Coord2.X &&
                         HitboxStart.Y + Entity.HitboxSize.Y >= Coord1.Y && HitboxStart.Y + Entity.HitboxSize.Y <= Coord2.Y)
                {
                    return true;
                }
                else if (HitboxStart.X >= Coord1.X && HitboxStart.X <= Coord2.X &&
                         HitboxStart.Y + Entity.HitboxSize.Y >= Coord1.Y && HitboxStart.Y + Entity.HitboxSize.Y <= Coord2.Y)
                {
                    return true;
                }
            }
            


            return false;
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
