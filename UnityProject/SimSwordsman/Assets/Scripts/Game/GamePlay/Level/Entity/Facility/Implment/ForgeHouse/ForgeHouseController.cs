using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ForgeHouseController : FacilityController
    {
        public ForgeHouseController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {

        }

        protected override bool CheckSubFunc()
        {
            if (m_FacilityState != FacilityState.Unlocked)
                return false;
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.ForgeHouse);

            foreach (var item in TDFacilityForgeHouseTable.GetLevelInfoDic().Values)
            {
                if (item.level <= level)
                {
                    var list = TDEquipmentConfigTable.MakeNeedItemIDsDic[item.EquipID];
                    if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}