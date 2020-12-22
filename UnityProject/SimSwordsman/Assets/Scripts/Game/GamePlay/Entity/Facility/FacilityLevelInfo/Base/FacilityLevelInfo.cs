using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class FacilityLevelInfo
	{
        public int level;
        public int upgradeCost;
        public FacilityUpgradePreconditions preconditions;
        public FacilityUpgradeRewards rewards;

        public FacilityLevelInfo()
        {

        }

        public FacilityLevelInfo(int level, int upgradeCost, FacilityUpgradePreconditions preconditions, FacilityUpgradeRewards rewards)
        {
            this.level = level;
            this.upgradeCost = upgradeCost;
            this.preconditions = preconditions;
            this.rewards = rewards;
        }
    
        public void Warp(FacilityLevelInfo levelInfo)
        {
            this.level = levelInfo.level;
            this.upgradeCost = levelInfo.upgradeCost;
            this.preconditions = levelInfo.preconditions;
            this.rewards = levelInfo.rewards;
        }
    }
}