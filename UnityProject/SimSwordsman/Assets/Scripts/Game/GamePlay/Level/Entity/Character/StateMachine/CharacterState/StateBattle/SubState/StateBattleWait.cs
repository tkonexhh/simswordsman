using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleWait : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        private float m_IdleTime;
        private float m_TimeCounter = 0;

        public StateBattleWait(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();
            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            if(m_Controller.CharacterCamp == CharacterCamp.OurCamp)
                m_Controller.CharacterView.PlayAnim("practice", true, null);
            if (m_Controller.CharacterCamp == CharacterCamp.EnemyCamp)
                m_Controller.CharacterView.PlayAnim("idle", true, null);
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
