using Bombarder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bombarder.Entities
{
    public class ActiveEnemySpawner
    {
        private uint NextEnemySpawnFrame;
        private (int Min, int Max) EnemySpawnDelay = (150, 600);
        private int SpawnExtraEnemyChance = 40;


        public ActiveEnemySpawner()
        {
            NextEnemySpawnFrame = BombarderGame.Instance.GameTick;
        }

        public void Update()
        {
            // Spawn Enemy
            if (NextEnemySpawnFrame <= BombarderGame.Instance.GameTick)
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
            int SpawnCount =
                RngUtils.Random.Next(BombarderGame.Instance.Settings.EnemySpawnCountRange.Item1, BombarderGame.Instance.Settings.EnemySpawnCountRange.Item2 + 1);

            for (int i = 0; i < SpawnCount; i++)
            {
                if (RngUtils.Random.Next(0, 4) == 0)
                {
                    // Demon Eye
                    BombarderGame.Instance.World.SpawnEnemy<DemonEye>();
                }
                else
                {
                    // Red Cube
                    BombarderGame.Instance.World.SpawnEnemy<RedCube>();
                }
            }
        }
    }
}
