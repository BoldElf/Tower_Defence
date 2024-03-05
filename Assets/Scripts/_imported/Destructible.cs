using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TowerDefence;

namespace SpaceShooter
{
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Обьект игнорирует повреждение
        /// </summary>
        [SerializeField] private bool m_Indestractible;
        public bool IsIndestractible => m_Indestractible;

        /// <summary>
        /// Стартовое к-во хитпоинтов
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int StartHitPoints => m_HitPoints;

        /// <summary>
        /// Текущее к-во хитпоинтов
        /// </summary>
        [SerializeField]private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        public bool m_AddSave;

        static public bool freezing;

        [SerializeField] private UnityEvent m_Spent;

        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }
        #endregion

        #region Public API
        /// <summary>
        /// Применение урона к объекту
        /// </summary>
        /// <param name="damage">Урон наносимый обьекту</param>
        public void AplayDamage(int damage)
        {
            if (m_Indestractible) return;
            if (m_CurrentHitPoints <= 0) return;

            /*  !!! Если использую старый вариант бустера
            if(freezing == true)
            {
                freezing = false;
                m_Spent.Invoke();
                return;
            }
            */

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                OnDeath();
            }
        }
        #endregion

        /// <summary>
        /// Переопределяемое событие уничтожения обьекта, когда хитпоинты ниже или равны 0
        /// </summary>
        protected void OnDeath()
        {
            Destroy(gameObject);
            m_EventOnDeath?.Invoke();
        }

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        private void OnEnable()
        {
            if(m_AllDestructibles == null)
            {
                m_AllDestructibles = new HashSet<Destructible>();
            }
            m_AllDestructibles.Add(this);
        }

        private void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        #region Score
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        #endregion

        protected void Use(EnemyAsset asset)
        {
            m_HitPoints = asset.hp;
            m_ScoreValue = asset.score;
        }
    }
}


