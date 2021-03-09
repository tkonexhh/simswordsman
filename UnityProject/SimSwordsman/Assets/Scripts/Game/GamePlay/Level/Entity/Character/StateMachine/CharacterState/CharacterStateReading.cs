using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateReading : CharacterState
    {
        private CharacterController m_Controller = null;
        private KongfuLibraryController m_KongFuController = null;
        private KungfuLibraySlot m_KongfuSlot = null;
        private int m_TimerID = -1;
        private float m_ReadingProgress = 0;

        public CharacterStateReading(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_KongFuController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary);

        }

        public override void Exit(ICharacterStateHander handler)
        {
            if (m_TimerID != -1) {
                Timer.S.Cancel(m_TimerID);
                m_TimerID = -1;
            }

            if (m_Controller != null) {
                m_Controller.ReleaseWorkProgressBar();
            }

            m_KongfuSlot.OnCharacterLeave();
            m_KongfuSlot = null;
            m_KongFuController = null;
            m_Controller = null;
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_KongfuSlot == null)
            {
                m_KongfuSlot = m_KongFuController.GetIdlePracticeSlot();
                if (m_KongfuSlot != null)
                {
                    m_KongfuSlot.OnCharacterEnter(m_Controller);
                    Vector3 practicePos = m_KongfuSlot.GetPosition();
                    m_Controller.MoveTo(practicePos, OnReachDestination);
                }
            }
        }

        private void UpdateReadKongFuProgress() 
        {
            if (m_Controller != null && m_KongfuSlot != null) 
            {
                m_ReadingProgress = m_KongfuSlot.GetProgress();
                m_Controller.SetWorkProgressPercent(m_ReadingProgress);
            }            
        }

        private void OnReachDestination()
        {
            if (m_Controller == null || m_Controller.CurState != CharacterStateID.Reading)
                return;

            m_Controller.CharacterView.PlayAnim("write", true, null);
            m_Controller.SpawnWorkProgressBar();

            UpdateReadKongFuProgress();
            m_TimerID = Timer.S.Post2Really((x) =>
            {
                UpdateReadKongFuProgress();
            }, 1, -1);
        }
    }
}
