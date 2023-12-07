using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    internal class Player
    {
        public Vector2 Position { get; set; }
        public Vector2 Momentum { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Player()
        {
            Position = new Vector2(0, 0);
            Momentum = new Vector2(0, 0);

            Width = 50;
            Height = 120;
        }
    }
}
