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
                Color = Color.White
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
            BombarderGame.Instance.Textures.DemonEye.Item1,
            new Rectangle(
                (int)(
                    Position.X -
                    BombarderGame.Instance.Textures.DemonEye.Item1.Width * 0.8 / 2 +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    BombarderGame.Instance.Textures.DemonEye.Item1.Height * 0.8 / 2
                    +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                (int)(BombarderGame.Instance.Textures.DemonEye.Item1.Width * 0.8),
                (int)(BombarderGame.Instance.Textures.DemonEye.Item1.Height * 0.8)
            ),
            Color.White
        );
    }
}