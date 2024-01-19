using System;
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
        public static bool EnactParticleDuration(Particle particle)
        {
            if (particle.Duration <= 0)
            {
                return true;
            }
            else
            {
                particle.Duration--;
                return false;
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
            public const int DurationMin = 25;
            public const int DurationMax = 200;
            public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
                (new List<Color> { Color.Turquoise, Color.DarkTurquoise, Color.MediumTurquoise, Color.MediumTurquoise});


            public float Length { get; set; }
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
        }
    }
}
