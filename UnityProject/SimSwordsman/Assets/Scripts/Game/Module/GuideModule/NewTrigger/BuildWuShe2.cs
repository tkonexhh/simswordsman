using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class BuildWuShe2 : ITrigger
	{
        bool m_CanStart = false;
        public bool isReady { get { return m_CanStart; } }

        Action<bool, ITrigger> m_Listener;

        public void SetParam(object[] param)
        {

        }

        public void Start(Action<bool, ITrigger> l)
        {
            m_Listener = l;
            EventSystem.S.Register(EventID.OnGuideDialog2, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
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
            EventSystem.S.UnRegister(EventID.OnGuideDialog2, OnEventListener);

            EventSystem.S.Send(EventID.OnGuideBuildFacilityTrigger1);
        }
    }	
}