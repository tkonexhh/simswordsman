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

        /// <summary>
        /// 获取藏经阁的列表
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        public static List<KongfuLibraryLevelInfo> GetPracticeFieldLevelInfoList(FacilityType facilityType)
        {
            List<KongfuLibraryLevelInfo> kongfuLibraryLevelInfos = new List<KongfuLibraryLevelInfo>();
            kongfuLibraryLevelInfos.AddRange(levelInfoDic.Values);
            return kongfuLibraryLevelInfos;
        }
        /// <summary>
        /// 获取相同的坑位的列表
        /// </summary>
        /// <param name="levelInfo"></param>
        /// <returns></returns>
        public static List<KongfuLibraryLevelInfo> GetSameSoltList(KungfuLibraySlot levelInfo)
        {
            List<KongfuLibraryLevelInfo> infos = new List<KongfuLibraryLevelInfo>();
            foreach (var item in levelInfoDic.Values)
            {
                if (item.GetCurCapacity() == levelInfo.Index)
                    infos.Add(item);
            }
            return infos;
        }

        /// <summary>
        /// 根据等级获取练功时间
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetDurationForLevel( int level)
        {
            if (levelInfoDic.ContainsKey(level))
                return levelInfoDic[level].GetDurationOfCopying();
            return 0;
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