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

    public Spider()
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
        if (NextJumpFrame > Game1.GameTick)
        {
            return;
        }

        float XDiff = Position.X - Player.Position.X;
        float YDiff = Position.Y - Player.Position.Y;

        float PlayerDistance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

        Velocity = PlayerDistance > JumpVelocityFullThreshold
            ? JumpVelocityMax
            : (float)Game1.random.Next((int)JumpVelocityMin * 100, (int)JumpVelocityMed * 100) / 100;
        Angle = (float)Math.Atan2(YDiff, XDiff);

        NextJumpFrame = PlayerDistance > ErraticDistanceThreshold
            ? Game1.GameTick + (uint)Game1.random.Next(JumpIntervalErraticMin, JumpIntervalErraticMax)
            : Game1.GameTick + (uint)Game1.random.Next(JumpIntervalMin, JumpIntervalMax);
    }

    public void EnactVelocity(Player Player)
    {
        if (Velocity <= 0)
        {
            return;
        }

        Vector2 PositionChange = new Vector2(
            Velocity * (float)Math.Cos(Angle),
            Velocity * (float)Math.Sin(Angle)
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
        if (Game1.GameTick - LastDamageFrame < DamageInterval)
        {
            return;
        }

        Point TopLeft = new Point((int)Position.X + HitBoxOffset.X, (int)Position.Y + HitBoxOffset.Y);
        Point TopRight = new Point((int)Position.X + HitBoxOffset.X + HitBoxSize.X,
            (int)Position.Y + HitBoxOffset.Y);
        Point BottomLeft = new Point((int)Position.X + HitBoxOffset.X,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y);
        Point BottomRight = new Point((int)Position.X + HitBoxOffset.X + HitBoxSize.X,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y);

        Point PlayerTopLeft = new Point((int)Player.Position.X - (Player.Width / 2),
            (int)Player.Position.Y - (Player.Height / 2));
        Point PlayerTopRight = new Point((int)Player.Position.X + (Player.Width / 2),
            (int)Player.Position.Y - (Player.Height / 2));
        Point PlayerBottomLeft = new Point((int)Player.Position.X - (Player.Width / 2),
            (int)Player.Position.Y + (Player.Height / 2));
        Point PlayerBottomRight = new Point((int)Player.Position.X + (Player.Width / 2),
            (int)Player.Position.Y + (Player.Height / 2));


        bool Contact = false;

        if (PlayerTopLeft.X >= TopLeft.X && PlayerTopLeft.X <= TopRight.X &&
            PlayerTopLeft.Y >= TopLeft.Y && PlayerTopLeft.Y <= BottomRight.Y)
        {
            Contact = true;
        }
        else if (PlayerTopRight.X >= TopLeft.X && PlayerTopRight.X <= TopRight.X &&
                 PlayerTopRight.Y >= TopLeft.Y && PlayerTopRight.Y <= BottomLeft.Y)
        {
            Contact = true;
        }
        else if (PlayerBottomLeft.X >= TopLeft.X && PlayerBottomLeft.X <= TopRight.X &&
                 PlayerBottomLeft.Y >= TopLeft.Y && PlayerBottomLeft.Y <= BottomLeft.Y)
        {
            Contact = true;
        }
        else if (PlayerBottomRight.X >= TopLeft.X && PlayerBottomRight.X <= TopRight.X &&
                 PlayerBottomRight.Y >= TopLeft.Y && PlayerBottomRight.Y <= BottomLeft.Y)
        {
            Contact = true;
        }


        if (!Contact)
        {
            return;
        }

        Player.GiveDamage(Damage);
        LastDamageFrame = Game1.GameTick;
    }

    public override void DrawEntity(Game1 Game1)
    {
        Game1.SpriteBatch.Draw(
            Game1.Textures.White,
            new Rectangle(
                (int)(
                    Position.X +
                    Parts[0].Offset.X +
                    Game1.Graphics.PreferredBackBufferWidth / 2F -
                    Game1.Player.Position.X
                ),
                (int)(
                    Position.Y +
                    Parts[0].Offset.Y +
                    Game1.Graphics.PreferredBackBufferHeight / 2F -
                    Game1.Player.Position.Y
                ),
                Parts[0].Width,
                Parts[0].Height
            ),
            Color.MediumPurple
        );
    }
}