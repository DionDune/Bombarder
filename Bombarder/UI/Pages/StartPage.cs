using System.Collections.Generic;
using Bombarder.UI.Items;
using Microsoft.Xna.Framework;

namespace Bombarder.UI.Pages;

public class StartPage : UIPage
{
    protected override void SetupUIItems()
    {
        UIItems = new List<UIItem>
        {
            // Start Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, -75),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.Green,
                BaseColor = Color.PaleGreen,

                Text = new TextElement
                {
                    Elements = TextElement.GetString("START"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Start New" }
            },
            // Quit Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, 100),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.DarkRed,
                BaseColor = Color.Red,

                Text = new TextElement
                {
                    Elements = TextElement.GetString("QUIT"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Quit" }
            },
            // Start Message
            new TextUIElement
            {
                Position = new Vector2(0, -250),

                Text = new TextElement
                {
                    Elements = TextElement.GetString("BOMBARDER"),
                    ElementSize = 16,
                    Color = Color.White
                }
            }
        };
    }
}