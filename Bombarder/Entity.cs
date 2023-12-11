﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Entity
    {
        public float X {  get; set; }
        public float Y { get; set; }

        public float Direction { get; set; }
        public float BaseSpeed { get; set; }

        public bool ChasesPlayer;

        public List<EntityBlock> Peices { get; set; }

        public Entity()
        {
            X = 0;
            Y = 0;

            Direction = 0;
            BaseSpeed = 5;

            ChasesPlayer = true;

            Peices = new List<EntityBlock>() { new EntityBlock(), new EntityBlock() { Width = 56, Height = 56, Offset = new Vector2(-33, -33), Color = Color.Red } };
        }

        public void MoveTowards(Vector2 Goal)
        {
            float XDifference = X - Goal.X;
            float YDifference = Y - Goal.Y;
            float Angle = (float)(Math.Atan2(XDifference, YDifference) * 180F / Math.PI);
            float AngleR = Angle * (float)(Math.PI / 180);

            X += BaseSpeed * (float)Math.Cos(AngleR);
            Y += BaseSpeed * (float)Math.Sin(AngleR);
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
