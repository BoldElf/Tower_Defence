using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("Space Ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперед сила.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращаюшая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;

       

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        private float m_VelocityBackup;
        public void HalfMaxLinearVelocity() 
        { 
            m_VelocityBackup = m_MaxLinearVelocity; 
            m_MaxLinearVelocity /= 2;
        }
        public void RestorMaxLinearVelocity() { m_MaxLinearVelocity = m_VelocityBackup;  }

        public float MaxLinearVelocity => m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращательная скорость. В градусах / сек.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Сохраненая сылка на регид.
        /// </summary>
        private Rigidbody2D m_Rigid;

        //[SerializeField] CollisionDamageApplicator CDA;

        #region PublicAPI

        /// <summary>
        /// Управления линейной тягой.  от -1.0 до + 1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. от -1.0 до + 1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            //InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();
            //UpdateEnergyRegen();
        }
        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения.
        /// </summary>

        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        /*

        private float m_timerSave;
        private float m_timerSpeed;

        private bool m_TimerSaveOnOff = false;
        private bool m_TimerSpeedOnOff = false;

        private float m_CurrentDamag;
        private float m_CurrentSpeed;
        */

        private void Update()
        {
            /*
            if(m_TimerSaveOnOff == true)
            {
                m_timerSave += Time.deltaTime;

                if(m_timerSave >= 2.0f)
                {
                    //DeletSave();
                    m_timerSave = 0.0f;
                    m_TimerSaveOnOff = false;
                }
            }

            if (m_TimerSpeedOnOff == true)
            {
                m_timerSpeed += Time.deltaTime;

                if (m_timerSpeed >= 2.0f)
                {
                    DeletSpeed();
                    m_timerSpeed = 0.0f;
                    m_TimerSpeedOnOff = false;
                }
            }
            */

        }

        /*
        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for(int i = 0; i < m_Turrets.Length; i++)
            {
                if(m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int e)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + e, 0, m_MaxAmmo);
        }

        public void AddSpeed(float m_BonusSpeed)
        {
            m_CurrentSpeed = m_Thrust;
            m_Thrust = m_BonusSpeed;
            m_TimerSpeedOnOff = true;
        }

        public void DeletSpeed()
        {
            m_Thrust = m_CurrentSpeed;
        }

        /*
        public void AddSave(float damage)
        {
            m_CurrentDamag = CDA.m_DamageConstant;
            CDA.m_DamageConstant = damage;
            m_TimerSaveOnOff = true;
        }

        public void DeletSave()
        {
            CDA.m_DamageConstant = m_CurrentDamag;
        }
        

        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float) m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy,0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count)
        {
            if(count == 0)
            {
                return true;
            }
            if(m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0)
            {
                return true;
            }
            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }
        
        
        public void AssignWeapon(TurretProperties props)
        {
            for(int i = 0; i < m_Turrets.Length;i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
        */

        /// <summary>
        /// TODO: Заменить временный метод заглушку.
        /// Используется турелями.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>

        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: Заменить временный метод заглушку.
        /// Используется турелями.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        
        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: Заменить временный метод заглушку.
        /// Используется ИИ.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>

        public void Fire(TurretMode mode)
        {
            return;
        }

        new public void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.moveSpeed;
            base.Use(asset);
        }
    }
        
}
   
