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
            new ButtonUIElement(() => BombarderGame.Instance.ResumeGame())
            {
                Orientation = Orientation.BOTTOM,
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
            },
            // Player Invincibility Button
            new ButtonUIElement(() => BombarderGame.Instance.Player.ToggleInvincibility())
            {
                Orientation = Orientation.TOP_LEFT,
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
            },
            // Player Infinite Mana Button
            new ButtonUIElement(() => BombarderGame.Instance.Player.ToggleInfiniteMana())
            {
                Orientation = Orientation.TOP_LEFT,
                Position = new Vector2(450, 250),

                Width = 400,
                Height = 150,

                BorderWidth = 3,
                BorderColor = Color.Purple,
                BaseColor = Color.BlueViolet,

                Text = new TextElement
                {
                    Elements = TextElement.GetString("INFINITE MANA"),
                    ElementSize = 8,
                    Color = Color.Black
                },
            },
            // Title Message
            new TextUIElement
            {
                Orientation = Orientation.TOP,
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