using System.Collections.Generic;
using Bombarder.UI.Items;
using Microsoft.Xna.Framework;

namespace Bombarder.UI.Pages;

public class SettingsPage : UIPage
{
    protected override void SetupUIItems()
    {
        UIItems = new List<UIItem>
        {
            // Resume Button
            new ButtonUIElement
            {
                Orientation = "Bottom",
                Position = new Vector2(-300, -225),
                Width = 600,
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
            // Player Invincibility Button
            new ButtonUIElement
            {
                Orientation = "Top Left",
                Position = new Vector2(25, 250),

                Width = 400,
                Height = 150,

                BorderWidth = 3,
                BorderColor = Color.Purple,
                BaseColor = Color.BlueViolet,

                Text = new TextElement
                {
                    Elements = TextElement.GetString("INVINCIBLE"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "PLAYER INVINCIBLE" }
            },
            // Title Message
            new TextUIElement
            {
                Orientation = "Top",
                Position = new Vector2(0, 100),

                Text = new TextElement
                {
                    Elements = TextElement.GetString("SETTINGS"),
                    ElementSize = 16,
                    Color = Color.White
                }
            }
        };
    }
}