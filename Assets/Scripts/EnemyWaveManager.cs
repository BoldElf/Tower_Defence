using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;
        [SerializeField] private Enemy m_EntityPrefab;
        [SerializeField] private Path[] paths;
        [SerializeField] private EnemyWave currentWave;
        public event Action OnAllWavesDead;

        private int activeEnemyCount = 0;
        private void RecordEnemyDead() 
        { 
            if(--activeEnemyCount == 0)
            { 
                ForceNextWave();
            }
        }

        private void Start()
        {
            currentWave.Prepare(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (currentWave)
            {
                TDPlayer.Instance.ChangeGold((int)currentWave.GetRemainingTime());
                SpawnEnemies();
            }
            else
            {
                if(activeEnemyCount == 0)
                {
                    OnAllWavesDead?.Invoke();
                }
                
            }
        }
        public int CheckWave;
        private void SpawnEnemies()
        {

            foreach ((EnemyAsset asset, int count,int PathIndex) in currentWave.EnumerateSquads())
            {
                if(PathIndex < paths.Length)
                {
                    for(int i = 0; i < count;i++)
                    {
                        if (!this.gameObject.scene.isLoaded) return; // !
                        var e = Instantiate(m_EntityPrefab, paths[PathIndex].StartArea.GetRandomInsideZone(),Quaternion.identity);
                        e.OnEnd += RecordEnemyDead;
                        e.Use(asset);
                        e.GetComponent<TDPatrolController>().SetPath(paths[PathIndex]);
                        activeEnemyCount += 1;
                        OnEnemySpawn?.Invoke(e);
                    }
                    
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }


            currentWave = currentWave.PrepareNext(SpawnEnemies);
            
            if(currentWave == null)
            {
                 CheckWave = 1;
            }
        }
    }
}

