using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityLobbyTable
    {
        public static Dictionary<int, FacilityLevelInfo> facilityLevelInfoDic = new Dictionary<int, FacilityLevelInfo>();

        static void CompleteRowAdd(TDFacilityLobby tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);

            if (!facilityLevelInfoDic.ContainsKey(tdData.level))
            {
                facilityLevelInfoDic.Add(tdData.level, levelInfo);
            }
        }

        public static FacilityLevelInfo GetLevelInfo(int level)
        {
            if (facilityLevelInfoDic.ContainsKey(level))
            {
                return facilityLevelInfoDic[level];
            }

            return null;
        }

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityLobby item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = PassLevelInfo(level, item.upgradeCost, 0, item.upgradeRes);
            }

            return levelInfo;
        }

        public static FacilityLevelInfo PassLevelInfo(int level, int coin, int upgradeNeedLobbyLevel, string upgradeCostsStr)
        {
            FacilityLevelInfo levelInfo = null;
            
            //Parse costs
            FacilityUpgradeCost upgradeCosts = new FacilityUpgradeCost();
            if (!string.IsNullOrEmpty(upgradeCostsStr))
            {
                string[] costs = upgradeCostsStr?.Split(';');
                
                for (int i = 0; i < costs.Length; i++)
                {
                    string[] str = costs[i].Split('|');

                    Debug.Assert(str.Length == 2, "Cost pattern error");

                    int id = int.Parse(str[0]);
                    int value = int.Parse(str[1]);

                    FacilityUpgradeCostItem rewardItem = new FacilityUpgradeCostItem(id, value);
                    upgradeCosts.AddRewardItem(rewardItem);
                }
            }

            levelInfo = new FacilityLevelInfo(level, coin, upgradeNeedLobbyLevel, upgradeCosts);

            return levelInfo;
        }
    }
}