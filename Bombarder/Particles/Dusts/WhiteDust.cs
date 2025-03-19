using Microsoft.Xna.Framework;

namespace Bombarder.Particles.Dusts;

public class WhiteDust : Dust
{
    public const int DefaultWidth = 10;
    public const int DefaultHeight = 10;

    public static readonly Color DefaultColour = Color.White;

    public WhiteDust(Vector2 Position) : base(Position)
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
        Colour = DefaultColour;
    }
}