using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter {
    /// <summary>
    /// ������� ���������
    /// </summary>
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] private Entity[] m_EntityPrefabs;

        [SerializeField] private CircleArea m_Area;

        [SerializeField] private SpawnMode m_SpawnMode;

        [SerializeField] private int m_NumSpawns;

        [SerializeField] private float m_RespawnTime;

        

        [SerializeField] private float m_Timer;

        private void Start()
        {
            if (m_SpawnMode == SpawnMode.Start)
            {
                SpawnEntities();
            }
            m_Timer = 10;
        }
        private void Update()
        {
           
            if (m_Timer > m_RespawnTime)
            {
                
                m_Timer -= m_RespawnTime * Time.deltaTime;
                
            }
            if (m_SpawnMode == SpawnMode.Loop && m_Timer <= m_RespawnTime)
            {
               
                SpawnEntities();

                m_Timer = 10;
            }

        }
        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);

                
                GameObject e = Instantiate(m_EntityPrefabs[index].gameObject);

                e.transform.position = m_Area.GetRandomInsideZone();
            }
        }
    }
}