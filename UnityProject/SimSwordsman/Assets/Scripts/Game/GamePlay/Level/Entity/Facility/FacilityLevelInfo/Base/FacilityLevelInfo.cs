using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
	public class FacilityLevelInfo
	{
        public int level;
        public int upgradeCoinCost;
        public int upgradeNeedLobbyLevel;
        public FacilityUpgradeCost upgradeResCosts;

        public List<string> unlockContent = new List<string>();
        public FacilityLevelInfo()
        {

        }
        public int GetUpgradeCondition()
        {
            return upgradeNeedLobbyLevel;
        }
        public FacilityLevelInfo(int level, int coinCost, int needLobbyLevel, FacilityUpgradeCost rewards)
        {
            this.level = level;
            this.upgradeCoinCost = coinCost;
            this.upgradeNeedLobbyLevel = needLobbyLevel;
            this.upgradeResCosts = rewards;
        }

        public void AnalysisUnlockContent(string unlock)
        {
            string[] str = unlock.Split(';');
            foreach (var item in str)
                unlockContent.Add(item);
        }


        public void Warp(FacilityLevelInfo levelInfo)
        {
            this.level = levelInfo.level;
            this.upgradeCoinCost = levelInfo.upgradeCoinCost;
            this.upgradeNeedLobbyLevel = levelInfo.upgradeNeedLobbyLevel;
            this.upgradeResCosts = levelInfo.upgradeResCosts;
        }
        public void Warp(TDFacilityPracticeField levelInfo)
        {
            this.level = levelInfo.level;
            this.upgradeCoinCost = levelInfo.upgradeCost;
            this.upgradeNeedLobbyLevel = levelInfo.upgradePreconditions;
            FacilityUpgradeCost upgradeCost = new FacilityUpgradeCost();

            string[] resArray = levelInfo.upgradeRes.Split(';');
            for (int i = 0; i < resArray.Length; i++)
            {
                string[] res = resArray[i].Split('|');
                upgradeCost.AddRewardItem(new CostItem(int.Parse(res[0]), int.Parse(res[1])));
            }
            this.upgradeResCosts = upgradeCost;
        }
        public List<CostItem> GetUpgradeResCosts()
        {
            return upgradeResCosts.facilityCosts;
        }

        public int GetNeedLobbyLevel()
        {
            return upgradeNeedLobbyLevel;
        }
    }
}