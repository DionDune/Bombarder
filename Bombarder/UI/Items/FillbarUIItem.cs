using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI.Items;

public class FillbarUIItem : UIItem
{
    public override void Draw(Textures Textures, Vector2 Offset)
    {
        Vector2 OffsetPosition = Offset + Position;
        var SpriteBatch = BombarderGame.Instance.SpriteBatch;
        
        //Border
        RenderUtils.RenderOutline(
            Textures.White,
            BorderColor,
            OffsetPosition.ToPoint(),
            Width,
            Height,
            BorderWidth,
            BorderTransparency
        );

        // Inner
        SpriteBatch.Draw(
            Textures.White,
            new Rectangle(
                (int)OffsetPosition.X + BorderWidth,
                (int)OffsetPosition.Y + BorderWidth,
                Width - BorderWidth * 2,
                Height - BorderWidth * 2
            ),
            SubBorderColor * SubBorderTransparency
        );

        // Bar
        SpriteBatch.Draw(
            Textures.White,
            new Rectangle(
                (int)OffsetPosition.X + BorderWidth,
                (int)OffsetPosition.Y + BorderWidth,
                (Value - MinValue) / MaxValue * (Width - BorderWidth * 2),
                Height - BorderWidth * 2
            ),
            BaseColor * BaseTransparency
        );
    }
}