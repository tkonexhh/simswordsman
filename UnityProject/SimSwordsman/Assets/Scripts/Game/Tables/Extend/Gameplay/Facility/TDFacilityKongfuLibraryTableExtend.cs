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
            List<KungfuType> kongfuTypeList = new List<KungfuType>();
            foreach (string item in kongfuStrs)
            {
                KungfuType kongfuType = EnumUtil.ConvertStringToEnum<KungfuType>(item);
                kongfuTypeList.Add(kongfuType);
            }
            string[] kongfuListStrs = tdData.kongfuList.Split(';');
            KungFuPoolConfig kungFuPoolConfig = null; 
            foreach (string item in kongfuListStrs)
            {
                string[] kungfuPoolStr =  item.Split('|');
                KungfuType kongfuType = EnumUtil.ConvertStringToEnum<KungfuType>(kungfuPoolStr[0]);
                kungFuPoolConfig = new KungFuPoolConfig(kongfuType,int.Parse(kungfuPoolStr[1]));
            }
            kongfuLibLevelInfo.SetInitData(kongfuTypeList, kungFuPoolConfig, tdData.duration, tdData.seat);

            if (!levelInfoDic.ContainsKey(tdData.level))
                levelInfoDic.Add(tdData.level, kongfuLibLevelInfo);
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