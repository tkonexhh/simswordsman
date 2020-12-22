using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityPracticeFieldTable
    {
        public static Dictionary<int, PracticeFieldLevelInfo> levelInfoDic = new Dictionary<int, PracticeFieldLevelInfo>();

        static void CompleteRowAdd(TDFacilityPracticeField tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            PracticeFieldLevelInfo practiceFieldLevelInfo = new PracticeFieldLevelInfo();
            practiceFieldLevelInfo.Warp(levelInfo);
            practiceFieldLevelInfo.SetCurCapatity(tdData.capability);
            practiceFieldLevelInfo.SetCurLevelUpSpeed(tdData.levelUpSpeed);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, practiceFieldLevelInfo);
            }
        }

        public static PracticeFieldLevelInfo GetLevelInfo(int level)
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

            TDFacilityPracticeField item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeReward);
            }

            return levelInfo;
        }
    }
}