using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI.Items;

public class ButtonUIElement : UIItem
{
    public override void Draw(
        SpriteBatch SpriteBatch,
        GraphicsDeviceManager Graphics,
        Textures Textures,
        Vector2 Offset)
    {
        Vector2 OffsetPosition = Offset + Position;
        Vector2 OffsetCentre = Offset + Centre;

        SpriteBatch.Draw(
            Textures.White,
            new Rectangle((int)OffsetPosition.X, (int)OffsetPosition.Y, Width, Height),
            BorderColor
        );
        SpriteBatch.Draw(
            Textures.White,
            new Rectangle(
                (int)OffsetPosition.X + BorderWidth,
                (int)OffsetPosition.Y + BorderWidth,
                Width - BorderWidth * 2,
                Height - BorderWidth * 2
            ),
            !Highlighted ? BaseColor : HighlightedColor
        );

        if (Text != null)
        {
            RenderTextElements(SpriteBatch, Textures, Text.Elements, OffsetCentre, Text.ElementSize, Text.Color);
        }
    }
}