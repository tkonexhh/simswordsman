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
        public List<CostItem> facilityCosts = new List<CostItem>();

        public void AddRewardItem(CostItem item)
        {
            facilityCosts.Add(item);
        }

        public string GetContent()
        {
            string str = string.Empty;
            foreach (CostItem item in facilityCosts)
            {
                //str += item.facilityType.ToString() + item.rewardType.ToString() + item.value;
            }

            return str;
        }
	}

    public struct CostItem
    {
        public int itemId;
        public int value;

        public CostItem(int id, int value)
        {
            this.itemId = id;
            this.value = value;
        }
    }

}