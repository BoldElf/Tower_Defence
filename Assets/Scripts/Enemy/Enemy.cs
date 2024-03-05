using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

namespace TowerDefence
{
    [RequireComponent(typeof(Destructible))]
    [RequireComponent(typeof(TDPatrolController))]
    public class Enemy : MonoBehaviour
    {
        public enum ArmorType { Base = 0, Magic = 1}
        private static Func<int, TDProjectile.damageType,int,int>[] ArmorDamageFunctions = 
        {
            // ArmorType.Base
            (int power,TDProjectile.damageType type, int armor ) =>
            {
                switch(type)
                {
                    case TDProjectile.damageType.Magic: return power;
                    default: return Mathf.Max(power - armor,1);
                }
            },
            // ArmorType.Magic
            (int power,TDProjectile.damageType type, int armor ) =>
            {
                if(TDProjectile.damageType.Base == type)
                {
                    armor = armor / 2;
                }
                return Mathf.Max(power - armor,1);
            },

        };

        [SerializeField] private int m_Damage = 1;
        [SerializeField] private int m_Gold;
        [SerializeField] private int m_Armor = 0;
        [SerializeField] private ArmorType m_ArmorType; 

        

        public event Action OnEnd;

        private Destructible m_Destructible;
        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
        }

        private void OnDestroy()
        {
            OnEnd?.Invoke();
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("VisualModel").GetComponent<SpriteRenderer>();
            sr.color = asset.color;

            sr.transform.localScale = new Vector3(asset.spriteScale.x, asset.spriteScale.y, 1);

            sr.GetComponent<Animator>().runtimeAnimatorController = asset.animations;

            GetComponent<SpaceShip>().Use(asset);

            GetComponentInChildren<CircleCollider2D>().radius = asset.radius;

            m_Armor = asset.armor;
            m_ArmorType = asset.armorType;
            m_Damage = asset.damage;
            m_Gold = asset.gold;
        }

        public void DamagePlayer()
        {
            TDPlayer.Instance.ReduceLife(m_Damage);
        }

        public void GiveGold()
        {
            TDPlayer.Instance.ChangeGold(m_Gold);
        }

        public void TakeDamage(int damage, TDProjectile.damageType damageType)
        {
            m_Destructible.AplayDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
        }
    }
#if UNITY_EDITOR    
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EnemyAsset a = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;
            if(a)
            {
                (target as Enemy).Use(a);
            }
        }
    }
#endif 

}

