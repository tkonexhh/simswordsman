using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class ForgeHouseView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new ForgeHouseController( FacilityType.ForgeHouse, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("ForgeHouse is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.ForgeHousePanel,facilityType);
        }
    }

}