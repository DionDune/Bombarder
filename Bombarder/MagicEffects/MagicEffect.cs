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

    public bool RadiusIsCircle { get; set; }
    public float DamageRadius { get; set; }
    public Point RadiusOffset { get; set; }
    public Point RadiusSize { get; set; }
    public int DamageDuration { get; set; }

    public bool HasDuration { get; set; }
    public int Duration { get; set; }

    public List<MagicEffectPiece> Pieces { get; set; }


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

    public static bool CheckCollision(Point Coord1, Point Coord2, Entity Entity)
    {
        Vector2 HitboxStart = Entity.Position + Entity.HitBoxOffset.ToVector2();


        // Effect Hitbox is smaller than the Entity Hitbox
        if (Coord2.X - Coord1.X < Entity.HitBoxSize.X &&
            Coord2.Y - Coord1.Y < Entity.HitBoxSize.Y)
        {
            if (Coord1.X >= HitboxStart.X && Coord1.X <= HitboxStart.X + Entity.HitBoxSize.X &&
                Coord1.Y >= HitboxStart.Y && Coord1.Y <= HitboxStart.Y + Entity.HitBoxSize.Y)
            {
                return true;
            }

            if (Coord2.X >= HitboxStart.X && Coord2.X <= HitboxStart.X + Entity.HitBoxSize.X &&
                Coord1.Y >= HitboxStart.Y && Coord1.Y <= HitboxStart.Y + Entity.HitBoxSize.Y)
            {
                return true;
            }

            if (Coord2.X >= HitboxStart.X && Coord2.X <= HitboxStart.X + Entity.HitBoxSize.X &&
                Coord2.Y >= HitboxStart.Y && Coord2.Y <= HitboxStart.Y + Entity.HitBoxSize.Y)
            {
                return true;
            }

            if (Coord1.X >= HitboxStart.X && Coord1.X <= HitboxStart.X + Entity.HitBoxSize.X &&
                Coord2.Y >= HitboxStart.Y && Coord2.Y <= HitboxStart.Y + Entity.HitBoxSize.Y)
            {
                return true;
            }
        }
        // Effect Entity is smaller than the Effect Hitbox
        else
        {
            if (HitboxStart.X >= Coord1.X && HitboxStart.X <= Coord2.X &&
                HitboxStart.Y >= Coord1.Y && HitboxStart.Y <= Coord2.Y)
            {
                return true;
            }

            if (HitboxStart.X + Entity.HitBoxSize.X >= Coord1.X && HitboxStart.X + Entity.HitBoxSize.X <= Coord2.X &&
                HitboxStart.Y >= Coord1.Y && HitboxStart.Y <= Coord2.Y)
            {
                return true;
            }

            if (HitboxStart.X + Entity.HitBoxSize.X >= Coord1.X && HitboxStart.X + Entity.HitBoxSize.X <= Coord2.X &&
                HitboxStart.Y + Entity.HitBoxSize.Y >= Coord1.Y && HitboxStart.Y + Entity.HitBoxSize.Y <= Coord2.Y)
            {
                return true;
            }

            if (HitboxStart.X >= Coord1.X && HitboxStart.X <= Coord2.X &&
                HitboxStart.Y + Entity.HitBoxSize.Y >= Coord1.Y && HitboxStart.Y + Entity.HitBoxSize.Y <= Coord2.Y)
            {
                return true;
            }
        }

        return false;
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
    public abstract void Draw(BombarderGame Game);
}