using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class EntitySpawnerDebris : MonoBehaviour
    {
        /// <summary>
        /// �������� ������
        /// </summary>
        [SerializeField] private Destructible[] m_DebrisPrefabs;
        
        [SerializeField] private int m_NumDebris;

        [SerializeField] private CircleArea m_Area;
        
        [SerializeField] private float m_RandomSpeed;

        
        

        /// <summary>
        /// ������� ������������� ������
        /// </summary>

        //[SerializeField] private GameObject[] m_DestroyedObj;

        //[SerializeField] private int m_NumDestObj;

        //[SerializeField] private CircleArea m_AreaDestroyedObj;


        private void Start()
        {
            for (int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebris();
            }
           
        }

        private void SpawnDebris()
        {
            int index = Random.Range(0, m_DebrisPrefabs.Length);
            
            GameObject debris = Instantiate(m_DebrisPrefabs[index].gameObject);//�������� ��������
            debris.transform.localScale = new Vector2(Random.Range(1f,1.5f),Random.Range(1f, 1.5f));
           // Debug.Log("�������� ������� ����������");
            debris.transform.position =  m_Area .GetRandomInsideZone();//����������� ������� ��������� �������� � ��������� ����� CircleAre

            Destructible des = debris.GetComponent<Destructible>();
            des.EventOnDeath.AddListener(OnDebrisDead);// ������ ��� ����� ������������ ������ - ���������� OnDebrisDead
            
           
            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();//����� ��������� �������� ������
            if (rb != null && m_RandomSpeed > 0)
            {
                rb.velocity = (Vector2)UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
            }
        }
        private void OnDebrisDead()
        {
         SpawnDebris();
            /////����������� �����
            //int i = Random.Range(0, m_DestroyedObj .Length);

            //GameObject debrisDest = Instantiate(m_DestroyedObj [i].gameObject);//�������� ��������
            //Debug.Log("�������� ��������� ����������");
            //debrisDest.transform.position = m_AreaDestroyedObj.GetRandomInsideZone();//����������� ������� ��������� �������� � ��������� ����� CircleAre

            //Rigidbody2D rb = debrisDest.GetComponent<Rigidbody2D>();//����� ��������� �������� ������
            //if (rb != null && m_RandomSpeed > 0)
            //{
            //    rb.velocity = (Vector2)UnityEngine.Random.insideUnitSphere * m_RandomSpeed;
            //}

            /////
           
        }
    }
}
