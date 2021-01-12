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
            Vector3 practicePos = practiceFieldController.GetIdlePracticeSlot().GetPosition();
            m_Controller.MoveTo(practicePos, OnReachDestination);
        }

        public override void Exit(ICharacterStateHander handler)    
        {
            m_IsExit = true;
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
            if(!m_IsExit)
                m_Controller.CharacterView.PlayAnim("practice", true, null);
        }
    }
}
