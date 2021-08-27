using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Ошраничитель уровня. Работает в связке с levelBoundary, если тот есть на сцене. Ограничивает объект на который добавлен
    /// </summary>
    public class LevelBoundaryLimiter : MonoBehaviour
    {
        
      
        void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var lb = LevelBoundary.Instance;
            var r = lb.Radius;

            if (transform.position.magnitude > r)
            {
                if (lb.LimitMode == LevelBoundary.Mode.Limit)
                {
                    transform.position = transform.position.normalized * r;
                }
                if (lb.LimitMode == LevelBoundary.Mode.Teleport)
                {
                    transform.position = -transform.position.normalized * r;
                }
                if (lb.LimitMode == LevelBoundary.Mode.Destr)
                {
                    GetComponent<Destructible>().ApplyDamage(1);//Сыро
                }
            }
        }
        
    }
}
