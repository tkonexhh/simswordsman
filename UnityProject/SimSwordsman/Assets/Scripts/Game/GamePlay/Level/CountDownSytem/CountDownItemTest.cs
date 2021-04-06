using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class CountDownItemTest : ICacheAble
    {
        public int CountDownIndex;
        public DateTime StartTime;
        public DateTime EndTime;
        public double TotalSecondsTime;
        public int tempTime = 0;
        private int m_TimerID = -1;
        private bool m_IsCache = false;
        private int m_SpeedUpMultiply = 1;
        private CountDownCallBackWrap OnUpdateCallBackWrap = null;
        private CountDownCallBackWrap OnEndCallBackWrap = null;
        public CountDownItemTest() { }
        public void Init(int countDownIndex, int totalTime, OnCountDownCallBackDel UpdateCallBack = null, OnCountDownCallBackDel EndCallBack = null)
        {
            CountDownIndex = countDownIndex;

            TotalSecondsTime = totalTime;

            if (OnUpdateCallBackWrap == null)
            {
                OnUpdateCallBackWrap = new CountDownCallBackWrap();
            }
            else {
                OnUpdateCallBackWrap.Clear();
            }
            OnUpdateCallBackWrap.Add(UpdateCallBack);

            if (OnEndCallBackWrap == null)
            {
                OnEndCallBackWrap = new CountDownCallBackWrap();
            }
            else {
                OnEndCallBackWrap.Clear();
            }
            OnEndCallBackWrap.Add(EndCallBack);

            Timer.S.Cancel(m_TimerID);

            m_TimerID = Timer.S.Post2Really((x) =>
            {
                tempTime += 1 * m_SpeedUpMultiply;

                if (tempTime >= TotalSecondsTime)
                {
                    if (this.OnEndCallBackWrap != null)
                    {
                        this.OnEndCallBackWrap.Fire(0);
                    }

                    CountDowntMgr.S.StopCountDownItemTest(CountDownIndex);
                }
                else
                {
                    if (OnUpdateCallBackWrap != null) {
                        OnUpdateCallBackWrap.Fire((int)(TotalSecondsTime - tempTime));
                    }
                }
            }, 1, -1);
        }
        public void SetSpeedUpMultiply(int speedUpMultiple) 
        {
            this.m_SpeedUpMultiply = speedUpMultiple;
        }
        public int GetCountDownID()
        {
            return CountDownIndex;
        }
        public void Stop()
        {
            ObjectPool<CountDownItemTest>.S.Recycle(this);
        }
        public bool RegisterUpdateCallBack(OnCountDownCallBackDel callback) 
        {
            if (OnUpdateCallBackWrap == null) {
                OnUpdateCallBackWrap = new CountDownCallBackWrap();
            }
            if (OnUpdateCallBackWrap.Add(callback)) {
                return true;
            }
            Debug.LogError("已经注册了该方法");
            return false;
        }
        public bool RegisterEndCallBack(OnCountDownCallBackDel callback) 
        {
            if (OnEndCallBackWrap == null)
            {
                OnEndCallBackWrap = new CountDownCallBackWrap();
            }
            if (OnEndCallBackWrap.Add(callback))
            {
                return true;
            }
            Debug.LogError("已经注册了该方法");
            return false;
        }
        public void UnRegisterUpdateCallBack(OnCountDownCallBackDel callback) 
        {
            if (OnUpdateCallBackWrap != null) 
            {
                OnUpdateCallBackWrap.Remove(callback);
            }
        }
        public void UnRegisterEndCallBack(OnCountDownCallBackDel callback) {
            if (OnEndCallBackWrap != null) {
                OnEndCallBackWrap.Remove(callback);
            }
        }
        #region ICacheAble
        public bool cacheFlag
        {
            get
            {
                return m_IsCache;
            }

            set
            {
                m_IsCache = value;
            }
        }
        public void OnCacheReset()
        {
            Timer.S.Cancel(m_TimerID);
            m_TimerID = -1;
            tempTime = 0;
            TotalSecondsTime = 0;
            if (OnUpdateCallBackWrap != null)
            {
                OnUpdateCallBackWrap.Clear();
            }
            if (OnEndCallBackWrap != null)
            {
                OnEndCallBackWrap.Clear();
            }
        }
        #endregion
    }
}