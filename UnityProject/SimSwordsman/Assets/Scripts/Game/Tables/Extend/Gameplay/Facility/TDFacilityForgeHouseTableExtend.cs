using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityForgeHouseTable
    {
        public static Dictionary<int, ForgeHouseInfo> levelInfoDic = new Dictionary<int, ForgeHouseInfo>();

        static void CompleteRowAdd(TDFacilityForgeHouse tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);
            ForgeHouseInfo forgeHouseLevelInfo = new ForgeHouseInfo();
            forgeHouseLevelInfo.Warp(levelInfo);

            string[] equipStrs = tdData.unlockEquip.Split('|');
            List<EquipmentType> equipTypeList = new List<EquipmentType>();
            foreach (string item in equipStrs)
            {
                EquipmentType equip = (EquipmentType)int.Parse(item);
                equipTypeList.Add(equip);
            }
            forgeHouseLevelInfo.SetCurEquipmentType(equipTypeList, int.Parse(tdData.unlockEquip));

            if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(tdData.level, forgeHouseLevelInfo);
            }
        }

        public static Dictionary<int, ForgeHouseInfo> GetLevelInfoDic()
        {
            return levelInfoDic;
        }

        public static ForgeHouseInfo GetLevelInfo(int level)
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

            TDFacilityForgeHouse item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}