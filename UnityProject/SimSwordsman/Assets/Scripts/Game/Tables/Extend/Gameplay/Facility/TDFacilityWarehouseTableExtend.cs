using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityWarehouseTable
    {
        public static Dictionary<int, WarehouseLevelInfo> facilityLevelInfoDic = new Dictionary<int, WarehouseLevelInfo>();

        static void CompleteRowAdd(TDFacilityWarehouse tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);

            WarehouseLevelInfo WarehouseLevelInfo = new WarehouseLevelInfo();
            WarehouseLevelInfo.Warp(levelInfo);
            WarehouseLevelInfo.SetReserves(tdData.reserves);

            if (!facilityLevelInfoDic.ContainsKey(tdData.level))
            {
                facilityLevelInfoDic.Add(tdData.level, WarehouseLevelInfo);
            }
        }

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityWarehouse item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }

        public static WarehouseLevelInfo GetLevelInfo(int level)
        {
            if (facilityLevelInfoDic.ContainsKey(level))
            {
                return facilityLevelInfoDic[level];
            }

            return null;
        }
    }
}