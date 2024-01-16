using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder
{
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
                    //Pause Quit Button
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
                    RenderOutline(_spriteBatch, Textures, Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
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
                    RenderOutline(_spriteBatch, Textures, Item.BorderColor, X, Y, Item.Width, Item.Height, Item.BorderWidth, Item.BorderTransparency);
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
                                RenderOutline(_spriteBatch, Textures, BorderColor, X, Y, InnerItem.Width, InnerItem.Height, InnerItem.BorderWidth, BorderTransparency);
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
