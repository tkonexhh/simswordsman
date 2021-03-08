using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityBaicaohuTable
    {
        public static Dictionary<int, BaicaohuInfo> levelInfoDic = new Dictionary<int, BaicaohuInfo>();

        static void CompleteRowAdd(TDFacilityBaicaohu tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            BaicaohuInfo baicaohuLevelInfo = new BaicaohuInfo();
            baicaohuLevelInfo.Warp(levelInfo);

            HerbType equip = (HerbType)int.Parse(tdData.unlockHerbID);
            baicaohuLevelInfo.SetCurMedicinalPowderType(equip);

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, baicaohuLevelInfo);
            }
        }

        public static BaicaohuInfo GetLevelInfo(int level)
        {
            if (levelInfoDic.ContainsKey(level))
            {
                return levelInfoDic[level];
            }

            return null;
        }
        public static Dictionary<int, BaicaohuInfo> GetLevelInfoDic()
        {
            return levelInfoDic;
        }
        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityBaicaohu item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}