using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class Impact : Particle
{
    public const int DurationDefault = 50;

    public const float DefaultRadius = 5;
    public const float RadiusSpread = 0.6F;

    public const float DefaultOpacity = 0.95F;
    public const float OpacityMultiplier = 0.98F;

    public const int DefaultFrequency = 10;

    public static readonly Color Colour = Color.White;
    public float Radius { get; set; }
    public float Opacity { get; set; }

    public Impact(Vector2 Position) : base(Position)
    {
        Duration = DurationDefault;
        Radius = DefaultRadius;
        Opacity = DefaultOpacity;
        DrawLater = true;
    }

    public override void EnactParticle(uint Tick)
    {
        EnactSpread();
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.WhiteCircle,
            new Rectangle(
                (int)(
                    Position.X -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                (int)Radius * 2,
                (int)Radius * 2
            ),
            Colour * Opacity
        );
    }

    private void EnactSpread()
    {
        Radius += RadiusSpread;
        Opacity *= OpacityMultiplier;
    }
}