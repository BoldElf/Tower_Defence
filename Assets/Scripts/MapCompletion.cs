using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefence
{

    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string filname = "completion.dat";

        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int score;
        }

        [SerializeField] private EpisodeScore[] completionData;

        public int TotalScore { private set; get; }

         private new void Awake()
        {
            base.Awake();
            Saver<EpisodeScore[]>.TryLoad(filname, ref completionData);

            foreach(var episodeScore in completionData )
            {
                TotalScore += episodeScore.score;
            }

        }
        
        public static  void SaveEpisodeResult(int levelScore)
        {
            if(Instance)
            {
                foreach (var item in Instance.completionData)
                { // Сохранение новых очков прохождения.
                    if (item.episode == LevelSequenceController.Instance.CurrentEpisode)
                    {
                        if (levelScore > item.score)
                        {
                            Instance.TotalScore += levelScore - item.score;
                            item.score = levelScore;
                            Saver<EpisodeScore[]>.Save(filname, Instance.completionData);
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"Episode complete with score {levelScore}");
            }
                
        }

        public int GetEpisodeScore(Episode m_episode)
        {
            foreach(var data in completionData)
            {
                if(data.episode == m_episode)
                {
                    return data.score;
                }
            }
            return 0;
        }

        /*
        private void SaveResult(Episode currentEpisode, int levelScore)
        {
            foreach (var item in completionData)
            {
                if (item.episode == currentEpisode)
                {
                    if (levelScore > item.score)
                    {
                        TotalScore += levelScore - item.score;
                        item.score = levelScore;
                        Saver<EpisodeScore[]>.Save(filname, completionData);
                    }
                }
            }
        }
        */
    }
}

