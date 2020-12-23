using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class FacilityLevelInfo
	{
        public int level;
        public FacilityUpgradePreconditions preconditions;
        public FacilityUpgradeCost upgradeCosts;

        public FacilityLevelInfo()
        {

        }

        public FacilityLevelInfo(int level, FacilityUpgradePreconditions preconditions, FacilityUpgradeCost rewards)
        {
            this.level = level;
            this.preconditions = preconditions;
            this.upgradeCosts = rewards;
        }
    
        public void Warp(FacilityLevelInfo levelInfo)
        {
            this.level = levelInfo.level;
            this.preconditions = levelInfo.preconditions;
            this.upgradeCosts = levelInfo.upgradeCosts;
        }
    }
}