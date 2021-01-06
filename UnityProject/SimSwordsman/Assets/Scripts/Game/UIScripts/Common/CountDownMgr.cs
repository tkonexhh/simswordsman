using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CountDownMgr : ITimeObserver
	{
        private string m_CountDownID;

        /// <summary>
        /// ���Ƶ��
        /// </summary>
        private int m_Interval;
        /// <summary>
        /// ����ʱ����
        /// </summary>
        private int m_TickCount;
        /// <summary>
        /// ����ˢ���¼�
        /// </summary>
        public Action<string> OnSecondRefreshEvent;
        /// <summary>
        /// ����ʱ�����¼�
        /// </summary>
        public Action OnCountDownOverEvent;

        public CountDownMgr() { }
        public CountDownMgr(string _name,int tickCount,int interval = 1)
        {
            m_CountDownID = _name;
            m_TickCount = tickCount;
            m_Interval = interval;
        }

        private void RefresCountDown(int m_TickCount)
        {
            if (m_TickCount<=0)
            {
                OnCountDownOverEvent?.Invoke();
                TimeUpdateMgr.S.AddFinishedObservers(this);
                return;
            }
            Debug.LogError("����ʱ" + SplicingTime(m_TickCount));

            OnSecondRefreshEvent?.Invoke(SplicingTime(m_TickCount));
        }

        public void IncreasTickTime(int delta)
        {
            m_TickCount += delta;
        }

        public string GetID()
        {
            return m_CountDownID;
        }
    
        /// <summary>
        /// ƴ��00��00��00��ʽ
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
        #region ITimeObserver

        public int GetTickCount()
        {
            return m_TickCount;
        }
        public void OnTick(int count)
        {
            RefresCountDown(m_TickCount);
            m_TickCount--;
        }

        public int GetTickInterval()
        {
            return m_Interval;
        }

        public int GetTotalSeconds()
        {
            return int.MaxValue;
        }

        public void OnFinished()
        {
            //����ʱ��������
        }

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }

        public void OnStart()
        {
            Debug.Log(m_CountDownID+"����ʱ��ʼ��");
        }

       

        public bool ShouldRemoveWhenMapChanged()
        {
            return true;
        }
        #endregion
    }
}