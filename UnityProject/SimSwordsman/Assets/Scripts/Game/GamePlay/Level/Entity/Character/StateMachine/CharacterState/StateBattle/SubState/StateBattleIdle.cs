using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleIdle : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        private float m_IdleTime;
        private float m_TimeCounter = 0;

        public StateBattleIdle(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();
            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            m_Controller.CharacterView.PlayAnim("idle_attack", true, null);

            m_IdleTime = UnityEngine.Random.Range(1, 4);
            m_TimeCounter = 0;

            if (m_Controller.IsDead())
            {
                m_BattleState.SetState(BattleStateID.Dead);
            }
        }

        public override void Exit(IBattleStateHander handler)
        {
            Log.i("Character exit idle state");
            m_TimeCounter = 0;
        }

        public override void Execute(IBattleStateHander handler, float dt)
        {
            //m_BattleState.RefreshTarget();

            //m_TimeCounter += Time.deltaTime;
            //if (m_TimeCounter > m_IdleTime)
            //{
            //    if (m_BattleState.IsTargetInAttackRange())
            //    {
            //        if (m_Controller.FightTarget.GetBattleState().IsAttacking() == false)
            //        {
            //            m_BattleState.SetState(BattleStateID.Attack);

            //            m_TimeCounter = 0;

            //        }
            //    }
            //    else
            //    {
            //        m_BattleState.SetState(BattleStateID.Move);
            //    }
            //}
        }

        public CharacterController GetCharacterController()
        {
            return m_Controller;
        }

        #region Private

        #endregion
    }
}
