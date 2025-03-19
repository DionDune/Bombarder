using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bombarder.Entities;

public class DemonEye : Entity
{
    public const int Damage = 15;
    public const int DamageInterval = 10;
    public uint LastDamageFrame;

    public const float BaseSpeed = 4;

    public DemonEye(Vector2 Position) : base(Position)
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
        var Game = BombarderGame.Instance;
        var Texture = Game.Textures.DemonEye.Item1;
        var TextureSize = new Vector2(Texture.Width, Texture.Height) * 0.8F;

        Game.SpriteBatch.Draw(
            Game.Textures.DemonEye.Item1,
            MathUtils.CreateRectangle(
                Position - TextureSize / 2F + Game.ScreenCenter - Game.Player.Position,
                TextureSize
            ),
            Color.White
        );
    }
}