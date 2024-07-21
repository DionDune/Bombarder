using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bombarder.Entities;

public class RedCube : Entity
{
    public const int Damage = 30;
    public int SelfDamage => Damage / 2;

    public const float BaseSpeed = 5;

    public RedCube()
    {
        HealthMax = 100;
        Health = HealthMax;
        HealthBarVisible = true;

        ChaseMode = true;

        KillHealthReward = 0;
        KillManaReward = 15;

        Parts = new List<EntityBlock>
        {
            new()
            {
                Width = 66,
                Height = 66,
                Offset = new Vector2(-33, -33),
                Color = Color.DarkRed,
            },
            new()
            {
                Width = 56,
                Height = 56,
                Offset = new Vector2(-28, -28),
                Color = Color.Red
            }
        };
        HitBoxOffset = new Point(-33, -33);
        HitBoxSize = new Point(66, 66);
        HealthBarDimensions = new Point(40, 10);
        HealthBarOffset = new Point(-20, -HitBoxOffset.Y + 5);
    }

    public override void EnactAI(Player Player)
    {
        MoveTowards(Player.Position, BaseSpeed);
        EnactAttack(Player);
    }

    public void EnactAttack(Player Player)
    {
        Point TopLeft = new Point(
            (int)Position.X + HitBoxOffset.X,
            (int)Position.Y + HitBoxOffset.Y
        );
        Point TopRight = new Point(
            (int)Position.X + HitBoxOffset.X + HitBoxSize.X,
            (int)Position.Y + HitBoxOffset.Y
        );
        Point BottomLeft = new Point(
            (int)Position.X + HitBoxOffset.X,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y
        );
        Point BottomRight = new Point(
            (int)Position.X + HitBoxOffset.X + HitBoxSize.X,
            (int)Position.Y + HitBoxOffset.Y + HitBoxSize.Y
        );

        Point PlayerTopLeft = new Point(
            (int)Player.Position.X - Player.Width / 2,
            (int)Player.Position.Y - Player.Height / 2
        );
        Point PlayerTopRight = new Point(
            (int)Player.Position.X + Player.Width / 2,
            (int)Player.Position.Y - Player.Height / 2
        );
        Point PlayerBottomLeft = new Point(
            (int)Player.Position.X - Player.Width / 2,
            (int)Player.Position.Y + Player.Height / 2
        );
        Point PlayerBottomRight = new Point(
            (int)Player.Position.X + Player.Width / 2,
            (int)Player.Position.Y + Player.Height / 2
        );

        if (TopLeft.X >= PlayerTopLeft.X && TopLeft.X <= PlayerTopRight.X &&
            TopLeft.Y >= PlayerTopLeft.Y && TopLeft.Y <= PlayerBottomLeft.Y)
        {
            Player.GiveDamage(Damage);
            GiveDamage(SelfDamage);
        }
        else if (TopRight.X >= PlayerTopLeft.X && TopRight.X <= PlayerTopRight.X &&
                 TopRight.Y >= PlayerTopLeft.Y && TopRight.Y <= PlayerBottomLeft.Y)
        {
            Player.GiveDamage(Damage);
            GiveDamage(SelfDamage);
        }
        else if (BottomLeft.X >= PlayerTopLeft.X && BottomLeft.X <= PlayerTopRight.X &&
                 BottomLeft.Y >= PlayerTopLeft.Y && BottomLeft.Y <= PlayerBottomLeft.Y)
        {
            Player.GiveDamage(Damage);
            GiveDamage(SelfDamage);
        }
        else if (BottomRight.X >= PlayerTopLeft.X && BottomRight.X <= PlayerTopRight.X &&
                 BottomRight.Y >= PlayerTopLeft.Y && BottomRight.Y <= PlayerBottomLeft.Y)
        {
            Player.GiveDamage(Damage);
            GiveDamage(SelfDamage);
        }
    }

    public void CreateDeathParticles()
    {
        EntityBlock Block = Parts.First();
        Vector2 StartPoint = Position + Block.Offset;

        for (int y = 0; y < Block.Height / RedCubeSegment.Height; y++)
        {
            for (int x = 0; x < Block.Width / RedCubeSegment.Width; x++)
            {
                float ParticleX = StartPoint.X + x * RedCubeSegment.Width;
                float ParticleY = StartPoint.Y + y * RedCubeSegment.Height;

                var XDifference = ParticleX - Position.X;
                var YDifference = ParticleY - Position.Y;
                var Angle =
                    (
                        (float)Math.Atan2(YDifference, XDifference) *
                        (float)(180 / Math.PI) +
                        BombarderGame.random.Next(
                            (int)(-RedCubeSegment.AngleOffsetAllowance * 10),
                            (int)(RedCubeSegment.AngleOffsetAllowance * 10)
                        ) / 10F
                    ) *
                    (float)Math.PI / 180F;

                BombarderGame.Instance.Particles.Add(
                    new RedCubeSegment(new Vector2(ParticleX, ParticleY), Angle)
                    {
                        HasDuration = true
                    }
                );
            }
        }
    }

    public override void DrawEntity()
    {
        foreach (EntityBlock Block in Parts)
        {
            Color BlockColor = Block.Color;
            Texture2D BlockTexture = BombarderGame.Instance.Textures.White;
            if (Block.Textures != null)
            {
                BlockColor = Color.White;
                BlockTexture = Block.Textures.First();
            }

            BombarderGame.Instance.SpriteBatch.Draw(
                BlockTexture,
                new Rectangle(
                    (int)(
                        Position.X +
                        Block.Offset.X +
                        BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                        BombarderGame.Instance.Player.Position.X
                    ),
                    (int)
                    (
                        Position.Y +
                        Block.Offset.Y +
                        BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                        BombarderGame.Instance.Player.Position.Y
                    ),
                    Block.Width,
                    Block.Height
                ),
                BlockColor);
        }
    }
}