using System.Collections.Generic;
using Bombarder.UI.Items;
using Microsoft.Xna.Framework;

namespace Bombarder.UI.Pages;

public class DeathPage : UIPage
{
    protected override void SetupUIItems()
    {
        UIItems = new List<UIItem>
        {
            // Respawn Button
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
                    Elements = TextElement.GetString("RESPAWN"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Respawn" }
            },
            // Resurrect Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, 100),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.Purple,
                BaseColor = Color.BlueViolet,

                Text = new TextElement
                {
                    Elements = TextElement.GetString("RESURRECT"),
                    ElementSize = 8,
                    Color = Color.Black
                },

                Data = new List<string> { "Resurrect" }
            },
            // Pause Quit Button
            new ButtonUIElement
            {
                Position = new Vector2(-200, 275),

                Width = 400,
                Height = 100,

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
            // Title Message
            new TextUIElement
            {
                Position = new Vector2(0, -250),

                Text = new TextElement
                {
                    Elements = TextElement.GetString("YOU FUCKING DIED"),
                    ElementSize = 16,
                    Color = Color.White
                }
            }
        };
    }
}