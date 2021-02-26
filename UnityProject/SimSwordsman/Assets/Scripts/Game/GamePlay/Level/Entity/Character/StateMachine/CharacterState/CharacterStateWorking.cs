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
        private string m_WorkStringId;

        public CharacterStateWorking(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_FacilityType = m_Controller.CharacterModel.GetTargetFacilityType();  

            m_WorkStringId = WorkSystem.GetStringId(m_FacilityType);
            m_Controller.SpawnWorkTipWhenWorkInFacility(m_FacilityType);

            FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);
            Vector3 targetPos = facilityController.GetDoorPos();

            m_Controller.MoveTo(targetPos, OnReachDestination);

            EventSystem.S.Register(EventID.OnAddWorkingRewardFacility, OnFacilityWorkEnd);
        }

        public override void Exit(ICharacterStateHander handler)
        {
            EventSystem.S.UnRegister(EventID.OnAddWorkingRewardFacility, OnFacilityWorkEnd);
        }

        public override void Execute(ICharacterStateHander handler, float dt)
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
            m_Controller.CharacterView.PlayAnim(anim, true, null);

            m_Controller.SpawnWorkProgressBar();
        }

        private string GetAnimName(FacilityType facilityType)
        {
            string animName = "clean";
            switch (facilityType)
            {
                case FacilityType.ForgeHouse:
                    animName = "forge_iron";
                    break;
                case FacilityType.Kitchen:
                    animName = "cook";
                    break;
                case FacilityType.Baicaohu:
                    animName = "pharmacy";
                    break;
                case FacilityType.Warehouse:
                    animName = "carry";
                    break;
            }

            return animName;
        }

        private void OnFacilityWorkEnd(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];

            ApplyReward(type);

            m_Controller.ReleaseWorkProgressBar();

            m_Controller.SetState(CharacterStateID.Wander);
        }

        private void ApplyReward(FacilityType type)
        {
            WorkSystem.S.GetReward(type);

            LobbyLevelInfo lobbyLevelInfo = (LobbyLevelInfo)TDFacilityLobbyTable.GetLevelInfo(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());

            int count = lobbyLevelInfo.workPay; 
            m_Controller.SpawnFacilityWorkRewardPop(type, count);

            //Add exp
            m_Controller.AddExp(lobbyLevelInfo.workExp);
        }
    }
}
