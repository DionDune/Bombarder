using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
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

        public int Duration { get; set; }

        public List<MagicEffectPiece> Pieces { get; set; }

        public object MagicObj { get; set; }



        public void EnactLifespan()
        {
            if (Duration > 0)
            {
                Duration--;
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

            Duration = StaticOrb.DefaultDuration;

            Pieces = new List<MagicEffectPiece>() { new MagicEffectPiece() { LifeSpan = DamageDuration } };
            MagicObj = new StaticOrb();
        }

        public class StaticOrb
        {
            const int Damage = 4;
            public const int DefaultDuration = 150;

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
        public class NonStaticOrb
        {
            const int Damage = 4;
            public const int DefaultDuration = 150;
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
        public class DissapationWave
        {
            public Color Colour = Color.MediumPurple;

            public float Damage { get; set; }
            public const int DefaultDuration = 150;
            private const float DefaultDamage = 12;
            private const float DamageMultiplier = 0.992F;

            public float Radius { get; set; }
            private const float DefaultRadius = 5;
            private const float RadiusSpread = 5;
            private const float EdgeEffectWith = 10;

            public float Opacity { get; set; }
            private const float DefaultOpacity = 0.95F;
            private const float OpacityMultiplier = 0.98F;

            public DissapationWave()
            {
                Damage = DefaultDamage;
                Radius = DefaultRadius;
                Opacity = DefaultOpacity;
            }


            public static void EnactEffect(MagicEffect Effect, List<Entity> Entites)
            {
                EnactSpread(Effect);
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
            }
            private static void EnactSpread(MagicEffect Effect)
            {
                DissapationWave Wave = (DissapationWave)Effect.MagicObj;

                ((DissapationWave)Effect.MagicObj).Radius += RadiusSpread;
                ((DissapationWave)Effect.MagicObj).Opacity *= OpacityMultiplier;
                ((DissapationWave)Effect.MagicObj).Damage *= DamageMultiplier;
            }
        }
        public class ForceWave
        {
            public Color Colour = Color.Crimson;

            public const int DefaultDuration = 200;

            public float Radius { get; set; }
            public const float RadiusMax = 400;
            private const float DefaultRadius = 5;
            private const float RadiusSpread = 10;

            public ForceWave()
            {
                Radius = DefaultRadius;
            }


            public static void EnactEffect(MagicEffect Effect, List<Entity> Entities)
            {
                EnactSpread(Effect);
                EnactForce(Effect, Entities);
            }
            private static void EnactForce(MagicEffect Effect, List<Entity> Entites)
            {
                ForceWave Wave = (ForceWave)Effect.MagicObj;

                foreach (Entity Entity in Entites)
                {
                    float XDiff = Math.Abs(Effect.X - Entity.X);
                    float YDiff = Math.Abs(Effect.Y - Entity.Y);
                    float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                    if (Distance <= Wave.Radius)
                    {
                        float Angle = (float)(Math.Atan2(XDiff, YDiff) * 180.0 / Math.PI);
                        float AngleRadians = Angle * (float)(Math.PI / 180);

                        Entity.X += (Wave.Radius - Distance) * (float)Math.Cos(AngleRadians);
                        Entity.Y += (Wave.Radius - Distance) * (float)Math.Sin(AngleRadians);
                    }
                }
            }
            private static void EnactSpread(MagicEffect Effect)
            {
                ForceWave Wave = (ForceWave)Effect.MagicObj;
                
                if (Wave.Radius < ForceWave.RadiusMax)
                {
                    Wave.Radius += RadiusSpread;
                }
            }
        }
        public class WideLazer
        {
            const int Damage = 1;
            const int DamageInterval = 10;

            public const int Range = 1500;
            public const int InitialDistance = 40;
            public const int Width = 60;
            public const int MarkerDistance = 75;

            public Color PrimaryColor = Color.Turquoise;
            public Color SecondaryColor = Color.White;
            public Color MarkerColor = Color.Red;
            public const float Opacity = 0.8F;

            public const int DefaultDuration = -1;

            public float Angle = 0;


            public static void EnactEffect(MagicEffect Effect, Player Player, List<Entity> Entites, uint Tick)
            {
                EnactDamage(Effect, Player, Entites, Tick);
            }
            private static void EnactDamage(MagicEffect Effect, Player Player, List<Entity> Entites, uint Tick)
            {
                WideLazer Lazer = (WideLazer)Effect.MagicObj;

                float X = Player.X;
                float Y = Player.Y;
                float AngleRadians = Lazer.Angle * (float)(Math.PI / 180);

                // Start an Initial Distance from source
                X += InitialDistance * (float)Math.Cos(AngleRadians);
                Y += InitialDistance * (float)Math.Sin(AngleRadians);

                for (int i = 0; i < WideLazer.Range / 3; i++)
                {
                    X += 3 * (float)Math.Cos(AngleRadians);
                    Y += 3 * (float)Math.Sin(AngleRadians);

                    foreach (Entity Entity in Entites)
                    {
                        float XDiff = Math.Abs(X - Entity.X);
                        float YDiff = Math.Abs(Y - Entity.Y);
                        float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                        if (Math.Abs(Distance) <= WideLazer.Width / 2 && Tick % WideLazer.DamageInterval == 0)
                        {
                            Entity.GiveDamage((int)WideLazer.Damage);
                        }
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
