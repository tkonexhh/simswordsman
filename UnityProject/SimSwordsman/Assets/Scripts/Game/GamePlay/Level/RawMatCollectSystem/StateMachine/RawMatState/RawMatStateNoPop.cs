using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class RawMatStateNoPop : RawMatState
    {
        private CharacterController m_Controller = null;
        private FacilityType m_FacilityType = FacilityType.None;
        private string m_WorkStringId;

        public RawMatStateNoPop(RawMatStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IRawMatStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_FacilityType = m_Controller.CharacterModel.GetTargetFacilityType();  

            m_WorkStringId = WorkSystem.GetStringId(m_FacilityType);
            m_Controller.SpawnWorkTipWhenWorkInFacility(m_FacilityType);

            FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);
            Vector3 targetPos = facilityController.GetDoorPos();

            m_Controller.RunTo(targetPos, OnReachDestination);

            EventSystem.S.Register(EventID.OnAddWorkingRewardFacility, OnFacilityWorkEnd); 
        }

        public override void Exit(IRawMatStateHander handler)
        {
            EventSystem.S.UnRegister(EventID.OnAddWorkingRewardFacility, OnFacilityWorkEnd);
        }

        public override void Execute(IRawMatStateHander handler, float dt)
        {
            Countdowner countDowner = CountdownSystem.S.GetCountdowner(m_WorkStringId, m_Controller.CharacterId);


            if (countDowner != null)
            {
                float percent = Mathf.Clamp01(countDowner.GetProgress());
                m_Controller.SetWorkProgressPercent(percent);
            }
        }

        private void OnReachDestination()
        {
            m_Controller.ReleaseWorkTip();

            string anim = GetAnimName(m_FacilityType);
            m_Controller.CharacterView.PlayAnim(anim, true, ()=> {
                if (IsClean(anim)) 
                {
                    AudioManager.S.PlaySweepSound(m_Controller.GetPosition());
                }
            });

            m_Controller.SpawnWorkProgressBar();
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

        private void OnFacilityWorkEnd(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];

            CharacterController controller = (CharacterController)param[1];

            if (controller == m_Controller)
            {
                ApplyReward(type);

                m_Controller.ReleaseWorkProgressBar();

                m_Controller.SetState(CharacterStateID.Wander);
            }            
        }

        private void ApplyReward(FacilityType type)
        {
            int count = WorkSystem.S.GetWorkPay();
            m_Controller.SpawnFacilityWorkRewardPop(type, count);

            WorkSystem.S.GetReward(type);

            LobbyLevelInfo lobbyLevelInfo = (LobbyLevelInfo)TDFacilityLobbyTable.GetLevelInfo(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());


            //Add exp
            m_Controller.AddExp(lobbyLevelInfo.workExp);
        }
    }
}
