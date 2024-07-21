using System;
using System.Collections.Generic;
using Bombarder.UI.Pages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI;

public abstract class UIPage
{
    public string Type { get; set; } = "Default";

    public List<UIItem> UIItems { get; set; } = new();

    protected UIPage()
    {
        SetupUIItems();
    }

    protected abstract void SetupUIItems();

    public static List<UIPage> GeneratePages()
    {
        List<UIPage> Pages = new List<UIPage>
        {
            // Start Page
            new StartPage(),
            // Pause Page
            new PausePage(),
            // Settings Page
            new SettingsPage(),
            // Death Screen
            new DeathPage(),
            // InGame
            new PlayPage(),
        };

        return Pages;
    }


    public void RenderElements(SpriteBatch SpriteBatch, GraphicsDeviceManager Graphics, Textures Textures)
    {
        foreach (UIItem Item in UIItems)
        {
            int OrientatePosX = Graphics.PreferredBackBufferWidth / 2;
            int OrientatePosY = Graphics.PreferredBackBufferHeight / 2;
            switch (Item.Orientation)
            {
                case "Bottom Left":
                    OrientatePosX = 0;
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
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
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    OrientatePosY = 0;
                    break;
                case "Right":
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    break;
                case "Bottom Right":
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
                    break;
                case "Bottom":
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
                    break;
            }

            Vector2 Offset = new Vector2(OrientatePosX, OrientatePosY);

            Item.Draw(SpriteBatch, Graphics, Textures, Offset);
        }
    }

    public static void RenderOutline(
        SpriteBatch _spriteBatch,
        Texture2D Texture,
        Color color,
        int X,
        int Y,
        int Width,
        int Height,
        int BorderWidth,
        float BorderTransparency)
    {
        _spriteBatch.Draw(
            Texture,
            new Rectangle(X, Y, Width, BorderWidth),
            color * BorderTransparency
        );
        _spriteBatch.Draw(
            Texture,
            new Rectangle(X + Width - BorderWidth, Y + BorderWidth, BorderWidth, Height - BorderWidth),
            color * BorderTransparency
        );
        _spriteBatch.Draw(
            Texture,
            new Rectangle(X, Y + Height - BorderWidth, Width - BorderWidth, BorderWidth),
            color * BorderTransparency
        );
        _spriteBatch.Draw(
            Texture,
            new Rectangle(X, Y + BorderWidth, BorderWidth, Height - BorderWidth * 2),
            color * BorderTransparency
        );
    }
}