using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
    public enum FacilityUpgradePreconditionType
    {
        FacilityCount,
        FacilityLevel,
    }

    public class FacilityUpgradePreconditions
	{
        public List<FacilityUpgradePreconditionItem> facilityConditions = new List<FacilityUpgradePreconditionItem>();

        public void AddCondition(FacilityUpgradePreconditionItem item)
        {
            facilityConditions.Add(item);
        }

        public FacilityUpgradePreconditionItem GetFacilityUpgradePreconditionItem()
        {
            return facilityConditions.FirstOrDefault();
        }
    }

    public class FacilityUpgradePreconditionItem
    {
        //public FacilityUpgradePreconditionType preditionType;
        public FacilityType facilityType;
        public int value;

        //public FacilityUpgradePreconditionItem(FacilityUpgradePreconditionType preditionType, FacilityType facilityType, int value)
        //{
        //    this.preditionType = preditionType;
        //    this.facilityType = facilityType;
        //    this.value = value;
        //}
        public FacilityUpgradePreconditionItem( FacilityType facilityType, int value)
        {
            this.facilityType = facilityType;
            this.value = value;
        }
    }

}