using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateWander : CharacterState
    {
        private CharacterController m_Controller = null;

        private bool m_IsExit = false;

        private RandomWayPoints m_RandomWayPoints = null;

        public CharacterStateWander(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_RandomWayPoints == null)
                m_RandomWayPoints = GameObject.FindObjectOfType<RandomWayPoints>();

            m_IsExit = false;

            Wander();
        }

        public override void Exit(ICharacterStateHander handler)
        {
            m_IsExit = true;
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void Wander()
        {
            float idleTime = UnityEngine.Random.Range(3, 8);
            m_Controller.CharacterView.StartCoroutine(IdleCor(idleTime));
        }

        private IEnumerator IdleCor(float idleTime)
        {
            m_Controller.CharacterView.PlayIdleAnim();
            yield return new WaitForSeconds(idleTime);

            if (m_IsExit == false)
            {
                m_Controller.MoveTo(m_RandomWayPoints.GetRandomWayPointPos(m_Controller.GetPosition()), OnReachDestination);
            }
        }

        private void OnReachDestination()
        {
            if (m_IsExit == false)
            {
                Wander();
            }
        }
    }
}
