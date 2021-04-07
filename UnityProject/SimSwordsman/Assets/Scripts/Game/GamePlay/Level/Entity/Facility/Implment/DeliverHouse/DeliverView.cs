using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class DeliverView : FacilityView
	{
        public override FacilityController GenerateContoller()
        {
            return new DeliverController(FacilityType.Deliver, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();
        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.DeliverPanel);
        }

        public override void SetViewByState(bool isFile = false)
        {
            base.SetViewByState(isFile);
        }
    }	
}