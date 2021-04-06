using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CharacterStateDeliver : CharacterState
    {
        private CharacterController m_Controller = null;

        private FacilityType m_FacilityType = FacilityType.None;

        private FacilityController m_FacilityController;

        public CharacterStateDeliver(CharacterStateID stateEnum) : base(stateEnum)
        {
        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            bool isFindPathToTargetPos = m_Controller.CharacterModel.IsFindPathToTargetPos();

            if (isFindPathToTargetPos)
            {
                m_FacilityType = FacilityType.Deliver;

                m_FacilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);

                Vector3 targetPos = m_FacilityController.GetDoorPos();

                m_Controller.RunTo(targetPos, OnReachDestination);
            }
            else {
                //吧人物移出场景
                m_Controller.SetPosition(Vector3.one * 20);
            }
        }

        private void OnReachDestination()
        {
            Qarth.EventSystem.S.Send(EventID.OnCharacterReachDeliverPos, m_Controller.CharacterId);
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        public override void Exit(ICharacterStateHander handler)
        {

        }
    }
}