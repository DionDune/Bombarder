using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class TeleportLine : Particle
{
    public const int LengthMin = 5;
    public const int LengthMax = 20;
    public const int ThicknessMin = 3;
    public const int ThicknessMax = 6;
    public const int RandomDistanceAllowance = 20;

    public const int DurationMin = 10;
    public const int DurationMax = 100;

    public const int AngleSpreadRange = 10;

    public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
        (new List<Color> { Color.Purple, Color.MediumPurple, Color.DarkMagenta });

    public const float OpacityDefault = 0;
    public const float OpacityIncreasingChange = 0.15F;
    public const int OpacityIncreaseInterval = 1;
    public const float OpacityDecreasingChange = 0.025F;
    public const int OpacityDecreasingInterval = 3;
    public bool OpacityIncreasing = true;


    public float Length { get; set; }
    public float Thickness { get; set; }
    public float Direction { get; set; }
    public Color Colour { get; set; }
    public float Opacity { get; set; }


    public TeleportLine(Vector2 Position) : base(Position)
    {
        DrawLater = true;
    }

    public override void EnactParticle(uint Tick)
    {
        EnactOpacityChange();
    }

    public override void Draw(BombarderGame Game)
    {
        Vector2 DrawPosition = new(
            Position.X + Game.Graphics.PreferredBackBufferWidth / 2F - Game.Player.Position.X,
            Position.Y + Game.Graphics.PreferredBackBufferHeight / 2F - Game.Player.Position.Y
        );
        Game.DrawLine(DrawPosition, Length, Direction, Colour * Opacity, Thickness);
    }

    private void EnactOpacityChange()
    {
        if (OpacityIncreasing)
        {
            if (BombarderGame.GameTick % OpacityIncreaseInterval != 0)
            {
                return;
            }

            Opacity += OpacityIncreasingChange;

            if (Opacity >= 1)
            {
                OpacityIncreasing = false;
            }
        }
        else
        {
            if (BombarderGame.GameTick % OpacityDecreasingInterval != 0)
            {
                return;
            }

            Opacity -= OpacityDecreasingChange;

            if (Opacity > 0)
            {
                return;
            }

            Duration = 1;
            HasDuration = true;
        }
    }


    public static void SpawnBetween(List<Particle> Particles, Vector2 Point1, Vector2 Point2)
    {
        float XDiff = Point1.X - Point2.X;
        float YDiff = Point1.Y - Point2.Y;

        float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));
        float Angle = (float)(Math.Atan2(YDiff, XDiff) * 180.0 / Math.PI);
        float AngleRadians = Angle * (float)(Math.PI / 180);

        int ParticleDistance = BombarderGame.random.Next(0, (int)Distance);
        int X = (int)(Point1.X - ParticleDistance * (float)Math.Cos(AngleRadians)) +
                BombarderGame.random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);
        int Y = (int)(Point1.Y - ParticleDistance * (float)Math.Sin(AngleRadians)) +
                BombarderGame.random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);

        Particles.Add(new TeleportLine(new Vector2(X, Y))
        {
            HasDuration = false,
            Duration = BombarderGame.random.Next(DurationMin, DurationMax),
            Length = BombarderGame.random.Next(LengthMin, LengthMax),
            Thickness = BombarderGame.random.Next(ThicknessMin, ThicknessMax),
            Direction = (Angle + BombarderGame.random.Next(-AngleSpreadRange, AngleSpreadRange)) * (float)(Math.PI / 180),
            Colour = Colours.First(),
            Opacity = OpacityDefault,
        });
    }
}