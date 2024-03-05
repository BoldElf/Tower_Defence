using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWave: MonoBehaviour
    {
        [Serializable]
        private class Squad
        {
            public EnemyAsset asset;
            public int count;
        }

        [Serializable]
        private class PathGroup
        {
            public Squad[] squads;
        }

        [SerializeField] private PathGroup[] groups; 


        [SerializeField] private float prepareTime = 10;

        public static Action<float> OnWavePrepare;
        public float GetRemainingTime() { return prepareTime - Time.time; }

        private void Awake()
        {
            enabled = false;
        }

        public IEnumerable<(EnemyAsset asset, int count, int PathIndex)> EnumerateSquads()
        {
            for(int i = 0; i < groups.Length;i++)
            {
                foreach(var squad in groups[i].squads)
                {
                    yield return (squad.asset, squad.count, i);
                }
            }

            
        }

        private Action OnWaveReady;

        public void Prepare(Action spawnEnemies)
        {

            OnWavePrepare?.Invoke(prepareTime);
            prepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        private void Update()
        {
            if(Time.time >= prepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
        }

        [SerializeField] private EnemyWave next;

        internal EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;
            if (next != null)
            {
                next.Prepare(spawnEnemies);
            }
            return next;
        }
    }
}