using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.UI.Items;

public class ContainerUIElement : UIItem
{
    public override void Draw(SpriteBatch SpriteBatch, GraphicsDeviceManager Graphics, Textures Textures,
        Vector2 Offset)
    {
        Vector2 OffsetPosition = Offset + Position;

        //Border
        UIPage.RenderOutline(
            SpriteBatch,
            Textures.White,
            BorderColor,
            OffsetPosition.ToPoint(),
            Width,
            Height,
            BorderWidth,
            BorderTransparency
        );
        //Inner
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

        if (uIItems.Count <= 0)
        {
            return;
        }

        foreach (UIItem InnerItem in uIItems)
        {
            Vector2 InnerPosition = InnerItem.Orientation.ToPosition(Graphics) + InnerItem.Position;

            if (InnerItem is ContainerSlotUIElement)
            {
                continue;
            }

            float InnerBorderTransparency = InnerItem.BorderTransparency;
            float InnerSubBorderTransparency = InnerItem.SubBorderTransparency;
            Color InnerBorderColor = InnerItem.BorderColor;
            Color InnerSubBorderColor = InnerItem.SubBorderColor;
            if (InnerItem.Highlighted)
            {
                InnerBorderTransparency = InnerItem.BorderHighlightedTransparency;
                InnerSubBorderTransparency = InnerItem.SubBorderHighlightedTransparency;
                InnerBorderColor = InnerItem.HighlightedBorderColor;
                InnerSubBorderColor = InnerItem.HighlightedColor;
            }


            // Border
            UIPage.RenderOutline(
                SpriteBatch,
                Textures.White,
                InnerBorderColor,
                InnerPosition.ToPoint(),
                InnerItem.Width,
                InnerItem.Height,
                InnerItem.BorderWidth,
                InnerBorderTransparency
            );
            // Inner
            SpriteBatch.Draw(
                Textures.White,
                new Rectangle(
                    (int)InnerPosition.X + InnerItem.BorderWidth,
                    (int)InnerPosition.Y + InnerItem.BorderWidth,
                    InnerItem.Width - InnerItem.BorderWidth * 2,
                    InnerItem.Height - InnerItem.BorderWidth * 2
                ),
                InnerSubBorderColor * InnerSubBorderTransparency
            );

            // Hotbar Item
            if (!Data.Contains("Hotbar") || InnerItem.NumericalData[0] <= 0)
            {
                continue;
            }

            SpriteBatch.Draw(
                Textures.White,
                new Rectangle(
                    (int)InnerPosition.X + InnerItem.BorderWidth * 2,
                    (int)InnerPosition.Y + InnerItem.BorderWidth * 2,
                    InnerItem.Width - InnerItem.BorderWidth * 4,
                    InnerItem.Height - InnerItem.BorderWidth * 4
                ),
                Color.Purple
            );
        }
    }
}