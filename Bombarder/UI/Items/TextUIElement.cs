using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI.Items;

public class TextUIElement : UIItem
{
    public override void Draw(
        SpriteBatch SpriteBatch,
        GraphicsDeviceManager Graphics,
        Textures Textures,
        Vector2 Offset)
    {
        if (Text == null)
        {
            return;
        }

        Vector2 OffsetCentre = Offset + Centre;

        RenderTextElements(SpriteBatch, Textures, Text.Elements, OffsetCentre, Text.ElementSize, Text.Color);
    }
}