using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class BartizanView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new BartizanController(facilityType, 1, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("Bartizan is clicked");    
        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            //UIMgr.S.OpenPanel(UIID.Bartizan);
        }
    }

}