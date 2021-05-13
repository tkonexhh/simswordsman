using UnityEngine;
using System.Collections;
using Qarth;
using System;

namespace GameWish.Game
{
    [TMonoSingletonAttribute("[Game]/GameMgr")]
    public class GameMgr : AbstractModuleMgr, ISingleton
    {
        private static GameMgr s_Instance;
        private int m_GameplayInitSchedule = 0;

        public bool showGuide;

        public static GameMgr S
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = MonoSingleton.CreateMonoSingleton<GameMgr>();
                }
                return s_Instance;
            }
        }

        public void InitGameMgr()
        {
            Log.i("Init[GameMgr]");
            
        }

        public void OnSingletonInit()
        {
            
        }

        protected override void OnActorAwake()
        {           
            ShowLogoPanel();
        }

        protected override void OnActorStart()
        {
            StartProcessModule module = AddMonoCom<StartProcessModule>();

            module.SetFinishListener(OnStartProcessFinish);            
        }

        protected void ShowLogoPanel()
        {
            UIDataModule.RegisterStaticPanel();

            Action callback = OnLogoPanelFinish;
            UIMgr.S.OpenTopPanel(UIID.LogoPanel, null, callback);
        }

        protected void OnLogoPanelFinish()
        {
            ++m_GameplayInitSchedule;
            TryStartGameplay();
        }

        protected void OnStartProcessFinish()
        {
            Log.i("Start process finished");

            ++m_GameplayInitSchedule;
            TryStartGameplay();
        }

        protected void TryStartGameplay()
        {            
            if (m_GameplayInitSchedule < 2)
            {
                return;
            }

            Log.i("Start game play");

            //AdsMgr.S.PreloadAllAd();
            //GameplayMgr.S.InitGameplay();

            SDKMgr.S.CheckPrivacy(OnGamePrivacyChecked);
        }
        void OnGamePrivacyChecked(int key, params object[] args)
        {
            GameplayMgr.S.InitGameplay();

            //GetCom<GuideModule>().StartGuide();

            LeBianSDKMgr.S.CallSetPrivacyChaecked();

            // 实名制调用封装至SDKMgr，调用该方法即可初始化
            SDKMgr.S.RequestRealNameSys("http://remoteconf.freeqingnovel.com", "D415");
            
        }
        public void StartGuide()
        {
            GetCom<GuideModule>().StartGuide();
        }

        private void OnApplicationQuit()
        {
            //databaseModule.SaveLeaveTime("Application quit");           
        }
    }
}
