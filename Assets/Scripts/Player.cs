using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        public event Action OnPlayerDead;

        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;
        [SerializeField] private GameObject m_EffectDeath;

        public SpaceShip ActiveShip => m_Ship;

        //[SerializeField] private CameraController m_CameraController;
        //[SerializeField] private MovementController m_MovementController;


        protected override void Awake()
        {
            base.Awake();
            if(m_Ship != null)
            {
                Destroy(m_Ship.gameObject);
            }
        }


        private void Start()
        {
            if(m_Ship)
            {
                Respawn();
            }
            
            //m_Ship.EventOnDeath.AddListener(OnShopDeath);
        }

        protected void TakeDamage(int m_Damage)
        {
            m_NumLives -= m_Damage;
            if(m_NumLives <= 0)
            {
                m_NumLives = 0;
                OnPlayerDead?.Invoke();
                //LevelSequenceController.Instance.FinishCurrentLevel(false);
            }
        }

        [SerializeField] private float delayTime;

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(delayTime);
            Respawn();
        }
        private void OnShopDeath()
        {
            m_NumLives--;

            if(m_Ship != null)
            {
                var effect = Instantiate(m_EffectDeath, new Vector3(m_Ship.transform.position.x,m_Ship.transform.position.y), Quaternion.identity);
                Destroy(effect,1);
            }

            if (m_NumLives > 0)
            {
                StartCoroutine(WaitCoroutine());
            }
            else
            {
                //LevelSequenceController.Instance.FinishCurrentLevel(false);
                LevelSequenceController.Instance.RestartLevel();
            }

        }

        private void Respawn()
        {
            if(LevelSequenceController.PlayerShip != null)
            {
                //var newPlayerShip = Instantiate(m_PlayerShipPrefab);

                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                //m_CameraController.SetTarget(m_Ship.transform);
                //m_MovementController.SetTargetShip(m_Ship);

                m_Ship.EventOnDeath.AddListener(OnShopDeath); // !
            }

            
        }

        #region Score

        public int Score { get; private set; }
        public int NumKills { get; private set; }

        public void AddKill()
        {
            NumKills++;
        }

        public void AddScore(int num)
        {
            Score += num;
        }

        #endregion
    }

}

