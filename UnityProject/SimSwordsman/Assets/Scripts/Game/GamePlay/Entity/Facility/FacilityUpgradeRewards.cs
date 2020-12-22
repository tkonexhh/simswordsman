using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum FacilityRewardType
    {
        AddFacility,
        UpgradeFacility,
    }

    public class FacilityUpgradeRewards
	{
        public List<FacilityUpgradeRewardItem> facilityRewards = new List<FacilityUpgradeRewardItem>();

        public void AddRewardItem(FacilityUpgradeRewardItem item)
        {
            facilityRewards.Add(item);
        }

        public string GetContent()
        {
            string str = string.Empty;
            foreach (FacilityUpgradeRewardItem item in facilityRewards)
            {
                str += item.facilityType.ToString() + item.rewardType.ToString() + item.value;
            }

            return str;
        }
	}

    public class FacilityUpgradeRewardItem
    {
        public FacilityRewardType rewardType;
        public FacilityType facilityType;
        public int value;

        public FacilityUpgradeRewardItem(FacilityRewardType rewardType, FacilityType facilityType, int value)
        {
            this.rewardType = rewardType;
            this.facilityType = facilityType;
            this.value = value;
        }
    }

}