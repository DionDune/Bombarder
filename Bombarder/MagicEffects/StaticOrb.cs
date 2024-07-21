using System;
using System.Collections.Generic;
using Bombarder.Entities;
using Bombarder.Particles;
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
            EffectStart = Position.ToPoint() + RadiusOffset;
            EffectEnd = EffectStart + RadiusSize;

            if (CheckCollision(EffectStart, EffectEnd, Entity))
            {
                Entity.GiveDamage(Damage);
            }
        }

        CreateParticles();
    }

    public override void Draw(BombarderGame Game)
    {
    }

    private void CreateParticles()
    {
        if (Math.Abs(BombarderGame.GameTick - LastParticleFrame) <= Impact.DefaultFrequency)
        {
            return;
        }

        BombarderGame.Particles.Add(new Impact(Position.Copy())
        {
            HasDuration = true,
        });

        LastParticleFrame = BombarderGame.GameTick;
    }
}