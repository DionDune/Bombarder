using Microsoft.Xna.Framework;
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

            Peices = new List<EntityBlock>() { new EntityBlock() };
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
            Width = 16;
            Height = 16;
            Offset = new Vector2(-8, -8);

            Color = Color.Purple;
        }
    }
}
