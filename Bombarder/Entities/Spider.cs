using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public class Spider : Entity
{
    public const int Damage = 125;
    public uint LastDamageFrame;
    public const int DamageInterval = 40;

    public const int JumpIntervalMin = 60;
    public const int JumpIntervalMax = 250;
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

    public void EnactJump(Player Player)
    {
        if (NextJumpFrame > BombarderGame.Instance.GameTick)
        {
            return;
        }

        Vector2 Diff = Position - Player.Position;

        float PlayerDistance = MathUtils.HypotF(Diff);

        Velocity = PlayerDistance > JumpVelocityFullThreshold
            ? JumpVelocityMax
            : RngUtils.Random.Next((int)JumpVelocityMin * 100, (int)JumpVelocityMed * 100) / 100F;
        Angle = MathF.Atan2(Diff.Y, Diff.X);

        NextJumpFrame = PlayerDistance > ErraticDistanceThreshold
            ? BombarderGame.Instance.GameTick +
              (uint)RngUtils.Random.Next(JumpIntervalErraticMin, JumpIntervalErraticMax)
            : BombarderGame.Instance.GameTick + (uint)RngUtils.Random.Next(JumpIntervalMin, JumpIntervalMax);
    }

    public void EnactVelocity(Player Player)
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

    public void EnactDamage(Player Player)
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

    public override void DrawEntity()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    Parts[0].Offset.X +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    Parts[0].Offset.Y +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                Parts[0].Width,
                Parts[0].Height
            ),
            Color.MediumPurple
        );
    }
}