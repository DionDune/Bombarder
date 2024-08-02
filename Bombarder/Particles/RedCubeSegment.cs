using System;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class RedCubeSegment : Particle
{
    public const int DurationMin = 70;
    public const int DurationMax = 135;

    public const int Width = 22;
    public const int Height = 22;

    public float Velocity;
    public const float VelocityMin = 2;
    public const float VelocityMax = 4;
    public const float VelocityMultiplier = 0.95F;

    public float Angle;
    public static float AngleOffsetAllowance = 30;

    public static readonly Color Colour = Color.Red;

    public RedCubeSegment(Vector2 Position, float Angle) : base(Position)
    {
        Duration = RngUtils.Random.Next(DurationMin, DurationMax);
        this.Angle = Angle;

        Velocity = RngUtils.Random.Next((int)(VelocityMin * 10), (int)(VelocityMax * 10)) / 10F;

        if (RngUtils.Random.Next(0, 4) == 0)
        {
            Velocity = RngUtils.Random.Next((int)((VelocityMin - 1) * 10), (int)(VelocityMin * 10)) / 10F;
        }
    }

    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactMovement();
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            MathUtils.CreateRectangle(
                Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position,
                new Vector2(Width, Height)
            ),
            Colour
        );
    }

    private void EnactMovement()
    {
        Position += new Vector2(MathF.Cos(Angle), MathF.Sin(Angle)) * Velocity;

        Velocity *= VelocityMultiplier;
    }
}