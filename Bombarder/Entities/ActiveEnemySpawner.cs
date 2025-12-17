using Bombarder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder.Entities
{
    public class ActiveEnemySpawner
    {
        private uint NextEnemySpawnFrame;
        private (int Min, int Max) EnemySpawnDelay = (120, 400);
        private int SpawnExtraEnemyChance = 50;


        public ActiveEnemySpawner()
        {
            NextEnemySpawnFrame = BombarderGame.Instance.GameTick;
        }

        public void Update()
        {
            // Spawn Enemy
            if (NextEnemySpawnFrame <= BombarderGame.Instance.GameTick && BombarderGame.Instance.World.EnemySpawnerIsActive)
            {
                SpawnRandomEnemy();

                // Spawn Extra
                int SpawnAgainChange = RngUtils.Random.Next(0, 100);
                while (SpawnAgainChange < SpawnExtraEnemyChance)
                {
                    SpawnRandomEnemy();
                    SpawnAgainChange = RngUtils.Random.Next(0, 100);
                }


                // Set new spawn frame
                NextEnemySpawnFrame = (uint)(BombarderGame.Instance.GameTick + RngUtils.Random.Next(EnemySpawnDelay.Min, EnemySpawnDelay.Max));
            }
        }
        public void SpawnRandomEnemy()
        {
            // Spawn Change Ranges:
            // -  Red Cube: 00 - 59
            // - Demon Eye: 60 - 94
            // -    Spider: 97 - 100


            int EnemySpawnChange = RngUtils.Random.Next(0, 101);

            if (EnemySpawnChange >= 0 && EnemySpawnChange <= 59)
            {
                // Red Cube
                BombarderGame.Instance.World.SpawnEnemy<RedCube>();
            }
            else if (EnemySpawnChange >= 60 && EnemySpawnChange <= 94)
            {
                // Demon Eye
                BombarderGame.Instance.World.SpawnEnemy<DemonEye>();
            }
            else if (EnemySpawnChange >= 97 && EnemySpawnChange <= 100)
            {
                // Spider
                BombarderGame.Instance.World.SpawnEnemy<Spider>();
            }
        }
    }
}
