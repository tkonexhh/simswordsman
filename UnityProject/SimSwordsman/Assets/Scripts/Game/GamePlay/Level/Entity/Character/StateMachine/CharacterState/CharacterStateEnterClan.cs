using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateEnterClan : CharacterState
    {
        private CharacterController m_Controller = null;
        private bool m_HasReachedDestination = false;

        public CharacterStateEnterClan(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            m_Controller = (CharacterController)handler.GetCharacterController();

            m_Controller.CharacterView.StartCoroutine(EnterClanCor());
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {

        }

        private void OnReachLobby()
        {
            m_HasReachedDestination = true;
        }

        private IEnumerator EnterClanCor()
        {
            m_Controller.CharacterView.PlayIdleAnim();

            yield return new WaitForSeconds(2);

            if (m_Controller.CurState == CharacterStateID.EnterClan)
            {

                Vector2 deltaPos = UnityEngine.Random.insideUnitCircle;
                m_Controller.MoveTo(MainGameMgr.S.FacilityMgr.GetDoorPos(FacilityType.Lobby) + new Vector3(deltaPos.x, deltaPos.y, 0), OnReachLobby);

                while (!m_HasReachedDestination)
                {
                    if (m_Controller.CurState != CharacterStateID.EnterClan)
                        yield break;

                    yield return null;
                }

                if (m_Controller.CurState == CharacterStateID.EnterClan)
                {
                    m_Controller.CharacterView.PlayIdleAnim();

                    m_Controller.SetState(CharacterStateID.Wander);
                }
            }
        }
    }
}
