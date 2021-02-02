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


        public CharacterStateReading(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            KongfuLibraryController kongfuLibrary = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary);
            Vector3 practicePos = kongfuLibrary.GetIdlePracticeSlot().GetPosition();
            m_Controller.MoveTo(practicePos, OnReachDestination);
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
            m_Controller.CharacterView.PlayAnim("write", true, null);
        }
    }
}
