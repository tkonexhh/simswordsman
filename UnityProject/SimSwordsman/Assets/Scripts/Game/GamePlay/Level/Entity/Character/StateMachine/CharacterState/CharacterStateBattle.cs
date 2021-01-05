using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateBattle : CharacterState, IBattleStateHander
    {
        private CharacterController m_Controller = null;
        private BattleStateMachine m_BattleStateMachine = null;
        private BattleStateID m_CurState = BattleStateID.None;
        private Vector2 m_MoveTargetPos = Vector2.zero;
        private float m_HitbackDistance = 0;

        public BattleStateID CurState { get => m_CurState;}
        public Vector2 MoveTargetPos { get => m_MoveTargetPos; set => m_MoveTargetPos = value; }
        public float HitbackDistance { get => m_HitbackDistance; set => m_HitbackDistance = value; }

        public CharacterStateBattle(CharacterStateID stateEnum) : base(stateEnum)
        {
            m_BattleStateMachine = new BattleStateMachine(this);
        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            //SetState(BattleStateID.Idle);
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            m_BattleStateMachine.UpdateState(Time.deltaTime);
        }

        public CharacterController GetCharacterController()
        {
            return m_Controller;
        }

        public CharacterStateBattle GetBattleState()
        {
            return this;
        }

        public bool IsTargetInAttackRange()
        {
            bool canAtk = m_Controller.FightTarget != null && Mathf.Abs(m_Controller.GetPosition().y - m_Controller.FightTarget.GetPosition().y) < 0.3f &&
                          Vector2.Distance(m_Controller.GetPosition(), m_Controller.FightTarget.GetPosition()) < m_Controller.GetAtkRange();

            return canAtk;
        }

        public bool IsTargetAttacking()
        {
            bool isAttacking = m_Controller.FightTarget.GetBattleState().IsAttacking() == true;
            return isAttacking;
        }

        public bool IsAttacking()
        {
            return m_CurState == BattleStateID.Attack;
        }

        public void SetState(BattleStateID battleState)
        {
            if (m_CurState != battleState)
            {
                m_CurState = battleState;
                m_BattleStateMachine.SetCurrentStateByID((int)m_CurState);
            }
        }
        public void RefreshTarget()
        {
            if (m_Controller.FightTarget == null || m_Controller.FightTarget.IsDead())
            {
                m_Controller.SetFightTarget(FindTarget());
            }
        }

        #region Private

        private CharacterController FindTarget()
        {
            CharacterController target = MainGameMgr.S.BattleFieldMgr.GetNearestCharacterAlive(m_Controller.CharacterCamp, m_Controller.GetPosition());
            return target;
        }

        #endregion
    }
}
