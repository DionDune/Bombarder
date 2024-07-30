using System;
using System.Collections.Generic;
using System.Linq;
using Bombarder.Entities;
using Microsoft.Xna.Framework;

namespace Bombarder.MagicEffects;

public abstract class MagicEffect
{
    public Vector2 Position { get; set; }

    public string? DamageTarget { get; set; }
    public int Damage { get; set; }
    public abstract int ManaCost { get; protected set; }

    public bool RadiusIsCircle { get; set; }
    public float DamageRadius { get; set; }
    public Point RadiusOffset { get; set; }
    public Point RadiusSize { get; set; }
    public int DamageDuration { get; set; }

    public bool HasDuration { get; set; }
    public int Duration { get; set; }
    public Rectangle HitBox => new(Position.ToPoint() + RadiusOffset, RadiusSize + RadiusSize);

    public List<MagicEffectPiece> Pieces { get; set; }

    public static readonly Dictionary<string, Func<Vector2, Player, MagicEffect>> MagicEffectsFactories = new()
    {
        { "DissipationWave", (Position, _) => new DissipationWave(Position) },
        { "ForceContainer", (Position, Player) => new ForceContainer(Player.Position, Position) },
        { "ForceWave", (Position, _) => new ForceWave(Position) },
        { "NonStaticOrb", (Position, Player) => new NonStaticOrb(Player.Position, Position) },
        { "PlayerTeleport", (Position, _) => new PlayerTeleport(Position) },
        { "StaticOrb", (Position, _) => new StaticOrb(Position) },
        { "WideLaser", (Position, Player) => new WideLaser(Player.Position, Position) },
    };

    public static void Update()
    {
        foreach (MagicEffect Effect in BombarderGame.Instance.MagicEffects)
        {
            Effect.EnactEffect(
                BombarderGame.Instance.Player,
                BombarderGame.Instance.Entities,
                BombarderGame.Instance.GameTick
            );
        }
    }

    public static void EnactDuration(List<MagicEffect> Effects)
    {
        List<MagicEffect> DeadEffects = new List<MagicEffect>();

        foreach (var Effect in Effects.Where(Effect => Effect.HasDuration))
        {
            if (Effect.Duration == 0)
            {
                DeadEffects.Add(Effect);
            }
            else
            {
                Effect.Duration--;
            }
        }

        foreach (MagicEffect Effect in DeadEffects)
        {
            Effects.Remove(Effect);
        }
    }

    protected MagicEffect(Vector2 Position)
    {
        this.Position = Position;
        DamageTarget = "Entities";
        Damage = 4;
        DamageDuration = 150;
        RadiusIsCircle = false;
        DamageRadius = 0;
        RadiusOffset = new Point(-24, -24);
        RadiusSize = new Point(24, 24);

        Pieces = new List<MagicEffectPiece> { new() { LifeSpan = DamageDuration } };
    }

    public abstract void EnactEffect(Player Player, List<Entity> Entities, uint GameTick);
    public abstract void Draw();
}