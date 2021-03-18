using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    [SerializeField]
    public class FoodBuffData
    {
        /// <summary>
        /// FoodConfig ±íµÄid
        /// </summary>
        public int FoodBufferID;
        public DateTime StartTime;
        public DateTime EndTime;
        public int TotalSecondsTime;
        public FoodBuffData() { }

        public FoodBuffData(int foodBufferID, DateTime StartTime, DateTime EndTime)
        {
            FoodBufferID = foodBufferID;
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            TotalSecondsTime = (int)(EndTime - StartTime).TotalSeconds;
        }
        public float GetRemainProgress()
        {
            float currentRemainTime = (float)(EndTime - DateTime.Now).TotalSeconds;

            if (currentRemainTime < 0)
            {
                currentRemainTime = 0;
            }

            return currentRemainTime / TotalSecondsTime;
        }
        public string GetRemainTime()
        {
            TimeSpan ts = (EndTime - DateTime.Now);
            return ts.ToString(@"hh\:mm\:ss");
        }
        public bool IsBufferActive()
        {
            return (EndTime - DateTime.Now).TotalSeconds > 0;
        }
    }
}