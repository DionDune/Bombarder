using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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

    }
}
