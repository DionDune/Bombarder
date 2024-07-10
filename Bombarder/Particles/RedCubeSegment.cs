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
        Duration = Game1.random.Next(DurationMin, DurationMax);
        this.Angle = Angle;

        Velocity = (float)Game1.random.Next((int)(VelocityMin * 10), (int)(VelocityMax * 10)) / 10;

        if (Game1.random.Next(0, 4) == 0)
        {
            Velocity = (float)Game1.random.Next((int)((VelocityMin - 1) * 10), (int)(VelocityMin * 10)) / 10;
        }
    }

    public override void EnactParticle(uint Tick)
    {
        EnactMovement();
    }

    public override void Draw(Game1 Game1)
    {
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(Position.X + Game1.Graphics.PreferredBackBufferWidth / 2F - Game1.Player.Position.X),
                (int)(Position.Y + Game1.Graphics.PreferredBackBufferHeight / 2F - Game1.Player.Position.Y),
                Width,
                Height
            ),
            Colour
        );
    }

    private void EnactMovement()
    {
        Position += new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)) * Velocity;

        Velocity *= VelocityMultiplier;
    }
}