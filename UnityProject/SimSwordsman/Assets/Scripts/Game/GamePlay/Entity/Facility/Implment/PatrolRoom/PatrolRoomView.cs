using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class PatrolRoomView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new PatrolRoomController( FacilityType.PatrolRoom, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("PatrolRoom is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            //UIMgr.S.OpenPanel(UIID.PatrolRoom);
        }
    }

}