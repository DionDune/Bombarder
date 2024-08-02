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

    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactSpread();
    }

    public override void Draw()
    {
        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(
            Game.Textures.WhiteCircle,
            MathUtils.CreateRectangle(
                Position - new Vector2(Radius) + Game.ScreenCenter - Game.Player.Position,
                new Vector2(Radius) * 2
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