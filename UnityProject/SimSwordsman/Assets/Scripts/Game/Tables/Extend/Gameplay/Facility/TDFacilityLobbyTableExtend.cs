using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityLobbyTable
    {
        public static Dictionary<int, FacilityLevelInfo> facilityLevelInfoDic = new Dictionary<int, FacilityLevelInfo>();

        static void CompleteRowAdd(TDFacilityLobby tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.level);

            if (!facilityLevelInfoDic.ContainsKey(tdData.level))
            {
                facilityLevelInfoDic.Add(tdData.level, levelInfo);
            }
        }

        public static FacilityLevelInfo GetLevelInfo(int level)
        {
            if (facilityLevelInfoDic.ContainsKey(level))
            {
                return facilityLevelInfoDic[level];
            }

            return null;
        }

        private static FacilityLevelInfo PassLevelInfo(int level)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityLobby item = null;
            bool haveData = m_DataCache.TryGetValue(level, out item);
            if (haveData)
            {
                levelInfo = PassLevelInfo(level, item.upgradePreconditions, item.upgradeReward);
            }

            return levelInfo;
        }

        public static FacilityLevelInfo PassLevelInfo(int level, string upgradePrecondition, string upgradeCostsStr)
        {
            FacilityLevelInfo levelInfo = null;

            //Parse conditions
            string[] conditions = upgradePrecondition?.Split(';');
            FacilityUpgradePreconditions preConditions = new FacilityUpgradePreconditions();
            for (int i = 0; i < conditions.Length; i++)
            {
                string[] str = conditions[i].Split('_');

                //Debug.Assert(str.Length == 3, "Condition pattern error");

                //FacilityUpgradePreconditionType preconditionType = EnumUtil.ConvertStringToEnum<FacilityUpgradePreconditionType>(str[0]);
                FacilityType facilityType = EnumUtil.ConvertStringToEnum<FacilityType>(str[0]);
                int value = int.Parse(str[1]);

                FacilityUpgradePreconditionItem conditionItem = new FacilityUpgradePreconditionItem(facilityType, value);
                preConditions.AddCondition(conditionItem);
            }

            //Parse costs
            string[] costs = upgradeCostsStr?.Split(';');
            FacilityUpgradeCost upgradeCosts = new FacilityUpgradeCost();
            for (int i = 0; i < costs.Length; i++)
            {
                string[] str = costs[i].Split('_');

                Debug.Assert(str.Length == 3, "Cost pattern error");

                FacilityCostType costType = EnumUtil.ConvertStringToEnum<FacilityCostType>(str[0]);
                int value = int.Parse(str[1]);

                FacilityUpgradeCostItem rewardItem = new FacilityUpgradeCostItem(costType, value);
                upgradeCosts.AddRewardItem(rewardItem);
            }

            levelInfo = new FacilityLevelInfo(level, preConditions, upgradeCosts);
            

            return levelInfo;
        }
    }
}