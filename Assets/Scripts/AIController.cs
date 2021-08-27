using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]

    public class AIController : MonoBehaviour
    {

        public enum AIBehavior
        {
            Null,
            Patrol
        }
        [SerializeField] private AIBehavior m_AIBehavior;
        /// <summary>
        /// массив точек
        /// </summary>
        [SerializeField] private AIPointPatrol[] m_PatrolPoint;
        /// <summary>
        /// Поле для индекса массивов точки
        /// </summary>
        private int m_Index;
        
        /// <summary>
        /// Линейная навигация 
        /// </summary>
        
        [SerializeField]private float m_NavigationLinear;

        /// <summary>
        /// Навигация угловая
        /// </summary>
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLenght;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;

        private Timer m_FireTimer;

        private Timer m_FindNewTargetTimer;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();
            InitTimers();
            m_Index = 0;
        }

        private void Update()
        {
            UpdateTimers();
            UpdateAI();
        }

        private void UpdateAI()
        {
             ActionEvadeCollision();

            if(m_AIBehavior == AIBehavior.Patrol)
            {
                UpdateBehaviorPatrol();
            }
            
        }

        private void UpdateBehaviorPatrol()
        {
            
            ActionEvadeCollision();//Избегаем столкновения
            ActionFindNewMovePosition();//поиск новой позиции
            ActionControlShip();//управление кораблём
            ActionFindNewAttackTarget();//поиск новой цели
            ActionFire();//Огонь
           
        }
        private void ActionFindNewMovePosition()
        {
           
            if (m_AIBehavior == AIBehavior.Patrol)
            {
                if(m_SelectedTarget !=null )
                {
                    m_MovePosition = m_SelectedTarget.transform.position;

                    ActionEvadeCollision();


                }
                else
                {
                    if(m_PatrolPoint[m_Index] != null)
                    {
                        ///
                        bool isInsidePatrolZone = (m_PatrolPoint[m_Index].transform.position - transform.position).sqrMagnitude < m_PatrolPoint[m_Index].Radius * m_PatrolPoint[m_Index].Radius;// дистанция от центра патрульной зоны меньше чем Радиус в кв
                        //то мы в круге
                        if(isInsidePatrolZone == true)
                        {

                            if (m_RandomizeDirectionTimer.IsFinished == true)
                            {
                                ActionEvadeCollision();
                                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint[m_Index].Radius + m_PatrolPoint[m_Index].transform.position;// случайная точка
                                m_MovePosition = newPoint;

                                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                                
                            }
                            m_Index++;
                            if(m_PatrolPoint.Length == m_Index)
                            {
                                m_Index = 0;
                            }
                            ActionEvadeCollision();
                        }
                        else
                        {
                            m_MovePosition = m_PatrolPoint[m_Index].transform.position;
                            ActionEvadeCollision();
                        }
                       
                    }
                }
            }
        }

        //

        //private float m_TimeFlyTarget;

        //public float MakeLead()///Упреждение
        //{
        //    //Узнаём скорость цели
        //    //Умножаем
        //    //Узнаём расстояние до цели 
        //    //Узнаём время полёта на такое расстояние
        //    Rigidbody2D rb = m_SelectedTarget.GetComponent<Rigidbody2D>();
        //    float  SpeedTarget = rb.velocity.magnitude;


        //    float dist = Vector2.Distance(transform.position, rb.transform.position);


        //    if (SpeedTarget != 0)
        //    {
        //        m_TimeFlyTarget = dist / SpeedTarget;// время за которое AI летит до цели

        //    }
        //    else
        //    {
        //        m_TimeFlyTarget = 0;
        //    }

        //    float lead = SpeedTarget * m_TimeFlyTarget;
        //    Debug.Log($"Упреждение ={lead}, Cкорость цели ={SpeedTarget}, Дистанция = {dist},Время до цели ={m_TimeFlyTarget}");
        //    return lead/lead;

        //}


        /// <summary>
        /// Метод предупреждения столкновений
        /// </summary>
        private RaycastHit2D hit;
        private void ActionEvadeCollision()//Избегать столкновений
        {
           
           
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLenght) == true)
            {
                //GameObject
              //  Debug.Log("Луч на препятствии");
                m_MovePosition = transform.position + transform.right * 100f - transform.up;

            }
           
        }
       
       private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized( m_MovePosition , m_SpaceShip.transform) * m_NavigationAngular;
        }
        private const float MAX_ANGLE = 45.0f;
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);
            
            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;
            return -angle;
        }
         private void ActionFindNewAttackTarget()
        {

            if(m_FindNewTargetTimer.IsFinished==true)
                {

                m_SelectedTarget = FindNearestDestructibleTarget();
                    m_FindNewTargetTimer.Start(m_ShootDelay);

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
        /// <summary>
        /// находит ближайший Destructible 
        /// </summary>
        /// <returns></returns>
        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = float.MaxValue;

            Destructible potentialTarget = null;

            foreach(var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;//нейтральные сущности типа астероиды не атакуем

                if (v.TeamId == m_SpaceShip.TeamId) continue;// исключаем команду(по своим не бьём)

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);
                if(dist < maxDist)
                {
                    maxDist = dist;
                    potentialTarget = v;
                }
            }
            return potentialTarget;
        }

        #region Timers
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }
        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime( Time.deltaTime); //отнимаем время
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);

        }
        private void SetPatrolBehavior(AIPointPatrol point)
        {
            m_AIBehavior = AIBehavior.Patrol;
            m_PatrolPoint[m_Index] = point;
        }
        #endregion


    }
}
