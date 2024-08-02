using System.Collections.Generic;
using System.Linq;
using Bombarder.Particles.Dusts;
using Microsoft.Xna.Framework;

namespace Bombarder.Particles;

public abstract class Particle
{
    public bool HasDuration { get; set; }
    public int Duration { get; set; }

    public Vector2 Position { get; set; }
    public bool DrawLater { get; protected set; } = false;


    protected Particle(Vector2 position)
    {
        Position = position;

        HasDuration = false;
        Duration = 0;
    }

    public static void SpawnParticles(List<Particle> Particles, Vector2 PlayerPos, uint Tick)
    {
        var Game = BombarderGame.Instance;

        Dust.Spawn(Particles, PlayerPos, Game.ScreenSize, Tick);
    }

    public virtual void Update(uint Tick)
    {
        if (HasDuration)
        {
            Duration--;
        }
    }

    public abstract void Draw();

    public virtual bool ShouldDelete()
    {
        return HasDuration && Duration <= 0;
    }
}