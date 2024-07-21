using System.Collections.Generic;
using Bombarder.UI.Pages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI;

public abstract class UIPage
{
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
            Vector2 Offset = Item.Orientation.ToPosition(Graphics);

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