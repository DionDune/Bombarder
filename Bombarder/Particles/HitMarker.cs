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

    public override void Draw(Game1 Game1)
    {
        Game1.SpriteBatch.Draw(
            Game1.Textures.HitMarker,
            new Rectangle(
                (int)(Position.X + Game1.Graphics.PreferredBackBufferWidth / 2F - Game1.Player.Position.X),
                (int)(Position.Y + Game1.Graphics.PreferredBackBufferHeight / 2F - Game1.Player.Position.Y),
                Width,
                Height
            ),
            Color.White
        );
    }
}