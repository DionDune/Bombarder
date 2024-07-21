using System.Collections.Generic;
using Bombarder;
using Bombarder.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public List<string> Data { get; set; } = null;
    public List<int> NumericalData { get; set; } = null;


    public int FillDirection { get; set; } = 0;

    public TextElement Text { get; set; } = null;

    public List<UIItem> uIItems { get; set; } = new();
    public bool Visible { get; set; } = true;

    public void SetHighlight(bool State)
    {
        Highlighted = State;
    }

    public (Point, Point) GetElementBounds(GraphicsDeviceManager Graphics)
    {
        Point OrientationPosition = Orientation.ToPoint(Graphics);
        Point TopLeft = new Point(OrientationPosition.X + (int)Position.X, OrientationPosition.Y + (int)Position.Y);
        Point BottomRight = new Point(TopLeft.X + Width, TopLeft.Y + Height);

        return (TopLeft, BottomRight);
    }

    public abstract void Draw(
        SpriteBatch SpriteBatch,
        GraphicsDeviceManager Graphics,
        Textures Textures,
        Vector2 Offset
    );

    public static void RenderTextElements(
        SpriteBatch SpriteBatch,
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

                SpriteBatch.Draw(Textures.White,
                    new Rectangle(StartX + x * ElementSize, StartY + y * ElementSize, ElementSize, ElementSize),
                    ElementColor
                );
            }
        }
    }
}