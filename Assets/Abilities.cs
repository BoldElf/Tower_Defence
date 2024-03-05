using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SpaceShooter;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class Abilities : SingletonBase<Abilities>
    {
        [SerializeField] private UpgradeAsset m_Fire;
        [SerializeField] private GameObject m_FirePanel;
        [SerializeField] private Button m_FireButton;
        [SerializeField] private int m_NeedManaFire;
        [SerializeField] private Text m_FireTextNeed;

        [SerializeField] private UpgradeAsset m_Ice;
        [SerializeField] private GameObject m_IcePanel;
        [SerializeField] private Button m_IceButton;
        [SerializeField] private int m_NeedManaIce;
        [SerializeField] private Text m_IceTextNeed;

        private void Start()
        {
            if(Upgrades.GetUpgradeLevel(m_Fire) > 0)
            {
                m_FirePanel.SetActive(true);
            }

            if (Upgrades.GetUpgradeLevel(m_Ice) > 0)
            {
                m_IcePanel.SetActive(true);
            }

            m_FireTextNeed.text = m_NeedManaFire.ToString();
            m_IceTextNeed.text =  m_NeedManaIce.ToString();
        }

        private void Update()
        {
            CheckMana();
        }

        private void CheckMana()
        {
            if(TDPlayer.Instance.Mana < m_NeedManaFire)
            {
                m_FireButton.interactable = false;
                m_FireTextNeed.color = Color.red;
            }
            if (TDPlayer.Instance.Mana >= m_NeedManaFire)
            {
                m_FireButton.interactable = true;
                m_FireTextNeed.color = Color.white;
            }

            if (TDPlayer.Instance.Mana < m_NeedManaIce)
            {
                m_IceButton.interactable = false;
                m_IceTextNeed.color = Color.red;
            }
            if (TDPlayer.Instance.Mana >= m_NeedManaIce)
            {
                m_IceButton.interactable = true;
                m_IceTextNeed.color = Color.white;
            }
        }

        [Serializable]
        public class FireAbility
        {
            [SerializeField] private int m_Cost = 5;
            [SerializeField] private int m_Damage = 2;

            private void CheckBoostFire()
            {
                if(Upgrades.GetUpgradeLevel(Instance.m_Fire) > 1)
                {
                    for (int i = 0; i < Upgrades.GetUpgradeLevel(Instance.m_Fire);i++ )
                    {
                        m_Damage++;
                    }

                }
            }

            private int m_NumberUse = 0;

            public void Use() 
            {
                if (m_NumberUse == 0)
                {
                    CheckBoostFire();
                    m_NumberUse++;
                }
                print(m_Damage);
                ClickProtection.Instance.Activate((Vector2 v) =>
                {
                    TDPlayer.Instance.ChangeMana(Instance.m_NeedManaFire);

                    Vector3 position = v;
                    position.z = -Camera.main.transform.position.z;

                    position = Camera.main.ScreenToWorldPoint(position);

                    foreach(var collider in Physics2D.OverlapCircleAll(position,5))
                    {
                        if(collider.transform.parent.TryGetComponent<Enemy>(out var enemy))
                        {
                            enemy.TakeDamage(m_Damage, TDProjectile.damageType.Magic);
                        }
                    }
                });
                
            }
        }

        [Serializable]
        public class TimeAbility
        {
            [SerializeField] private float m_Cooldown = 15f;
            [SerializeField] private float m_Duration = 5f;

            private void CheckBoostIce()
            {
                if (Upgrades.GetUpgradeLevel(Instance.m_Ice) > 1)
                {
                    for (int i = 0; i < Upgrades.GetUpgradeLevel(Instance.m_Ice); i++)
                    {
                        m_Duration ++;
                    }

                }
            }

            private int m_NumberUse = 0;

            public void Use() 
            {
                TDPlayer.Instance.ChangeMana(Instance.m_NeedManaIce);

                if(m_NumberUse == 0)
                {
                    CheckBoostIce();
                    m_NumberUse++;
                }
                
                print(m_Duration);

                void Slow(Enemy ship)
                {
                    ship.GetComponent<SpaceShip>().HalfMaxLinearVelocity();
                }
                foreach(var ship in FindObjectsOfType<SpaceShip>()) 
                    ship.HalfMaxLinearVelocity(); 
                EnemyWaveManager.OnEnemySpawn += Slow;

                IEnumerator Restore()
                {
                    yield return new WaitForSeconds(m_Duration);
                    foreach (var ship in FindObjectsOfType<SpaceShip>())
                        ship.RestorMaxLinearVelocity();
                    EnemyWaveManager.OnEnemySpawn -= Slow;
                }
                Instance.StartCoroutine(Restore());

                IEnumerator TimeAbilityButton()
                {
                    Instance.m_TimeButton.interactable = false;
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_TimeButton.interactable = true;
                }
                Instance.StartCoroutine(TimeAbilityButton());
            }
            
        }

        [SerializeField] private Button m_TimeButton;
        [SerializeField] private Image m_TargetingCircle;

        [SerializeField] private FireAbility m_FireAbility;
        public void UseFireAbility() => m_FireAbility.Use();
        [SerializeField] private TimeAbility m_TimeAbility;
        public void UseTimeAbility() => m_TimeAbility.Use();
    }
}

