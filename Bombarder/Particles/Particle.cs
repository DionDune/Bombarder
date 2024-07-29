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

    public static void EnactDuration(List<Particle> Particles)
    {
        List<Particle> Dead = new List<Particle>();

        foreach (var Particle in Particles.Where(Particle => Particle.HasDuration))
        {
            if (Particle.Duration <= 0)
            {
                Dead.Add(Particle);
            }
            else
            {
                Particle.Duration--;
            }
        }

        foreach (Particle Particle in Dead)
        {
            Particles.Remove(Particle);
        }
    }

    public static void Update(List<Particle> Particles, uint Tick)
    {
        foreach (Particle Particle in Particles)
        {
            Particle.Update(Tick);
        }
    }

    public static void SpawnParticles(List<Particle> Particles, Vector2 PlayerPos, GraphicsDeviceManager Graphics,
        uint Tick)
    {
        int ClutterSpawnRangeX = Graphics.PreferredBackBufferWidth;
        int ClutterSpawnRangeY = Graphics.PreferredBackBufferHeight;

        Dust.Spawn(Particles, PlayerPos, ClutterSpawnRangeX, ClutterSpawnRangeY, Tick);
    }

    public virtual void Update(uint Tick)
    {
    }

    public abstract void Draw();
}