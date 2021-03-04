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
        public static Dictionary<int, LobbyLevelInfo> facilityLevelInfoDic = new Dictionary<int, LobbyLevelInfo>();

        static void CompleteRowAdd(TDFacilityLobby tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            levelInfo.level = tdData.level;
            LobbyLevelInfo lobbyLevelInfo = new LobbyLevelInfo();
            lobbyLevelInfo.Warp(levelInfo);

            lobbyLevelInfo.commonTaskCount = tdData.commonTaskAmount;
            lobbyLevelInfo.AnalysisUnlockContent(tdData.unlockContent);
            lobbyLevelInfo.PracticeLevelMax = tdData.practiceLevelMax;
            if (!facilityLevelInfoDic.ContainsKey(tdData.level))
            {
                facilityLevelInfoDic.Add(tdData.level, lobbyLevelInfo);
            }
        }
        /// <summary>
        /// 获取练功长最大等级||弟子升级限制
        /// </summary>
        /// <param name="lobbyLevel"></param>
        /// <returns></returns>
        public static int GetPracticeLevelMax(int lobbyLevel)
        {
            if (facilityLevelInfoDic.ContainsKey(lobbyLevel))
            {
                return facilityLevelInfoDic[lobbyLevel].PracticeLevelMax;
            }
            return 1;
        }
        
        public static int GetMaxLevel()
        {
            return facilityLevelInfoDic.Values.Count - 1;
        }

        public static FacilityLevelInfo GetLevelInfo(int level)
        {
            if (facilityLevelInfoDic.ContainsKey(level))
            {
                return facilityLevelInfoDic[level];
            }

            return null;
        }

        public static List<string> GetUnlockContent(int level)
        {
            if (facilityLevelInfoDic.ContainsKey(level))
                return facilityLevelInfoDic[level].unlockContent;
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

                    CostItem rewardItem = new CostItem(id, value);
                    upgradeCosts.AddRewardItem(rewardItem);
                }
            }

            levelInfo = new FacilityLevelInfo(level, coin, upgradeNeedLobbyLevel, upgradeCosts);

            return levelInfo;
        }

        //public static int GetCommonTaskCount(int lobbyLevel)
        //{
        //    FacilityLevelInfo levelInfo = GetLevelInfo(lobbyLevel);
        //    if (levelInfo != null)
        //    {
        //        return levelInfo.commonTaskCount;
        //    }

        //    Log.e("Facility level info not found");

        //    return -1;
        //}
    }
}