using Microsoft.Xna.Framework.Graphics;

namespace Bombarder;

public class Textures
{
    public Texture2D White { get; set; }
    public Texture2D WhiteCircle { get; set; }
    public Texture2D HalfWhiteCirlce { get; set; }
    public Texture2D HollowCircle { get; set; }
    public Texture2D Cursor { get; set; }
    public (Texture2D, Texture2D) DemonEye { get; set; }

    public Texture2D HitMarker { get; set; }
}