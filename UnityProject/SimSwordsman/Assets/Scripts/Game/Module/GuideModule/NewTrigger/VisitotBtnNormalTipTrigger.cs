using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Qarth;

namespace GameWish.Game
{
	public class VisitotBtnNormalTipTrigger : ITrigger
	{
        bool m_CanStart = false;
        public bool isReady { get { return m_CanStart; } }

        Action<bool, ITrigger> m_Listener;
        private bool m_IsExcuteListener = false;
        public void SetParam(object[] param)
        {

        }

        public void Start(Action<bool, ITrigger> l)
        {
            m_Listener = l;
            EventSystem.S.Register(EventID.OnVisitorBtnNormalTipTrigger, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
            if (m_IsExcuteListener) return;

            m_IsExcuteListener = true;

            EventSystem.S.Send(EventID.OnCloseAllUIPanel);
            EventSystem.S.Send(EventID.OnCreateVisitor);

            m_CanStart = true;

            if (isReady)
            {
                m_Listener(true, this);
            }
            else
            {
                m_Listener(false, this);
            }
        }
        public void Stop()
        {
            m_CanStart = false;
            m_Listener = null;
            EventSystem.S.UnRegister(EventID.OnVisitorBtnNormalTipTrigger, OnEventListener);

            EventSystem.S.Send(EventID.OnClickVisitorBtnTrigger);
        }
    }	
}