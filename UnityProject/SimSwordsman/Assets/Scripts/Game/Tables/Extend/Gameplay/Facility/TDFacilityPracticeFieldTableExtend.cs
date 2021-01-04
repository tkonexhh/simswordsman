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
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            PracticeFieldLevelInfo practiceFieldLevelInfo = new PracticeFieldLevelInfo();
            practiceFieldLevelInfo.Warp(levelInfo);
            practiceFieldLevelInfo.SetCurCapatity(tdData.capability);
            practiceFieldLevelInfo.SetCurLevelUpSpeed(tdData.levelUpSpeed);
            practiceFieldLevelInfo.SetHouseID((FacilityType)tdData.houseId);
            levelInfoList.Add(practiceFieldLevelInfo);

        }



        public static PracticeFieldLevelInfo GetLevelInfo(FacilityType facilityType, int level)
        {
            PracticeFieldLevelInfo practiceFieldLevelInfo = levelInfoList.Where(i => i.GetHouseID() == facilityType && i.level == level).FirstOrDefault();
            if (practiceFieldLevelInfo != null)
                return practiceFieldLevelInfo;
            return null;
        }

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
    }
}