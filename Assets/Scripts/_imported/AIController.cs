using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class AIController : MonoBehaviour
    {
        public enum AIBehavior
        {
            Null,
            Patrol
        }
        public enum fight
        {
            Player,
            Bots
        }

        [SerializeField] private fight m_Fight;

        [SerializeField] private AIBehavior m_AIBehavior;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;
        [SerializeField] private float m_FindNewTargetTime;
        [SerializeField] private float m_ShootDelay;
        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private float m_PointTime;

        [SerializeField] private float m_EvadeTime; //!

        private SpaceShip m_SpaceShip;
        private Vector3 m_MovePosition;
        private Destructible m_SelectedTarget;

        /*
        [SerializeField]private GameObject m_SelectedTargetPoint;
        [SerializeField] private GameObject m_SelectedTargetPoint_01;
        [SerializeField] private GameObject m_SelectedTargetPoint_02;
        [SerializeField] private GameObject m_SelectedTargetPoint_03;
        */

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private Timer m_PointTimer;

        private Timer m_EvadeTimer; // !

        //private bool isEvade;

        //[SerializeField] private Rigidbody2D rb;

        //public Vector2 Speed => rb.velocity;


        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();

        }

        private void UpdateAI()
        {
            /*
            if (m_AIBehavior == AIBehavior.Patrol || m_AIBehavior == AIBehavior.Point)
            {
                UpdateBehaviourPatrol();
            }
            */
            if (m_AIBehavior == AIBehavior.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        private void UpdateBehaviourPatrol()
        {
            //ActionEvadeCollision();
            ActionFindNewMovePosition();
            ActionConrolShip();
            ActionFindNewAttackTarget();
            ActionFire();
        }
        private bool abc;
        private bool bce;
        private void ActionFindNewMovePosition()
        {
            //if (isEvade) return;

            if (m_AIBehavior == AIBehavior.Patrol)
            {
                if(m_SelectedTarget !=null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                {
                    if(m_PatrolPoint != null)
                    {
                        bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                        if (isInsidePatrolZone == true)
                        {
                            GetNewPoint();
                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint.transform.position;
                        }
                    }
                }
            }
            
        }

        protected virtual void GetNewPoint()
        {
            if (m_RandomizeDirectionTimer.IsFinished == true)
            {
                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                m_MovePosition = newPoint;
                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
            }
        }

        private void ActionEvadeCollision()
        {
            
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;

                //isEvade = true;
                m_EvadeTimer.Start(m_EvadeTime);

            }
            else
            {
                if(m_EvadeTimer.IsFinished)
                {
                    //isEvade = false;
                }
            }
        
            
            /*
                if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
                {
                    m_MovePosition = transform.position + transform.right * 100.0f;
                }
            */

            }


        private void OnDestroy()
        {
            //Player.Instance.AddKill();
        }

        private void ActionConrolShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl  = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }
        
        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructbleTarget();

                m_FindNewTargetTimer.Start(m_FindNewTargetTime);
            }
            
        }
        private void ActionFire()
        {
            
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }

        }

        private Destructible FindNearestDestructbleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach(var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;
                if (v.TeamId == Destructible.TeamIdNeutral) continue;
                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if(dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition,Vector3.up,Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;


            return -angle;
        }

        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehavior = AIBehavior.Patrol;
            m_PatrolPoint = point;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);

            m_PointTimer = new Timer(m_PointTime); // !
            m_EvadeTimer = new Timer(m_EvadeTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

            m_PointTimer.RemoveTime(Time.deltaTime);
            m_EvadeTimer.RemoveTime(Time.deltaTime);
        }

        
        private void MakeLead()
        {
            Rigidbody2D abc = m_SelectedTarget.GetComponent<Rigidbody2D>();
            m_MovePosition = (Vector2)m_SelectedTarget.transform.position + (abc.velocity * 2.0f);
        }

        #endregion
    }
}
