using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanelController : SingletonBase<ResultPanelController>
    {
        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;

        [SerializeField] private Text m_Result;

        [SerializeField] private Text m_ButtonNextText;

        [SerializeField] private GameObject m_PanelSuccess;
        [SerializeField] private GameObject m_PanelFailure;


        private bool m_Succes;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        //public void ShowResults(PlayerStatistics levelResult, bool succes)
        public void ShowResults(bool succes)
        {
            gameObject.SetActive(true);

            //m_Succes = succes;

            /*
            m_Kills.text = "Kills : " + levelResult.numKills.ToString();
            m_Score.text = "Score : " + levelResult.score.ToString();
            m_Time.text = "Time : " + levelResult.time.ToString();
            

            m_Result.text = succes ? "Win" : "Lose";
            m_ButtonNextText.text = succes ? "Next" : "Restart";
            */
            m_PanelSuccess?.gameObject.SetActive(succes);
            m_PanelFailure?.gameObject.SetActive(!succes);


            //Time.timeScale = 0;

            // сохраняем 
            /*
            PlayerPrefs.SetInt("TotalKills", PlayerPrefs.GetInt("TotalKills") + levelResult.numKills);
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + levelResult.score);
            PlayerPrefs.Save();
            */
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            Time.timeScale = 1;

            if(m_Succes)
            {
                LevelSequenceController.Instance.AdvanceLevel();
            }
            else
            {
                LevelSequenceController.Instance.RestartLevel();
            }
        }

        public void OnResratrLevel()
        {
            LevelSequenceController.Instance.RestartLevel();
        }

        public void OnPlayNext()
        {
            LevelSequenceController.Instance.AdvanceLevel();
        }

    }
}


