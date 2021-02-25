using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class CountdownSystem : TSingleton<CountdownSystem>
    {
        List<Countdowner> m_AllCDs = new List<Countdowner>();

        public void Init()
        {
            //先注册事件
            FoodBuffSystem.S.Init();
            CollectSystem.S.Init();
            WorkSystem.S.Init();

            //检查存档
            bool isChange = false;
            TimeSpan offset;
            for (int i = GameDataMgr.S.GetCountdownData().countdownerData.Count - 1; i >= 0; i--)
            {
                Countdowner cd = GameDataMgr.S.GetCountdownData().countdownerData[i];
                offset = DateTime.Parse(cd.EndTime) - DateTime.Now;
                if (offset.TotalSeconds > 0)
                {
                    m_AllCDs.Add(cd);
                    CountdownStart(cd);
                }
                else
                {
                    isChange = true;
                    EventSystem.S.Send(EventID.OnCountdownerEnd, cd);
                    GameDataMgr.S.GetCountdownData().countdownerData.RemoveAt(i);
                }
            }
            if (isChange)
                GameDataMgr.S.GetCountdownData().SetDataDirty();

            //检查其他依赖系统存档
            CollectSystem.S.CheckData();
            WorkSystem.S.UpdateCanWorkFacilitys();
        }

        /// <summary>
        /// 添加一个倒计时
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalMinutes">倒计时时长 单位：分钟</param>
        /// <param name="interval">每次tick的时间 单位：秒</param>
        public void StartCountdownerWithMin(string stringid, int id, float totalMinutes, int interval = 1)
        {
            StartCountdownerWithSec(stringid, id, totalMinutes * 60, interval);
        }
        /// <summary>
        /// 添加一个倒计时
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalMinutes">倒计时时长 单位：秒</param>
        /// <param name="interval">每次tick的时间 单位：秒</param>
        public void StartCountdownerWithSec(string stringid, int id, float totalSeconds, int interval = 1)
        {
            foreach (var item in m_AllCDs)
            {
                if (item.stringID.Equals(stringid) && item.ID == id)
                {
                    //重新计时
                    item.startTime = DateTime.Now.ToString();
                    item.EndTime = (DateTime.Now + TimeSpan.FromSeconds(totalSeconds)).ToString();
                    item.SetProgress(0);
                    CountdownStart(item);
                    return;
                }
            }
            Countdowner cd = new Countdowner();
            cd.ID = id;
            cd.stringID = stringid;
            cd.startTime = DateTime.Now.ToString();
            cd.SetProgress(0);
            cd.EndTime = (DateTime.Now + TimeSpan.FromSeconds(totalSeconds)).ToString();
            m_AllCDs.Add(cd);
            //开始计时
            CountdownStart(cd);

            //存档
            GameDataMgr.S.GetCountdownData().countdownerData.Add(cd);
            GameDataMgr.S.GetCountdownData().SetDataDirty();
        }

        void CountdownStart(Countdowner cd)
        {
            //结束之前的计时（如果有）
            Timer.S.Cancel(cd.TimerID);

            TimeSpan offset = DateTime.Parse(cd.EndTime) - DateTime.Now;
            DateTime endtime = DateTime.Parse(cd.EndTime);
            DateTime starttime = DateTime.Parse(cd.startTime);
            TimeSpan total = endtime - starttime;
            cd.SetProgress((float)(1f - (offset.TotalSeconds / total.TotalSeconds)));
            if (offset.TotalSeconds >= 0)
            {
                EventSystem.S.Send(EventID.OnCountdownerStart, cd, offset.ToString(@"hh\:mm\:ss"));
                cd.TimerID = Timer.S.Post2Really(count =>
                {
                    offset -= TimeSpan.FromSeconds(1);
                    if (offset.TotalSeconds > 0)
                    {
                        cd.SetProgress((float)(1f - (offset.TotalSeconds / total.TotalSeconds)));
                        EventSystem.S.Send(EventID.OnCountdownerTick, cd, offset.ToString(@"hh\:mm\:ss"));
                    }
                    else
                        CountdownEnd(cd);//结束
                }, 1, -1);
            }
        }
        
        void CountdownEnd(Countdowner cd)
        {
            Timer.S.Cancel(cd.TimerID);
            cd.TimerID = -1;
            EventSystem.S.Send(EventID.OnCountdownerEnd, cd);
            GameDataMgr.S.GetCountdownData().countdownerData.Remove(cd);
            GameDataMgr.S.GetCountdownData().SetDataDirty();
            m_AllCDs.Remove(cd);
        }
        
        public Countdowner GetCountdowner(string stringid, int id)
        {
            foreach (var item in m_AllCDs)
            {
                if (id == item.ID && item.stringID.Equals(stringid))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 关闭一个倒计时
        /// </summary>
        public void Cancel(string stringid, int id)
        {
            var cd = GetCountdowner(stringid, id);
            if (cd != null)
            {
                Timer.S.Cancel(cd.TimerID);
                GameDataMgr.S.GetCountdownData().countdownerData.Remove(cd);
                GameDataMgr.S.GetCountdownData().SetDataDirty();
                m_AllCDs.Remove(cd);
            }
        }

        /// <summary>
        /// 是否正在倒计时
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsActive(string stringid, int id)
        {
            foreach (var item in m_AllCDs)
            {
                if (id == item.ID && item.stringID.Equals(stringid))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 得到一个倒计时的剩余时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCurrentCountdownTime(string stringid, int id)
        {
            Countdowner cd = GetCountdowner(stringid, id);
            if (cd != null)
            {
                TimeSpan offset = GetCurrentCountdownTimeSpan(cd);
                return offset.ToString(@"hh\:mm\:ss");
            }
            return null;
        }
        TimeSpan GetCurrentCountdownTimeSpan(Countdowner cd)
        {
            return DateTime.Parse(cd.EndTime) - DateTime.Now;
        }
    }
    public class Countdowner
    {
        public string stringID;
        public int ID;

        public string startTime;
        public string EndTime;//DateTime.Parse

        public int TimerID;

        private float Progress;
        public float GetProgress()
        {
            return Progress;
        }
        public void SetProgress(float value)
        {
            Progress = value;
        }
    }
}