using Microsoft.Xna.Framework;
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
        public int Health { get; set; }
        public int HealthMax { get; set; }

        public float X {  get; set; }
        public float Y { get; set; }

        public float Direction { get; set; }
        public float BaseSpeed { get; set; }

        public bool ChasesPlayer { get; set; }

        public List<EntityBlock> Peices { get; set; }

        public Entity()
        {
            Health = 80;
            HealthMax = 100;

            X = 0;
            Y = 0;

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

        public bool GiveDamage(int Damage)
        {
            Health -= Damage;

            if (Health <= 0)
            {
                return true;
            }
            return false;
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
