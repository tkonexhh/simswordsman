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
        private FacilityType m_FacilityType = FacilityType.None;

        public CharacterStateWorking(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_FacilityType = m_Controller.CharacterModel.GetTargetFacilityType();

            FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);
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
            string anim = GetAnimName(m_FacilityType);
            m_Controller.CharacterView.PlayAnim(anim, true, null);
        }

        private string GetAnimName(FacilityType facilityType)
        {
            string animName = "clean";
            switch (facilityType)
            {
                case FacilityType.ForgeHouse:
                    animName = "forge_iron";
                    break;
                case FacilityType.Kitchen:
                    animName = "cook";
                    break;
                case FacilityType.Baicaohu:
                    animName = "pharmacy";
                    break;
                case FacilityType.Warehouse:
                    animName = "carry";
                    break;
            }

            return animName;
        }
    }
}
