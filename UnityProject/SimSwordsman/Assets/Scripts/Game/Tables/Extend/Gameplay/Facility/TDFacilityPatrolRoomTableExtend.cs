using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityPatrolRoomTable
    {
        public static Dictionary<int, PatrolRoomInfo> levelInfoDic = new Dictionary<int, PatrolRoomInfo>();

        static void CompleteRowAdd(TDFacilityPatrolRoom tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            PatrolRoomInfo patrolLevelInfo = new PatrolRoomInfo();
            patrolLevelInfo.Warp(levelInfo);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, patrolLevelInfo);
            }
        }

        public static PatrolRoomInfo GetLevelInfo(int level)
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

            TDFacilityPatrolRoom item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}