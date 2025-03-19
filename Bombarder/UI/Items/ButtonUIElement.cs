using System;
using Microsoft.Xna.Framework;

namespace Bombarder.UI.Items;

public class ButtonUIElement : UIItem
{
    public Action[] Functions { get; set; }

    public ButtonUIElement(params Action[] Functions)
    {
        this.Functions = Functions;
    }

    public override void Click()
    {
        foreach (Action Function in Functions)
        {
            Function();
        }
    }

    public override void Draw(Textures Textures, Vector2 Offset)
    {
        Vector2 OffsetPosition = Offset + Position;
        Vector2 OffsetCentre = Offset + Centre;

        var SpriteBatch = BombarderGame.Instance.SpriteBatch;

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
            RenderTextElements(Textures, Text.Elements, OffsetCentre, Text.ElementSize, Text.Color);
        }
    }
}