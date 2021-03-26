using System;
using Qarth;
using UnityEngine;

namespace GameWish.Game
{
    public class GuideTriggerEventMgr : TSingleton<GuideTriggerEventMgr>
    {
        FixedFollowCamera m_FollowCamera;
        private bool m_IsFixedCamera = false;

        private bool m_IsInBattleing = false;

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
            EventSystem.S.Register(EventID.OnAddCharacterPanelClosed, OnAddCharacterPanelClosedCallBack);

            EventSystem.S.Register(EventID.OnFinishedClickWuWoodBubbleTrigger, OnFinishedClickWuWoodBubbleTrigger1CallBack);
            EventSystem.S.Register(EventID.OnGuideClickTaskDetailsTrigger1, OnGuidClickTaskDetailsTrigger1CallBack);

            EventSystem.S.Register(EventID.OnEnterBattle, OnEnterBattleCallBack);
            EventSystem.S.Register(EventID.OnExitBattle, OnExitBattleCallBack);

            EventSystem.S.Register(EventID.OnCloseAllUIPanel, OnCloseAllUIPanelCallBack);
        }

        private void OnCloseAllUIPanelCallBack(int key, object[] param)
        {
            //if (GuideMgr.S.IsGuiding())
            //{
            //    return;
            //}
            UIMgr.S.ClosePanelAsUIID(UIID.SignInPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardChooseDisciple);
            UIMgr.S.ClosePanelAsUIID(UIID.WarehousePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ItemDetailsPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.KitchenPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ConstructionFacilitiesPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.SupplementFoodPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.DisciplePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.DicipleDetailsPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.WearableLearningPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.LearnKungfuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengeBattlePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.IdentifyChallengesPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.SendDisciplesPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengeChooseDisciple);
            UIMgr.S.ClosePanelAsUIID(UIID.PracticeFieldPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChooseDisciplePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.RewardPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.VisitorPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.PromotionPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ForgeHousePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.BaicaohuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.LivableRoomPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.KongfuLibraryPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.KungfuChooseDisciplePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.GetDisciplePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.UserAccountPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.LogPanel);
        }

        private void OnExitBattleCallBack(int key, object[] param)
        {
            m_IsInBattleing = false;

            CheckIsStartGuideKungFuTrigger();

            CheckIsStartGuideArmorTrigger();
        }

        private void OnEnterBattleCallBack(int key, object[] param)
        {
            m_IsInBattleing = true;
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
            if (GuideMgr.S.IsGuideFinish(31) == false && m_IsFixedCamera == false) 
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

        private void OnAddCharacterPanelClosedCallBack(int key, object[] param)
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
            if (GuideMgr.S.IsGuideFinish(29))
            {
                return;
            }
            int isOpenChallengePanel = PlayerPrefs.GetInt(Define.Is_Enter_Challenge_Panel, -1);
            //-1  表示未打开挑战面板
            if (isOpenChallengePanel == -1) 
            {
                int facilityLevel = GameDataMgr.S.GetClanData().GetFacilityDbData().GetFacilityLevel(FacilityType.Lobby);
                int characterCount = GameDataMgr.S.GetClanData().GetOwnedCharacterData().GetCharacterCount();
                if (facilityLevel >= 2 && characterCount >= 5)
                {
                    EventSystem.S.Send(EventID.OnChallengeSystemTrigger_IntroduceTrigger);
                }
            }            
        }

        private void OnCharacterUpLevelCallBack(int key, object[] param)
        {
            if (m_IsInBattleing == false)
            {
                CheckIsStartGuideKungFuTrigger();

                CheckIsStartGuideArmorTrigger();
            }
        }

        private void OnGetKungFuCallBack(int key, object[] param)
        {
            if (m_IsInBattleing == false) 
            {
                CheckIsStartGuideKungFuTrigger();
            }             
        }

        private void OnAddArmsCallBack(int key, object[] param)
        {
            if (m_IsInBattleing == false) {
                CheckIsStartGuideArmorTrigger();
            }            
        }
        private void CheckIsStartGuideArmorTrigger() 
        {
            //装备武器引导
            if (GuideMgr.S.IsGuideFinish(28)) {
                return;
            }

            bool isHaveDiscipleLevelGreate30 = GameDataMgr.S.GetClanData().GetOwnedCharacterData().IsHaveCharacterLevelGreaterNumber(30);
            
            if (isHaveDiscipleLevelGreate30) 
            {                
                bool isHaveArms = GameDataMgr.S.GetClanData().inventoryData.IsHaveFreeArms();
                
                if (isHaveArms) 
                {
                    bool isEquipArms = GameDataMgr.S.GetClanData().GetOwnedCharacterData().IsEquipmentArms();
                    //如果用户没装备过武器，才引导
                    if (isEquipArms == false) 
                    {
                        Timer.S.Post2Really((x) =>
                        {
                            if (MainGameMgr.S.IsMainMenuPanelOpen == false)
                            {
                                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                                OnCloseAllUIPanelCallBack(0, null);
                            }

                            EventSystem.S.Send(EventID.OnArmsTrigger_IntroduceTrigger);
                        }, .3f, 1);
                    }                    
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

                if (isHaveKungFu) 
                {
                    bool isStudyKungFu = GameDataMgr.S.GetClanData().GetOwnedCharacterData().IsStudyKungFu();
                    if (isStudyKungFu == false) {
                        Timer.S.Post2Really((x) =>
                        {
                            if (MainGameMgr.S.IsMainMenuPanelOpen == false)
                            {
                                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                                OnCloseAllUIPanelCallBack(0, null);
                            }

                            EventSystem.S.Send(EventID.OnKungFuTrigger_IntroduceTrigger);
                        }, .3f, 1);
                    }                                
                }                
            }
        }

        private void OnCloseFightingPanelCallBack(int key, object[] param)
        {
            int id = (int)param[0];

            if (id == 9001) 
            {
                if (GuideMgr.S.IsGuideFinish(37) == false)
                {
                    Timer.S.Post2Really(x =>
                    {
                        EventSystem.S.Send(EventID.InGuideProgress, true);

                        UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

                        EventSystem.S.Send(EventID.RandomFightTrigger_FinishedIntroduce);
                    }, 0.5f);
                }
            }
        }

        private void OnTaskStart(int key, object[] param)
        {
            //int taskid = (int)param[0];
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
            if (!GuideMgr.S.IsGuideFinish(36))
                return;
            //第20步结束不出现建造仓库的引导
            if (GuideMgr.S.IsGuideFinish(20))
                return;
            //在战斗中不显示引导
            if (m_IsInBattleing) 
            {
                return;
            }

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