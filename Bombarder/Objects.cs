﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Bombarder
{
    public class Object
    {
        public string Type { get; set; }

        public bool HasDuration { get; set; }
        public int Duration { get; set; }

        public int TextureTag { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }


        public Object(object Obj, int x, int y)
        {

        }

        public class ObjectContainer
        {
            public List<Object> GeneralObjects;

            public ObjectContainer()
            {
                GeneralObjects = new List<Object>();
            }
        }
    }

    public class Particle
    {
        public bool HasDuration { get; set; }
        public int Duration { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public object ParticleObj { get; set; }

        public Particle(int x, int y)
        {
            X = x;
            Y = y;

            HasDuration = false;
            Duration = 0;

            ParticleObj = null;
        }

        public static void EnactDuration(List<Particle> Particles)
        {
            List<Particle> Dead = new List<Particle>();

            foreach (Particle particle in Particles)
            {
                if (particle.HasDuration)
                {
                    if (particle.Duration <= 0)
                    {
                        Dead.Add(particle);
                    }
                    else
                    {
                        particle.Duration--;
                    }
                }
            }

            foreach (Particle particle in Dead)
            {
                
                Particles.Remove(particle);
            }
        }
        public static void EnactParticles(List<Particle> Particles, uint Tick)
        {
            foreach (Particle particle in Particles)
            {
                switch (particle.ParticleObj)
                {
                    case LazerLine:
                        LazerLine.EnactParticle(particle);
                        break;
                    case Impact:
                        Impact.EnactParticle(particle);
                        break;
                    case Dust:
                        Dust.EnactParticle(particle, Tick);
                        break;
                    case RedCubeSegment:
                        RedCubeSegment.EnactParticle(particle);
                        break;
                    case TeleportLine:
                        TeleportLine.EnactParticle(particle);
                        break;
                }
            }
        }
        public static void SpawnParticles(List<Particle> Particles, Vector2 PlayerPos, GraphicsDeviceManager Graphics, uint Tick)
        {
            int ClutterSpawnRangeX = Graphics.PreferredBackBufferWidth;
            int ClutterSpawnRangeY = Graphics.PreferredBackBufferHeight;


            Dust.Spawn(Particles, PlayerPos, ClutterSpawnRangeX, ClutterSpawnRangeY, Tick);
        }


        //Attack Related
        public class HitMarker
        {
            public const int Width = 30;
            public const int Height = 30;

            public const int DefaultDuration = 50;
        }
        public class LazerLine
        {
            public const int LengthMin = 10;
            public const int LengthMax = 200;
            public const int ThicknessMin = 2;
            public const int ThicknessMax = 4;
            public const int DurationMin = 25;
            public const int DurationMax = 200;
            public const int SpeedMin = 15;
            public const int SpeedMax = 75;
            public const int AngleSpreadRange = 5;
            public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
                (new List<Color> { Color.Turquoise, Color.DarkTurquoise, Color.MediumTurquoise, Color.MediumTurquoise});
            public static Color CentralLazerColor = Color.Red;


            public float Length { get; set; }
            public int Thickness { get; set; }
            public float Direction { get; set; }
            public float Speed { get; set;  }
            public Color Colour { get; set; }


            public static void EnactParticle(Particle Particle)
            {
                EnactMovement(Particle);
            }
            public static void EnactMovement(Particle Particle)
            {
                LazerLine Line = (LazerLine)Particle.ParticleObj;

                Particle.X = (int)((float)Particle.X + Line.Speed * (float)Math.Cos(Line.Direction));
                Particle.Y = (int)((float)Particle.Y + Line.Speed * (float)Math.Sin(Line.Direction));
            }

            public LazerLine()
            {
                Length = Game1.random.Next(LengthMin, LengthMax);
                Thickness = Game1.random.Next(ThicknessMin, ThicknessMax);
                Speed = Game1.random.Next(SpeedMin, SpeedMax);
                Colour = Color.Turquoise;
            }
        }
        public class TeleportLine
        {
            public const int LengthMin = 5;
            public const int LengthMax = 20;
            public const int ThicknessMin = 3;
            public const int ThicknessMax = 6;
            public const int RandomDistanceAllowance = 20;

            public const int DurationMin = 10;
            public const int DurationMax = 100;

            public const int AngleSpreadRange = 10;
            public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
                (new List<Color> { Color.Purple, Color.MediumPurple, Color.DarkMagenta });

            public const float OpacityDefault = 0;
            public const float OpacityIncreasingChange = 0.15F;
            public const int OpacityIncreaseInterval = 1;
            public const float OpacityDecreasingChange = 0.025F;
            public const int OpacityDecreasingInterval = 3;
            public bool OpacityIncreasing = true;



            public float Length { get; set; }
            public float Thickness { get; set; }
            public float Direction { get; set; }
            public Color Colour { get; set; }
            public float Opacity { get; set; }


            public static void EnactParticle(Particle Particle)
            {
                EnactOpacityChange(Particle);
            }
            private static void EnactOpacityChange(Particle Particle)
            {
                TeleportLine Line = (TeleportLine)Particle.ParticleObj;

                if (Line.OpacityIncreasing)
                {
                    if (Game1.GameTick % OpacityIncreaseInterval == 0)
                    {
                        Line.Opacity += OpacityIncreasingChange;
                        if (Line.Opacity >= 1)
                        {
                            Line.OpacityIncreasing = false;
                        }
                    }
                }
                else
                {
                    if (Game1.GameTick % OpacityDecreasingInterval == 0)
                    {
                        Line.Opacity -= OpacityDecreasingChange;
                        if (Line.Opacity <= 0)
                        {
                            Particle.Duration = 1;
                            Particle.HasDuration = true;
                        }
                    }
                    
                }
            }


            public static void SpawnBetween(List<Particle> Particles, Vector2 Point1, Vector2 Point2)
            {
                float xDiff = Point1.X - Point2.X;
                float yDiff = Point1.Y - Point2.Y;

                float Distance = (float)Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
                float Angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                float AngleRadians = Angle * (float)(Math.PI / 180);

                int ParticleDistance = Game1.random.Next(0, (int)Distance);
                int X = (int)(Point1.X - (ParticleDistance * (float)Math.Cos(AngleRadians))) + Game1.random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);
                int Y = (int)(Point1.Y - (ParticleDistance * (float)Math.Sin(AngleRadians))) + Game1.random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);

                Particles.Add(new Particle(X, Y)
                {
                    HasDuration = false,
                    Duration = Game1.random.Next(DurationMin, DurationMax),

                    ParticleObj = new TeleportLine()
                    {
                        Length = Game1.random.Next(LengthMin, LengthMax),
                        Thickness = Game1.random.Next(ThicknessMin, ThicknessMax),
                        Direction = (Angle + Game1.random.Next(-AngleSpreadRange, AngleSpreadRange)) * (float)(Math.PI / 180),
                        Colour = Colours.First(),
                        Opacity = OpacityDefault,
                    }
                });
            }
        }
        public class Impact
        {
            public const int DurationDefault = 50;

            public const float DefaultRadius = 5;
            public const float RadiusSpread = 0.6F;

            public const float DefaultOpacity = 0.95F;
            public const float OpacityMultiplier = 0.98F;

            public const int DefaultFrequency = 10;

            public readonly static Color Colour = Color.White;
            public float Radius { get; set; }
            public float Opacity { get; set; }

            public Impact(float Scalar)
            {
                Radius = DefaultRadius * Scalar;
            }


            public static void EnactParticle(Particle Particle)
            {
                EnactSpread(Particle);
            }
            private static void EnactSpread(Particle Particle)
            {
                Impact Effect = (Impact)Particle.ParticleObj;

                Effect.Radius += RadiusSpread;
                Effect.Opacity *= OpacityMultiplier;
            }
        }

        //Entity Related
        public class RedCubeSegment
        {
            public const int DurationMin = 70;
            public const int DurationMax = 135;

            public const int Width = 22;
            public const int Height = 22;

            public float Velocity;
            public const float VelocityMin = 2;
            public const float VelocityMax = 4;
            public const float VelocityMultiplier = 0.95F;

            public float Angle;
            public static float AngleOffsetAllowance = 30;

            public readonly static Color Colour = Color.Red;

            public RedCubeSegment()
            {
                Velocity = (float)Game1.random.Next((int)(VelocityMin * 10), (int)(VelocityMax * 10)) / 10;

                if (Game1.random.Next(0, 4) == 0)
                {
                    Velocity = (float)Game1.random.Next((int)((VelocityMin - 1) * 10), (int)(VelocityMin * 10)) / 10;
                }
            }


            public static void EnactParticle(Particle Particle)
            {
                EnactMovement(Particle);
            }
            private static void EnactMovement(Particle Particle)
            {
                RedCubeSegment Segment = (RedCubeSegment)Particle.ParticleObj;

                Particle.X = Particle.X + Segment.Velocity * (float)Math.Cos(Segment.Angle);
                Particle.Y = Particle.Y + Segment.Velocity * (float)Math.Sin(Segment.Angle);

                Segment.Velocity *= RedCubeSegment.VelocityMultiplier;
            }
        }

        //Aesthetics
        public class Dust
        {
            public const int SpawnInterval = 1;
            public const int MaxSpawnCount = 5;
            public const int DurationDefault = -1;
            public const float OpacityDefault = 0;
            public const float OpacityChange = 0.025F;
            public const int OpacityChangeInterval = 5;

            public int Width;
            public int Height;

            public Color Colour;
            public float Opacity;

            public bool OpacityIncreasing = true;


            public static void EnactParticle(Particle Particle, uint Tick)
            {
                EnactOpacityChange(Particle, Tick);
            }
            private static void EnactOpacityChange(Particle Particle, uint Tick)
            {
                if (Tick % OpacityChangeInterval == 0)
                {
                    Dust Effect = (Dust)Particle.ParticleObj;

                    if (Effect.OpacityIncreasing)
                    {
                        Effect.Opacity += Particle.Dust.OpacityChange;
                        if (Effect.Opacity >= 1)
                        {
                            Effect.OpacityIncreasing = false;
                        }
                    }
                    else
                    {
                        Effect.Opacity -= Particle.Dust.OpacityChange;
                        if (Effect.Opacity <= 0)
                        {
                            Particle.Duration = 1;
                            Particle.HasDuration = true;
                        }
                    }
                }
            }


            public static void Spawn(List<Particle> Particles, Vector2 PlayerPos, int RangeX, int RangeY, uint Tick)
            {
                if (Tick % Dust.SpawnInterval == 0)
                {
                    for (int i = 0; i < Game1.random.Next(0, MaxSpawnCount); i++)
                    {
                        int NewRangeX = RangeX * 2;
                        int NewRangeY = RangeY * 2;

                        float Opacity = OpacityDefault;

                        object DustObj = Dust.GetRandom(Opacity);
                        int DustX = Game1.random.Next((int)PlayerPos.X - NewRangeX, (int)PlayerPos.X + NewRangeX);
                        int DustY = Game1.random.Next((int)PlayerPos.Y - NewRangeY, (int)PlayerPos.Y + NewRangeY);

                        Particles.Add(new Particle(DustX, DustY)
                        {
                            HasDuration = false,
                            Duration = DurationDefault,

                            ParticleObj = DustObj
                        });
                    }
                }
            }
            public static Dust GetRandom(float Opacity)
                //Gets a random dust instance
            {
                const int ChanceRange = 105;
                const int RedChance = 35;
                const int PrupleChance = 2;



                int TypeVal = Game1.random.Next(0, ChanceRange);

                if (TypeVal < PrupleChance)
                {
                    //Gold Dust
                    return new Dust()
                    {
                        Width = PrupleDust.Width,
                        Height = PrupleDust.Height,
                        Colour = PrupleDust.Colour,
                        Opacity = Opacity
                    };
                }
                else if (TypeVal < RedChance)
                {
                    // Red Dust
                    return new Dust()
                    {
                        Width = RedDust.Width,
                        Height = RedDust.Height,
                        Colour = RedDust.Colour,
                        Opacity = Opacity
                    };
                }
                else
                {
                    // White Dust
                    return new Dust()
                    {
                        Width = WhiteDust.Width,
                        Height = WhiteDust.Height,
                        Colour = WhiteDust.Colour,
                        Opacity = Opacity
                    };
                }
            }



            public class WhiteDust
            {
                public const int Width = 10;
                public const int Height = 10;

                public static readonly Color Colour = Color.White;
            }
            public class RedDust
            {
                public const int Width = 8;
                public const int Height = 8;

                public static readonly Color Colour = Color.Red;
            }
            public class PrupleDust
            {
                public const int Width = 12;
                public const int Height = 12;

                public static readonly Color Colour = new Color(204, 51, 255);
            }
        }
    }
}
