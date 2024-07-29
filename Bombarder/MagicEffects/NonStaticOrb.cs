using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class NonStaticOrb : MagicEffect
{
    public override int ManaCost { get; protected set; } = 0;
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

    public NonStaticOrb(Vector2 Position, Vector2 Destination) : base(Position)
    {
        Vector2 DestinationDiff = Destination - Position;
        Angle = MathF.Atan2(DestinationDiff.Y, DestinationDiff.X) * 180F / MathF.PI;
        Duration = DefaultDuration;
        Velocity = DefaultVelocity;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        //EnactVelocity
        EnactVelocity();

        //Enact Damage
        foreach (var Entity in Entities.Where(Entity => HitBox.Intersects(Entity.HitBox)))
        {
            Entity.GiveDamage(Damage);
        }
    }

    public override void Draw()
    {
    }

    private void EnactVelocity()
    {
        float AngleRadians = Angle * MathF.PI / 180F;

        Position += new Vector2(
            Velocity * MathF.Cos(AngleRadians),
            Velocity * MathF.Sin(AngleRadians)
        );

        Velocity *= VelocityLoss;

        if (Velocity < 1)
        {
            Velocity = 0;
        }
    }
}