using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    public class LevelBoundary : SingletonBase<LevelBoundary>
    {
        
        //public static LevelBoundary instance;

        //private void Awake()
        //{
        //    if (instance != null)
        //    {
        //        Debug.LogError("LevelBoundary уже существует!");
        //        Destroy(gameObject);
        //        return;
        //    }
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);


        //}
       

        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        public enum Mode
        {
            Limit,
            Teleport,
            Destr
        }
        [SerializeField] private Mode m_LimitMode;
            public Mode LimitMode => m_LimitMode;


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, m_Radius);
        }

#endif
    }
}
