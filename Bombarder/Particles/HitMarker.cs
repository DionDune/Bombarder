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

    public override void Draw(BombarderGame Game)
    {
        Game.SpriteBatch.Draw(
            Game.Textures.HitMarker,
            new Rectangle(
                (int)(Position.X + Game.Graphics.PreferredBackBufferWidth / 2F - Game.Player.Position.X),
                (int)(Position.Y + Game.Graphics.PreferredBackBufferHeight / 2F - Game.Player.Position.Y),
                Width,
                Height
            ),
            Color.White
        );
    }
}