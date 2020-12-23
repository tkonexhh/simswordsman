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

            string[] equipStrs = tdData.unlockMedicinalPowder.Split('|');
            List<MedicinalPowderType> medicinalPowderTypeList = new List<MedicinalPowderType>();
            foreach (string item in equipStrs)
            {
                MedicinalPowderType equip = EnumUtil.ConvertStringToEnum<MedicinalPowderType>(item);
                medicinalPowderTypeList.Add(equip);
            }
            baicaohuLevelInfo.SetCurMedicinalPowderType(medicinalPowderTypeList);

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

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityBaicaohu item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradePreconditions, item.upgradeReward);
            }

            return levelInfo;
        }
    }
}