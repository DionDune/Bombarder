using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.Entities;

public class EntityBlock
{
    public List<Texture2D> Textures { get; set; } = null;
    public Vector2 Offset { get; set; } = new(-33, -33);
    public int Width { get; set; } = 66;
    public int Height { get; set; } = 66;
    public Vector2 Size => new(Width, Height);

    public Color Color { get; set; } = Color.DarkRed;
}