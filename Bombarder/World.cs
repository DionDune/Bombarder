using Bombarder.Entities;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder
{

    public class World
    {
        public readonly List<Entity> Entities;
        public readonly List<Entity> EntitiesToAdd;
        public readonly List<Particle> Particles;
        public readonly List<MagicEffect> MagicEffects;
        public readonly List<MagicEffect> SelectedEffects;


        public World()
        {
            Entities = new();
            EntitiesToAdd = new();
            Particles = new();
            MagicEffects = new();
            SelectedEffects = new();
        }

        public void Reset()
        {
            Entities.Clear();
            EntitiesToAdd.Clear();
            Particles.Clear();
            MagicEffects.Clear();
            SelectedEffects.Clear();
        }
    }
}
