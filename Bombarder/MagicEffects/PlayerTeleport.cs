using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class PlayerTeleport : MagicEffect
{
    public const int ManaCost = 150;

    public const int DefaultDuration = 2;
    public const bool HasDurationWhenReached = true;

    public const float Speed = 80;
    public Vector2 Goal;
    public bool GoalReacted = false;
    public bool JustStarted = true;

    public const int ParticleCountMin = 65;
    public const int ParticleCountMed = 100;
    public const int ParticleCountMax = 150;
    public const int ParticleCountMaxDistanceThreshold = 400;

    public PlayerTeleport(Vector2 Position, Vector2 Goal) : base(Position)
    {
        this.Goal = Goal;
        Duration = DefaultDuration;
        HasDuration = false;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        if (JustStarted)
        {
            CreateParticles();
        }

        EnactMovement(Player);
        EnactDuration(Player);
    }

    public override void Draw()
    {
    }

    public void EnactMovement(Player Player)
    {
        Vector2 Diff = Player.Position - Goal;
        float Distance = (float)Math.Sqrt(Math.Pow(Diff.X, 2) + Math.Pow(Diff.Y, 2));
        float Angle = (float)(Math.Atan2(Diff.Y, Diff.X) * 180.0 / Math.PI);
        float AngleRadians = Angle * (float)(Math.PI / 180);

        float DistanceToMove = Speed;

        if (Distance <= Speed)
        {
            DistanceToMove = Distance;
            GoalReacted = true;
        }

        Player.Position -= new Vector2(
            DistanceToMove * (float)Math.Cos(AngleRadians), DistanceToMove * (float)Math.Sin(AngleRadians));
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

    public void CreateParticles()
    {
        Vector2 Diff = Position - Goal;
        float Distance = (float)Math.Sqrt(Math.Pow(Diff.X, 2) + Math.Pow(Diff.Y, 2));

        int Count = ParticleCountMed;
        if (Distance > ParticleCountMaxDistanceThreshold)
        {
            Count = ParticleCountMax;
        }

        for (int i = 0; i < BombarderGame.random.Next(ParticleCountMin, Count); i++)
        {
            TeleportLine.SpawnBetween(BombarderGame.Instance.Particles, Position.Copy(), Goal.Copy());
        }
    }
}