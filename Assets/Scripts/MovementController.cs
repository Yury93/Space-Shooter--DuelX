using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }
        [SerializeField] private SpaceShip m_TargetShip;
       
        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private ControlMode m_ControlMode;

        [SerializeField] private PointerClickHold m_MobileFirePrimary;
        [SerializeField] private PointerClickHold m_MobileFireSecondary;

        [SerializeField] private PointerClickHold m_ButtonRight;
        [SerializeField] private PointerClickHold m_ButtonLeft;

        [SerializeField] private ParticleSystem m_SlowMoButtonRightLeft;

        private void Awake()
        {
            m_SlowMoButtonRightLeft = Instantiate<ParticleSystem>(m_SlowMoButtonRightLeft, transform.position, Quaternion.identity);
        }

        private void Start()
        {
         
            if(m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);

                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);

                m_ButtonLeft.gameObject.SetActive(false);
                m_ButtonRight.gameObject.SetActive(false);
            }
            else
            {
                m_MobileJoystick.gameObject.SetActive(true);
                m_MobileFirePrimary.gameObject.SetActive(true);
                m_MobileFireSecondary.gameObject.SetActive(true);

                m_ButtonLeft.gameObject.SetActive(true);
                m_ButtonRight.gameObject.SetActive(true);
            }
        }
        private void Update()
        {
            if(m_TargetShip == null)
            {
                return;
            }
            if(m_ControlMode == ControlMode.Keyboard)
            {
                ControlKeyboard();
            }
            if (m_ControlMode == ControlMode.Mobile)
            {
                ControlMobile();
            }
        }
        private void ControlMobile()
        {
            m_SlowMoButtonRightLeft.transform.position = m_TargetShip.transform.position;
            Vector3 dir = m_MobileJoystick.Value;

            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if(m_ButtonLeft.IsClick == true)
            {
                m_SlowMoButtonRightLeft.Play();
                m_TargetShip.RollLeftOrRight(-5);
                
                //Замедление времени
                Time.timeScale = 0.5f;

                
                StartCoroutine(TimeScaleReturn());

            }
            if(m_ButtonRight.IsClick == true)
            {
                m_SlowMoButtonRightLeft.Play();
                m_TargetShip.RollLeftOrRight(5);
                //Замедление времени
                Time.timeScale = 0.5f;
                
                StartCoroutine(TimeScaleReturn());
            }
            if (m_MobileFirePrimary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }
            if (m_MobileFireSecondary.IsHold == true)
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

        }
        private void ControlKeyboard()
        {
            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow))
                thrust = 1.0f;

            if (Input.GetKey(KeyCode.DownArrow))
                thrust = -1.0f;

            if (Input.GetKey(KeyCode.RightArrow))
                torque = 1.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                torque = - 1.0f;

            if(Input.GetKey(KeyCode.Space))
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }
            if(Input.GetKey(KeyCode.LeftAlt))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;
        }
        public void SetTargetShip(SpaceShip ship)
        {
            m_TargetShip = ship;
        }
        IEnumerator TimeScaleReturn()
        {
            yield return new WaitForSeconds(0.5f);
            if (m_SlowMoButtonRightLeft != null)
            {
                m_SlowMoButtonRightLeft.Stop();
            }
            Time.timeScale = 1;
        }
    }
}

