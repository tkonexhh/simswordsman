using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class GuideModule : AbstractModule
    {
        public void StartGuide()
        {
            if (!AppConfig.S.isGuideActive)
            {
                return;
            }

            int id = 0;
            var list = TDGuideTable.dataList;
            for (int i = 0; i < list.Count; i++)
            {
                if (GuideMgr.S.IsGuideFinish(list[i].id))
                    continue;
                id = list[i].id * 100 + 10001;
                break;
            }

            if (id != 0)
            {
                GuideTriggerEventMgr.S.Init();
                InitCustomTrigger();
                InitCustomCommand();
                GuideMgr.S.StartGuideTrack();
                GameplayMgr.S.CheckIsFirstGameStart();
                if (GameDataMgr.S.GetPlayerData().isGuideStart)
                {
                    try
                    {
                        EventID eventid = (EventID)id;
                        EventSystem.S.Send(eventid);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        protected override void OnComAwake()
        {

        }
        protected void InitCustomTrigger()
        {
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(TakeNameTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityPanelTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger3));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickLobbyTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(LobbyPanelGetCharacterTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger4));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskBtnTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskDetailsTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterOnTaskBtnTrigger1)); 
            GuideMgr.S.RegisterGuideTrigger(typeof(SendCharacterOnTaskTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardBtnTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardBtnTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskBtnTrigger11));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskBtnTrigger22));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger5));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityPanelTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger6));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickLobbyTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(LobbyPanelGetCharacterTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger7));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskBtnTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskDetailsTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterOnTaskBtnTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterPanelTrigger2_1));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterPanelTrigger2_2));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterSureTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(SendCharacterOnTaskTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(KitchenDialogTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacility_KitchenTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityPanel_KitchenTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger8));
            GuideMgr.S.RegisterGuideTrigger(typeof(SendCharacterWorkTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(WarehouseDialogTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacility_WarehouseTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityPanel_WarehouseTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildBaicaohuTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildPracticeFieldTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildKungfuLibraryTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildForgehouseTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(FoodBuffTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(CollectSystemTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterPanelTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterSureTrigger1));
         
        }

        protected void InitCustomCommand()
        {
            GuideMgr.S.RegisterGuideCommand(typeof(GuideButtonCommand));
            GuideMgr.S.RegisterGuideCommand(typeof(NormalTipsCommand));
            GuideMgr.S.RegisterGuideCommand(typeof(MyDelayCommand));
            GuideMgr.S.RegisterGuideCommand(typeof(GuideClickWorldCommand));
            GuideMgr.S.RegisterGuideCommand(typeof(TakeNameCommand));
            GuideMgr.S.RegisterGuideCommand(typeof(DialogCommand));

        }

    }
}
