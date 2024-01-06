using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Pieces = new List<MagicEffectPiece>() { new MagicEffectPiece() { LifeSpan = DamageDuration } };
            MagicObj = new StaticOrb();
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


        internal class StaticOrb
        {
            const int Damage = 4;

            public static void EnactEffect(MagicEffect Effect, List<Entity> Entites)
            {
                Point EffectStart;
                Point EffectEnd;

                foreach (Entity Entity in Entites)
                {
                    EffectStart = new Point(Effect.X + Effect.RadiusOffset.X, Effect.Y + Effect.RadiusOffset.Y);
                    EffectEnd = new Point(EffectStart.X + Effect.RadiusSize.X, EffectStart.Y + Effect.RadiusSize.Y);

                    if (CheckCollision(EffectStart, EffectEnd, Entity))
                    {
                        Entity.GiveDamage(Damage);
                    }
                }
            }
        }
        internal class NonStaticOrb
        {
            const int Damage = 4;
            public float Angle { get; set; }
            public float Velocity { get; set; }
            public const float VelocityLoss = 0.95F;
            public const float DefaultVelocity = 25;

            public static void EnactEffect(MagicEffect Effect, List<Entity> Entites)
            {
                Point EffectStart;
                Point EffectEnd;

                //EnactVelocity
                NonStaticOrb Orb = (NonStaticOrb)Effect.MagicObj;
                EnactVelocity(Effect);

                //Enact Damage
                foreach (Entity Entity in Entites)
                {
                    EffectStart = new Point(Effect.X + Effect.RadiusOffset.X, Effect.Y + Effect.RadiusOffset.Y);
                    EffectEnd = new Point(EffectStart.X + Effect.RadiusSize.X, EffectStart.Y + Effect.RadiusSize.Y);

                    if (CheckCollision(EffectStart, EffectEnd, Entity))
                    {
                        Entity.GiveDamage(Damage);
                    }
                }
            }
            private static void EnactVelocity(MagicEffect Effect)
            {
                NonStaticOrb Orb = (NonStaticOrb)Effect.MagicObj;

                float AngleRadians = Orb.Angle * (float)(Math.PI / 180);

                Effect.X += (int)(Orb.Velocity * (float)Math.Cos(AngleRadians));
                Effect.Y += (int)(Orb.Velocity * (float)Math.Sin(AngleRadians));

                Orb.Velocity *= VelocityLoss;

                if (Orb.Velocity < 1)
                {
                    Orb.Velocity = 0;
                }
            }
        }
        internal class DissapationWave
        {
            public float Damage { get; set; }
            private const float DefaultDamage = 15;

            public float Radius { get; set; }
            private const float DefaultRadius = 5;
            private const float RadiusSpread = 5;
            private const float EdgeEffectWith = 10;

            public float Opacity { get; set; }
            private const float DefaultOpacity = 0.95F;
            private const float OpacityMultiplier = 0.9F;

            public DissapationWave()
            {
                Damage = DefaultDamage;
                Radius = DefaultRadius;
                Opacity = DefaultOpacity;
            }


            public static void EnactEffect(MagicEffect Effect, List<Entity> Entites)
            {
                EnactDamage(Effect, Entites);
            }
            private static void EnactDamage(MagicEffect Effect, List<Entity> Entites)
            {
                foreach (Entity Entity in Entites)
                {
                    float XDiff = Math.Abs(Effect.X - Entity.X);
                    float YDiff = Math.Abs(Effect.Y - Entity.Y);
                    float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                    if (Math.Abs(((DissapationWave)Effect.MagicObj).Radius - Distance) <= EdgeEffectWith)
                    {
                        Entity.GiveDamage((int)((DissapationWave)Effect.MagicObj).Damage);
                    }
                }

                EnactSpread(Effect);
            }
            private static void EnactSpread(MagicEffect Effect)
            {
                DissapationWave Wave = (DissapationWave)Effect.MagicObj;

                ((DissapationWave)Effect.MagicObj).Radius += RadiusSpread;
                ((DissapationWave)Effect.MagicObj).Opacity *= OpacityMultiplier;
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
