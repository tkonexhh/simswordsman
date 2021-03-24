using System;
using Qarth;


namespace GameWish.Game
{
	public class DialogTrigger8 : ITrigger
	{
        bool m_CanStart = false;
        public bool isReady { get { return m_CanStart;  } }

        Action<bool, ITrigger> m_Listener;

        public void SetParam(object[] param)
        {
            
        }

        public void Start(Action<bool, ITrigger> l)
        {
            m_Listener = l;
            EventSystem.S.Register(EventID.OnGuideDialog8, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
            //解锁工作系统
            EventSystem.S.Send(EventID.OnUnlockWorkSystem);
            EventSystem.S.Send(EventID.OnAddCanWorkFacility, FacilityType.Lobby);
            EventSystem.S.Send(EventID.OnCloseAllUIPanel);
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
            EventSystem.S.UnRegister(EventID.OnGuideDialog8, OnEventListener);

            EventSystem.S.Send(EventID.OnGuideSendCharacterWorkTrigger);
        }
    }
}