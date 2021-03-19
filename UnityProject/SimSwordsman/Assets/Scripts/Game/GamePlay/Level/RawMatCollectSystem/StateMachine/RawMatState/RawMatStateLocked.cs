using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class RawMatStateLocked : RawMatState
    {
        private RawMatItem m_RawMatItem = null;

        public RawMatStateLocked(RawMatStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IRawMatStateHander handler)
        {
            m_RawMatItem = (RawMatItem)handler.GetRawMatItem();

            m_RawMatItem.HideBubble();

            RegisterEvents();
        }

        public override void Exit(IRawMatStateHander handler)
        {
            UnregisterEvents();
        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
           
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnEndUpgradeFacility, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnEndUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnEndUpgradeFacility:
                    FacilityType facilityType = (FacilityType)param[0];
                    if (facilityType == FacilityType.Lobby)
                    {
                        bool isUnlocked = m_RawMatItem.IsUnlocked();
                        if (isUnlocked)
                        {
                            m_RawMatItem.SetState(RawMatStateID.Idle);
                        }
                    }
                    break;
            }
        }
    }
}
