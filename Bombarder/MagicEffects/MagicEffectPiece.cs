using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class MagicEffectPiece
{
    public int LifeSpan { get; set; } = 150;
    public Color Color { get; set; } = Color.Turquoise;

    public string BaseShape { get; set; } = "Circle";

    public Point Offset { get; set; } = new(-25, -25);
    public int Width { get; set; } = 50;
    public int Height { get; set; } = 50;
}