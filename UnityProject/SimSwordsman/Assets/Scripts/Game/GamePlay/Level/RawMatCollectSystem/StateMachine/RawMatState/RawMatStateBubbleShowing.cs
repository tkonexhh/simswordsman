﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class RawMatStateBubbleShowing : RawMatState
    {
        private RawMatItem m_RawMatItem = null;
        private DateTime m_ShowBubbleTime;

        public RawMatStateBubbleShowing(RawMatStateID stateEnum) : base(stateEnum)
        {
            
        }

        public override void Enter(IRawMatStateHander handler)
        {
            m_RawMatItem = handler.GetRawMatItem();
            m_ShowBubbleTime = DateTime.Now;

            m_RawMatItem.ShowBubble();

            RegisterEvents();
        }

        public override void Exit(IRawMatStateHander handler)
        {

        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {
            TimeSpan timeSpan = DateTime.Now - m_ShowBubbleTime;

            if (!(KongfuLibraryPanel.isOpened || PracticeFieldPanel.isOpened))
            {
                if (timeSpan.TotalSeconds > m_RawMatItem.WorkConfigItem.waitingTime && (DateTime.Now - GameplayMgr.resumeTime).TotalSeconds > 5)
                {
                    AutoSelectCharacter();
                }
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

        private void AutoSelectCharacter()
        {
            if (m_RawMatItem.IsFoodEnough() == false)
            {
                m_ShowBubbleTime = DateTime.Now; // Check next interval
                return;
            }

            m_RawMatItem.SelectIdleCharacterToCollectRes(false, (character) => 
            {
                if (character != null)
                {
                    m_RawMatItem.SetState(RawMatStateID.Working);
                }
            });

        }
    }
}
