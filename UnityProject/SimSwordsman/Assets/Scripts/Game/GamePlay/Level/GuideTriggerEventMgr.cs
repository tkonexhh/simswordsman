using System;
using Qarth;

namespace GameWish.Game
{
	public class GuideTriggerEventMgr : TSingleton<GuideTriggerEventMgr>
	{
        FixedFollowCamera m_FollowCamera;
        public void Init()
        {
            EventSystem.S.Register(EventID.OnGuideFirstGetCharacter, StartGuide_Task1);
            EventSystem.S.Register(EventID.OnGuideSecondGetCharacter, StartGuide_Task2);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockFacility);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, LobbyLvUp);
            EventSystem.S.Register(EventID.OnAddItem, WareHouseGuide);
            EventSystem.S.Register(EventID.OnCommonTaskFinish, OnTaskFinish);
            EventSystem.S.Register(EventID.OnCommonTaskStart, OnTaskStart);
        }

        private void OnTaskStart(int key, object[] param)
        {
            int taskid = (int)param[0];
            if (taskid == 9001 || taskid == 9002)
            {
                int id = MainGameMgr.S.CommonTaskMgr.GetSimGameTask(taskid).CharacterIDs[0];
                EventSystem.S.Send(EventID.InGuideProgress, false);
                UIMgr.S.OpenTopPanel(UIID.GuideMaskPanel, null);

                m_FollowCamera = MainGameMgr.S.MainCamera.gameObject.AddComponent<FixedFollowCamera>();
                m_FollowCamera.TweenOrthoSize(10);
                m_FollowCamera.SetTarget(MainGameMgr.S.CharacterMgr.GetCharacterController(id).CharacterView.transform);
            }
        }

        private void OnTaskFinish(int key, object[] param)
        {
            int id = (int)param[0];
            if (id == 9001)
            {
                Timer.S.Post2Really(x => 
                {
                    EventSystem.S.Send(EventID.InGuideProgress, true);
                    m_FollowCamera.TweenOrthoSize(13);
                    m_FollowCamera.DestorySelf();
                    UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

                    EventSystem.S.Send(EventID.OnGuideReceiveTaskRewardClickBtnTrigger1);
                }, 0.5f);
            }
            else if(id == 9002)
            {
                Timer.S.Post2Really(x =>
                {
                    EventSystem.S.Send(EventID.InGuideProgress, true);
                    m_FollowCamera.TweenOrthoSize(13);
                    m_FollowCamera.DestorySelf();
                    UIMgr.S.ClosePanelAsUIID(UIID.GuideMaskPanel);

                    EventSystem.S.Send(EventID.OnGuideReceiveTaskRewardClickBtnTrigger2);
                }, 0.5f);
            }
        }

        private void LobbyLvUp(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            if (type == FacilityType.Lobby && MainGameMgr.S.FacilityMgr.GetLobbyCurLevel() == 3)
            {
                UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
                EventSystem.S.Send(EventID.OnGuideUnlockCollectSystem);
            }
        }

        private void WareHouseGuide(int key, object[] param)
        {
            //第18步结束才能出现建造仓库的引导
            if (!GuideMgr.S.IsGuideFinish(18))
                return;
            if (GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.Warehouse).facilityState != FacilityState.Unlocked)
            {
                var list = TDFacilityWarehouseTable.GetLevelInfo(1).GetUpgradeResCosts();
                foreach (var item in list)
                {
                    if (MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)item.itemId) < item.value)
                        return;
                }
                EventSystem.S.Send(EventID.OnGuideUnlockWarehouse);
            }
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