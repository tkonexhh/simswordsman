using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class BaicaohuController : FacilityController
    {
        public BaicaohuController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {

        }
        protected override bool CheckSubFunc()
        {
            if (m_FacilityState != FacilityState.Unlocked)
                return false;
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Baicaohu);

            //foreach (var item in TDFacilityForgeHouseTable.GetLevelInfoDic().Values)
            foreach (var item in TDFacilityBaicaohuTable.GetLevelInfoDic().Values)
            {
                if (item.level <= level)
                {
                    var list = TDHerbConfigTable.MakeNeedItemIDsDic[(int)item.GetCurMedicinalPowderType()];
                    if (MainGameMgr.S.InventoryMgr.HaveEnoughItem(list, false))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

}