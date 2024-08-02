using System.Collections.Generic;
using Bombarder;
using Bombarder.UI;
using Microsoft.Xna.Framework;

public abstract class UIItem
{
    public bool Highlighted { get; set; }

    public Orientation Orientation { get; set; } = Orientation.CENTER;

    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Centre => Position + new Vector2(Width / 2F, Height / 2F);

    public int Width { get; set; } = 10;
    public int Height { get; set; } = 10;


    public int BorderWidth { get; set; } = 0;

    public Color BaseColor { get; set; } = Color.Purple;
    public Color BorderColor { get; set; } = Color.Black;
    public Color SubBorderColor { get; set; } = Color.Purple;
    public Color HighlightedColor { get; set; } = Color.Gold;
    public Color HighlightedBorderColor { get; set; } = Color.DarkGoldenrod;

    public float BorderTransparency { get; set; } = 1F;
    public float SubBorderTransparency { get; set; }
    public float BaseTransparency { get; set; } = 1F;
    public float BorderHighlightedTransparency { get; set; } = 1F;
    public float SubBorderHighlightedTransparency { get; set; } = 1F;

    public int MinValue { get; set; } = 0;
    public int MaxValue { get; set; } = 1;
    public int Value { get; set; } = 0;

    public List<int> NumericalData { get; set; } = null;


    public int FillDirection { get; set; } = 0;

    public TextElement Text { get; set; } = null;

    public List<UIItem> uIItems { get; set; } = new();
    public bool Visible { get; set; } = true;

    public void SetHighlight(bool State)
    {
        Highlighted = State;
    }

    public Rectangle GetElementBounds()
    {
        Point OrientationPosition = Orientation.ToPoint();
        Point StartPosition = OrientationPosition + Position.ToPoint();

        return new Rectangle(StartPosition, new Point(Width, Height));
    }

    public bool IsMouseOver()
    {
        var Game = BombarderGame.Instance;
        Rectangle ElementBounds = GetElementBounds();
        var MousePosition = Game.MouseInput.Position;
        return ElementBounds.Contains(MousePosition);
    }

    public virtual void Click()
    {
    }

    public abstract void Draw(Textures Textures, Vector2 Offset);

    public void Update()
    {
        SetHighlight(IsMouseOver());

        if (BombarderGame.Instance.MouseInput.HasJustPressed(MouseButtons.Left) && IsMouseOver())
        {
            Click();
        }
    }

    public static void RenderTextElements(
        Textures Textures,
        List<List<bool>> Elements,
        Vector2 Centre,
        int ElementSize,
        Color ElementColor)
    {
        int StartX = (int)Centre.X - (Elements[0].Count * ElementSize) / 2;
        int StartY = (int)Centre.Y - (Elements.Count * ElementSize) / 2;

        for (int y = 0; y < Elements.Count; y++)
        {
            for (int x = 0; x < Elements[0].Count; x++)
            {
                if (!Elements[y][x])
                {
                    continue;
                }

                BombarderGame.Instance.SpriteBatch.Draw(
                    Textures.White,
                    new Rectangle(StartX + x * ElementSize, StartY + y * ElementSize, ElementSize, ElementSize),
                    ElementColor
                );
            }
        }
    }
}