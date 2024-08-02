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
            MathUtils.CreateRectangle(
                Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position,
                new Vector2(Width, Height)
            ),
            Color.White
        );
    }
}