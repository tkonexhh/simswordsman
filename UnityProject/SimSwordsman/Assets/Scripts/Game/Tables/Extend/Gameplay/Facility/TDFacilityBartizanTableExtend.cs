using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityBartizanTable
    {
        public static Dictionary<int, BartizanInfo> levelInfoDic = new Dictionary<int, BartizanInfo>();

        static void CompleteRowAdd(TDFacilityBartizan tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            BartizanInfo patrolLevelInfo = new BartizanInfo();
            patrolLevelInfo.Warp(levelInfo);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, patrolLevelInfo);
            }
        }


        public static BartizanInfo GetLevelInfo(int level)
        {
            if (levelInfoDic.ContainsKey(level))
            {
                return levelInfoDic[level];
            }

            return null;
        }

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityBartizan item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}