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

        public List<EntityBlock> Peices { get; set; }
    }
    internal class EntityBlock
    {
        public Vector2 Offset { get; set; }
        public Color Color { get; set; }
    }
}
