using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class HitMarker : Particle
{
    public const int Width = 30;
    public const int Height = 30;

    public const int DefaultDuration = 50;

    public HitMarker(Vector2 position) : base(position)
    {
        HasDuration = true;
        DrawLater = true;
        Duration = DefaultDuration;
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.HitMarker,
            new Rectangle(
                (int)(
                    Position.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                Width,
                Height
            ),
            Color.White
        );
    }
}