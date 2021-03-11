using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStatePractice : CharacterState
    {
        private CharacterController m_Controller = null;

        private bool m_IsExit = false;

        private BaseSlot m_PracticeSlot = null;

        private int m_TimerID = -1;

        private float m_ReadingProgress = 0;

        public CharacterStatePractice(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_IsExit = false;

            FacilityType facilityType = m_Controller.CharacterModel.GetTargetFacilityType();
            if (facilityType != FacilityType.PracticeFieldEast && facilityType != FacilityType.PracticeFieldWest)
            {
                Qarth.Log.e("facilityType not right: " + facilityType.ToString());
                return;
            }

            PracticeFieldController practiceFieldController = (PracticeFieldController)MainGameMgr.S.FacilityMgr.GetFacilityController(facilityType);
            m_PracticeSlot = practiceFieldController.GetIdlePracticeSlot(facilityType);
            if (m_PracticeSlot != null)
            {
                m_PracticeSlot.OnCharacterEnter(m_Controller);
                Vector3 practicePos = m_PracticeSlot.GetPosition();
                m_Controller.RunTo(practicePos, OnReachDestination);
            }
            else
            {
                Log.e("Practice field slot not found");
            }
        }

        public override void Exit(ICharacterStateHander handler)    
        {
            m_IsExit = true;

            m_PracticeSlot.OnCharacterLeave();
            m_PracticeSlot = null;

            if (m_Controller != null) 
            {
                m_Controller.ReleaseWorkProgressBar();
            }            

            if(m_TimerID != -1)
            {
                Timer.S.Cancel(m_TimerID);
                m_TimerID = -1;
            }
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void UpdateProgress() 
        {
            if (m_PracticeSlot != null && m_Controller != null) 
            {
                m_ReadingProgress = m_PracticeSlot.GetProgress();
                m_Controller.SetWorkProgressPercent(m_ReadingProgress);
            }            
        }

        private void OnReachDestination()
        {
            if (m_Controller == null || m_Controller.CurState != CharacterStateID.Practice)
                return;

            if (!m_IsExit) {
                m_Controller.CharacterView.PlayAnim("practice", true, null);

                m_Controller.SpawnWorkProgressBar();

                UpdateProgress();
                m_TimerID = Timer.S.Post2Really((x) =>
                {
                    UpdateProgress();
                }, 1, -1);
            }            
        }
    }
}
