using System;
using Qarth;
using UnityEngine;

namespace GameWish.Game
{
    public class GuideTriggerEventMgr : TSingleton<GuideTriggerEventMgr>
    {
        FixedFollowCamera m_FollowCamera;
        private bool m_IsFixedCamera = false;
        public void Init()
        {
            EventSystem.S.Register(EventID.OnGuideFirstGetCharacter, StartGuide_Task1);
            EventSystem.S.Register(EventID.OnGuideSecondGetCharacter, StartGuide_Task2);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockFacility);
            EventSystem.S.Register(EventID.OnAddItem, WareHouseGuide);
            EventSystem.S.Register(EventID.OnCommonTaskFinish, OnTaskFinish);
            EventSystem.S.Register(EventID.OnCommonTaskStart, OnTaskStart);
            EventSystem.S.Register(EventID.OnCloseFightingPanel, OnCloseFightingPanelCallBack);

            EventSystem.S.Register(EventID.OnCharacterUpLevel, OnCharacterUpLevelCallBack);
            EventSystem.S.Register(EventID.OnGetKungFu, OnGetKungFuCallBack);

            EventSystem.S.Register(EventID.OnAddArms, OnAddArmsCallBack);

            EventSystem.S.Register(EventID.OnUpgradeFacility, OnUpgradeFacilityCallBack);
            EventSystem.S.Register(EventID.OnAddCharacter, OnAddCharacterCallBack);

            EventSystem.S.Register(EventID.OnFinishedClickWuWoodBubbleTrigger, OnFinishedClickWuWoodBubbleTrigger1CallBack);
            EventSystem.S.Register(EventID.OnGuideClickTaskDetailsTrigger1, OnGuidClickTaskDetailsTrigger1CallBack);
        }

        private void OnGuidClickTaskDetailsTrigger1CallBack(int key, object[] param)
        {
            EventSystem.S.Send(EventID.InGuideProgress, true);
            
            if (m_FollowCamera != null) {
                m_FollowCamera.TweenOrthoSize(10);
                m_FollowCamera.DestorySelf();
            }

            EventSystem.S.Send(EventID.OnLimitCameraTouchMove, false);

            UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);
        }

        private void OnFinishedClickWuWoodBubbleTrigger1CallBack(int key, object[] param)
        {
            if (GuideMgr.S.IsGuideFinish(8) == false && m_IsFixedCamera == false) 
            {
                m_IsFixedCamera = true;
                EventSystem.S.Send(EventID.OnLimitCameraTouchMove, true);
                EventSystem.S.Send(EventID.InGuideProgress, false);
                UIMgr.S.OpenTopPanel(UIID.GuideMaskPanel, null);
                UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);

                m_FollowCamera = MainGameMgr.S.MainCamera.gameObject.AddComponent<FixedFollowCamera>();
                m_FollowCamera.TweenOrthoSize(10);

                //引导固定某个人视角
                Transform target = GameplayMgr.S.transform.Find("EntityRoot/Character_normal_1");
                if (target != null)
                {
                    m_FollowCamera.SetTarget(target);
                }
            }            
        }

        private void OnAddCharacterCallBack(int key, object[] param)
        {
            CheckIsStartChallengeSystemGuide();
        }

        private void OnUpgradeFacilityCallBack(int key, object[] param)
        {
            if (GuideMgr.S.IsGuideFinish(29)) {
                return;
            }
            FacilityType facilityType = EnumUtil.ConvertStringToEnum<FacilityType>(param[0].ToString());
            if (facilityType == FacilityType.Lobby) {
                CheckIsStartChallengeSystemGuide();
            }
        }
        /// <summary>
        /// 检测是否开始挑战系统引导
        /// </summary>
        private void CheckIsStartChallengeSystemGuide() 
        {
            int facilityLevel = GameDataMgr.S.GetClanData().GetFacilityDbData().GetFacilityLevel(FacilityType.Lobby);
            int characterCount = GameDataMgr.S.GetClanData().GetOwnedCharacterData().GetCharacterCount();
            if (facilityLevel >= 2 && characterCount >= 5)
            {
                EventSystem.S.Send(EventID.OnChallengeSystemTrigger_IntroduceTrigger);
            }
        }

        private void OnCharacterUpLevelCallBack(int key, object[] param)
        {
            CheckIsStartGuideKungFuTrigger();

            CheckIsStartGuideArmorTrigger();
        }

        private void OnGetKungFuCallBack(int key, object[] param)
        {
            CheckIsStartGuideKungFuTrigger();
        }

        private void OnAddArmsCallBack(int key, object[] param)
        {
            CheckIsStartGuideArmorTrigger();
        }
        private void CheckIsStartGuideArmorTrigger() 
        {
            //装备武器引导
            if (GuideMgr.S.IsGuideFinish(28)) {
                return;
            }

            bool isHaveDiscipleLevelGreate30 = GameDataMgr.S.GetClanData().GetOwnedCharacterData().IsHaveCharacterLevelGreaterNumber(30);
            if (isHaveDiscipleLevelGreate30) {
                bool isHaveArms = GameDataMgr.S.GetClanData().inventoryData.IsHaveFreeArms();
                if (isHaveArms) {
                    EventSystem.S.Send(EventID.OnArmsTrigger_IntroduceTrigger);
                }
            }
        }

        /// <summary>
        /// 检测是否可以开始功夫的引导
        /// </summary>
        private void CheckIsStartGuideKungFuTrigger() 
        {
            //完成武功学习，则return
            if (GuideMgr.S.IsGuideFinish(27)) 
            {
                return;
            }
            bool isHaveDiscipleLevelGreate10 = GameDataMgr.S.GetClanData().GetOwnedCharacterData().IsHaveCharacterLevelGreaterNumber(10);

            if (isHaveDiscipleLevelGreate10)
            {
                bool isHaveKungFu = GameDataMgr.S.GetClanData().inventoryData.IsHaveFreeKungFu();

                if (isHaveKungFu) {
                    EventSystem.S.Send(EventID.OnKungFuTrigger_IntroduceTrigger);
                }                
            }
        }

        private void OnCloseFightingPanelCallBack(int key, object[] param)
        {
            int id = (int)param[0];
            if (id == 9001) 
            {
                Timer.S.Post2Really(x =>
                {
                    EventSystem.S.Send(EventID.InGuideProgress, true);

                    UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

                    EventSystem.S.Send(EventID.BuildPracticeFieldEastTrigger);
                }, 0.5f);
            }
        }

        private void OnTaskStart(int key, object[] param)
        {
            int taskid = (int)param[0];

            Debug.LogError("task id:" + taskid);
            //if (taskid == 9001 || taskid == 9002)
            //{
            //    int id = MainGameMgr.S.CommonTaskMgr.GetSimGameTask(taskid).CharacterIDs[0];
            //    EventSystem.S.Send(EventID.InGuideProgress, false);
            //    UIMgr.S.OpenTopPanel(UIID.GuideMaskPanel, null);
            //    UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);

            //    m_FollowCamera = MainGameMgr.S.MainCamera.gameObject.AddComponent<FixedFollowCamera>();
            //    m_FollowCamera.TweenOrthoSize(10);
            //    m_FollowCamera.SetTarget(MainGameMgr.S.CharacterMgr.GetCharacterController(id).CharacterView.transform);
            //}
        }

        private void OnTaskFinish(int key, object[] param)
        {
            //return;
            //int id = (int)param[0];
            //if (id == 9001)
            //{
            //    Timer.S.Post2Really(x => 
            //    {
            //        EventSystem.S.Send(EventID.InGuideProgress, true);
            //        //m_FollowCamera.TweenOrthoSize(10);
            //        //m_FollowCamera.DestorySelf();
            //        UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

            //        //EventSystem.S.Send(EventID.OnGuideReceiveTaskRewardClickBtnTrigger1);
            //        EventSystem.S.Send(EventID.BuildPracticeFieldEastTrigger);
            //    }, 0.5f);
            //}
            //else if(id == 9002)
            //{
            //    Timer.S.Post2Really(x =>
            //    {
            //        EventSystem.S.Send(EventID.InGuideProgress, true);
            //        m_FollowCamera.TweenOrthoSize(10);
            //        m_FollowCamera.DestorySelf();
            //        UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

            //        EventSystem.S.Send(EventID.OnGuideReceiveTaskRewardClickBtnTrigger2);
            //    }, 0.5f);
            //}
        }

        private void WareHouseGuide(int key, object[] param)
        {
            //第18步结束才能出现建造仓库的引导
            if (!GuideMgr.S.IsGuideFinish(18))
                return;
            //第20步结束不出现建造仓库的引导
            if (GuideMgr.S.IsGuideFinish(20))
                return;

            foreach (var item in TDFacilityWarehouseTable.GetLevelInfo(1).GetUpgradeResCosts())
            {
                if (MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)item.itemId) < item.value)
                    return;
            }
            MainGameMgr.S.FacilityMgr.SetFacilityState(FacilityType.Warehouse, FacilityState.ReadyToUnlock);
            EventSystem.S.Send(EventID.OnGuideUnlockWarehouse);
        }

        private void StartGuide_Task1(int key, object[] param)
        {
            //Timer.S.Post2Really(x => 
            //{
            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
                EventSystem.S.Send(EventID.OnGuideDialog4);
            //}, 1f);
        }

        private void StartGuide_Task2(int key, object[] param)
        {
            //Timer.S.Post2Really(x =>
            //{
            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
            EventSystem.S.Send(EventID.OnGuideDialog7);
            //}, 1f);
        }

        private void UnlockFacility(int key, object[] param)
        {
            //第20步结束才能出现建造仓库的引导
            if (!GuideMgr.S.IsGuideFinish(20))
                return;
            FacilityType type = (FacilityType)param[0];
            switch (type)
            {
                case FacilityType.KongfuLibrary:
                    EventSystem.S.Send(EventID.OnGuideUnlockKungfuLibrary);
                    break;
                case FacilityType.ForgeHouse:
                    EventSystem.S.Send(EventID.OnGuideUnlockForgeHouse);
                    break;
                case FacilityType.Baicaohu:
                    EventSystem.S.Send(EventID.OnGuideUnlockBaicaohu);
                    break;
                case FacilityType.PracticeFieldEast:
                    if (GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.PracticeFieldWest).facilityState != FacilityState.Unlocked)
                        EventSystem.S.Send(EventID.OnGuideUnlockPracticeField);
                    break;
                case FacilityType.PracticeFieldWest:
                    if (GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.PracticeFieldEast).facilityState != FacilityState.Unlocked)
                        EventSystem.S.Send(EventID.OnGuideUnlockPracticeField);
                    break;
            }
        }
    }
	
}