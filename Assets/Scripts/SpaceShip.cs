using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
       [Header("SpaceShip")]
        /// <summary>
        /// Масса автоматической установки для ригида
        /// </summary>
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперёд сила
        /// </summary>
       [SerializeField] private float m_Thrust;

        /// <summary>
        /// Вращающая сила
        /// </summary>
       [SerializeField] private float m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость
        /// </summary>
       [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращающая скорость угл/сек
        /// </summary>
       [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// Спрайт Корабля
        /// </summary>
       [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Ссылка на ригид
        /// </summary>
       private Rigidbody2D m_Rigid;

        [SerializeField] private float m_SpeedRoll;

        public float SpeedRoll => m_SpeedRoll;


        #region Public API
        /// <summary>
        /// Усиление линейной тягой
        /// </summary>
        public float ThrustControl { get; set; }


        /// <summary>
        /// Усиление вращательной тягой
        /// </summary>
        public float TorqueControl { get; set; }


        public float MaxLinearVelocity => m_MaxLinearVelocity;

        public float MaxAngularVelocity => m_MaxAngularVelocity;
        #endregion

        #region Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;
            m_Rigid.inertia = 2;

            InitOffensive();


        }
        
        private void FixedUpdate()
        {
            
            UpdateRigidBody();
            UpdateEnergyRegen();
            if (m_MaxLinearVelocity >= 6 || m_Gods == true)
            {
                m_TimerForProperties -= Time.fixedDeltaTime;
                Debug.Log("Скорость больше 6  или  god = true. Отсчёт таймера");
            }
            if (m_TimerForProperties <= 1 )
            {
                m_MaxLinearVelocity = 2;

                m_Gods = false;

                IndestructibleShipPlayer(m_Gods);

                m_TimerForProperties = 15f;
                Debug.Log("God = false. Скорость корабля = 2. Таймер установлен на 15");
            }
           

        }
        #endregion
        /// <summary>
        /// Метод добавления сил коробля для движения
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce( m_Thrust * ThrustControl *  transform.up * Time.fixedDeltaTime,ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust/ m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(m_Mobility * TorqueControl * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity)  * Time.fixedDeltaTime, ForceMode2D.Force);

        }
        /// <summary>
        /// Турели
        /// </summary>
        [SerializeField] private Turret[] m_Turrets;

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if(m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        public void RollLeftOrRight(float speed)
        {
            m_SpeedRoll = speed;
            m_Rigid.velocity = transform.right * m_SpeedRoll;
            
        }

        /// <summary>
        /// свойства поверапов статистики
        /// </summary>
        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;
        [SerializeField] private float m_TimerForProperties;

        
       

        private float m_PrimaryEnergy;
        private int m_SecondaryAmmo;
        private bool m_Gods;
        
        
        
        public void AddEnergy(int e, float time)
        {
            m_TimerForProperties = time;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e,0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo, float time )
        {
            m_TimerForProperties = time;
            
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }
        public void AddSpeed(int speed, float time)
        {
            m_TimerForProperties = time;
            m_MaxLinearVelocity += speed;
           
        }
        public void AddGod(bool m_dest, float time)
        {
            m_TimerForProperties = time;
            m_Gods = m_dest;
           IndestructibleShipPlayer(m_Gods);
            


        }
        
        private void InitOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
           
            
        }
        
        private void  UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy , 0, m_MaxEnergy);
        }
        public bool DrawEnergy(int count)
        {
            if (count == 0)
            {
                return true;
            }
            if (m_PrimaryEnergy >= count)
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
            if(m_SecondaryAmmo >=count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }
            return false;
        }

        public void AssignWeapon(TurretProperties props)
        {
            for(int i = 0; i< m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadOut(props);
            }
        }
    }
}
