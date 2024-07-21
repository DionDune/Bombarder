using System.Collections.Generic;
using Bombarder.UI.Items;
using Microsoft.Xna.Framework;

namespace Bombarder.UI.Pages;

public class PausePage : UIPage
{
    protected override void SetupUIItems()
    {
        UIItems = new List<UIItem>
        {
            // Resume Button
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
                    Text = "RESUME",
                    Elements = TextElement.GetString("RESUME"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Resume" }
            },
            // Settings Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, 100),

                Width = 400,
                Height = 150,

                BorderWidth = 10,
                BorderColor = Color.Blue,
                BaseColor = Color.Turquoise,

                Text = new TextElement
                {
                    Text = "SETTINGS",
                    Elements = TextElement.GetString("SETTINGS"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Settings" }
            },
            // Quit Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, 275),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.DarkRed,
                BaseColor = Color.Red,

                Text = new TextElement
                {
                    Text = "QUIT",
                    Elements = TextElement.GetString("QUIT"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Quit" }
            },
            // Title Message
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