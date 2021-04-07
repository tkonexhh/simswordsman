using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityDeliverTable
    {
        public static Dictionary<int, DeliverLevelInfo> DeliverConfigConfigDic = new Dictionary<int, DeliverLevelInfo>();
        static void CompleteRowAdd(TDFacilityDeliver tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            DeliverLevelInfo deliverLevelInfo = new DeliverLevelInfo();
            deliverLevelInfo.Warp(levelInfo);
            deliverLevelInfo.teamUnlock = tdData.teamUnlock;

            DeliverConfigConfigDic.Add(tdData.level, deliverLevelInfo);
        }
        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityDeliver item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }

        internal static FacilityLevelInfo GetLevelInfo(int level)
        {
            if (DeliverConfigConfigDic.ContainsKey(level))
            {
                return DeliverConfigConfigDic[level];
            }
            return null;
        }
    }

    public class DeliverLevelInfo : FacilityLevelInfo
    {
        public int teamUnlock;
    }
}