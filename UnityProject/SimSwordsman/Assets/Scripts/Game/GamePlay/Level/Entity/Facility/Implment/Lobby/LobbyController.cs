using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class LobbyController : FacilityController
    {
        //private LobbyView m_View = null;
        //private LobbyModel m_Model = null;

        public LobbyController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            EventSystem.S.Register(EventID.OnSendRecruitable, HandleSubAddListenerEvent);
        }
        ~LobbyController()
        {
            EventSystem.S.UnRegister(EventID.OnSendRecruitable, HandleSubAddListenerEvent);
        }
        private bool CheackRecruitmentOrder()
        {
            return CommonUIMethod.CheackRecruitmentOrder();
        }
        protected override bool CheckSubFunc()
        {
            return CheackRecruitmentOrder();
        }

        private void HandleSubAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSendRecruitable:
                    ((LobbyView)m_View).SetLobbyChallenging((bool)param[0]);
                    //RefreshExclamatoryMark((bool)param[0]);
                    break;
            }
        }
    }
}