using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CountDownItem : ITimeObserver
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

        private string startTime;

        public CountDownItem() { }
        public CountDownItem(string _name,int tickCount,int interval = 1)
        {
            m_CountDownID = _name;
            m_TickCount = tickCount;
            if (m_TickCount <= 0)
                m_TickCount = 0;
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
            //Debug.LogError("����ʱ" + SplicingTime(m_TickCount));

            OnSecondRefreshEvent?.Invoke(SplicingTime(m_TickCount));
        }

        public void IncreasTickTime(int delta)
        {
            m_TickCount += delta;
        }
        public void ReduceTickTime(int delta)
        {
            m_TickCount -= delta;
            if (m_TickCount<=0)
            {
                OnCountDownOverEvent?.Invoke();
                TimeUpdateMgr.S.AddFinishedObservers(this);
            }
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
            EventSystem.S.UnRegister(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
        }

        public void OnPause()
        {
        }

        public void OnResume()
        {
        }

        public void OnStart()
        {
            Log.i("CoungDown is start!");
            EventSystem.S.Register(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
        }
        private void HandleAddListenerEvent(int key, object[] param)
        {

            if ((bool)param[0])
            {
                Debug.LogError("�л���ǰ̨ʱִ��");
                ReduceTickTime(ComputingTime(startTime));
            }
            else
            {
                //�л�����̨ʱִ��
                Debug.LogError("�л�����̨ʱִ��");
                startTime = DateTime.Now.ToString();
                // RefreshPracticeFieldState();

            }
        }

        private int ComputingTime(string time)
        {
            DateTime dateTime;
            DateTime.TryParse(time, out dateTime);
            if (dateTime != null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalSeconds;
            }
            return 0;
        }
        public bool ShouldRemoveWhenMapChanged()
        {
            return true;
        }
        #endregion
    }
}