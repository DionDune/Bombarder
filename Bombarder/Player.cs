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
        public float X { get; set; }
        public float Y { get; set; }

        public float Momentum_X { get; set; }
        public float Momentum_Y { get; set; }
        public float BaseSpeed { get; set; }
        public float BoostMultiplier { get; set; }
        public float Acceleration { get; set; }
        public float Slowdown { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Player()
        {
            X = 0;
            Y = 0;

            Momentum_X = 0;
            Momentum_Y = 0;
            BaseSpeed = 5;
            BoostMultiplier = 1.85F;

            Acceleration = 0.85F;
            Slowdown = 0.75F;

            Width = 50;
            Height = 100;
        }
    }
}
