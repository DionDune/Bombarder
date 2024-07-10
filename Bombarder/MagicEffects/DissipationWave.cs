using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class DissipationWave : MagicEffect
{
    public Color Colour = Color.MediumPurple;

    public static readonly int ManaCost = 50;

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


    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        EnactSpread();
        EnactDamage(Entities);
    }

    public override void Draw(Game1 Game1)
    {
        Game1.SpriteBatch.Draw(
            Game1.Textures.WhiteCircle,
            new Rectangle(
                (int)(Position.X - Radius + Game1.Graphics.PreferredBackBufferWidth / 2F - Game1.Player.Position.X),
                (int)(Position.Y - Radius + Game1.Graphics.PreferredBackBufferHeight / 2F - Game1.Player.Position.Y),
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
            float XDiff = Math.Abs(Position.X - Entity.Position.X);
            float YDiff = Math.Abs(Position.Y - Entity.Position.Y);
            float Distance = (float)Math.Sqrt(Math.Pow(XDiff, 2) + Math.Pow(YDiff, 2));

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