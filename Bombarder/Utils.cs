﻿using System;
using Microsoft.Xna.Framework;

namespace Bombarder;

public static class Utils
{
    public static Vector2 ToPosition(this Orientation Orientation, GraphicsDeviceManager Graphics) =>
        Orientation switch
        {
            Orientation.TOP_LEFT => Vector2.Zero,
            Orientation.TOP => new Vector2(Graphics.PreferredBackBufferWidth / 2F, 0),
            Orientation.TOP_RIGHT => new Vector2(Graphics.PreferredBackBufferWidth, 0),
            Orientation.LEFT => new Vector2(0, Graphics.PreferredBackBufferHeight / 2F),
            Orientation.CENTER => new Vector2(Graphics.PreferredBackBufferWidth / 2F,
                Graphics.PreferredBackBufferHeight / 2F),
            Orientation.RIGHT => new Vector2(Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight / 2F),
            Orientation.BOTTOM_LEFT => new Vector2(0, Graphics.PreferredBackBufferHeight),
            Orientation.BOTTOM => new Vector2(Graphics.PreferredBackBufferWidth / 2F,
                Graphics.PreferredBackBufferHeight),
            Orientation.BOTTOM_RIGHT => new Vector2(Graphics.PreferredBackBufferWidth,
                Graphics.PreferredBackBufferHeight),
            _ => Vector2.Zero
        };

    public static Point ToPoint(this Orientation Orientation, GraphicsDeviceManager Graphics) =>
        Orientation.ToPosition(Graphics).ToPoint();

    public static Vector2 Copy(this Vector2 Vector) => new(Vector.X, Vector.Y);
    public static Point Copy(this Point Point) => new(Point.X, Point.Y);
    public static Vector2 Abs(Vector2 Vector2) => new(Math.Abs(Vector2.X), Math.Abs(Vector2.Y));
}