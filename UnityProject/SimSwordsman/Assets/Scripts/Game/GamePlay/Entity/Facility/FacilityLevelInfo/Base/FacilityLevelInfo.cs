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
        public FacilityUpgradePreconditions preconditions;
        public FacilityUpgradeCost upgradeResCosts;

        public FacilityLevelInfo()
        {

        }

        public FacilityLevelInfo(int level, int coinCost, FacilityUpgradePreconditions preconditions, FacilityUpgradeCost rewards)
        {
            this.level = level;
            this.upgradeCoinCost = coinCost;
            this.preconditions = preconditions;
            this.upgradeResCosts = rewards;
        }
    
        public void Warp(FacilityLevelInfo levelInfo)
        {
            this.level = levelInfo.level;
            this.preconditions = levelInfo.preconditions;
            this.upgradeResCosts = levelInfo.upgradeResCosts;
        }

        public int GetNeedLobbyLevel()
        {
            int level = -1;
            FacilityUpgradePreconditionItem item = preconditions.facilityConditions.FirstOrDefault(i => i.facilityType == FacilityType.Lobby);
            if (item != null)
            {
                level = item.value;
            }

            return level;
        }
    }
}