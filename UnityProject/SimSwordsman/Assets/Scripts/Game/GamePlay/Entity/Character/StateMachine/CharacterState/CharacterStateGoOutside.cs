using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateGoOutside : CharacterState
    {
        private CharacterController m_Controller = null;

        private TaskPos m_TaskPos = null;

        public CharacterStateGoOutside(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_TaskPos == null)
            {
                m_TaskPos = GameObject.FindObjectOfType<TaskPos>();
            }

            Vector3 pos = m_TaskPos.GetDoorPos();
            m_Controller.MoveTo(pos, OnReachDestination);
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void OnReachDestination()
        {
            m_Controller.HideBody();
        }
    }
}
