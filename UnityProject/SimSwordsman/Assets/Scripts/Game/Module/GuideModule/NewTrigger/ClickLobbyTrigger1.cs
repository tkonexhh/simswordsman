using System;
using Qarth;


namespace GameWish.Game
{
	public class ClickLobbyTrigger1 : ITrigger
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
            EventSystem.S.Register(EventID.OnGuideClickLobbyTrigger1, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
            EventSystem.S.Send(EventID.OnCloseAllUIPanel);
            EventSystem.S.Send(EventID.OnShowMaskWithAlphaZeroPanel);

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
            EventSystem.S.UnRegister(EventID.OnGuideClickLobbyTrigger1, OnEventListener);

            EventSystem.S.Send(EventID.OnGuideClickRecruitTrigger1);
        }

	}
	
}