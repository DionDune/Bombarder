﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Bombarder.MagicEffect;

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

        public int X { get; set; }
        public int Y { get; set; }

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
        public static void EnactParticles(List<Particle> Particles)
        {
            foreach (Particle particle in Particles)
            {
                string ParticleType = particle.ParticleObj.ToString();
                switch (ParticleType)
                {
                    case "Bombarder.Particle+LazerLine":
                        LazerLine.EnactParticle(particle);
                        break;
                    case "Bombarder.Particle+Impact":
                        Impact.EnactParticle(particle);
                        break;
                }
            }
        }
        public static void SpawnParticles(List<Particle> Particles, Vector2 PlayerPos, GraphicsDeviceManager Graphics, uint Tick)
        {
            int ClutterSpawnRangeX = Graphics.PreferredBackBufferWidth;
            int ClutterSpawnRangeY = Graphics.PreferredBackBufferHeight;


            if (Tick % Dust.SpawnInterval  == 0)
            {
                object DustObj = Dust.GetRandom();
                int DustX = Game1.random.Next((int)PlayerPos.X - ClutterSpawnRangeX, (int)PlayerPos.X + ClutterSpawnRangeX);
                int DustY = Game1.random.Next((int)PlayerPos.Y - ClutterSpawnRangeY, (int)PlayerPos.Y + ClutterSpawnRangeY);

                Particles.Add(new Particle(DustX, DustY)
                {
                    HasDuration = true,
                    Duration = Game1.random.Next(100, 1000),

                    ParticleObj = DustObj
                });
            }
        }


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

        public class Dust
        {
            public const int SpawnInterval = 30;


            public static object GetRandom()
                //Gets a random dust instance
            {
                const int ChanceRange = 100;
                const int RedChance = 30;

                int TypeVal = Game1.random.Next(0, ChanceRange);
                if (TypeVal < RedChance)
                {
                    return new RedDust();
                }
                else
                {
                    return new WhiteDust();
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
        }
    }
}
