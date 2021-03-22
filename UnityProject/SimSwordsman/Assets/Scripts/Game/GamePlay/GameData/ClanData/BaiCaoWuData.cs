using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class BaiCaoWuData
    {
        public int HerbID;
        public DateTime StartTime;
        public DateTime EndTime;
        public int TotalSecondsTime;//总时长：秒
        public int AlreadyPassTime;//已经经过时长：秒

        private int m_CountDownID = -1;

        public BaiCaoWuData() { }

        public BaiCaoWuData(int herbID, DateTime StartTime, DateTime EndTime)
        {
            this.HerbID = herbID;
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            TotalSecondsTime = (int)(EndTime - StartTime).TotalSeconds;
        }
        public void UpdateData(DateTime StartTime, DateTime EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            TotalSecondsTime = (int)(EndTime - StartTime).TotalSeconds;
            AlreadyPassTime = 0;
        }
        public int GetRemainTime()
        {
            return Mathf.Max(0, TotalSecondsTime - AlreadyPassTime);
        }
        public string GetRemainTimeStr() 
        {
            return (DateTime.Now.AddSeconds(GetRemainTime()) - DateTime.Now).ToString(@"hh\:mm\:ss");
        }
        public void SetCouontDownID(int countDownID)
        {
            m_CountDownID = countDownID;
        }
        public int GetCountDownID()
        {
            return m_CountDownID;
        }
        public float GetProgress()
        {
            return Mathf.Clamp01((AlreadyPassTime * 1.0f / TotalSecondsTime));
        }
    }
}