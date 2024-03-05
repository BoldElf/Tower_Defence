using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class NextWaveGUI : MonoBehaviour
    {
        //[SerializeField] private Text bonusAmount;
        [SerializeField] private Text NewWave;
        private EnemyWaveManager manager;

        private float timeToNext;

        void Start()
        {
            manager = FindObjectOfType<EnemyWaveManager>();
            EnemyWave.OnWavePrepare += (float time) =>
            {
                timeToNext = time;
            };
 
        }

        public void CallNext()
        {
            manager.ForceNextWave();
        }
        private void Update()
        {
            var bonus = (int)timeToNext;
            if (bonus < 0) bonus = 0;
            NewWave.text = bonus.ToString();
            //bonusAmount.text = (bonus).ToString();
            timeToNext -= Time.deltaTime;

            if(manager.CheckWave == 1)
            {
                NewWave.text = 0.ToString();
                print("b");
            }
        }

    }
}

