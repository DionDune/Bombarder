using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.Entities;

public class EntityBlock
{
    public List<Texture2D> Textures { get; set; }
    public Vector2 Offset { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Color Color { get; set; }

    public EntityBlock()
    {
        Width = 66;
        Height = 66;
        Offset = new Vector2(-33, -33);

        Color = Color.DarkRed;

        Textures = null;
    }
}