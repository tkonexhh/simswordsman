using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class RawMatStateIdle : RawMatState
    {
        private RawMatItem m_RawMatItem = null;
        private DateTime m_LastJobFinishedTime;

        public RawMatStateIdle(RawMatStateID stateEnum) : base(stateEnum)
        {
            
        }

        public override void Enter(IRawMatStateHander handler)
        {
            m_RawMatItem = handler.GetRawMatItem();

            m_LastJobFinishedTime = DateTime.Parse(GameDataMgr.S.GetClanData().GetLastJobFinishedTime(m_RawMatItem.collectedObjType));

            RegisterEvents();
        }

        public override void Exit(IRawMatStateHander handler)
        {

        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {
            TimeSpan timeSpan = DateTime.Now - m_LastJobFinishedTime;

            if (timeSpan.TotalSeconds > m_RawMatItem.WorkConfigItem.workInterval)
            {
                ShowBubble();
            }
            
        }

        private void OnReachDestination()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            //EventSystem.S.Register(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void UnregisterEvents()
        {
            //EventSystem.S.UnRegister(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void HandleEvent(int key, object[] param)
        {
            switch (key)
            {
                //case (int)EventID.OnTaskObjCollected:
                //    break;
            }
        }

        private void ShowBubble()
        {
            m_LastJobFinishedTime = DateTime.Now.AddSeconds(m_RawMatItem.WorkConfigItem.workTime);

            m_RawMatItem.SetState(RawMatStateID.BubbleShowing);
        }
    }
}
