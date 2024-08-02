using Microsoft.Xna.Framework;

namespace Bombarder.Particles.Dusts;

public class RedDust : Dust
{
    public const int DefaultWidth = 8;
    public const int DefaultHeight = 8;

    public static readonly Color DefaultColour = Color.Red;

    public RedDust(Vector2 Position) : base(Position)
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
        Colour = DefaultColour;
    }
}