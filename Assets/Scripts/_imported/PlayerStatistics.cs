using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerStatistics : Object
    {
        public int numKills;
        public int score;
        public int time;

        private int GlobalKills;
        private int GlobalScores;

        public void Reset()
        {
            numKills = 0;
            score = 0;
            time = 0;
        }
    }
}
