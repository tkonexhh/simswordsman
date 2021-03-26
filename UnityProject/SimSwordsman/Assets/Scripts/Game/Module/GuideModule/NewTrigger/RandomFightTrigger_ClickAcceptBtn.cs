using System;
using Qarth;


namespace GameWish.Game
{
	public class RandomFightTrigger_ClickAcceptBtn : ITrigger
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
            EventSystem.S.Register(EventID.OnRandomFightTrigger_ClickAcceptBtn, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
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
            EventSystem.S.UnRegister(EventID.OnRandomFightTrigger_ClickAcceptBtn, OnEventListener);

            //EventSystem.S.Send(EventID.RandomFightTrigger_FinishedIntroduce);
        }

    }
	
}