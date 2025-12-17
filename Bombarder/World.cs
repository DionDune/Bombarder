using Bombarder.Entities;
using Bombarder.MagicEffects;
using Bombarder.Particles;
using Microsoft.Xna.Framework;
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

        public ActiveEnemySpawner EnemySpawner;


        public World()
        {
            EnemySpawner = new ActiveEnemySpawner();
            Entities = new();
            EntitiesToAdd = new();
            Particles = new();
            MagicEffects = new();
            SelectedEffects = new();
        }
        public void Update()
        {
            EnemySpawner.Update();
        }
        public void Reset()
        {
            Entities.Clear();
            EntitiesToAdd.Clear();
            Particles.Clear();
            MagicEffects.Clear();
            SelectedEffects.Clear();
        }


        public void SpawnEnemy<T>(Vector2? Location = null) where T : Entity
        {
            Vector2 SpawnPoint = Location ?? RngUtils.GetRandomSpawnPoint();

            var Factory = Entity.EntityFactories[typeof(T).Name];
            var Enemy = Factory?.Invoke(SpawnPoint);

            if (Enemy == null)
            {
                return;
            }

            EntitiesToAdd.Add(Enemy);
        }
        
    }
}
