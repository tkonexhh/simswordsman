using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDFacilityPracticeFieldTable
    {
        public static List<PracticeFieldLevelInfo> levelInfoList = new List<PracticeFieldLevelInfo>();

        static void CompleteRowAdd(TDFacilityPracticeField tdData)
        {
            PracticeFieldLevelInfo practiceFieldLevelInfo = new PracticeFieldLevelInfo();
            practiceFieldLevelInfo.Warp(tdData);
            practiceFieldLevelInfo.SetCurData(tdData);
            levelInfoList.Add(practiceFieldLevelInfo);
        }
        public static List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList()
        {
            return levelInfoList;
        }
        public static List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList(FacilityType facilityType)
        {
            List<PracticeFieldLevelInfo> levelInfo = new List<PracticeFieldLevelInfo>();
            levelInfoList.ForEach(i =>
            {
                if (i.GetHouseID() == facilityType)
                    levelInfo.Add(i);
            });
            return levelInfo;
        }

        public static PracticeFieldLevelInfo GetLevelInfo(FacilityType facilityType, int level)
        {
            PracticeFieldLevelInfo practiceFieldLevelInfo = levelInfoList.Where(i => i.GetHouseID() == facilityType && i.level == level).FirstOrDefault();
            if (practiceFieldLevelInfo != null)
                return practiceFieldLevelInfo;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityPracticeField item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }

        /// <summary>
        /// 根据等级获取练功时间
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetDurationForLevel(FacilityType facilityType, int level)
        {
            PracticeFieldLevelInfo levelInfo = levelInfoList.Where(i => i.GetHouseID() == facilityType && i.level == level).FirstOrDefault();
            if (levelInfo != null)
                return levelInfo.GetDuration();
            return 0;
        }

        /// <summary>
        /// 根据等级获取练功经验
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetExpValue(FacilityType facilityType, int level)
        {
            PracticeFieldLevelInfo levelInfo = levelInfoList.Where(i => i.GetHouseID() == facilityType && i.level == level).FirstOrDefault();
            if (levelInfo != null)
                return levelInfo.GetCurLevelUpSpeed();
            return 0;
        }

        public static int GetSeatNeedLevel(int seat)
        {
            return seat;
        }
    }
}