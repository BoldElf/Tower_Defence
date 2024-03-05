using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class TDProjectile : Projectile
    {
        public enum damageType { Base , Magic }
        [SerializeField] private damageType m_DamageType;
        [SerializeField] private Sound m_ShotSound = Sound.Arrow;
        [SerializeField] private Sound m_HitSound = Sound.ArrowHit;

        private void Start()
        {
            m_ShotSound.Play();
        }

        protected override void OnHit(RaycastHit2D hit)
        {
           var enemy = hit.collider.transform.root.GetComponent<Enemy>();

           if (enemy != null)
           {
                m_HitSound.Play();
               enemy.TakeDamage(m_Damage, m_DamageType);
           }
        }
       
    }
}

