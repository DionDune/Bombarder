using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class PlayerTeleport : MagicEffect
{
    public override int ManaCost { get; protected set; } = 150;
    public const int DefaultDuration = 2;
    public const bool HasDurationWhenReached = true;

    public const float Speed = 80;
    public bool GoalReacted = false;
    public bool JustStarted = true;

    public const int ParticleCountMin = 65;
    public const int ParticleCountMed = 100;
    public const int ParticleCountMax = 150;
    public const int ParticleCountMaxDistanceThreshold = 400;

    public PlayerTeleport(Vector2 Goal) : base(Goal)
    {
        Duration = DefaultDuration;
        HasDuration = false;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        if (JustStarted)
        {
            CreateParticles(Player);
        }

        EnactMovement(Player);
        EnactDuration(Player);
    }

    public override void Draw()
    {
    }

    public void EnactMovement(Player Player)
    {
        Vector2 Diff = Player.Position - Position;
        float Distance = Utils.HypotF(Diff);
        float Angle = MathF.Atan2(Diff.Y, Diff.X) * 180F / MathF.PI;
        float AngleRadians = Angle * (MathF.PI / 180F);

        float DistanceToMove = Speed;

        if (Distance <= Speed)
        {
            DistanceToMove = Distance;
            GoalReacted = true;
        }

        Player.Position -=
            new Vector2(DistanceToMove * MathF.Cos(AngleRadians), DistanceToMove * MathF.Sin(AngleRadians));
        Player.IsInvincible = true;

        JustStarted = false;
    }

    public void EnactDuration(Player Player)
    {
        if (!GoalReacted)
        {
            return;
        }

        HasDuration = true;
        Player.IsInvincible = false;
    }

    public void CreateParticles(Player Player)
    {
        Vector2 Diff = Player.Position - Position;
        float Distance = Utils.HypotF(Diff);

        int Count = ParticleCountMed;
        if (Distance > ParticleCountMaxDistanceThreshold)
        {
            Count = ParticleCountMax;
        }

        for (int i = 0; i < BombarderGame.random.Next(ParticleCountMin, Count); i++)
        {
            TeleportLine.SpawnBetween(BombarderGame.Instance.Particles, Player.Position.Copy(), Position.Copy());
        }
    }
}