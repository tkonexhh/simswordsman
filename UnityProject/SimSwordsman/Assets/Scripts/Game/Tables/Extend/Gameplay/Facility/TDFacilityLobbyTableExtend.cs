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
                levelInfo = PassLevelInfo(level, item.upgradeCost, item.upgradePreconditions, item.upgradeReward);
            }

            return levelInfo;
        }

        public static FacilityLevelInfo PassLevelInfo(int level, int upgradeCost, string upgradePrecondition, string upgradeReward)
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

            //Parse rewards
            string[] rewards = upgradeReward?.Split(';');
            FacilityUpgradeRewards upgradeRewards = new FacilityUpgradeRewards();
            for (int i = 0; i < rewards.Length; i++)
            {
                string[] str = rewards[i].Split('_');

                Debug.Assert(str.Length == 3, "Reward pattern error");

                FacilityRewardType rewardType = EnumUtil.ConvertStringToEnum<FacilityRewardType>(str[0]);
                FacilityType facilityType = EnumUtil.ConvertStringToEnum<FacilityType>(str[1]);
                int value = int.Parse(str[2]);

                FacilityUpgradeRewardItem rewardItem = new FacilityUpgradeRewardItem(rewardType, facilityType, value);
                upgradeRewards.AddRewardItem(rewardItem);
            }

            levelInfo = new FacilityLevelInfo(level, upgradeCost, preConditions, upgradeRewards);
            

            return levelInfo;
        }
    }
}