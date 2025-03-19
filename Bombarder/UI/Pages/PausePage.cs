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
            new ButtonUIElement(() => BombarderGame.Instance.ResumeGame())
            {
                Position = new Vector2(-200, -75),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.Green,
                BaseColor = Color.PaleGreen,

                Text = new TextElement("RESUME")
                {
                    ElementSize = 8,
                    Color = Color.Black
                },
            },
            // Settings Button
            new ButtonUIElement(() => BombarderGame.Instance.OpenSettings())
            {
                Position = new Vector2(-200, 100),

                Width = 400,
                Height = 150,

                BorderWidth = 10,
                BorderColor = Color.Blue,
                BaseColor = Color.Turquoise,

                Text = new TextElement("SETTINGS")
                {
                    ElementSize = 8,
                    Color = Color.Black
                },
            },
            // Quit Button
            new ButtonUIElement(() => BombarderGame.Instance.Exit())
            {
                Position = new Vector2(-200, 275),

                Width = 400,
                Height = 150,

                BorderWidth = 5,
                BorderColor = Color.DarkRed,
                BaseColor = Color.Red,

                Text = new TextElement("QUIT")
                {
                    ElementSize = 8,
                    Color = Color.Black
                },
            },
            // Title Message
            new TextUIElement
            {
                Position = new Vector2(0, -250),

                Text = new TextElement("BOMBARDER")
                {
                    ElementSize = 16,
                    Color = Color.White
                }
            }
        };
    }
}