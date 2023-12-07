using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Bombarder
{
    internal class UIItem
    {
        public string Type { get; set; }
        public bool Highlighted { get; set; }

        public string Orientation { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int CentreX { get; set; }
        public int CentreY { get; set; }

        public int BorderWidth { get; set; }

        public Color BaseColor { get; set; }
        public Color BorderColor { get; set; }
        public Color SubBorderColor { get; set; }
        public Color HighlightedColor { get; set; }
        public Color HighlightedBorderColor { get; set; }

        public float BorderTransparency { get; set; }
        public float SubBorderTransparency { get; set; }
        public float BaseTransparency { get; set; }
        public float BorderHighlightedTransparency { get; set; }
        public float SubBorderHighlightedTransparency { get; set; }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; set; }

        public List<string> Data { get; set; }
        public List<int> NumericalData { get; set; }


        public int FillDirection { get; set; }

        public TextElement Text { get; set; }

        public List<UIItem> uIItems { get; set; }
        public bool Visible { get; set; }

        public UIItem()
        {
            Type = "Button";
            Highlighted = false;

            Orientation = "Centre";

            X = 0;
            Y = 0;

            Width = 10;
            Height = 10;

            CentreX = 5;
            CentreY = 5;

            BorderWidth = 0;

            BaseColor = Color.Purple;
            BorderColor = Color.Black;
            SubBorderColor = Color.Purple;
            HighlightedColor = Color.Gold;
            HighlightedBorderColor = Color.DarkGoldenrod;

            BorderTransparency = 1F;
            BaseTransparency = 1F;
            BorderHighlightedTransparency = 1F;
            SubBorderHighlightedTransparency = 1F;

            MinValue = 0;
            MaxValue = 1;
            Value = 0;
            FillDirection = 0;

            Data = null;
            NumericalData = null;


            Text = null;

            uIItems = new List<UIItem>();
            Visible = true;
        }

        public void SetHighlight(bool State)
        {
            Highlighted = State;
        }
    }
}
