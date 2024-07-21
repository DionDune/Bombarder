﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public class LaserLine : Particle
{
    public const int LengthMin = 10;
    public const int LengthMax = 200;
    public const int ThicknessMin = 2;
    public const int ThicknessMax = 4;
    public const int DurationMin = 25;
    public const int DurationMax = 200;
    public const int SpeedMin = 15;
    public const int SpeedMax = 75;
    public const int AngleSpreadRange = 5;

    public static readonly IList<Color> Colours = new ReadOnlyCollection<Color>
        (new List<Color> { Color.Turquoise, Color.DarkTurquoise, Color.MediumTurquoise, Color.MediumTurquoise });

    public static Color CentralLaserColor = Color.Red;

    public float Length { get; set; }
    public int Thickness { get; set; }
    public float Direction { get; set; }
    public float Speed { get; set; }
    public Color Colour { get; set; }

    public LaserLine(Vector2 Position, float Direction) : base(Position)
    {
        this.Direction = Direction;
        Length = BombarderGame.random.Next(LengthMin, LengthMax);
        Thickness = BombarderGame.random.Next(ThicknessMin, ThicknessMax);
        Speed = BombarderGame.random.Next(SpeedMin, SpeedMax);
        Colour = Color.Turquoise;
        DrawLater = true;
    }

    public override void EnactParticle(uint Tick)
    {
        EnactMovement();
    }

    public override void Draw(BombarderGame Game)
    {
        Vector2 DrawPosition = new(
            Position.X + Game.Graphics.PreferredBackBufferWidth / 2F - Game.Player.Position.X,
            Position.Y + Game.Graphics.PreferredBackBufferHeight / 2F - Game.Player.Position.Y
        );
        Game.DrawLine(DrawPosition, Length, Direction, Colour, Thickness);
    }

    public void EnactMovement()
    {
        Position += new Vector2((float)Math.Cos(Direction), (float)Math.Sin(Direction)) * Speed;
    }
}