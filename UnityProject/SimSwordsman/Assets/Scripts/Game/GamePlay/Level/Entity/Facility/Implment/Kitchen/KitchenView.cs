using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class KitchenView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new KitchenController( FacilityType.Kitchen, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("Kitchen is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.KitchenPanel,facilityType);
            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_check, facilityType.ToString());
        }
    }

}