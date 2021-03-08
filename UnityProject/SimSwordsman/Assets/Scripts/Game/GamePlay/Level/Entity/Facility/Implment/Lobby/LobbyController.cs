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
            int allCount = MainGameMgr.S.InventoryMgr.GetAllRecruitmentOrderCount();
            if (allCount > 0)
            {
                EventSystem.S.Send(EventID.OnSendRecruitable, true);
                return true;
            }
            else
            {
                EventSystem.S.Send(EventID.OnSendRecruitable, false);
                return false;
            }
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
                    RefreshExclamatoryMark((bool)param[0]);
                    break;
            }
        }
        /// <summary>
        /// Ë¢ÐÂÕÐÄ¼¾ªÌ¾ºÅ
        /// </summary>
    
    }
}