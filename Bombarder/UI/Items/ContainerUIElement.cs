﻿using Microsoft.Xna.Framework;
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
            (int)OffsetPosition.X,
            (int)OffsetPosition.Y,
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
            int OrientatePosX = 0;
            int OrientatePosY = 0;

            switch (InnerItem.Orientation)
            {
                case "Bottom Left":
                    OrientatePosX = 0;
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
                    break;
                case "Left":
                    OrientatePosX = 0;
                    break;
                case "Top Left":
                    OrientatePosX = 0;
                    OrientatePosY = 0;
                    break;
                case "Top":
                    OrientatePosY = 0;
                    break;
                case "Top Right":
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    OrientatePosY = 0;
                    break;
                case "Right":
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    break;
                case "Bottom Right":
                    OrientatePosX = Graphics.PreferredBackBufferWidth;
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
                    break;
                case "Bottom":
                    OrientatePosY = Graphics.PreferredBackBufferHeight;
                    break;
            }

            Vector2 InnerPosition = new Vector2(OrientatePosX, OrientatePosY) + InnerItem.Position;

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
                (int)InnerPosition.X,
                (int)InnerPosition.Y,
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