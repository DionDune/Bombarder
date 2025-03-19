using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles.Dusts;

public class Dust : Particle
{
    public const int SpawnInterval = 1;
    public const int MaxSpawnCount = 5;
    public const int DurationDefault = -1;
    public const float OpacityDefault = 0;
    public const float OpacityChange = 0.025F;
    public const int OpacityChangeInterval = 5;

    public int Width;
    public int Height;
    public Vector2 Size => new(Width, Height);

    public Color Colour;
    public float Opacity;

    public bool OpacityIncreasing = true;

    public Dust(Vector2 Position) : base(Position)
    {
        Opacity = OpacityDefault;
        HasDuration = false;
        Duration = DurationDefault;
    }


    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactOpacityChange(Tick);
    }

    public override void Draw()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            MathUtils.CreateRectangle(
                Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position,
                Size
            ),
            Colour * Opacity
        );
    }

    private void EnactOpacityChange(uint Tick)
    {
        if (Tick % OpacityChangeInterval != 0)
        {
            return;
        }

        if (OpacityIncreasing)
        {
            Opacity += OpacityChange;
            if (Opacity >= 1)
            {
                OpacityIncreasing = false;
            }
        }
        else
        {
            Opacity -= OpacityChange;
            if (Opacity > 0)
            {
                return;
            }

            Duration = 1;
            HasDuration = true;
        }
    }


    public static void Spawn(List<Particle> Particles, Vector2 PlayerPos, Vector2 Range, uint Tick)
    {
        if (Tick % SpawnInterval != 0) return;
        for (int i = 0; i < RngUtils.Random.Next(0, MaxSpawnCount); i++)
        {
            Vector2 NewRange = Range * 2;
            Vector2 DustPosition = RngUtils.GetRandomVector(PlayerPos - NewRange, PlayerPos + NewRange);

            Particles.Add(CreateRandomDust(DustPosition));
        }
    }

    public static Dust CreateRandomDust(Vector2 Position)
    {
        // Creates a random dust instance
        const int chanceRange = 105;
        const int redChance = 35;
        const int purpleChance = 2;

        int TypeVal = RngUtils.Random.Next(0, chanceRange);

        return TypeVal switch
        {
            < purpleChance => new PurpleDust(Position),
            < redChance => new RedDust(Position),
            _ => new WhiteDust(Position)
        };
    }
}