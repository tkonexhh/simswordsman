using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class LivableRoomView : FacilityView
    {
        public int subId;
        public override FacilityController GenerateContoller()
        {
            return new LivableRoomController(facilityType, subId, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("LivableRoom is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.LivableRoomPanel, facilityType, subId);
        }
    }

}