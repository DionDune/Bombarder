using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder;

public static class RngUtils
{
    public static readonly Random Random = new();

    public static Vector2 GetRandomSpawnPoint()
    {
        var Player = BombarderGame.Instance.Player;
        var Graphics = BombarderGame.Instance.Graphics;

        // Spawns randomly from edges of screen
        float SpawnAngle = MathUtils.ToRadians(Random.Next(0, 360));
        int SpawnDistance = Random.Next(
            (int)(Graphics.PreferredBackBufferWidth * 0.6F),
            (int)(Graphics.PreferredBackBufferWidth * 1.2)
        );

        return new Vector2(
            Player.Position.X + SpawnDistance * MathF.Cos(SpawnAngle),
            Player.Position.Y + SpawnDistance * MathF.Sin(SpawnAngle)
        );
    }

    public static T GetRandomElement<T>(this IList<T> List)
    {
        return List[Random.Next(0, List.Count)];
    }

    public static T GetRandomElement<T>(this T[] Array)
    {
        return Array[Random.Next(0, Array.Length)];
    }
}