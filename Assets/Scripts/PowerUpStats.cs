using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
       public enum  EffectType
        {
            AddAmmo,
            AddEnergy,
            AddGod,
            AddSpeed
        }
        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private float m_Value;
        [SerializeField] private bool m_God;
        [SerializeField] private float m_Time;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if(m_EffectType == EffectType.AddEnergy)
            {
                ship.AddEnergy((int)m_Value,(float) m_Time);
            }
            if (m_EffectType == EffectType.AddAmmo)
            {
                ship.AddAmmo((int)m_Value, (float)m_Time);
            }
            if(m_EffectType == EffectType.AddGod)
            {
                ship.AddGod(m_God, (float)m_Time);
                
            }
            if(m_EffectType == EffectType.AddSpeed)
            {
                ship.AddSpeed((int)m_Value, (float)m_Time);
            }
        }
    }
}
