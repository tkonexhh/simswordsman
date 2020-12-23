using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDFacilityLivableRoomTable
    {
        public static List<LivableRoomLevelInfo> levelInfoDic = new List<LivableRoomLevelInfo>();

        static void CompleteRowAdd(TDFacilityLivableRoom tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            LivableRoomLevelInfo roomLevelInfo = new LivableRoomLevelInfo();
            roomLevelInfo.Warp(levelInfo);
            roomLevelInfo.SetCurCapatity( tdData.capability);

            //if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(roomLevelInfo);
            }
        }

        public static LivableRoomLevelInfo GetLevelInfo(int roomId, int level)
        {
            LivableRoomLevelInfo info = levelInfoDic.FirstOrDefault(i => i.r)
            if (levelInfoDic.ContainsKey(level))
            {
                return levelInfoDic[level];
            }

            return null;
        }

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityLivableRoom item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeReward);
            }

            return levelInfo;
        }
    }
}