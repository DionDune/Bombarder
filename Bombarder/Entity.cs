using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Entity
    {
        public string Type { get; set; }

        public int Health { get; set; }
        public int HealthMax { get; set; }
        public bool IsDead { get; set; }

        public Point HealthBarDimentions { get; set; }
        public Point HealthBarOffset { get; set; }
        public bool HealthBarVisible { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public Point HitboxOffset { get; set; }
        public Point HitboxSize { get; set; }

        public float Direction { get; set; }
        public float BaseSpeed { get; set; }

        public bool ChasesPlayer { get; set; }

        public List<EntityBlock> Peices { get; set; }
        public List<Object> HitMarkers;
        public uint LastHitMakerFrame;

        public Entity()
        {
            Type = "Default";

            Health = 80;
            HealthMax = 100;
            IsDead = false;

            X = 0;
            Y = 0;
            HitboxOffset = new Point(-33, -33);
            HitboxSize = new Point(66, 66);

            HealthBarVisible = true;
            HealthBarDimentions = new Point(40, 10);
            HealthBarOffset = new Point(-20, -HitboxOffset.Y + 5);

            Direction = 0;
            BaseSpeed = 5;

            ChasesPlayer = true;

            Peices = new List<EntityBlock>() { new EntityBlock(), new EntityBlock() { Width = 56, Height = 56, Offset = new Vector2(-28, -28), Color = Color.Red } };
            HitMarkers = new List<Object>();
            LastHitMakerFrame = 0;
        }

        public void MoveTowards(Vector2 Goal)
        {
            float XDifference = X - Goal.X;
            float YDifference = Y - Goal.Y;
            float Angle = (float)(Math.Atan2(YDifference, XDifference));

            Direction = Angle;

            X -= BaseSpeed * (float)Math.Cos(Angle);
            Y -= BaseSpeed * (float)Math.Sin(Angle);
        }

        public void GiveDamage(int Damage)
        {
            Health -= Damage;

            if (Health <= 0)
            {
                IsDead = true;
            }

            ApplyHitMarker();
        }

        public void ApplyHitMarker()    
        {
            if (Math.Abs(Game1.GameTick - LastHitMakerFrame) > 20)
            {
                HitMarkers.Add(new Object(new Object.HitMarker(), Game1.random.Next((int)X + HitboxOffset.X, (int)X + HitboxOffset.X + HitboxSize.X),
                                                                 Game1.random.Next((int)Y + HitboxOffset.Y, (int)Y + HitboxOffset.Y + HitboxSize.Y)));

                LastHitMakerFrame = Game1.GameTick;
            }
        }
    }







    internal class EntityBlock
    {
        public List<Texture2D> Textures { get; set; }
        public Vector2 Offset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Color Color { get; set; }

        public EntityBlock()
        {
            Width = 66;
            Height = 66;
            Offset = new Vector2(-33, -33);

            Color = Color.DarkRed;

            Textures = null;
        }
    }
}
