using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class DamagArea : Projectile
    {
        [SerializeField] private CircleArea m_CircleArea;
        Destructible dest;
        [SerializeField] int damage;

        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            var colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, m_CircleArea.Radius);

            foreach (var collider in colliders)
            {
                Destructible dest = collider.transform.root.GetComponent<Destructible>();

                if (dest != null)
                {
                    dest.AplayDamage(damage);
                }
            }

            base.OnProjectileLifeEnd(col, pos);
        }
    }
}

