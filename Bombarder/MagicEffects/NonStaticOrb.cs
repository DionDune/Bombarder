using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class NonStaticOrb : MagicEffect
{
    public const int DefaultDuration = 150;
    public float Angle { get; set; }
    public float Velocity { get; set; }
    public const float VelocityLoss = 0.95F;
    public const float DefaultVelocity = 25;

    public NonStaticOrb(Vector2 Position, float Angle) : base(Position)
    {
        this.Angle = Angle;
        Duration = DefaultDuration;
        Velocity = DefaultVelocity;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        //EnactVelocity
        EnactVelocity();

        //Enact Damage
        foreach (Entity Entity in Entities)
        {
            Point EffectStart = Position.ToPoint() + RadiusOffset;
            Point EffectEnd = EffectStart + RadiusSize;

            if (CheckCollision(EffectStart, EffectEnd, Entity))
            {
                Entity.GiveDamage(Damage);
            }
        }
    }

    public override void Draw()
    {
    }

    private void EnactVelocity()
    {
        float AngleRadians = Angle * (float)(Math.PI / 180);

        Position += new Vector2(
            (int)(Velocity * (float)Math.Cos(AngleRadians)),
            (int)(Velocity * (float)Math.Sin(AngleRadians))
        );

        Velocity *= VelocityLoss;

        if (Velocity < 1)
        {
            Velocity = 0;
        }
    }
}