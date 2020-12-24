using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityKongfuLibraryTable
    {
        public static Dictionary<int, KongfuLibraryLevelInfo> levelInfoDic = new Dictionary<int, KongfuLibraryLevelInfo>();

        static void CompleteRowAdd(TDFacilityKongfuLibrary tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            KongfuLibraryLevelInfo kongfuLibLevelInfo = new KongfuLibraryLevelInfo();
            kongfuLibLevelInfo.Warp(levelInfo);

            string[] kongfuStrs = tdData.unlockKongfu.Split('|');
            List<KongfuType> kongfuTypeList = new List<KongfuType>();
            foreach (string item in kongfuStrs)
            {
                KongfuType kongfuType = EnumUtil.ConvertStringToEnum<KongfuType>(item);
                kongfuTypeList.Add(kongfuType);
            }
            kongfuLibLevelInfo.SetCurLevelUnlockedKongfuList(kongfuTypeList);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, kongfuLibLevelInfo);
            }
        }

        public static KongfuLibraryLevelInfo GetLevelInfo(int level)
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

            TDFacilityKongfuLibrary item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}