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
        public List<Texture2D> Textures { get; set; }

        public int Health { get; set; }
        public int HealthMax { get; set; }
        public bool IsDead { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public Point HitboxOffset { get; set; }
        public Point HitboxSize { get; set; }

        public float Direction { get; set; }
        public float BaseSpeed { get; set; }

        public bool ChasesPlayer { get; set; }

        public List<EntityBlock> Peices { get; set; }

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

            Direction = 0;
            BaseSpeed = 5;

            ChasesPlayer = true;

            Peices = new List<EntityBlock>() { new EntityBlock(), new EntityBlock() { Width = 56, Height = 56, Offset = new Vector2(-28, -28), Color = Color.Red } };
        }

        public void MoveTowards(Vector2 Goal)
        {
            float XDifference = X - Goal.X;
            float YDifference = Y - Goal.Y;
            float Angle = (float)(Math.Atan2(YDifference, XDifference));

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
        }

        public void CheckMagicOverlap(MagicEffect Effect)
        {
            Vector2 HitboxStart = new Vector2(X + HitboxOffset.X, Y + HitboxOffset.Y);

            //Check if the centre is within the effect
            float Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(HitboxStart.X + (HitboxSize.X / 2) - Effect.X), 2) +
                                                       Math.Pow(Math.Abs(HitboxStart.Y + (HitboxSize.Y / 2) - Effect.Y), 2));
            if (Distance <= Effect.DamageRadius)
            {
                GiveDamage(Effect.Damage);
                return;
            }

            //If hitbox is encumpasing the effect
            if (HitboxSize.X > Effect.DamageRadius * 2 || HitboxSize.Y > Effect.DamageRadius * 2)
            {
                float XDifference = Effect.X - X;
                float YDifference = Effect.Y - Y;
                float Angle = (float)(Math.Atan2(YDifference, XDifference));
                Vector2 ClosestPoint = new Vector2(Effect.DamageRadius * (float)Math.Cos(Angle), Effect.DamageRadius * (float)Math.Sin(Angle));

                if (ClosestPoint.X > HitboxStart.X && ClosestPoint.X < HitboxStart.X + HitboxSize.X &&
                    ClosestPoint.Y > HitboxStart.Y && ClosestPoint.Y < HitboxStart.Y + HitboxSize.Y)
                {
                    GiveDamage(Effect.Damage);
                    return;
                }
            }

            //Check if any points are within the effect
            //Top Left
            Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(HitboxStart.X - Effect.X), 2) +
                                                    Math.Pow(Math.Abs(HitboxStart.Y - Effect.Y), 2));
            if (Distance <= Effect.DamageRadius)
            {
                GiveDamage(Effect.Damage);
                return;
            }
            //Top Right
            Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(HitboxStart.X + HitboxSize.X - Effect.X), 2) +
                                                    Math.Pow(Math.Abs(HitboxStart.Y - Effect.Y), 2));
            if (Distance <= Effect.DamageRadius)
            {
                GiveDamage(Effect.Damage);
                return;
            }

            //Bottom Left
            Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(HitboxStart.X - Effect.X), 2) +
                                                    Math.Pow(Math.Abs(HitboxStart.Y + HitboxSize.Y - Effect.Y), 2));
            if (Distance <= Effect.DamageRadius)
            {
                GiveDamage(Effect.Damage);
                return;
            }

            //Bottom Right
            Distance = (float)Math.Sqrt(Math.Pow(Math.Abs(HitboxStart.X + HitboxSize.X - Effect.X), 2) +
                                                    Math.Pow(Math.Abs(HitboxStart.Y + HitboxSize.Y - Effect.Y), 2));
            if (Distance <= Effect.DamageRadius)
            {
                GiveDamage(Effect.Damage);
                return;
            }
            
        }
    }

    internal class EntityBlock
    {
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
        }
    }
}
