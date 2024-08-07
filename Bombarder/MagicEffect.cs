﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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

        public bool HasDuration { get; set; }
        public int Duration { get; set; }

        public List<MagicEffectPiece> Pieces { get; set; }

        public object MagicObj { get; set; }



        public static void EnactDuration(List<MagicEffect> Effects)
        {
            List<MagicEffect> DeadEffects = new List<MagicEffect>();

            foreach (MagicEffect Effect in Effects)
            {
                if (Effect.HasDuration)
                {
                    if (Effect.Duration == 0)
                    {
                        DeadEffects.Add(Effect);
                    }
                    else
                    {
                        Effect.Duration--;
                    }
                }
            }
            foreach (MagicEffect Effect in DeadEffects)
            {
                Effects.Remove(Effect);
            }
        }
        public static bool CheckCollision(Point Coord1, Point Coord2, Entity Entity)
        {
            Vector2 HitboxStart = new Vector2(Entity.Position.X + Entity.HitboxOffset.X, Entity.Position.Y + Entity.HitboxOffset.Y);


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
            const int Damage = 2;
            public static readonly int ManaCost = 399;

            public const int DefaultDuration = 150;
            public const bool HasDuration = true;

            public uint LastParticleFrame;

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
                CreateParticles(Effect);
            }
            private static void CreateParticles(MagicEffect Effect)
            {
                StaticOrb Orb = (StaticOrb)Effect.MagicObj;

                if (Math.Abs(Game1.GameTick - Orb.LastParticleFrame) > Particle.Impact.DefaultFrequency)
                {
                    Game1.Particles.Add(new Particle(Effect.X, Effect.Y)
                    {
                        HasDuration = true,
                        Duration = Particle.Impact.DurationDefault,

                        ParticleObj = new Particle.Impact(1)
                        {
                            Radius = Particle.Impact.DefaultRadius,
                            Opacity = Particle.Impact.DefaultOpacity
                        }
                    });

                    Orb.LastParticleFrame = Game1.GameTick;
                }
            }

            public StaticOrb()
            {
                LastParticleFrame = 0;
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

            public static readonly int ManaCost = 50;

            public float Damage { get; set; }
            public const int DefaultDuration = 150;
            private const float DefaultDamage = 10;
            private const float DamageMultiplier = 0.992F;

            public float Radius { get; set; }
            private const float DefaultRadius = 5;
            private const float RadiusSpread = 5;
            private const float EdgeEffectWith = 10;

            public float Opacity { get; set; }
            private const float DefaultOpacity = 0.95F;
            private const float OpacityMultiplier = 0.98F;

            public const bool HasDuration = true;

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
                    float XDiff = Math.Abs(Effect.X - Entity.Position.X);
                    float YDiff = Math.Abs(Effect.Y - Entity.Position.Y);
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

            public static readonly int ManaCost = 150;

            public const int DefaultDuration = 200;
            public const bool HasDuration = true;

            public float Radius { get; set; }
            public const float RadiusMax = 200;
            private const float DefaultRadius = 5;
            private const float RadiusSpread = 10;
            public const float RadiusDecreaseMultiplier = 0.9F;
            public const int DurationSpreadCutoff = 5;

            public static readonly int BorderWidth = 20;

            public ForceWave()
            {
                Radius = DefaultRadius;
            }


            public static void EnactEffect(MagicEffect Effect, List<Entity> Entities)
            {
                EnactSpread(Effect);
                EnactForce(Effect, Entities);
                EnactDuration(Effect);
            }
            private static void EnactForce(MagicEffect Effect, List<Entity> Entites)
            {
                ForceWave Wave = (ForceWave)Effect.MagicObj;

                foreach (Entity Entity in Entites)
                {
                    float XDiff = Math.Abs(Effect.X - Entity.Position.X);
                    float YDiff = Math.Abs(Effect.Y - Entity.Position.Y);
                    float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));
                    
                    if (Distance <= Wave.Radius)
                    {
                        float xDiff = Entity.Position.X - Effect.X;
                        float yDiff = Entity.Position.Y - Effect.Y;
                        float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                        float AngleRadians = Angle * (float)(Math.PI / 180);

                        Vector2 PositionChange = new Vector2(
                            (Wave.Radius - Distance) * (float)Math.Cos(AngleRadians),
                            (Wave.Radius - Distance) * (float)Math.Sin(AngleRadians)
                            );

                        Entity.Position += PositionChange;
                    }
                }
            }
            private static void EnactSpread(MagicEffect Effect)
            {
                if (Effect.Duration > DurationSpreadCutoff)
                {
                    ForceWave Wave = (ForceWave)Effect.MagicObj;

                    if (Wave.Radius < ForceWave.RadiusMax)
                    {
                        Wave.Radius += RadiusSpread;
                    }
                }
            }
            private static void EnactDuration(MagicEffect Effect)
            {
                ForceWave Wave = (ForceWave)Effect.MagicObj;

                if (Effect.Duration < DurationSpreadCutoff)
                {
                    if (Wave.Radius > 1)
                    {
                        Effect.HasDuration = false;

                        Wave.Radius *= ForceWave.RadiusDecreaseMultiplier;
                    }
                    else
                    {
                        Effect.HasDuration = true;
                    }
                }
            }
        }
        public class ForceContainer
        {
            public readonly Color Colour = Color.OrangeRed;

            public static readonly int ManaCost = 150;

            public const int DurationDefault = 350;
            public const float Radius = 250;
            public const float RadiusMoving = 50;
            public const float MovementSpeed = 25;
            public const float EdgeEffectWith = 10;
            public const float RadiusIncrease = 16;
            public const float OpacityDefault = 0.7F;
            public const float DeathOpacityMultiplier = 0.9F;

            public const bool HasDuration_Moving = false;
            public const bool HasDuration_DestinationReached = true;
            public const bool IsActiveDefault = true;

            public Point Destination;
            public bool DestinationReached = false;
            public float CurrentRadius = RadiusMoving;
            public bool IsActive = IsActiveDefault;

            public static readonly int BorderWidth = 15;
            public float Opacity = OpacityDefault;

            public List<Entity> ContainedEntities = new List<Entity>();


            public static void EnactEffect(MagicEffect Effect, List<Entity> Entities)
            {
                EnactMovement(Effect);
                EnactForce(Effect, Entities);
                EnactDuration(Effect);
            }
            public static void EnactMovement(MagicEffect Effect)
            {
                ForceContainer Container = (ForceContainer)Effect.MagicObj;

                if (!Container.DestinationReached)
                {
                    float XDiff = Container.Destination.X - Effect.X;
                    float YDiff = Container.Destination.Y - Effect.Y;
                    float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));
                    float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);
                    float AngleRadians = Angle * (float)(Math.PI / 180);

                    if (Distance <= MovementSpeed)
                    {
                        Effect.X = Container.Destination.X;
                        Effect.Y = Container.Destination.Y;
                        Container.DestinationReached = true;
                        Effect.HasDuration = HasDuration_DestinationReached;
                    }
                    else
                    {
                        Effect.X += (int)(MovementSpeed * (float)Math.Cos(AngleRadians));
                        Effect.Y += (int)(MovementSpeed * (float)Math.Sin(AngleRadians));
                    }
                }
                else if (Container.CurrentRadius < ForceContainer.Radius)
                {
                    if (ForceContainer.Radius - Container.CurrentRadius < ForceContainer.RadiusIncrease)
                    {
                        Container.CurrentRadius = ForceContainer.Radius;
                    }
                    else
                    {
                        Container.CurrentRadius += ForceContainer.RadiusIncrease;
                    }
                }
            }
            public static void EnactForce(MagicEffect Effect, List<Entity> Entities)
            {
                ForceContainer Container = (ForceContainer)Effect.MagicObj;

                if (Container.IsActive)
                {
                    float XDiff;
                    float YDiff;
                    float Distance;


                    //Store new entites in Container list
                    foreach (Entity Entity in Entities)
                    {
                        if (!Container.ContainedEntities.Contains(Entity))
                        {
                            XDiff = Math.Abs(Effect.X - Entity.Position.X);
                            YDiff = Math.Abs(Effect.Y - Entity.Position.Y);
                            Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                            if (Distance <= Container.CurrentRadius)
                            {
                                Container.ContainedEntities.Add(Entity);
                            }
                        }
                    }

                    //Keep all contained entites within confines
                    foreach (Entity Entity in Container.ContainedEntities)
                    {
                        XDiff = Math.Abs(Effect.X - Entity.Position.X);
                        YDiff = Math.Abs(Effect.Y - Entity.Position.Y);
                        Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                        if (Distance >= Container.CurrentRadius - EdgeEffectWith)
                        {
                            float xDiff = Entity.Position.X - Effect.X;
                            float yDiff = Entity.Position.Y - Effect.Y;
                            float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                            float AngleRadians = Angle * (float)(Math.PI / 180);

                            Vector2 PositionChange = new Vector2(
                                Math.Abs((Container.CurrentRadius - EdgeEffectWith) - Distance) * (float)Math.Cos(AngleRadians),
                                Math.Abs((Container.CurrentRadius - EdgeEffectWith) - Distance) * (float)Math.Sin(AngleRadians)
                                );

                            Entity.Position -= PositionChange;
                        }
                    }
                }
            }
            public static void EnactDuration(MagicEffect Effect)
            {
                ForceContainer Container = (ForceContainer)Effect.MagicObj;

                if (Effect.Duration < 3)
                {
                    if (Container.IsActive)
                    {
                        Effect.HasDuration = false;
                        Container.IsActive = false;
                    }


                    if (Container.Opacity > 0.05F)
                    {
                        Container.Opacity *= ForceContainer.DeathOpacityMultiplier;
                    }
                    else
                    {
                        Effect.HasDuration = true;
                    }
                }
            }
        }
        public class WideLazer
        {
            public const int ManaCost = 2;
            public const uint ManaCostInterval = 2;

            const int Damage = 4;
            const int DamageInterval = 3;

            public const int Range = 1500;
            public const int InitialDistance = 40;
            public const int Width = 60;
            public const int MarkerDistance = 75;

            public Color PrimaryColor = Color.Turquoise;
            public Color SecondaryColor = Color.White;
            public Color MarkerColor = Color.Red;
            public const float Opacity = 0.8F;

            public const int Spread = 1;
            public const float TrueSpreadMultiplier = 4.5F;

            public const int DefaultDuration = -1;

            public float Angle = 0;


            public static void EnactEffect(MagicEffect Effect, Player Player, List<Entity> Entites, uint Tick)
            {
                if (Player.CheckUseMana(ManaCost))
                {
                    EnactDamage(Effect, Player, Entites, Tick);
                    CreateParticles(Effect);
                }
            }
            private static void EnactDamage(MagicEffect Effect, Player Player, List<Entity> Entites, uint Tick)
            {
                WideLazer Lazer = (WideLazer)Effect.MagicObj;

                float AngleRadians = Lazer.Angle * (float)(Math.PI / 180);

                float XDiff;
                float YDiff;
                float Distance;
                
                float RotatedX;
                float RotatedY;
                float CurrentLazerWidth;
                float EntityStartX;
                float EntityStartY;


                if (Tick % DamageInterval == 0)
                {
                    //Calculate radius of the Lazers Spread every 1 Distance
                    float SpreadValue = (float)Math.Sin(Spread * (float)(Math.PI / 180));

                    foreach (Entity Entity in Entites)
                    {
                        XDiff = Math.Abs(Effect.X - Entity.Position.X);
                        YDiff = Math.Abs(Effect.Y - Entity.Position.Y);
                        Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

                        if (Distance < Range)
                            // Entity is close enough to the lazer
                        {
                            // Point along lazer with equal distance as Entity
                            RotatedX = Effect.X + (Distance * (float)Math.Cos(AngleRadians));
                            RotatedY = Effect.Y + (Distance * (float)Math.Sin(AngleRadians));

                            EntityStartX = Entity.Position.X + Entity.HitboxOffset.X;
                            EntityStartY = Entity.Position.Y + Entity.HitboxOffset.Y;

                            //Calculate radius of the Lazers Current spread
                            CurrentLazerWidth = (SpreadValue * Distance) * TrueSpreadMultiplier;

                            if (RotatedX >= EntityStartX - CurrentLazerWidth && RotatedX <= EntityStartX + Entity.HitboxSize.X + CurrentLazerWidth &&
                                RotatedY >= EntityStartY - CurrentLazerWidth && RotatedY <= EntityStartY + Entity.HitboxSize.Y + CurrentLazerWidth)
                            {
                                Entity.GiveDamage((int)WideLazer.Damage);
                            }
                        }
                    }
                }
            }
            public static void CreateParticles(MagicEffect Effect)
            {
                WideLazer Lazer = (WideLazer)Effect.MagicObj;

                float AngleRadians;
                int Count = Game1.random.Next(1, 4);


                // Central Lazer
                AngleRadians = (Lazer.Angle + ((float)Game1.random.Next(-Spread * 10, Spread * 10) / 10)) * (float)(Math.PI / 180);
                Game1.Particles.Add(new Particle(Effect.X, Effect.Y)
                {
                    HasDuration = true,
                    Duration = Particle.LazerLine.DurationMin,

                    ParticleObj = new Particle.LazerLine()
                    {
                        Length = Particle.LazerLine.LengthMax,
                        Thickness = Particle.LazerLine.ThicknessMax,
                        Direction = AngleRadians,
                        Speed = Particle.LazerLine.SpeedMax * 2,
                        Colour = Particle.LazerLine.CentralLazerColor
                    }
                });

                //Others
                for (int i = 0; i < Count; i++)
                {
                    AngleRadians = (Lazer.Angle + Game1.random.Next(-Particle.LazerLine.AngleSpreadRange, Particle.LazerLine.AngleSpreadRange)) * (float)(Math.PI / 180);

                    Game1.Particles.Add(new Particle(Effect.X, Effect.Y)
                    {
                        HasDuration = true,
                        Duration = Game1.random.Next(Particle.LazerLine.DurationMin, Particle.LazerLine.DurationMax),

                        ParticleObj = new Particle.LazerLine()
                        {
                            Direction = AngleRadians
                        }
                    });
                }
            }
        }

        public class PlayerTeleport
        {
            public const int ManaCost = 150;

            public const int DefaultDuration = 2;
            public const bool HasDuration = false;
            public const bool HasDurationWhenReached = true;

            public const float Speed = 80;
            public Vector2 Goal;
            public bool GoalReacted = false;
            public bool JustStarted = true;

            public const int ParticleCountMin = 65;
            public const int ParticleCountMed = 100;
            public const int ParticleCountMax = 150;
            public const int ParticleCountMaxDistanceThreshold = 400;



            public static void EnactEffect(MagicEffect Effect, Player Player)
            {
                PlayerTeleport Teleport = (PlayerTeleport)Effect.MagicObj;


                if (Teleport.JustStarted)
                {
                    CreateParticles(Effect);
                }
                EnactMovement(Effect, Player);
                EnactDuration(Effect, Player);
            }
            public static void EnactMovement(MagicEffect Effect, Player Player)
            {
                PlayerTeleport Teleport = (PlayerTeleport)Effect.MagicObj;

                float xDiff = Player.Position.X - Teleport.Goal.X;
                float yDiff = Player.Position.Y - Teleport.Goal.Y;
                float Distance = (float)Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                float AngleRadians = Angle * (float)(Math.PI / 180);

                float DistanceToMove = Speed;

                if (Distance <= Speed)
                {
                    DistanceToMove = Distance;
                    Teleport.GoalReacted = true;
                }


                Player.Position -= new Vector2(
                    DistanceToMove * (float)Math.Cos(AngleRadians), DistanceToMove * (float)Math.Sin(AngleRadians));
                Player.IsImmune = true;

                Teleport.JustStarted = false;
            }
            public static void EnactDuration(MagicEffect Effect, Player Player)
            {
                PlayerTeleport Teleport = (PlayerTeleport)Effect.MagicObj;

                if (Teleport.GoalReacted)
                {
                    Effect.HasDuration = true;
                    Player.IsImmune = false;
                }
            }
            public static void CreateParticles(MagicEffect Effect)
            {
                PlayerTeleport Teleport = (PlayerTeleport)Effect.MagicObj;

                float xDiff = Effect.X - Teleport.Goal.X;
                float yDiff = Effect.Y - Teleport.Goal.Y;
                float Distance = (float)Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));

                int Count = PlayerTeleport.ParticleCountMed;
                if (Distance > PlayerTeleport.ParticleCountMaxDistanceThreshold)
                {
                    Count = PlayerTeleport.ParticleCountMax;
                }

                for (int i = 0; i < Game1.random.Next(ParticleCountMin, Count); i++)
                {
                    Particle.TeleportLine.SpawnBetween(Game1.Particles, new Vector2(Effect.X, Effect.Y), new Vector2(Teleport.Goal.X, Teleport.Goal.Y));
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
