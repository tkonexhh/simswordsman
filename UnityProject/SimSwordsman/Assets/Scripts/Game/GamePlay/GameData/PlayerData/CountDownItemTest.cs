using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CountDownItemTest
    {
        public DateTime StartTime;
        public DateTime EndTime;

        public double OffsetSecond;
        public int tempTime = 0;

        private int m_TimerID = -1;
        private float m_Progress = 0;

        public CountDownItemTest(DateTime startTime, DateTime endTime, Action EndCallBack = null)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;

            OffsetSecond = (EndTime - StartTime).TotalSeconds;

            m_TimerID = Timer.S.Post2Really((x) =>
            {
                tempTime += 1;

                if (tempTime >= OffsetSecond)
                {
                    if (EndCallBack != null)
                    {
                        EndCallBack();
                    }

                    Timer.S.Cancel(m_TimerID);
                    m_TimerID = -1;
                }
            }, 1, -1);
        }

        public float GetProgress()
        {
            m_Progress = (float)(EndTime - DateTime.Now).TotalSeconds;

            if (m_Progress < 0)
            {
                return 0;
            }

            return 1 - Mathf.Clamp01((float)(m_Progress / OffsetSecond));
        }
    }

}