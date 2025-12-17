using System;
using System.Collections.Generic;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public class Spider : Entity
{
    public const int Damage = 95;
    private uint LastDamageFrame;
    private const int DamageInterval = 40;

    private readonly (uint Min, uint Max) JumpInterval = (60, 250);
    private const float AngleJumpChance = 0.6f;
    private readonly (int Min, int Max) JumpIntervalErratic = (60, 140);
    private const int ErraticDistanceThreshold = 800;
    private uint NextJumpFrame;

    private uint NextAttackFrame;
    private readonly (int Min, int Max) AttackInterval = (120, 180);
    private (Vector2 Start, Vector2 End) TargetMonitoredPositions = (Vector2.Zero, Vector2.Zero);
    private const int TargetMonitorDuration = 60;

    private readonly (int Min, int Med, int Max) JumpVelocity = (20, 40, 55);
    private const float JumpVelocityFullThreshold = 650;
    private const float VelocityMultiplier = 0.95F;
    private float Velocity;
    private float Angle;

    public Spider(Vector2 Position) : base(Position)
    {
        HealthMax = 500;
        Health = HealthMax;
        HealthBarVisible = true;

        KillHealthReward = 50;
        KillManaReward = 400;

        HitBoxOffset = new Point(-100, -100);
        HitBoxSize = new Point(200, 200);
        HealthBarOffset = new Point(-40, -HitBoxOffset.Y + 5);
        HealthBarDimensions = new Point(80, 16);

        Parts = new List<EntityBlock>
        {
            new()
            {
                Width = 200,
                Height = 200,
                Offset = new Vector2(-100, -100),
                Color = Color.MediumPurple
            }
        };
    }

    public override void EnactAI(Player Player)
    {
        EnactJump(Player);
        EnactVelocity(Player);
        EnactDamage(Player);
        EnactAttack(Player);
    }

    private void EnactJump(Player Player)
    {
        if (NextJumpFrame > BombarderGame.Instance.GameTick ||
            (NextAttackFrame < BombarderGame.Instance.GameTick && NextAttackFrame + TargetMonitorDuration > BombarderGame.Instance.GameTick))
        {
            return;
        }

        Vector2 Diff = Position - Player.Position;

        float PlayerDistance = MathUtils.HypotF(Diff);


        // Perform Angled Jump
        if (RngUtils.Random.Next(new Vector2(0, 100)) < 100f * AngleJumpChance)
        {
            float PlayerReletiveAngle = (float)Math.Atan2(Position.Y - Player.Position.Y,
                                                            Position.X - Player.Position.X) * (float)(180 / Math.PI);
            PlayerReletiveAngle += (RngUtils.Random.Next(-22, 22) * 3);
            Angle = PlayerReletiveAngle * ((float)Math.PI / 180f);
            Velocity = JumpVelocity.Max;
            NextJumpFrame = BombarderGame.Instance.GameTick + (JumpInterval.Min / 2);
            CreateJumpParticles();

            return;
        }

        // Create Jump Particles
        CreateJumpParticles();
        

        Velocity = PlayerDistance > JumpVelocityFullThreshold
            ? JumpVelocity.Max
            : RngUtils.Random.Next((int)JumpVelocity.Min * 100, (int)JumpVelocity.Med * 100) / 100F;
        Angle = MathF.Atan2(Diff.Y, Diff.X);


        NextJumpFrame = PlayerDistance > ErraticDistanceThreshold
            ? BombarderGame.Instance.GameTick +
              (uint)RngUtils.Random.Next(JumpIntervalErratic.Min, JumpIntervalErratic.Max)
            : BombarderGame.Instance.GameTick + (uint)RngUtils.Random.Next((int)JumpInterval.Min, (int)JumpInterval.Max);
    }
    private void EnactVelocity(Player Player)
    {
        if (NextAttackFrame < BombarderGame.Instance.GameTick && NextAttackFrame + TargetMonitorDuration > BombarderGame.Instance.GameTick)
        {
            Velocity = 0;
        }
        if (Velocity <= 0)
        {
            return;
        }

        Vector2 PositionChange = new Vector2(
            Velocity * MathF.Cos(Angle),
            Velocity * MathF.Sin(Angle)
        );
        Position -= PositionChange;

        Velocity *= VelocityMultiplier;
        if (Velocity <= 1)
        {
            Velocity = 0;
        }
    }

    private void EnactDamage(Player Player)
    {
        if (BombarderGame.Instance.GameTick - LastDamageFrame < DamageInterval)
        {
            return;
        }

        if (!HitBox.Intersects(Player.HitBox))
        {
            return;
        }

        Player.GiveDamage(Damage);
        LastDamageFrame = BombarderGame.Instance.GameTick;
    }
    private void EnactAttack(Player Player)
    {
        if (NextAttackFrame > BombarderGame.Instance.GameTick)
        {
            return;
        }


        if (NextAttackFrame + TargetMonitorDuration >= BombarderGame.Instance.GameTick)
        {
            if (TargetMonitoredPositions.Start == Vector2.Zero)
                TargetMonitoredPositions.Start = Player.Position;

            if (NextAttackFrame + TargetMonitorDuration == BombarderGame.Instance.GameTick)
                TargetMonitoredPositions.End = Player.Position;
        }
        else
        {
            Vector2 Diff = Position - Player.Position;
            float TargetDistance = MathUtils.HypotF(Diff);
            float WebToTargetTime = TargetDistance / MagicEffects.SpiderWeb.MovingSpeed;
            // Times the time with the distance the target traveled in the monitor time, predict location
            // Spider should stop when monitoring


            float DistanceMultiplier = WebToTargetTime / TargetMonitorDuration;
            Vector2 TargetDistanceTraveled = new Vector2(TargetMonitoredPositions.End.X - TargetMonitoredPositions.Start.X,
                                                            TargetMonitoredPositions.End.Y - TargetMonitoredPositions.Start.Y);
            // Get PreddictedPosition
            TargetDistanceTraveled *= DistanceMultiplier;
            MagicEffect.CreateMagic<SpiderWeb>(Player.Position + TargetDistanceTraveled, null, this);

            TargetMonitoredPositions.Start = Vector2.Zero;
            TargetMonitoredPositions.End = Vector2.Zero;

            NextAttackFrame = BombarderGame.Instance.GameTick + (uint)RngUtils.Random.Next(AttackInterval.Min, AttackInterval.Max);
        }
    }

    private void CreateJumpParticles()
    {
        (int Min, int Max) ParticleCountRange = (3, 8);
        (int Min, int Max) ParticleSpawnOffsetAllowance = (-10, 10);

        int ParticleCount = RngUtils.Random.Next(ParticleCountRange.Min, ParticleCountRange.Max);
        float ParticleMovementAngleStep = 360f / ParticleCount;
        (int Left, int Right) ParticleMovementAngleOffsetAllowance = (
                                                                        -((int)ParticleMovementAngleStep / 2),
                                                                        (int)ParticleMovementAngleStep / 2
                                                                        );

        for (int i = 0; i < ParticleCount; i++)
        {
            float ParticleAngle = MathUtils.ToRadians((ParticleMovementAngleStep * i) + 
                                                        RngUtils.Random.Next(ParticleMovementAngleOffsetAllowance.Left,
                                                                                ParticleMovementAngleOffsetAllowance.Right));

            Vector2 ParticlePosition = new Vector2(
                                                    this.Position.X + RngUtils.Random.Next(ParticleSpawnOffsetAllowance.Min,
                                                                                            ParticleSpawnOffsetAllowance.Max),
                                                    this.Position.Y + RngUtils.Random.Next(ParticleSpawnOffsetAllowance.Min,
                                                                                            ParticleSpawnOffsetAllowance.Max));

            BombarderGame.Instance.World.Particles.Add(
                    new SpiderJumpParticle(ParticlePosition, ParticleAngle)
                    {
                        HasDuration = true
                    }
                );
        }

    }
    public override void DrawEntity()
    {
        var Game = BombarderGame.Instance;

        Game.SpriteBatch.Draw(
            Game.Textures.White,
            MathUtils.CreateRectangle(
                Position + Parts[0].Offset + Game.ScreenCenter - Game.Player.Position,
                new Vector2(Parts[0].Width, Parts[0].Height)
            ),
            Color.MediumPurple
        );
    }
}