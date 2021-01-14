using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateWorking : CharacterState
    {
        private CharacterController m_Controller = null;


        public CharacterStateWorking(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            FacilityType facilityType = m_Controller.CharacterModel.GetTargetFacilityType();

            FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(facilityType);
            Vector3 targetPos = facilityController.GetDoorPos();

            m_Controller.MoveTo(targetPos, OnReachDestination);
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
            m_Controller.CharacterView.PlayAnim("practice", true, null);
        }
    }
}
