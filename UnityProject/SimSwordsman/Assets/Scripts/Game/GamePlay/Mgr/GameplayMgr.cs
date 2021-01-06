using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using BitBenderGames;
using UnityEngine.SceneManagement;
using Int64 = System.Int64;
using Random = UnityEngine.Random;

namespace GameWish.Game
{
    public partial class GameplayMgr : TMonoSingleton<GameplayMgr>
    {
        [SerializeField] private Transform m_EntityRoot;

        public Transform EntityRoot { get => m_EntityRoot; set => m_EntityRoot = value; }
        public MonoBehaviour Mono { get => m_Mono; set => m_Mono = value; }

        private bool m_IsLoadingBarFinished = false;
        private bool m_IsGameStart = false;

        private MonoBehaviour m_Mono;

        public void InitGameplay()
        {
            m_Mono = GetComponent<MonoBehaviour>();
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            // Init Managers
            GameDataMgr.S.Init();

            //AssetPreloaderMgr.S.Init();

            InputMgr.S.Init();

            //EventSystem.S.Send(EventID.OnUpdateLoadProgress, 0.2f);

            //yield return null;

            //EventSystem.S.Send(EventID.OnUpdateLoadProgress, 0.2f);

            //yield return null;

            //EventSystem.S.Send(EventID.OnUpdateLoadProgress, 0.2f);

            //yield return null;

            AudioMgr.S.OnSingletonInit();

            EventSystem.S.Register(EngineEventID.OnApplicationQuit, ApplicationQuit);
            EventSystem.S.Register(EngineEventID.OnApplicationPauseChange, OnGamePauseChange);
            EventSystem.S.Register(EngineEventID.OnApplicationFocusChange, OnGameFocusChange);

            //Set language
            //I18Mgr.S.SwitchLanguage(SystemLanguage.German);

            GameMgr.S.StartGuide();

            //GuideObjectMgr.S.Init();

            //EventSystem.S.Send(EventID.OnUpdateLoadProgress, 0.2f);

            //yield return null;

            //Camera.main.transform.GetComponent<MobileTouchCamera>().Init();

            //OfflineRewardMgr.S.Init();

            //TimeUpdateMgr.S.Start();

            //GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Clear();
            //GameDataMgr.S.GetPlayerData().SetDataDirty();
            //客人来访系统
            VisitorSystem.S.Init();

            BuffSystem.S.Init();

            if (string.IsNullOrEmpty(GameDataMgr.S.GetPlayerData().firstPlayTime))
            {
                GameDataMgr.S.GetPlayerData().firstPlayTime = DateTime.Now.ToString();
            }

            RemoteConfigMgr.S.StartChecker(null);

            m_IsLoadingBarFinished = true;

            yield return null;
        }

        private void OnGamePauseChange(int key, params object[] args)
        {
            bool pause = (bool)args[0];
            if (!pause)
            {
                TimeUpdateMgr.S.Resume();
            }
            else
            {
                TimeUpdateMgr.S.Pause();
            }
        }

        private void OnGameFocusChange(int key, params object[] args)
        {
            bool focusState = (bool)args[0];
            if (focusState)
            {
                return;
            }

            GameDataMgr.S.GetPlayerData().SetLastPlayTime(GameExtensions.GetTimeStamp());

            GameDataMgr.S.Save();
        }

        private void ApplicationQuit(int key, params object[] args)
        {
            //GameDataMgr.S.GetPlayerInfoData().SetLoginTime();
        }

        private void Update()
        {
            if (m_IsLoadingBarFinished == false)
                return;

            if (m_IsGameStart == false)
            {
                //if (AssetPreloaderMgr.S.IsPreloaderDone())
                {
                    m_IsGameStart = true;
                    //Timer.S.OnSingletonInit();

                    CheckMedalRefresh();
                    UIMgr.S.ClosePanelAsUIID(UIID.LogoPanel);
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);

                    MusicMgr.S.PlayBgMusic();
                    MainGameMgr.S.OnInit();
                    FoodRecoverySystem.S.Init();
                }
            }
            else
            {
                MainGameMgr.S.OnUpdate();
            }

            ////For test
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    GameDataMgr.S.GetPlayerInfoData().AddCoinNum(1E100);
            //}

            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    GameDataMgr.S.GetPlayerInfoData().AddLevel(1);
            //}

            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    GameDataMgr.S.GetPlayerInfoData().AddStarCount(3);
            //}
        }

        private void CheckMedalRefresh()
        {
            int hour = System.DateTime.Now.Hour;
            int minute = System.DateTime.Now.Minute;
            switch (hour)
            {
                case 23:
                    if (minute>=59)
                        EventSystem.S.Send(EventID.OnGoldMedalRefresh);
                    break;
                case 00:
                    if (minute<=1)
                        EventSystem.S.Send(EventID.OnGoldMedalRefresh);
                    break;
                case 11:
                    if(minute>=59)
                        EventSystem.S.Send(EventID.OnSilverMedalRefresh);
                    break;
                case 12:
                    if (minute <= 1)
                        EventSystem.S.Send(EventID.OnSilverMedalRefresh);
                    break;
            }
            
        }

        private void FixedUpdate()
        {

        }
    }
}