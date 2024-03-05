using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;
using System;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Episode m_episode;

        [SerializeField] private RectTransform resultPanel;
        [SerializeField] private Image[] resultImage;

        public bool IsComplete { get { return gameObject.activeSelf && resultPanel.gameObject.activeSelf; } }

        public void LoadLevel()
        {
            LevelSequenceController.Instance.StartEpisode(m_episode);
        }

        public int Initialise()
        {
            var score = MapCompletion.Instance.GetEpisodeScore(m_episode);

            resultPanel.gameObject.SetActive(score > 0);

            for (int i = 0; i < score; i++)
            {
                resultImage[i].color = Color.white;
            }
            return score;
        }
    }
}

