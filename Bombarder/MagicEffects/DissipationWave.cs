using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class DissipationWave : MagicEffect
{
    public Color Colour = Color.MediumPurple;
    public override int ManaCost { get; protected set; } = 50;
    public float Damage { get; set; }
    public const int DefaultDuration = 150;
    private const float DefaultDamage = 10;
    private const float DamageMultiplier = 0.992F;

    public float Radius { get; set; }
    private const float DefaultRadius = 5;
    private const float RadiusSpread = 5;
    private const float EdgeEffectWith = 10;

    public float Opacity { get; set; }
    private const float DefaultOpacity = 0.95F;
    private const float OpacityMultiplier = 0.98F;

    public DissipationWave(Vector2 Position) : base(Position)
    {
        Damage = DefaultDamage;
        Radius = DefaultRadius;
        Opacity = DefaultOpacity;
        Duration = DefaultDuration;
        HasDuration = true;
    }


    public override void Update(Player Player, List<Entity> Entities, uint GameTick)
    {
        base.Update(Player, Entities, GameTick);
        EnactSpread();
        EnactDamage(Entities);
    }

    public override void DrawEffect()
    {
        BombarderGame.Instance.SpriteBatch.Draw(
            BombarderGame.Instance.Textures.WhiteCircle,
            new Rectangle(
                (int)(
                    Position.X -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferWidth / 2F -
                    BombarderGame.Instance.Player.Position.X
                ),
                (int)(
                    Position.Y -
                    Radius +
                    BombarderGame.Instance.Graphics.PreferredBackBufferHeight / 2F -
                    BombarderGame.Instance.Player.Position.Y
                ),
                (int)Radius * 2,
                (int)Radius * 2
            ),
            Colour * Opacity
        );
    }

    private void EnactDamage(List<Entity> Entities)
    {
        foreach (Entity Entity in Entities)
        {
            Vector2 Diff = MathUtils.Abs(Position - Entity.Position);
            float Distance = MathUtils.HypotF(Diff);

            if (Math.Abs(Radius - Distance) <= EdgeEffectWith)
            {
                Entity.GiveDamage((int)Damage);
            }
        }
    }

    private void EnactSpread()
    {
        Radius += RadiusSpread;
        Opacity *= OpacityMultiplier;
        Damage *= DamageMultiplier;
    }
}