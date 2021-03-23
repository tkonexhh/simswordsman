using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class StateBattleAttacked : BattleState
    {
        private CharacterController m_Controller = null;
        private CharacterStateBattle m_BattleState = null;

        public StateBattleAttacked(BattleStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IBattleStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_BattleState == null)
                m_BattleState = (CharacterStateBattle)handler.GetBattleState();

            m_Controller.TriggerCachedDamage();

            if (!m_Controller.IsDead())
            {
                m_Controller.CharacterView.PlayAnim(GetHurtAnimName(), false, null);
                AudioManager.S.PlayCharacterAttackedSound(m_Controller.GetPosition());

                float hitBackDistance = m_BattleState.HitbackDistance;
                float hurtTime = m_BattleState.NextHurtTime;//0.1f;
                Timer.S.Post2Scale(i =>
                {
                    if (m_BattleState.CurState == BattleStateID.Attacked)
                        m_BattleState.SetState(BattleStateID.Idle);
                }, hurtTime);

                // m_Controller.CharacterView.transform.DOMoveX(-m_Controller.CharacterView.GetFaceDir() * hitBackDistance, hurtTime).SetRelative().
                //     SetEase(Ease.Linear).OnComplete(() =>
                //     {
                //         if (m_BattleState.CurState == BattleStateID.Attacked)
                //             m_BattleState.SetState(BattleStateID.Idle);

                //     });
            }
            else
            {
                m_BattleState.SetState(BattleStateID.Dead);
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
        private void OnAtkedAnimEnd()
        {
            Debug.LogError("OnAtkedAnimEnd");

            m_BattleState.SetState(BattleStateID.Idle);

        }

        private string GetHurtAnimName()
        {
            if (m_Controller.CharacterCamp == CharacterCamp.OurCamp)
                return "hurt_1";

            return "hurt";
        }
        #endregion
    }
}
