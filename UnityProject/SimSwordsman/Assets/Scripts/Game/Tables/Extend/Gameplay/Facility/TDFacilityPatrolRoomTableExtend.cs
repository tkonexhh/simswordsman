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

            patrolLevelInfo.InitPatrolRoomInfo(tdData);

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

        internal static List<PatrolRoomInfo> GetPatrolRoomLevelInfoList(FacilityType facilityType)
        {
            List<PatrolRoomInfo> oatrolRoomInfo = new List<PatrolRoomInfo>();
            oatrolRoomInfo.AddRange(levelInfoDic.Values);
            return oatrolRoomInfo;
        }

        public static List<PatrolRoomInfo> GetSameSoltList(PatrolRoomSlot levelInfo)
        {
            List<PatrolRoomInfo> infos = new List<PatrolRoomInfo>();
            foreach (var item in levelInfoDic.Values)
            {
                if (item.GetCurCapacity() == levelInfo.Index)
                    infos.Add(item);
            }
            return infos;
        }
    }
}