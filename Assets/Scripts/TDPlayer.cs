using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefence
{
    public class TDPlayer : Player
    {
        [SerializeField] private int m_Gold = 0;
        [SerializeField] private int armor = 0;
        [SerializeField] private int m_Mana = 0;
        public int Mana => m_Mana;

        private event Action<int> OnGoldUpdate;
        private event Action<int> OnArmorUpdate;
        private event Action<int> OnManaUpdate;

        public void ManaUpdateSubscribe(Action<int> act)
        {
            OnManaUpdate += act;
            act(m_Mana);
        }

        public void GoldUpdateSubscribe(Action<int> act)
        {
            OnGoldUpdate += act;
            //act(Instance.m_Gold);
            act(m_Gold);
        }
        public void ArmorUpdateSubscribe(Action<int> act)
        {
            OnArmorUpdate += act;
            //act(Instance.armor);
            act(armor);
        }

        public event Action<int> OnLifeUpdate;

        public void LifeUpdateSubscribe(Action<int> act)
        {
            OnLifeUpdate += act;
            //act(Instance.NumLives);
            act(NumLives);
        }

        public static new TDPlayer Instance
        {
            get
            {
                return Player.Instance as TDPlayer;
            }

        }

        public void ChangeGold(int m_Change)
        {
            m_Gold += m_Change;
            OnGoldUpdate(m_Gold);
        }
        public void ChangeMana(int m_ManaNeed)
        {
            m_Mana -= m_ManaNeed;
            OnManaUpdate(m_Mana);
        }
        public void ReduceLife(int m_Change)
        {
            
            if(armor >= m_Change)
            {
                armor = armor - m_Change;
                m_Change = 0;

                if(armor < 0)
                {
                    m_Change = armor;
                    armor = 0;
                }
            }
            else
            {
                m_Change -= armor;
                armor = 0;
            }
            OnArmorUpdate(armor);

            TakeDamage(m_Change);
            OnLifeUpdate(NumLives);
        }

        [SerializeField] private Tower m_TowerPrefab;
        
        public void TryBuld(TowerAsset m_TowerAsset, Transform m_buildSite)
        {
            ChangeGold(-m_TowerAsset.goldCost);
            var tower = Instantiate(m_TowerPrefab, m_buildSite.position, Quaternion.identity);
            tower.Use(m_TowerAsset);
            Destroy(m_buildSite.gameObject);
        }

        [SerializeField] private UpgradeAsset healthUpgrade;
        [SerializeField] private UpgradeAsset ArmorUpgrade;
        [SerializeField] private UpgradeAsset GoldUpgrade;

        private void Start()
        {
            //base.Awake();
            var level = Upgrades.GetUpgradeLevel(healthUpgrade);
            TakeDamage((-level) * 5);

            var level_02 = Upgrades.GetUpgradeLevel(ArmorUpgrade);
            armor = (level_02 * 2);

            var level_03 = Upgrades.GetUpgradeLevel(GoldUpgrade);
            
            m_Gold += (level_03 * 10);
        }


        private float time;
        [SerializeField] private float m_TimeSpawnMana;
        private void Update()
        {
            time += Time.deltaTime;
            if(time >= m_TimeSpawnMana)
            {
                ChangeMana(-1);
                time = 0;
            }
        }
    }
}


