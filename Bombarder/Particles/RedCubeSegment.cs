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
        Duration = BombarderGame.random.Next(DurationMin, DurationMax);
        this.Angle = Angle;

        Velocity = BombarderGame.random.Next((int)(VelocityMin * 10), (int)(VelocityMax * 10)) / 10F;

        if (BombarderGame.random.Next(0, 4) == 0)
        {
            Velocity = BombarderGame.random.Next((int)((VelocityMin - 1) * 10), (int)(VelocityMin * 10)) / 10F;
        }
    }

    public override void Update(uint Tick)
    {
        EnactMovement();
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
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
            Colour
        );
    }

    private void EnactMovement()
    {
        Position += new Vector2(MathF.Cos(Angle), MathF.Sin(Angle)) * Velocity;

        Velocity *= VelocityMultiplier;
    }
}