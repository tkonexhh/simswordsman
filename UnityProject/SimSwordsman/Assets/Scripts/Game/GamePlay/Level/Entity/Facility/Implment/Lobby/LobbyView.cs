using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class LobbyView : FacilityView
    {
        [SerializeField]
        private GameObject m_LobbyChallenging;

        public override FacilityController GenerateContoller()
        {
            return new LobbyController( FacilityType.Lobby, this);
        }

        public void SetLobbyChallenging(bool active)
        {
            if (m_Controller.GetState() == FacilityState.Unlocked)
                m_LobbyChallenging.SetActive(active);
        }

        public override void SetViewByState(bool isFile = false)
        {
            base.SetViewByState(isFile);
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