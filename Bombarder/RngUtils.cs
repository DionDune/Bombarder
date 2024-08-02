using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder;

public static class RngUtils
{
    public static readonly Random Random = new();
    
    public static int Next(this Random RandomInstance, Vector2 Range)
    {
        return RandomInstance.Next((int)Range.X, (int)Range.Y);
    }

    public static Vector2 GetRandomSpawnPoint()
    {
        var Game = BombarderGame.Instance;
        var Player = Game.Player;

        // Spawns randomly from edges of screen
        float SpawnAngle = MathUtils.ToRadians(Random.Next(0, 360));
        int SpawnDistance = Random.Next(Game.ScreenSize * new Vector2(0.6F, 1.2F));

        return new Vector2(
            Player.Position.X + SpawnDistance * MathF.Cos(SpawnAngle),
            Player.Position.Y + SpawnDistance * MathF.Sin(SpawnAngle)
        );
    }
    
    public static Vector2 GetRandomVector(Vector2 Min, Vector2 Max)
    {
        return new Vector2(
            Random.Next((int)Min.X, (int)Max.X),
            Random.Next((int)Min.Y, (int)Max.Y)
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