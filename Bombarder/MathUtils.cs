﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Bombarder;

public static class MathUtils
{
    public static Vector2 ToPosition(this Orientation Orientation)
    {
        var Graphics = BombarderGame.Instance.Graphics;

        return Orientation switch
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
    }

    public static Point ToPoint(this Orientation Orientation) => Orientation.ToPosition().ToPoint();
    public static Vector2 Copy(this Vector2 Vector) => new(Vector.X, Vector.Y);
    public static Point Copy(this Point Point) => new(Point.X, Point.Y);
    public static Vector2 Abs(Vector2 Vector2) => new(Math.Abs(Vector2.X), Math.Abs(Vector2.Y));
    public static double Hypot(params double[] Values) => Math.Sqrt(Values.Sum(Value => Math.Pow(Value, 2)));

    public static double Hypot(params Vector2[] Vectors) =>
        Hypot(Array.ConvertAll(Vectors, Vector => Hypot(Vector.X, Vector.Y)));

    public static double Hypot(params Point[] Points) =>
        Hypot(Array.ConvertAll(Points, Point => Hypot(Point.ToVector2())));

    public static float HypotF(params double[] Values) => (float)Hypot(Values);
    public static float HypotF(params Vector2[] Vectors) => (float)Hypot(Vectors);
    public static float HypotF(params Point[] Points) => (float)Hypot(Points);

    public const double Radian = Math.PI / 180;
    public static double ToRadians(double Degree) => Degree * Radian;
    public static float ToRadians(float Degree) => Degree * (float)Radian;
    public static double ToDegrees(double Radians) => Radians / Radian;
    public static float ToDegrees(float Radians) => Radians / (float)Radian;
    public static Rectangle CreateRectangle(Vector2 Position, Vector2 Size) => new(Position.ToPoint(), Size.ToPoint());
}