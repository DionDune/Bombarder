using Microsoft.Xna.Framework;

namespace Bombarder.Particles.Dusts;

public class PurpleDust : Dust
{
    public const int DefaultWidth = 12;
    public const int DefaultHeight = 12;

    public static readonly Color DefaultColour = new(204, 51, 255);

    public PurpleDust(Vector2 Position) : base(Position)
    {
        Width = DefaultWidth;
        Height = DefaultHeight;
        Colour = DefaultColour;
    }
}