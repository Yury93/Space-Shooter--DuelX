using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class PlayerShipSelectionController : MonoBehaviour
    {
        [SerializeField] private SpaceShip m_Prefab;

        [SerializeField] private Text m_ShipName;
        [SerializeField] private Text m_Hitpoints;
        [SerializeField] private Text m_Speed;
        [SerializeField] private Text m_Agility;

        [SerializeField] private Image m_PreviewImage;

        private void Start()
        {
            if(m_Prefab !=null)
            {
                m_ShipName.text = m_Prefab.name;

                m_Hitpoints.text = "HP: "+ m_Prefab.HitPoints.ToString();
                m_Speed.text ="Speed: "+ m_Prefab.MaxLinearVelocity.ToString();
                m_Agility.text = "Agility: "+ m_Prefab.MaxAngularVelocity.ToString();

                m_PreviewImage.sprite = m_Prefab.PreviewImage;
            }
        }

        public void OnSelectShip()
        {
            LevelSequenceController.PlayerShip = m_Prefab;

            MainMenuController.Instance.gameObject.SetActive(true);
        }

    }
}
