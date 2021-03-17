using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class RawMatStateWorking : RawMatState
    {
        private RawMatItem m_RawMatItem = null;

        public RawMatStateWorking(RawMatStateID stateEnum) : base(stateEnum)
        {
            
        }

        public override void Enter(IRawMatStateHander handler)
        {
            m_RawMatItem = handler.GetRawMatItem();
            m_RawMatItem.HideBubble();

            RegisterEvents();
        }

        public override void Exit(IRawMatStateHander handler)
        {

        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
            UnregisterEvents();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void HandleEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnTaskObjCollected:
                    CollectedObjType collectedObjType = (CollectedObjType)param[0];
                    if (collectedObjType == m_RawMatItem.collectedObjType)
                    {
                        GameDataMgr.S.GetClanData().SetLastJobFinishedTime(m_RawMatItem.collectedObjType, DateTime.Now);

                        m_RawMatItem.SetState(RawMatStateID.Idle);
                    }
                    break;
            }
        }
    }
}
