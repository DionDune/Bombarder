using System;
using System.Collections.Generic;
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
            if (Obj.ToString() == "Bombarder.Object+HitMarker")
            {
                Type = HitMarker.Name;
                HasDuration = HitMarker.HasDuration;
                Duration = HitMarker.Duration;

                X = x;
                Y = y;
                Width = HitMarker.Width;
                Height = HitMarker.Height;
            }
        }

        public class HitMarker
        {
            public const string Name = "Hitmarker";

            public const int Width = 25;
            public const int Height = 25;

            public const bool HasDuration = true;
            public const int Duration = 50;

            public static void PurgeDead(Entity Entity)
            {
                List<Object> DeadMarkers = new List<Object>();

                foreach (Object Marker in Entity.HitMarkers)
                {
                    Marker.Duration--;
                    if (Marker.Duration <= 0)
                    {
                        DeadMarkers.Add(Marker);
                    }
                }

                foreach (Object DeadMarker in DeadMarkers)
                {
                    Entity.HitMarkers.Remove(DeadMarker);
                }
            }
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
}
