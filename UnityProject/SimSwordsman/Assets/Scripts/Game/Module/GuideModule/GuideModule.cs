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
            GuideMgr.S.RegisterGuideTrigger(typeof(LobbyPanelGetCharacterTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(FirstEndFoodTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(BossMeetTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(RoadBlockTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(WeaponUnlockTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(MagicCloudTrigger));
            //GuideMgr.S.RegisterGuideTrigger(typeof(MagicCloudEndTriger));



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
