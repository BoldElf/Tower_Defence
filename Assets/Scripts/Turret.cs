using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter 
{
    /*
    public enum ProjectileMode
    {
        Standart,
        Auto
    }
    */

    
    public class Turret : MonoBehaviour
    {
        /*
        [SerializeField] private ProjectileMode m_Mode1;
            public ProjectileMode Mode1 => m_Mode1;
        */

        [SerializeField] TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        //[SerializeField] private GameObject m_Target;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_ship;

        #region start and update
        private void Start()
        {
            m_ship = transform.root.GetComponent<SpaceShip>();
        }
        private void Update()
        {
            if(m_RefireTimer > 0)
            {
                m_RefireTimer -= Time.deltaTime;
            }
            else if(Mode == TurretMode.Auto)
            {
                Fire();
            }
        }
        #endregion

        //public API

        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if(m_ship)
            {
                if(m_ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                {
                    return;
                }

                if (m_ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)
                {
                    return;
                }
            }

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;
                
            projectile.SetParentShooter(m_ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX
            }

        }

        public void AssignLoadout(TurretProperties props)
        {
            if (m_Mode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;
        }

    }


}

