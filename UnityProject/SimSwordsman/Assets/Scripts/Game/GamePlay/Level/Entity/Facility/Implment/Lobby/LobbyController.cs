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
        private void HandleSubAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSendRecruitable:
                    RefreshRecruitableExclamatoryMark((bool)param[0]);
                    break;
            }
        }
        /// <summary>
        /// ˢ����ļ��̾��
        /// </summary>
        private void RefreshRecruitableExclamatoryMark(bool active)
        {
            m_View.SetTips(active);
        }
    }

}