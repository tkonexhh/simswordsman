using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class FoodBuffSystem : TSingleton<FoodBuffSystem>
	{
        private TDFoodConfig m_TempTable;
                
        /// <summary>
        /// �Ƿ��ڼ���״̬
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsFoodBuffActive(int id)
        {
            return GameDataMgr.S.GetClanData().IsBuffActiveState(id);
        }

        public long Coin(long originalCoin)
        {
            int add = 0;
            List<FoodBuffData> buffDataList = GameDataMgr.S.GetClanData().FoodBufferDataList;
            for (int i = 0; i < buffDataList.Count; i++)
            {
                m_TempTable = TDFoodConfigTable.GetData(buffDataList[i].FoodBufferID);
                
                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddCoin.ToString())) 
                {
                    add += Mathf.RoundToInt(originalCoin * m_TempTable.buffRate * 0.01f);
                }
            }

            return originalCoin + add;
        }

        public long KongFuExp(long originalValue)
        {
            int add = 0;
            List<FoodBuffData> buffDataList = GameDataMgr.S.GetClanData().FoodBufferDataList;
            for (int i = 0; i < buffDataList.Count; i++)
            {
                m_TempTable = TDFoodConfigTable.GetData(buffDataList[i].FoodBufferID);

                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddRoleExp.ToString()))
                {
                    add += Mathf.RoundToInt(originalValue * m_TempTable.buffRate * 0.01f);
                }
            }

            return originalValue + add;
        }

        public long Exp(long originalValue)
        {
            int add = 0;
            List<FoodBuffData> buffDataList = GameDataMgr.S.GetClanData().FoodBufferDataList;
            for (int i = 0; i < buffDataList.Count; i++)
            {
                m_TempTable = TDFoodConfigTable.GetData(buffDataList[i].FoodBufferID);

                if (m_TempTable.buffType.Equals(FoodBuffType.Food_AddExp.ToString()))
                {
                    add += Mathf.RoundToInt(originalValue * m_TempTable.buffRate * 0.01f);
                }
            }
            return originalValue + add;
        }

    }
    public enum FoodBuffType
    {
        /// <summary>
        /// ���ӵ��ӻ�þ���
        /// </summary>
        Food_AddExp = 0,
        /// <summary>
        /// ���ӵ��ӻ�õĹ�����
        /// </summary>
        Food_AddRoleExp,
        /// <summary>
        /// ���ӻ�õ�ͭǮ
        /// </summary>
        Food_AddCoin,
    }
}