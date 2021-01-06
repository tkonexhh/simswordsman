using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class FoodBuffSystem : TSingleton<FoodBuffSystem>
	{
        TDFoodConfig m_TempTable;

        public void Init()
        {
            EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals(FoodBuffType.Food_AddATK.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddCoin.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddRoleExp.ToString()))
            {
                EventSystem.S.Send(EventID.OnFoodBuffEnd, cd.ID, param[1]);
            }
        }

        private void OnTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals(FoodBuffType.Food_AddATK.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddCoin.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddRoleExp.ToString()))
            {
                EventSystem.S.Send(EventID.OnFoodBuffTick, cd.ID, param[1]);
            }
        }

        private void OnStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals(FoodBuffType.Food_AddATK.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddCoin.ToString()) || cd.stringID.Equals(FoodBuffType.Food_AddRoleExp.ToString()))
            {
                EventSystem.S.Send(EventID.OnFoodBuffStart, cd.ID, param[1]);
            }
        }

        public void StartBuff(int id, bool ad = false)
        {
            var table = TDFoodConfigTable.GetData(id);
            CountdownSystem.S.StartCountdownerWithMin(table.buffType,id, ad ? table.buffTimeAD : table.buffTime);
        }
        
        public string GetEffectDesc(TDFoodConfig tb)
        {
            FoodBuffType type;
            if (Enum.TryParse(tb.buffType, out type))
            {
                switch (type)
                {
                    case FoodBuffType.Food_AddATK:
                        return string.Format("弟子战力+<color=#8C343C>{0}%</color>", tb.buffRate);
                    case FoodBuffType.Food_AddRoleExp:
                        return string.Format("获得弟子经验+<color=#8C343C>{0}%</color>", tb.buffRate);
                    case FoodBuffType.Food_AddCoin:
                        return string.Format("获得铜钱+<color=#8C343C>{0}%</color>", tb.buffRate);
                    default:
                        break;
                }
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
            return CountdownSystem.S.IsActive(TDFoodConfigTable.GetData(id).buffType, id);
        }

        public string GetCurrentCountdown(int id)
        {
            return CountdownSystem.S.GetCurrentCountdownTime(TDFoodConfigTable.GetData(id).buffType, id);
        }

        public long Coin(long originalCoin)
        {
            int add = 0;
            foreach (var item in GameDataMgr.S.GetPlayerData().unlockFoodItemIDs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item);
                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddCoin.ToString()) && IsActive(item))
                {
                    add +=  Mathf.RoundToInt(originalCoin * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalCoin + add;
        }

        public long RoleExp(long originalExp)
        {
            int add = 0;
            foreach (var item in GameDataMgr.S.GetPlayerData().unlockFoodItemIDs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item);
                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddRoleExp.ToString()) && IsActive(item))
                {
                    add += Mathf.RoundToInt(originalExp * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalExp + add;
        }

        public long ATK(long originalATK)
        {
            int add = 0;
            foreach (var item in GameDataMgr.S.GetPlayerData().unlockFoodItemIDs)
            {
                m_TempTable = TDFoodConfigTable.GetData(item);
                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddATK.ToString()) && IsActive(item))
                {
                    add += Mathf.RoundToInt(originalATK * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalATK + add;
        }

    }
    public enum FoodBuffType
    {
        /// <summary>
        /// 增加弟子战斗力
        /// </summary>
        Food_AddATK = 0,
        /// <summary>
        /// 增加获得的弟子经验
        /// </summary>
        Food_AddRoleExp,
        /// <summary>
        /// 增加获得的铜钱
        /// </summary>
        Food_AddCoin,
    }
}