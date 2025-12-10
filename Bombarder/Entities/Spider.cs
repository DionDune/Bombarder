using System;
using System.Collections.Generic;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public class Spider : Entity
{
    public const int Damage = 125;
    public uint LastDamageFrame;
    public const int DamageInterval = 40;

    public const int JumpIntervalMin = 60;
    public const int JumpIntervalMax = 250;
    public const float AngleJumpChance = 0.3f;
    public const int JumpIntervalErraticMin = 60;
    public const int JumpIntervalErraticMax = 140;
    public const int ErraticDistanceThreshold = 800;
    public uint NextJumpFrame;

    public const float JumpVelocityMin = 20;
    public const float JumpVelocityMed = 40;
    public const float JumpVelocityMax = 55;
    public const float JumpVelocityFullThreshold = 650;
    public const float VelocityMultiplier = 0.95F;
    public float Velocity;
    public float Angle;

    public Spider(Vector2 Position) : base(Position)
    {
        HealthMax = 650;
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
    }

    private void EnactJump(Player Player)
    {
        if (NextJumpFrame > BombarderGame.Instance.GameTick)
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
            Velocity = JumpVelocityMax;
            NextJumpFrame = BombarderGame.Instance.GameTick + (JumpIntervalMin / 2);

            return;
        }

        // Create Jump Particles
        CreateJumpParticles();
        

        Velocity = PlayerDistance > JumpVelocityFullThreshold
            ? JumpVelocityMax
            : RngUtils.Random.Next((int)JumpVelocityMin * 100, (int)JumpVelocityMed * 100) / 100F;
        Angle = MathF.Atan2(Diff.Y, Diff.X);


        NextJumpFrame = PlayerDistance > ErraticDistanceThreshold
            ? BombarderGame.Instance.GameTick +
              (uint)RngUtils.Random.Next(JumpIntervalErraticMin, JumpIntervalErraticMax)
            : BombarderGame.Instance.GameTick + (uint)RngUtils.Random.Next(JumpIntervalMin, JumpIntervalMax);
    }

    private void EnactVelocity(Player Player)
    {
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

            BombarderGame.Instance.Particles.Add(
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