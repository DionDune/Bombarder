using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder
{
    internal class TextElement
    {
        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public string Text { get; set; }

        public List<List<bool>> Elements { get; set; }
        public int ElementSize { get; set; }

        public Color Color { get; set; }
        public Color BackgroundColor { get; set; }

        public bool hasBackground { get; set; }

        public TextElement()
        {
            XOffset = 0;
            YOffset = 0;

            Text = "EXAMPLE";
            Elements = GetString(Text);
            ElementSize = 5;

            Color = Color.Black;
            BackgroundColor = Color.White;

            hasBackground = false;
        }



        public static List<List<bool>> GetLetter(char Character)
        {
            List<List<bool>> letter = new List<List<bool>>();

            if (Character == 'A')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'B')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
            }
            if (Character == 'C')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'D')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
            }
            if (Character == 'E')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'F')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
            }
            if (Character == 'G')
            {
                letter.Add(new List<bool>() { true, true, true, false });
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, true, true });
                letter.Add(new List<bool>() { true, false, false, true });
                letter.Add(new List<bool>() { true, true, true, true });
            }
            if (Character == 'H')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'I')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'J')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'K')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'L')
            {
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'M')
            {
                letter.Add(new List<bool>() { true, true, false, true, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
            }
            if (Character == 'N')
            {
                letter.Add(new List<bool>() { true, false, false, true });
                letter.Add(new List<bool>() { true, true, false, true });
                letter.Add(new List<bool>() { true, true, true, true });
                letter.Add(new List<bool>() { true, false, true, true });
                letter.Add(new List<bool>() { true, false, false, true });
            }
            if (Character == 'O')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'P')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, false, false });
            }
            if (Character == 'Q')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { false, true, true });
            }
            if (Character == 'R')
            {
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'S')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'T')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'U')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == 'V')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'W')
            {
                letter.Add(new List<bool>() { true, false, false, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, false, true, false, true });
                letter.Add(new List<bool>() { true, true, false, true, true });
            }
            if (Character == 'X')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
            }
            if (Character == 'Y')
            {
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == 'Z')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '1')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '2')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '3')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '4')
            {
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, false, false });
                letter.Add(new List<bool>() { true, false, true, false });
                letter.Add(new List<bool>() { true, true, true, true });
                letter.Add(new List<bool>() { false, false, true, false });
            }
            if (Character == '5')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '6')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, false });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '7')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
            }
            if (Character == '8')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
            }
            if (Character == '9')
            {
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, true, true });
                letter.Add(new List<bool>() { false, false, true });
                letter.Add(new List<bool>() { false, false, true });
            }
            if (Character == '0')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { true, false, true });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == ':')
            {
                letter.Add(new List<bool>() { false, false, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, false, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, false, false });
            }
            if (Character == ' ')
            {
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
            }
            if (Character == '!')
            {
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, true, false });
                letter.Add(new List<bool>() { false, false, false });
                letter.Add(new List<bool>() { false, true, false });
            }
            if (Character == '.')
            {
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { false });
                letter.Add(new List<bool>() { true });
            }
            return letter;
        }

        public static List<List<bool>> GetString(string Text)
        {
            List<List<bool>> Elements = new List<List<bool>>();
            foreach (List<bool> Lst in GetLetter('A'))
            {
                Elements.Add(new List<bool>());
            }

            foreach (char c in Text)
            {
                List<List<bool>> Element = GetLetter(c);

                for (int i = 0; i < Element.Count(); i++)
                {
                    foreach (bool b in Element[i])
                    {
                        Elements[i].Add(b);
                    }

                    if (Text.IndexOf(c) != Text.Length - 1)
                    {
                        Elements[i].Add(false);
                    }
                }

            }

            return Elements;
        }
    }

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


        public static Point GetOritentationPosition(GraphicsDeviceManager _graphics, string Orientation)
        {
            switch (Orientation)
            {
                case "Top Left":
                    return new Point(0, 0);
                case "Top":
                    return new Point(_graphics.PreferredBackBufferWidth / 2, 0);
                case "Top Right":
                    return new Point(_graphics.PreferredBackBufferWidth, 0);
                case "Middle Left":
                    return new Point(0, _graphics.PreferredBackBufferHeight / 2);
                case "Middle":
                    return new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
                case "Middle Right":
                    return new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
                case "Bottom Left":
                    return new Point(0, _graphics.PreferredBackBufferHeight);
                case "Bottom":
                    return new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight);
                case "Bottom Right":
                    return new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
                default:
                    return new Point(0, 0);
            }
        }
        public (Point, Point) getElementBounds(GraphicsDeviceManager _graphics)
        {
            Point OritentationPosition = GetOritentationPosition(_graphics, Orientation);
            Point TopLeft = new Point(OritentationPosition.X + X, OritentationPosition.Y + Y);
            Point BottomRight = new Point(TopLeft.X + Width, TopLeft.Y + Height);

            return (TopLeft, BottomRight);
        }
    }

    internal class UIPage
    {
        public string Type { get; set; }

        public List<UIItem> UIItems { get; set; }

        public UIPage()
        {
            Type = "Default";

            UIItems = new List<UIItem>();
        }

        public static List<UIPage> GeneratePages()
        {
            List<UIPage> Pages = new List<UIPage>();

            //Start Page
            Pages.Add(new UIPage()
            {
                Type = "Start",

                UIItems = new List<UIItem>()
                {
                    //Start Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = -75,
                        Width = 400,
                        Height = 150,
                        CentreX = -200 + (400 / 2),
                        CentreY = -75 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.Green,
                        BaseColor = Color.PaleGreen,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("START"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Start New" }
                    },
                    //Quit Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = -75 + 175,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = 100 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.DarkRed,
                        BaseColor = Color.Red,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("QUIT"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Quit" }
                    },
                    //Start Message
                    new UIItem()
                    {
                        Type = "Text",
                        X = 0,
                        Y = 0,
                        CentreX = 0,
                        CentreY = -250,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("BOMBARDER"),
                            ElementSize = 16,
                            Color = Color.White
                        }
                    }
                }
            });
            //Pause Page
            Pages.Add(new UIPage()
            {
                Type = "Pause",

                UIItems = new List<UIItem>()
                {
                    //Resume Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = -75,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = -75 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.Green,
                        BaseColor = Color.PaleGreen,

                        Text = new TextElement()
                        {
                            Text = "RESUME",
                            Elements = TextElement.GetString("RESUME"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Resume" }
                    },
                    //Settings Button
                    new UIItem()
                    {
                        Type = "Button",
                        X = -200,
                        Y = 100,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = 100 + (150 / 2),

                        BorderWidth = 10,
                        BorderColor = Color.Blue,
                        BaseColor = Color.Turquoise,

                        Text = new TextElement()
                        {
                            Text = "SETTINGS",
                            Elements = TextElement.GetString("SETTINGS"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Settings" }
                    },
                    //Pause Quit Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = 275,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = 275 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.DarkRed,
                        BaseColor = Color.Red,

                        Text = new TextElement()
                        {
                            Text = "QUIT",
                            Elements = TextElement.GetString("QUIT"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Quit" }
                    },
                    //Title Message
                    new UIItem()
                    {
                        Type = "Text",
                        X = 0,
                        Y = 0,
                        CentreX = 0,
                        CentreY = -250,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("BOMBARDER"),
                            ElementSize = 16,
                            Color = Color.White
                        }
                    }
                }
            });
            //Settings Page
            Pages.Add(new UIPage()
            {
                Type = "Settings",

                UIItems = new List<UIItem>()
                {
                    //Resume Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = -75,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = -75 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.Green,
                        BaseColor = Color.PaleGreen,

                        Text = new TextElement()
                        {
                            Text = "RESUME",
                            Elements = TextElement.GetString("RESUME"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Resume" }
                    },
                    //Player Invincibility Button
                    new UIItem()
                    {
                        Type = "Button",

                        Orientation = "Top Left",
                        X = 25,
                        Y = 250,
                        Width =  400,
                        Height = 150,
                        CentreX = 25 + 200,
                        CentreY = 250 + 75,

                        BorderWidth = 3,
                        BorderColor = Color.Purple,
                        BaseColor = Color.BlueViolet,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("INVINCIBLE"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "PLAYER INVINCIBLE" }
                    },
                    //Title Message
                    new UIItem()
                    {
                        Type = "Text",
                        CentreX = 0,
                        CentreY = 100,
                        Orientation = "Top",

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("SETTINGS"),
                            ElementSize = 16,
                            Color = Color.White
                        }
                    }
                }
            });

            //Death Screen
            Pages.Add(new UIPage()
            {
                Type = "Death",

                UIItems = new List<UIItem>()
                {
                    //Respawn Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = -75,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = -75 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.Green,
                        BaseColor = Color.PaleGreen,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("RESPAWN"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Respawn" }
                    },
                    //Resurrect Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = 100,

                        Width = 400,
                        Height = 150,

                        CentreX = -200 + (400 / 2),
                        CentreY = 100 + (150 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.Purple,
                        BaseColor = Color.BlueViolet,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("RESURRECT"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Resurrect" }
                    },
                    //Pause Quit Button
                    new UIItem()
                    {
                        Type = "Button",

                        X = -200,
                        Y = 275,

                        Width = 400,
                        Height = 100,

                        CentreX = -200 + (400 / 2),
                        CentreY = 275 + (100 / 2),

                        BorderWidth = 5,
                        BorderColor = Color.DarkRed,
                        BaseColor = Color.Red,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("QUIT"),
                            ElementSize = 8,
                            Color = Color.Black
                        },

                        Data = new List<string>() { "Quit" }
                    },
                    //Title Message
                    new UIItem()
                    {
                        Type = "Text",
                        X = 0,
                        Y = 0,
                        CentreX = 0,
                        CentreY = -250,

                        Text = new TextElement()
                        {
                            Elements = TextElement.GetString("YOU FUCKING DIED"),
                            ElementSize = 16,
                            Color = Color.White
                        }
                    }
                }
            });

            //InGame
            Pages.Add(new UIPage()
            {
                Type = "Play",

                UIItems = new List<UIItem>()
                {

                }
            });

            return Pages;
        }


        public static void RenderElements(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics, Textures Textures, List<UIItem> UIItems)
        {
            foreach (UIItem Item in UIItems)
            {
                int OrientatePosX = _graphics.PreferredBackBufferWidth / 2;
                int OrientatePosY = _graphics.PreferredBackBufferHeight / 2;
                switch (Item.Orientation)
                {
                    case "Bottom Left":
                        OrientatePosX = 0;
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                    case "Left":
                        OrientatePosX = 0;
                        break;
                    case "Top Left":
                        OrientatePosX = 0;
                        OrientatePosY = 0;
                        break;
                    case "Top":
                        OrientatePosY = 0;
                        break;
                    case "Top Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        OrientatePosY = 0;
                        break;
                    case "Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        break;
                    case "Bottom Right":
                        OrientatePosX = _graphics.PreferredBackBufferWidth;
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                    case "Bottom":
                        OrientatePosY = _graphics.PreferredBackBufferHeight;
                        break;
                }

                int X = OrientatePosX + Item.X;
                int Y = OrientatePosY + Item.Y;
                int CentreX = OrientatePosX + Item.CentreX;
                int CentreY = OrientatePosY + Item.CentreY;

                if (Item.Type == "Text")
                {
                    if (Item.Text != null)
                    {
                        RenderTextElements(_spriteBatch, Textures, Item.Text.Elements, CentreX, CentreY, Item.Text.ElementSize, Item.Text.Color);
                    }
                }
                if (Item.Type == "Button")
                {
                    _spriteBatch.Draw(Textures.White, new Rectangle(X, Y, Item.Width, Item.Height), Item.BorderColor);
                    if (!Item.Highlighted)
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2), Item.BaseColor);
                    }
                    else
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2), Item.HighlightedColor);
                    }

                    if (Item.Text != null)
                    {
                        RenderTextElements(_spriteBatch, Textures, Item.Text.Elements, CentreX, CentreY, Item.Text.ElementSize, Item.Text.Color);
                    }
                }
                if (Item.Type == "Fillbar")
                {
                    //Border
                    RenderOutline(_spriteBatch, Textures.White, Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
                    //Inner
                    _spriteBatch.Draw(Textures.White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2),
                                                                   Item.SubBorderColor * Item.SubBorderTransparency);
                    //Bar
                    _spriteBatch.Draw(Textures.White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   (int)((Item.Value - Item.MinValue) / (float)Item.MaxValue * (Item.Width - Item.BorderWidth * 2)),
                                                                   Item.Height - Item.BorderWidth * 2), Item.BaseColor * Item.BaseTransparency);
                }
                if (Item.Type == "Container")
                {
                    //Border
                    RenderOutline(_spriteBatch, Textures.White, Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
                    //Inner
                    _spriteBatch.Draw(Textures.White, new Rectangle(X + Item.BorderWidth, Y + Item.BorderWidth,
                                                                   Item.Width - Item.BorderWidth * 2, Item.Height - Item.BorderWidth * 2),
                                                                   Item.SubBorderColor * Item.SubBorderTransparency);
                    if (Item.uIItems.Count > 0)
                    {
                        foreach (UIItem InnerItem in Item.uIItems)
                        {
                            switch (InnerItem.Orientation)
                            {
                                case "Bottom Left":
                                    OrientatePosX = 0;
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                                case "Left":
                                    OrientatePosX = 0;
                                    break;
                                case "Top Left":
                                    OrientatePosX = 0;
                                    OrientatePosY = 0;
                                    break;
                                case "Top":
                                    OrientatePosY = 0;
                                    break;
                                case "Top Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    OrientatePosY = 0;
                                    break;
                                case "Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    break;
                                case "Bottom Right":
                                    OrientatePosX = _graphics.PreferredBackBufferWidth;
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                                case "Bottom":
                                    OrientatePosY = _graphics.PreferredBackBufferHeight;
                                    break;
                            }
                            X = OrientatePosX + InnerItem.X;
                            Y = OrientatePosY + InnerItem.Y;
                            CentreX = OrientatePosX + InnerItem.CentreX;
                            CentreY = OrientatePosY + InnerItem.CentreY;

                            if (InnerItem.Type == "Container Slot")
                            {
                                float BorderTransparency = InnerItem.BorderTransparency;
                                float SubBorderTransparency = InnerItem.SubBorderTransparency;
                                Color BorderColor = InnerItem.BorderColor;
                                Color SubBorderColor = InnerItem.SubBorderColor;
                                if (InnerItem.Highlighted)
                                {
                                    BorderTransparency = InnerItem.BorderHighlightedTransparency;
                                    SubBorderTransparency = InnerItem.SubBorderHighlightedTransparency;
                                    BorderColor = InnerItem.HighlightedBorderColor;
                                    SubBorderColor = InnerItem.HighlightedColor;
                                }


                                //Border
                                RenderOutline(_spriteBatch, Textures.White, BorderColor, X, Y, InnerItem.Width, InnerItem.Height, InnerItem.BorderWidth, BorderTransparency);
                                //Inner
                                _spriteBatch.Draw(Textures.White, new Rectangle(X + InnerItem.BorderWidth, Y + InnerItem.BorderWidth,
                                                                               InnerItem.Width - InnerItem.BorderWidth * 2, InnerItem.Height - InnerItem.BorderWidth * 2),
                                                                               SubBorderColor * SubBorderTransparency);

                                //Hotbar Item
                                if (Item.Data.Contains("Hotbar"))
                                {
                                    if (InnerItem.NumericalData[0] > 0)
                                    {
                                        _spriteBatch.Draw(Textures.White, new Rectangle(X + InnerItem.BorderWidth * 2, Y + InnerItem.BorderWidth * 2,
                                                                               InnerItem.Width - InnerItem.BorderWidth * 4, InnerItem.Height - InnerItem.BorderWidth * 4),
                                                                               Color.Purple);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void RenderTextElements(SpriteBatch _spriteBatch, Textures Textures, List<List<bool>> Elements, int CentreX, int CentreY, int elementSize, Color elementColor)
        {
            int StartX = CentreX - ((Elements[0].Count * elementSize) / 2);
            int StartY = CentreY - ((Elements.Count * elementSize) / 2);

            for (int y = 0; y < Elements.Count; y++)
            {
                for (int x = 0; x < Elements[0].Count; x++)
                {
                    if (Elements[y][x])
                    {
                        _spriteBatch.Draw(Textures.White, new Rectangle(StartX + (x * elementSize), StartY + (y * elementSize), elementSize, elementSize), elementColor);
                    }
                }
            }
        }
        public static void RenderOutline(SpriteBatch _spriteBatch, Texture2D Texture, Color color, int X, int Y, int Width, int Height, int BorderWidth, float BorderTransparency)
        {
            _spriteBatch.Draw(Texture, new Rectangle(X, Y, Width, BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Texture, new Rectangle(X + Width - BorderWidth, Y + BorderWidth, BorderWidth, Height - BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Texture, new Rectangle(X, Y + Height - BorderWidth, Width - BorderWidth, BorderWidth), color * BorderTransparency);
            _spriteBatch.Draw(Texture, new Rectangle(X, Y + BorderWidth, BorderWidth, Height - (BorderWidth * 2)), color * BorderTransparency);
        }
    }
}
