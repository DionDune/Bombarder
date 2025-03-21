﻿using System;
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

    public override void Update(uint Tick)
    {
        base.Update(Tick);
        EnactOpacityChange();
    }

    public override void Draw()
    {
        Vector2 DrawPosition = Position + BombarderGame.Instance.ScreenCenter - BombarderGame.Instance.Player.Position;
        RenderUtils.DrawLine(DrawPosition, Length, Direction, Colour * Opacity, Thickness);
    }

    private void EnactOpacityChange()
    {
        if (OpacityIncreasing)
        {
            if (BombarderGame.Instance.GameTick % OpacityIncreaseInterval != 0)
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
            if (BombarderGame.Instance.GameTick % OpacityDecreasingInterval != 0)
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
        Vector2 Diff = Point1 - Point2;

        float Distance = MathUtils.HypotF(Diff);
        float Angle = MathF.Atan2(Diff.Y, Diff.X);

        int ParticleDistance = RngUtils.Random.Next(0, (int)Distance);
        int X = (int)(Point1.X - ParticleDistance * MathF.Cos(Angle)) +
                RngUtils.Random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);
        int Y = (int)(Point1.Y - ParticleDistance * MathF.Sin(Angle)) +
                RngUtils.Random.Next(-RandomDistanceAllowance, RandomDistanceAllowance);

        Particles.Add(new TeleportLine(new Vector2(X, Y))
        {
            HasDuration = false,
            Duration = RngUtils.Random.Next(DurationMin, DurationMax),
            Length = RngUtils.Random.Next(LengthMin, LengthMax),
            Thickness = RngUtils.Random.Next(ThicknessMin, ThicknessMax),
            Direction = Angle + MathUtils.ToRadians(RngUtils.Random.Next(-AngleSpreadRange, AngleSpreadRange)),
            Colour = Colours.First(),
            Opacity = OpacityDefault,
        });
    }
}