using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class MainMenuController : SingletonBase<MainMenuController>
    {
        [SerializeField] private SpaceShip m_DefaultSpaceShip;
        [SerializeField] private GameObject m_EpisodeSelection;
        [SerializeField] private GameObject m_ShipSelection;
        [SerializeField] private GameObject m_Statistics;

        private void Start()
        {
            LevelSequenceController.PlayerShip = m_DefaultSpaceShip;
        }

        public void OnButtonStartNew()
        {
            m_EpisodeSelection.gameObject.SetActive(true);
            gameObject.SetActive(false);
           // print("episode");
        }

        public void OnSelectShip()
        {
            m_ShipSelection.gameObject.SetActive(true);
            gameObject.SetActive(false);
           // print("select ship");
        }

        public void OnSelectStatistics()
        {
            m_Statistics.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        public void OnSelectMenu()
        {
            m_Statistics.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}