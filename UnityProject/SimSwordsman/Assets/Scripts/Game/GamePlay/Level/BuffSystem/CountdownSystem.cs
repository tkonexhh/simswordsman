using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class CountdownSystem : TSingleton<CountdownSystem>
	{
        List<Countdowner> m_AllBuffs = new List<Countdowner>();

        public void Init()
        {
            //���浵
            bool isChange = false;
            TimeSpan offset;
            for (int i = GameDataMgr.S.GetPlayerData().countdownerData.Count - 1; i >= 0; i--)
            {
                Countdowner cd = GameDataMgr.S.GetPlayerData().countdownerData[i];
                offset = DateTime.Parse(cd.EndTime) - DateTime.Now;
                if (offset.TotalSeconds > 0)
                {
                    m_AllBuffs.Add(cd);
                    StartCountdown(cd);
                }
                else
                {
                    isChange = true;
                    GameDataMgr.S.GetPlayerData().countdownerData.RemoveAt(i);
                }
            }
            if (isChange)
                GameDataMgr.S.GetPlayerData().SetDataDirty();
        }

        /// <summary>
        /// ���һ������ʱ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalMinutes">����ʱʱ�� ��λ������</param>
        /// <param name="interval">ÿ��tick��ʱ�� ��λ����</param>
        public void StartCountdownerWithMin(string stringid, int id, int totalMinutes, int interval = 1)
        {
            StartCountdownerWithSec(stringid, id, totalMinutes * 60, interval);
        }
        /// <summary>
        /// ���һ������ʱ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="totalMinutes">����ʱʱ�� ��λ����</param>
        /// <param name="interval">ÿ��tick��ʱ�� ��λ����</param>
        public void StartCountdownerWithSec(string stringid, int id, int totalSeconds, int interval = 1)
        {
            foreach (var item in m_AllBuffs)
            {
                if (item.stringID.Equals(id) && item.ID == id)
                {
                    //���¼�ʱ
                    item.EndTime = (DateTime.Now + TimeSpan.FromSeconds(totalSeconds)).ToString();
                    StartCountdown(item);
                    return;
                }
            }
            Countdowner cd = new Countdowner();
            cd.ID = id;
            cd.stringID = stringid;
            cd.EndTime = (DateTime.Now + TimeSpan.FromSeconds(totalSeconds)).ToString();
            m_AllBuffs.Add(cd);
            //��ʼ��ʱ
            StartCountdown(cd);

            //�浵
            GameDataMgr.S.GetPlayerData().countdownerData.Add(cd);
            GameDataMgr.S.GetPlayerData().SetDataDirty();
        }

        void StartCountdown(Countdowner cd)
        {
            //����֮ǰ�ļ�ʱ������У�
            Timer.S.Cancel(cd.TimerID);

            TimeSpan offset = DateTime.Parse(cd.EndTime) - DateTime.Now;
            if (offset.TotalSeconds >= 0)
            {
                EventSystem.S.Send(EventID.OnCountdownerStart, cd, offset.ToString(@"hh\:mm\:ss"));
                cd.TimerID = Timer.S.Post2Really(count =>
                {
                    offset -= TimeSpan.FromSeconds(1);
                    if (offset.TotalSeconds > 0)
                        EventSystem.S.Send(EventID.OnCountdownerTick, cd, offset.ToString(@"hh\:mm\:ss"));
                    else
                        StopCountdown(cd);//����
                }, 1, -1);
            }
        }
        
        void StopCountdown(Countdowner cd)
        {
            Timer.S.Cancel(cd.TimerID);
            cd.TimerID = -1;
            EventSystem.S.Send(EventID.OnCountdownerEnd, cd);
            GameDataMgr.S.GetPlayerData().countdownerData.Remove(cd);
            GameDataMgr.S.GetPlayerData().SetDataDirty();
            m_AllBuffs.Remove(cd);
        }
        
        Countdowner GetCountdowner(string stringid, int id)
        {
            foreach (var item in m_AllBuffs)
            {
                if (id == item.ID && item.stringID.Equals(stringid))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// �Ƿ����ڵ���ʱ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsActive(string stringid, int id)
        {
            foreach (var item in m_AllBuffs)
            {
                if (id == item.ID && item.stringID.Equals(stringid))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// �õ�һ������ʱ��ʣ��ʱ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCurrentCountdownTime(string stringid, int id)
        {
            Countdowner buff = GetCountdowner(stringid, id);
            if (buff != null)
            {
                TimeSpan offset = DateTime.Parse(buff.EndTime) - DateTime.Now;
                return offset.ToString(@"hh\:mm\:ss");
            }
            return null;
        }
    }
    public class Countdowner
    {
        public string stringID;

        public int ID;

        public string EndTime;//DateTime.Parse

        public int TimerID;

        public int Interval;
    }
}