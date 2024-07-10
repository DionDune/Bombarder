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

    public Color Colour;
    public float Opacity;

    public bool OpacityIncreasing = true;

    public Dust(Vector2 Position) : base(Position)
    {
        Opacity = OpacityDefault;
        HasDuration = false;
        Duration = DurationDefault;
    }


    public override void EnactParticle(uint Tick)
    {
        EnactOpacityChange(Tick);
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


    public static void Spawn(List<Particle> Particles, Vector2 PlayerPos, int RangeX, int RangeY, uint Tick)
    {
        if (Tick % SpawnInterval != 0) return;
        for (int i = 0; i < Game1.random.Next(0, MaxSpawnCount); i++)
        {
            int NewRangeX = RangeX * 2;
            int NewRangeY = RangeY * 2;

            int DustX = Game1.random.Next((int)PlayerPos.X - NewRangeX, (int)PlayerPos.X + NewRangeX);
            int DustY = Game1.random.Next((int)PlayerPos.Y - NewRangeY, (int)PlayerPos.Y + NewRangeY);

            Particles.Add(GetRandom(new Vector2(DustX, DustY)));
        }
    }

    public static Dust GetRandom(Vector2 Position)
    {
        //Gets a random dust instance
        const int chanceRange = 105;
        const int redChance = 35;
        const int purpleChance = 2;

        int TypeVal = Game1.random.Next(0, chanceRange);

        return TypeVal switch
        {
            < purpleChance => new PurpleDust(Position),
            < redChance => new RedDust(Position),
            _ => new WhiteDust(Position)
        };
    }
}