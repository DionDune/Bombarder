using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Bombarder.Particles;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public class StaticOrb : MagicEffect
{
    const int Damage = 2;
    public override int ManaCost { get; protected set; } = 399;
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
        foreach (var Entity in Entities.Where(Entity => HitBox.Intersects(Entity.HitBox)))
        {
            Entity.GiveDamage(Damage);
        }

        CreateParticles();
    }

    public override void Draw()
    {
    }

    private void CreateParticles()
    {
        if (Math.Abs(BombarderGame.Instance.GameTick - LastParticleFrame) <= Impact.DefaultFrequency)
        {
            return;
        }

        BombarderGame.Instance.Particles.Add(new Impact(Position.Copy())
        {
            HasDuration = true,
        });

        LastParticleFrame = BombarderGame.Instance.GameTick;
    }
}