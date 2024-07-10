using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public class DemonEye : Entity
{
    public const int Damage = 15;
    public const int DamageInterval = 10;
    public uint LastDamageFrame;

    public const float BaseSpeed = 4;

    public DemonEye()
    {
        HealthMax = 150;
        Health = HealthMax;
        HealthBarVisible = true;
        ChaseMode = true;

        KillHealthReward = 0;
        KillManaReward = 100;

        HitBoxOffset = new Point(-119, -100);
        HitBoxSize = new Point(238, 200);
        HealthBarOffset = new Point(-40, -HitBoxOffset.Y + 5);
        HealthBarDimensions = new Point(80, 16);

        Parts = new List<EntityBlock>
        {
            new()
            {
                Width = 0,
                Height = 0,
                Offset = new Vector2(0, 0),
                Color = Color.White,
            }
        };
    }

    public override void EnactAI(Player Player)
    {
        MoveTowards(Player.Position, BaseSpeed);
        EnactAttack(Player);
    }

    public void EnactAttack(Player Player)
    {
        if (BombarderGame.GameTick - LastDamageFrame < DamageInterval)
        {
            return;
        }

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
            (int)Player.Position.X - (Player.Width / 2),
            (int)Player.Position.Y - (Player.Height / 2)
        );
        Point PlayerTopRight = new Point(
            (int)Player.Position.X + (Player.Width / 2),
            (int)Player.Position.Y - (Player.Height / 2)
        );
        Point PlayerBottomLeft = new Point(
            (int)Player.Position.X - (Player.Width / 2),
            (int)Player.Position.Y + (Player.Height / 2)
        );
        Point PlayerBottomRight = new Point(
            (int)Player.Position.X + (Player.Width / 2),
            (int)Player.Position.Y + (Player.Height / 2)
        );


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
        LastDamageFrame = BombarderGame.GameTick;
    }

    public override void DrawEntity(BombarderGame Game)
    {
        Game.SpriteBatch.Draw(
            Game.Textures.DemonEye.Item1,
            new Rectangle(
                (int)(
                    Position.X -
                    Game.Textures.DemonEye.Item1.Width * 0.8 / 2 +
                    Game.Graphics.PreferredBackBufferWidth / 2F -
                    Game.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    Game.Textures.DemonEye.Item1.Height * 0.8 / 2
                ) +
                Game.Graphics.PreferredBackBufferHeight / 2 - (int)Game.Player.Position.Y,
                (int)(Game.Textures.DemonEye.Item1.Width * 0.8),
                (int)(Game.Textures.DemonEye.Item1.Height * 0.8)
            ),
            Color.White
        );
    }
}