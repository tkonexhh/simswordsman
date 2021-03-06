using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleAttack : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        public StateBattleAttack(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            string atkName = m_BattleState.NextAtkAnimName;
            //TODO 测试动画
            // atkName = "yijinjing";//"attack";
            EventSystem.S.Send(EventID.OnBattleAtkStart, m_Controller, atkName);

            bool hasAnim = m_Controller.CharacterView.HasAnim(atkName);
            if (hasAnim)
            {
                m_Controller.CharacterView.PlayAnim(atkName, false, OnAtkAnimEnd);
            }
            else
            {
                Log.e("Atk animation not found: " + atkName);
                OnAtkAnimEnd();
            }
        }

        public override void Exit(IBattleStateHander handler)
        {
        }

        public override void Execute(IBattleStateHander handler, float dt)
        {

        }

        public CharacterController GetCharacterController()
        {
            return m_Controller;
        }

        #region Private

        private void OnAtkAnimEnd()
        {
            //Debug.LogError("OnAtkAnimEnd");

            m_BattleState.SetState(BattleStateID.Idle);

            EventSystem.S.Send(EventID.OnBattleAtkEnd, m_Controller);
        }


        #endregion
    }
}
