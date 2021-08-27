using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
   
    public class GeneralStatistics : MonoBehaviour
    {

        [SerializeField] private Text m_KillsText;

        [SerializeField] private Text m_ScoreText;

        [SerializeField] private Text m_TimeText;


        private void OnEnable()
        {
            var kill = 0;
            var score = 0;
            var time = 0;

            if(LevelSequenceController.Instance.TryGetComponent<PlayerStatistics>(out var stats))
            {
               kill =  stats.numKills;
               score = stats.score;
               time = stats.time;
            }
            
                m_KillsText.text = "Kills: " + kill.ToString();
                m_ScoreText.text = "Score: " + score.ToString();
                m_TimeText.text = "Time: " + time.ToString();
            
        }




    }
}