using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class LobbyView : FacilityView
    {
        public override FacilityController GenerateContoller()
        {
            return new LobbyController( FacilityType.Lobby, this);
        }

        public override void OnClicked()
        {
            base.OnClicked();

            Debug.Log("Lobby is clicked");

        }

        protected override void OpenUIElement()
        {
            base.OpenUIElement();
            UIMgr.S.OpenPanel(UIID.LobbyPanel);

            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_check,facilityType.ToString());
        }
    }
}