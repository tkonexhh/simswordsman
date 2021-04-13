using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateWorking : CharacterState
    {
        private CharacterController m_Controller = null;

        private FacilityType m_FacilityType = FacilityType.None;

        private FacilityController m_FacilityController;

        private bool m_ReachTargetPos = false;

        private WorkItemData m_WorkItemData = null;

        private float m_CurrentWorkTime = 0;

        private bool m_IsWorkingFinished = false;

        public CharacterStateWorking(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {

            try
            {
                m_IsWorkingFinished = false;
                m_ReachTargetPos = false;

                if (m_Controller == null)
                    m_Controller = (CharacterController)handler.GetCharacterController();

                m_FacilityType = m_Controller.CharacterModel.GetTargetFacilityType();
                m_WorkItemData = GameDataMgr.S.GetClanData().GetWorkItemData(m_FacilityType);
                if (m_WorkItemData == null)
                {
                    m_Controller.SetState(CharacterStateID.Wander);
                    return;
                }
                m_CurrentWorkTime = m_WorkItemData.CurrentWorkTime;
                m_Controller.SpawnWorkTipWhenWorkInFacility(m_FacilityType);
                m_FacilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);
                Vector3 targetPos = m_FacilityController.GetDoorPos();
                m_Controller.RunTo(targetPos, OnReachDestination);
            }
            catch (Exception e)
            {
                Debug.LogError("报错=============" + e.Message + " " + e.StackTrace);
            }

        }

        public override void Exit(ICharacterStateHander handler)
        {

        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_IsWorkingFinished) return;

            if (m_ReachTargetPos)
            {
                m_CurrentWorkTime += Time.deltaTime;

                GameDataMgr.S.GetClanData().UpdateWorkTime(m_FacilityType, (int)m_CurrentWorkTime);

                m_Controller.SetWorkProgressPercent(m_CurrentWorkTime / m_WorkItemData.WorkTotalTime);

                if (m_CurrentWorkTime > m_WorkItemData.WorkTotalTime)
                {
                    m_IsWorkingFinished = true;

                    GetReward();

                    GameDataMgr.S.GetClanData().RemoveWorkData(m_FacilityType);

                    m_Controller.ReleaseWorkProgressBar();

                    m_FacilityController.ChangeFacilityWorkingState(FacilityWorkingStateEnum.Idle);

                    DataAnalysisMgr.S.CustomEvent(DotDefine.work_finish, "Coin");

                    m_Controller.SetState(CharacterStateID.Wander);

                    m_Controller.CharacterView.SetSweepingSmoke(false);
                }
            }
        }

        private void GetReward()
        {
            TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());

            if (lobbyData != null)
            {
                m_Controller.AddExp(lobbyData.workExp);
                m_Controller.SpawnFacilityWorkRewardPop(m_FacilityType, lobbyData.workPay);
                GameDataMgr.S.GetPlayerData().AddCoinNum(lobbyData.workPay);
            }
            else
            {
                Debug.LogError("lobby data is null");
            }

        }

        private void OnReachDestination()
        {
            m_Controller.ReleaseWorkTip();

            string anim = GetAnimName(m_FacilityType);

            m_Controller.CharacterView.PlayAnim(anim, true, () =>
            {
                if (IsClean(anim))
                {
                    m_Controller.CharacterView.SetSweepingSmoke(true);
                    AudioManager.S.PlaySweepSound(m_Controller.GetPosition());
                }
            });

            m_Controller.SpawnWorkProgressBar();

            m_Controller.SetWorkProgressPercent(0);

            m_ReachTargetPos = true;
        }
        private bool IsClean(string anim)
        {
            return anim.Equals("clean");
        }
        private string GetAnimName(FacilityType facilityType)
        {
            string animName = "clean";
            switch (facilityType)
            {
                case FacilityType.ForgeHouse:
                    animName = "forge_iron";
                    AudioManager.S.PlayForgeSound(m_Controller.GetPosition());
                    break;
                case FacilityType.Kitchen:
                    animName = "cook";
                    break;
                case FacilityType.Baicaohu:
                    animName = "pharmacy";
                    AudioManager.S.PlayPoundSound(m_Controller.GetPosition());
                    break;
                case FacilityType.Warehouse:
                    animName = "carry";
                    break;
                default:
                    animName = "clean";
                    break;
            }

            return animName;
        }
    }
}
