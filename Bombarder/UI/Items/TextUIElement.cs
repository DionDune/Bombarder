using Microsoft.Xna.Framework;

namespace Bombarder.UI.Items;

public class TextUIElement : UIItem
{
    public override void Draw(Textures Textures, Vector2 Offset)
    {
        if (Text == null)
        {
            return;
        }

        Vector2 OffsetCentre = Offset + Centre;

        RenderTextElements(Textures, Text.Elements, OffsetCentre, Text.ElementSize, Text.Color);
    }
}