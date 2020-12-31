using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class BuffSystem : TSingleton<BuffSystem>
	{
        List<FoodBuff> m_AllBuffs = new List<FoodBuff>();

        TDFoodConfig m_TempTable;

        public void Init()
        {
            //检查存档
            TimeSpan offset;
            foreach (var item in GameDataMgr.S.GetPlayerData().foodBuffData)
            {
                offset = DateTime.Parse(item.endTime) - DateTime.Now;
                if (offset.TotalSeconds > 0)
                {
                    m_AllBuffs.Add(item);
                    StartCountdown(item);
                }
            }
        }

        public void StartBuff(int id, bool ad = false)
        {
            foreach (var item in m_AllBuffs)
            {
                if (item.foodID == id)
                    return;
            }
            FoodBuff buff = new FoodBuff();
            buff.foodID = id;
            buff.endTime = (DateTime.Now + TimeSpan.FromMinutes(ad ? TDFoodConfigTable.GetData(buff.foodID).buffTimeAD : TDFoodConfigTable.GetData(buff.foodID).buffTime)).ToString();
            m_AllBuffs.Add(buff);
            //开始计时
            StartCountdown(buff);

            //存档
            GameDataMgr.S.GetPlayerData().foodBuffData.Add(buff);
            GameDataMgr.S.GetPlayerData().SetDataDirty();
        }

        void StartCountdown(FoodBuff buff)
        {
            TimeSpan offset = DateTime.Parse(buff.endTime) - DateTime.Now;
            if (offset.TotalSeconds >= 0)
            {
                EventSystem.S.Send(EventID.OnFoodBuffStart, buff.foodID, offset.ToString(@"hh\:mm\:ss"));
                buff.TimerID = Timer.S.Post2Really(count =>
                {
                    offset -= TimeSpan.FromSeconds(1);
                    if (offset.TotalSeconds > 0)
                        EventSystem.S.Send(EventID.OnFoodBuffInterval, buff.foodID, offset.ToString(@"hh\:mm\:ss"));
                    else
                        StopCountdown(buff.foodID);//结束
                }, 1, -1);
            }
        }
        

        void StopCountdown(int id)
        {
            for (int i = m_AllBuffs.Count - 1; i >= 0; i--)
            {
                if (m_AllBuffs[i].foodID == id)
                {
                    Timer.S.Cancel(m_AllBuffs[i].TimerID);
                    EventSystem.S.Send(EventID.OnFoodBuffEnd, m_AllBuffs[i]);
                    m_AllBuffs.RemoveAt(i);
                    return;
                }
            }
        }
        
        public string GetEffectDesc(TDFoodConfig tb)
        {
            BuffType type;
            if (Enum.TryParse(tb.buffType, out type))
            {
                switch (type)
                {
                    case BuffType.AddATK:
                        return string.Format("弟子战力+{0}%", tb.buffRate);
                    case BuffType.AddRoleExp:
                        return string.Format("获得弟子经验+{0}%", tb.buffRate);
                    case BuffType.AddCoin:
                        return string.Format("获得铜钱+{0}%", tb.buffRate);
                    default:
                        break;
                }
            }
            return null;
        }

        FoodBuff GetBuff(int id)
        {
            foreach (var item in m_AllBuffs)
            {
                if (item.foodID == id)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 是否在激活状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsActive(int id)
        {
            foreach (var item in m_AllBuffs)
            {
                if (item.foodID == id)
                    return true;
            }
            return false;
        }

        public string GetCurrentCountdown(int id)
        {
            FoodBuff buff = GetBuff(id);
            if (buff != null)
            {
                TimeSpan offset = DateTime.Parse(buff.endTime) - DateTime.Now;
                return offset.ToString(@"hh\:mm\:ss");
            }
            return null;
        }

        public double Coin(double originalCoin)
        {
            int add = 0;
            BuffType type;
            foreach (var item in m_AllBuffs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item.foodID);
                if (Enum.TryParse(m_TempTable.buffType, out type) && type == BuffType.AddCoin)
                {
                    add += (int)(originalCoin * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalCoin + add;
        }

        public int RoleExp(int originalExp)
        {
            int add = 0;
            BuffType type;
            foreach (var item in m_AllBuffs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item.foodID);
                if (Enum.TryParse(m_TempTable.buffType, out type) && type == BuffType.AddRoleExp)
                {
                    add += (int)(originalExp * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalExp + add;
        }

        public int ATK(int originalATK)
        {
            int add = 0;
            BuffType type;
            foreach (var item in m_AllBuffs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item.foodID);
                if (Enum.TryParse(m_TempTable.buffType, out type) && type == BuffType.AddATK)
                {
                    add += (int)(originalATK * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalATK + add;
        }

    }
    public enum BuffType
    {
        /// <summary>
        /// 增加弟子战斗力
        /// </summary>
        AddATK = 0,
        /// <summary>
        /// 增加获得的弟子经验
        /// </summary>
        AddRoleExp,
        /// <summary>
        /// 增加获得的铜钱
        /// </summary>
        AddCoin,
    }
    public class FoodBuff
    {
        public int foodID;

        public string endTime;

        public int TimerID;
    }
}