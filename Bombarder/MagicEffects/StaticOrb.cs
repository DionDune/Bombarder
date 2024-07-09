﻿using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class StaticOrb : MagicEffect
{
    const int Damage = 2;
    public static readonly int ManaCost = 399;

    public const int DefaultDuration = 150;

    public uint LastParticleFrame;

    public StaticOrb(Vector2 Position) : base(Position)
    {
        HasDuration = true;
        LastParticleFrame = 0;
        Duration = DefaultDuration;
    }

    public override void EnactEffect(Player Player, List<Entity> Entities, uint GameTick)
    {
        Point EffectStart;
        Point EffectEnd;

        foreach (Entity Entity in Entities)
        {
            EffectStart = new Point((int)Position.X + RadiusOffset.X, (int)Position.Y + RadiusOffset.Y);
            EffectEnd = new Point(EffectStart.X + RadiusSize.X, EffectStart.Y + RadiusSize.Y);

            if (CheckCollision(EffectStart, EffectEnd, Entity))
            {
                Entity.GiveDamage(Damage);
            }
        }

        CreateParticles();
    }

    private void CreateParticles()
    {
        if (Math.Abs(Game1.GameTick - LastParticleFrame) <= Particle.Impact.DefaultFrequency)
        {
            return;
        }

        Game1.Particles.Add(new Particle((int)Position.X, (int)Position.Y)
        {
            HasDuration = true,
            Duration = Particle.Impact.DurationDefault,

            ParticleObj = new Particle.Impact(1)
            {
                Radius = Particle.Impact.DefaultRadius,
                Opacity = Particle.Impact.DefaultOpacity
            }
        });

        LastParticleFrame = Game1.GameTick;
    }
}