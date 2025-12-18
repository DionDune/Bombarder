using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class PlayerTeleport : MagicEffect
{
    public override int ManaCost { get; protected set; } = 150;
    public const int DefaultDuration = 12;

    public const float PlayerMovementSpeed = 80;
    private float MovementAngle;
    

    public const int ParticleCountMin = 65;
    public const int ParticleCountMed = 100;
    public const int ParticleCountMax = 150;
    public const int ParticleCountMaxDistanceThreshold = 400;

    public readonly (int Min, int Max) ParticlesPerFrame = (5, 12);
    public int ParticleOffsetAllowance = 30;

    public PlayerTeleport(Vector2 Goal) : base(Goal)
    {
        Duration = DefaultDuration;
        HasDuration = true;

        Vector2 PositionDifference = BombarderGame.Instance.Player.Position - Position;
        MovementAngle = MathF.Atan2(PositionDifference.Y, PositionDifference.X);
    }

    public override void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        base.Update(Player, Entities, GameTick);

        EnactMovement();

        //CreateParticles();
        for (int i = 0; i < RngUtils.Random.Next(ParticlesPerFrame.Min, ParticlesPerFrame.Max); i++)
        {
            Vector2 ParticleOffset = new Vector2(RngUtils.Random.Next(0, ParticleOffsetAllowance),
                                                RngUtils.Random.Next(0, ParticleOffsetAllowance));
            TeleportLine.Create(Player.Position + ParticleOffset, MovementAngle);
        }
            
    }

    public override void DrawEffect()
    {
    }

    public void EnactMovement()
    {
        BombarderGame.Instance.Player.Position -= new Vector2(PlayerMovementSpeed * MathF.Cos(MovementAngle),
                                                                PlayerMovementSpeed * MathF.Sin(MovementAngle));
    }

    public void CreateParticles()
    {
        Vector2 Diff = BombarderGame.Instance.Player.Position - Position;
        float Distance = MathUtils.HypotF(Diff);

        int Count = ParticleCountMed;
        if (Distance > ParticleCountMaxDistanceThreshold)
        {
            Count = ParticleCountMax;
        }

        for (int i = 0; i < RngUtils.Random.Next(ParticleCountMin, Count); i++)
        {
            TeleportLine.SpawnBetween(BombarderGame.Instance.World.Particles, BombarderGame.Instance.Player.Position.Copy(), Position.Copy());
        }
    }
}