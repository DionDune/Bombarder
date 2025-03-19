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


    public void RenderElements(Textures Textures)
    {
        foreach (UIItem Item in UIItems)
        {
            Vector2 Offset = Item.Orientation.ToPosition();

            Item.Draw(Textures, Offset);
        }
    }
}