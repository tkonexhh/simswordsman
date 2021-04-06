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
        }
    }	
}