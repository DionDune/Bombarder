﻿using Microsoft.Xna.Framework;
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

        private void EnactVelocity(float Angle, float Velocity, float VelocityLoss)
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

            public static void EnactEffect(MagicEffect Effect, List<Entity> Entites)
            {
                Point EffectStart;
                Point EffectEnd;

                //EnactVelocity
                NonStaticOrb Orb = (NonStaticOrb)Effect.MagicObj;
                Effect.EnactVelocity(Orb.Angle, Orb.Velocity, VelocityLoss);

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
