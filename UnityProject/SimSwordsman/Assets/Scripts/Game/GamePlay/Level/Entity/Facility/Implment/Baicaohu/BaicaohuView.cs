using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class BaicaohuView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new BaicaohuController(FacilityType.Baicaohu, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("Baicaohu is clicked");

        }
        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.BaicaohuPanel,facilityType);
        }
    }

}