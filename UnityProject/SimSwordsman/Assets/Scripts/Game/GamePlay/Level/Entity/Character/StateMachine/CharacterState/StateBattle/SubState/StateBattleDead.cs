using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleDead : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        public StateBattleDead(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();
            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            m_Controller.CharacterView.PlayDeadAnim();

            AudioManager.S.PlayCharacterDeadSound(m_Controller.CharacterModel.IsWoman());
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

        #endregion
    }
}
