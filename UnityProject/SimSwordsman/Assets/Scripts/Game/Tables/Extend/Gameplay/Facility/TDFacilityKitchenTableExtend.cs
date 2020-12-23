using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityKitchenTable
    {
        public static Dictionary<int, KitchLevelInfo> levelInfoDic = new Dictionary<int, KitchLevelInfo>();

        static void CompleteRowAdd(TDFacilityKitchen tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            KitchLevelInfo kongfuLibLevelInfo = new KitchLevelInfo();
            kongfuLibLevelInfo.Warp(levelInfo);


            kongfuLibLevelInfo.SetCurFoodLimit(tdData.foodLimit);
            kongfuLibLevelInfo.SetCurAddSpeed(tdData.foodAddSpeed);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, kongfuLibLevelInfo);
            }
        }

        public static KitchLevelInfo GetLevelInfo(int level)
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

            TDFacilityKitchen item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}