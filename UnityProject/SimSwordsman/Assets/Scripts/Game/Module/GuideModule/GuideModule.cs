﻿using System;
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
            InitCustomTrigger();
            InitCustomCommand();
            GuideMgr.S.StartGuideTrack();
            
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
            GuideMgr.S.RegisterGuideTrigger(typeof(SendCharacterOnTaskTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardTrigger1));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger5));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(BuildFacilityPanelTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger6));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickLobbyTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(LobbyPanelGetCharacterTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(DialogTrigger7));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskBtnTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(ClickTaskDetailsTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(SelectCharacterOnTaskTrigger));
            GuideMgr.S.RegisterGuideTrigger(typeof(SendCharacterOnTaskTrigger2));
            GuideMgr.S.RegisterGuideTrigger(typeof(ReceiveTaskRewardTrigger2));
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
