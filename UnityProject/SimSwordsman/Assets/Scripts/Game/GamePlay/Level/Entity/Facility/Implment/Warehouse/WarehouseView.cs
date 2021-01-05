using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class WarehouseView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new WarehouseController( FacilityType.Warehouse, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("WarehouseView is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.WarehousePanel);
        }
    }

}