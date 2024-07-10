using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.Entities;

public class CubeMother : Entity
{
    public const float BaseSpeed = 3;

    public const int SpawnIntervalMin = 15;
    public const int SpawnIntervalMax = 100;
    public const int SpawnDistanceMin = 150;
    public const int SpawnDistanceMax = 350;
    public uint NextSpawnFrame;

    public const float PreferredDistance = 3500;

    public CubeMother()
    {
        HealthMax = 1500;
        Health = HealthMax;
        HealthBarVisible = true;

        ChaseMode = false;

        HitBoxOffset = new Point(-231, -231);
        HitBoxSize = new Point(462, 462);
        HealthBarDimensions = new Point(350, 74);
        HealthBarOffset = new Point(-175, -37);

        KillHealthReward = 2000;
        KillManaReward = 2000;

        Parts = new List<EntityBlock>
        {
            new()
            {
                Width = 462,
                Height = 462,
                Offset = new Vector2(-231, -231),
                Color = Color.DarkRed
            },
            new()
            {
                Width = 442,
                Height = 442,
                Offset = new Vector2(-221, -221),
                Color = Color.Red
            }
        };
    }

    public override void EnactAI(Player Player)
    {
        EnactMovement(Player);
        EnactSpawn();
    }

    public void EnactMovement(Player Player)
    {
        float Distance = Vector2.Distance(Position, Player.Position);

        switch (Distance)
        {
            case > PreferredDistance:
                MoveTowards(Player.Position, BaseSpeed);
                break;
            case < PreferredDistance:
                MoveAwayFrom(Player.Position, BaseSpeed);
                break;
        }
    }
    
    public void EnactSpawn()
    {
        if (NextSpawnFrame != BombarderGame.GameTick && BombarderGame.GameTick <= NextSpawnFrame)
        {
            return;
        }

        float SpawnAngle = BombarderGame.random.Next(0, 360) * (float)(Math.PI / 180);
        int SpawnDistance = BombarderGame.random.Next(SpawnDistanceMin, SpawnDistanceMax);
        Vector2 SpawnPoint = new Vector2(
            Position.X + SpawnDistance * (float)Math.Cos(SpawnAngle),
            Position.Y + SpawnDistance * (float)Math.Sin(SpawnAngle)
        );

        //Red Cube
        BombarderGame.EntitiesToAdd.Add(new RedCube
        {
            Position = new Vector2(SpawnPoint.X, SpawnPoint.Y)
        });

        NextSpawnFrame = (uint)(BombarderGame.GameTick + BombarderGame.random.Next(SpawnIntervalMin, SpawnIntervalMax));
    }

    public override void DrawEntity(BombarderGame Game)
    {
        foreach (EntityBlock Block in Parts)
        {
            Color BlockColor = Block.Color;
            Texture2D BlockTexture = Game.Textures.White;
            if (Block.Textures != null)
            {
                BlockColor = Color.White;
                BlockTexture = Block.Textures.First();
            }

            Game.SpriteBatch.Draw(
                BlockTexture,
                new Rectangle(
                    (int)(
                        Position.X +
                        Block.Offset.X +
                        Game.Graphics.PreferredBackBufferWidth / 2F -
                        Game.Player.Position.X
                    ),
                    (int)(
                        Position.Y +
                        Block.Offset.Y +
                        Game.Graphics.PreferredBackBufferHeight / 2F -
                        Game.Player.Position.Y
                    ),
                    Block.Width, Block.Height), BlockColor
            );
        }
    }
}