using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public enum FacilityCostType
    {
        Coin = 1,
        Food = 2,
    }

    public class FacilityUpgradeCost
	{
        public List<FacilityUpgradeCostItem> facilityCosts = new List<FacilityUpgradeCostItem>();

        public void AddRewardItem(FacilityUpgradeCostItem item)
        {
            facilityCosts.Add(item);
        }

        public string GetContent()
        {
            string str = string.Empty;
            foreach (FacilityUpgradeCostItem item in facilityCosts)
            {
                //str += item.facilityType.ToString() + item.rewardType.ToString() + item.value;
            }

            return str;
        }
	}

    public class FacilityUpgradeCostItem
    {
        public FacilityCostType rewardType;
        public int value;

        public FacilityUpgradeCostItem(FacilityCostType costType, int value)
        {
            this.rewardType = costType;
            this.value = value;
        }
    }

}