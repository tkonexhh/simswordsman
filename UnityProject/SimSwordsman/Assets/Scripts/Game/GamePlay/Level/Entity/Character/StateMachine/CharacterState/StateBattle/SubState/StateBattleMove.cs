using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleMove : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        private float m_MoveSpeed = 1f;
        private Vector2 m_TargetPos = Vector3.zero;

        public StateBattleMove(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            m_Controller.CharacterView.PlayRunAnim();

            m_TargetPos = m_BattleState.MoveTargetPos;

            //Qarth.Log.i("State battle move:" + m_Controller.CharacterCamp);
        }

        public override void Exit(IBattleStateHander handler)
        {
        }

        public override void Execute(IBattleStateHander handler, float dt)
        {
            if (Vector3.Distance(m_TargetPos, m_Controller.GetPosition()) > 0.1f)
            {
                MoveToTargetPos();
            }
            else
            {
                m_BattleState.SetState(BattleStateID.Idle);

                EventSystem.S.Send(EventID.OnBattleMoveEnd, m_Controller);
            }
            //m_BattleState.RefreshTarget();

            //if (m_Controller.FightTarget == null)
            //{
            //    m_BattleState.SetState(BattleStateID.Idle);
            //    return;
            //}

            //if (m_BattleState.IsTargetInAttackRange())
            //{
            //    m_BattleState.SetState(BattleStateID.Attack);
            //}
            //else
            //{
            //    bool isTargetInRight = m_Controller.GetPosition().x <= m_Controller.FightTarget.GetPosition().x ? true : false;
            //    m_TargetPos = m_Controller.FightTarget.GetPosition();
            //    if (isTargetInRight)
            //    {
            //        m_TargetPos.x -= m_Controller.GetAtkRange() * 0.8f;
            //    }
            //    else
            //    {
            //        m_TargetPos.x += m_Controller.GetAtkRange() * 0.8f;
            //    }

            //    MoveToTargetPos();
            //}
        }

        #region Private

        private void MoveToTargetPos()
        {
            Vector2 dir = (m_TargetPos - m_Controller.GetPosition()).normalized;
            Vector2 deltaPos = dir * m_MoveSpeed * Time.deltaTime;
            m_Controller.Move(deltaPos);
        }

        #endregion
    }
}
