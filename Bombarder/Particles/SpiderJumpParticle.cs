using System;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class SpiderJumpParticle : Particle
{
    public (int Min, int Max) DurationRange = (90, 155);

    public int Width;
    public static (int Min, int Max) WidthRange = (15, 22);

    public float Velocity;
    public static (int Min, int Max) VelocityRange = (4, 5);
    public const float VelocityMultiplier = 0.95F;

    public float MovementAngle;

    public float Opacity;
    public const float OpacityMultiplier = 0.98f;
    public const float OpacityDefault = 0.9f;

    public static readonly Color Colour = Color.White;

    public SpiderJumpParticle(Vector2 Position, float Angle) : base(Position)
    {
        Duration = RngUtils.Random.Next(DurationRange.Min, DurationRange.Max);
        this.MovementAngle = Angle;
        Width = RngUtils.Random.Next(WidthRange.Min, WidthRange.Max);
        Opacity = OpacityDefault;

        Velocity = RngUtils.Random.Next((int)(VelocityRange.Min * 10), (int)(VelocityRange.Max * 10)) / 10F;

        if (RngUtils.Random.Next(0, 4) == 0)
        {
            Velocity = RngUtils.Random.Next((int)((VelocityRange.Min - 1) * 10), (int)(VelocityRange.Max * 10)) / 10F;
        }
    }

    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactMovement();

        Opacity *= OpacityMultiplier;
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            MathUtils.CreateRectangle(
                Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position,
                new Vector2(Width, Width)
            ),
            Colour * Opacity
        );
    }

    private void EnactMovement()
    {
        Position += new Vector2(MathF.Cos(MovementAngle), MathF.Sin(MovementAngle)) * Velocity;

        Velocity *= VelocityMultiplier;
    }
}

