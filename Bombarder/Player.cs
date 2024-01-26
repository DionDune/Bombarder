using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{
    public class Player
    {
        public float X { get; set; }
        public float Y { get; set; }

        public int Health { get; set; }
        public int HealthMax { get; set; }
        public Point HealthBarDimentions { get; set; }
        public Point HealthBarOffset { get; set; }
        public bool HealthBarVisible { get; set; }

        public bool ManaInfinite { get; set; }
        public const uint ManaRegainInterval = 8;
        public const int ManaRegainDefault = 5;
        public int ManaRegain = ManaRegainDefault;
        public int Mana { get; set; }
        public int ManaMax { get; set; }

        public Point ManaBarDimentions { get; set; }
        public string ManaBarScreenOrientation { get; set; }
        public Point ManaBarOffset { get; set; }
        public bool ManaBarHorizontalFill { get; set; }
        public bool ManaBarVisible { get; set; }

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

            Health = 130;
            HealthMax = 150;
            HealthBarDimentions = new Point(40, 10);
            HealthBarOffset = new Point(-20, 55);
            HealthBarVisible = true;

            ManaInfinite = true;
            Mana = 0;
            ManaMax = 400;
            ManaBarDimentions = new Point(25, 450);
            ManaBarScreenOrientation = "Bottom Left";
            ManaBarOffset = new Point(20, -20);
            ManaBarHorizontalFill = false;
            ManaBarVisible = true;

            Momentum_X = 0;
            Momentum_Y = 0;
            BaseSpeed = 5;
            BoostMultiplier = 1.85F;

            Acceleration = 0.45F;
            Slowdown = 0.75F;

            Width = 50;
            Height = 100;
        }

        public void Handler()
        {
            ManaHandler();
        }

        public void ManaHandler()
        {
            if (Mana < ManaMax && Game1.GameTick % ManaRegainInterval == 0)
            {
                if (ManaMax - Mana < ManaRegain)
                {
                    Mana = ManaMax;
                }
                else
                {
                    Mana += ManaRegain;
                }
            }
        }
        public bool CheckUseMana(int Cost)
        {
            if (ManaInfinite)
            {
                return true;
            }


            if (Mana > Cost)
            {
                Mana -= Cost;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
