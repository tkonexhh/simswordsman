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

            //string atkName = GetAnimName();

            string atkName = "01_taizuchangquan";//"attack";

            m_Controller.CharacterView.PlayAnim(atkName, false, OnAtkAnimEnd);
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
            Debug.LogError("OnAtkAnimEnd");

            m_BattleState.SetState(BattleStateID.Idle);

            EventSystem.S.Send(EventID.OnBattleAtkEnd, m_Controller);
        }

        private string GetAnimName()
        {
            if (m_Controller.CharacterCamp == CharacterCamp.OurCamp)
            {
                List<KongfuType> kongfuList = m_Controller.CharacterModel.GetKongfuTypeList();
                KongfuType kongfu = kongfuList[UnityEngine.Random.Range(0, kongfuList.Count)];
                string animName = TDKongfuConfigTable.GetKungfuConfigInfo(kongfu).AnimName;
                return animName;
            }
            else
            {
                EnemyInfo enemyInfo = TDEnemyConfigTable.GetEnemyInfo(m_Controller.CharacterId);
                List<string> animNameList = enemyInfo.animNameList;
                if (animNameList.Count > 0)
                {
                    return animNameList[UnityEngine.Random.Range(0, animNameList.Count)];
                }

                Log.e("Enemy anim list empty");

                return string.Empty;
            }
        }
        #endregion
    }
}
