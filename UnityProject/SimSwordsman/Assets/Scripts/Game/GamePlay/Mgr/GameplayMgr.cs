﻿using System.Collections;
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

            AssetPreloaderMgr.S.Init();

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

            //GuideObjectMgr.S.Init();

            //EventSystem.S.Send(EventID.OnUpdateLoadProgress, 0.2f);

            //yield return null;

            //Camera.main.transform.GetComponent<MobileTouchCamera>().Init();

            //OfflineRewardMgr.S.Init();

            //TimeUpdateMgr.S.Start();

            //GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Clear();
            //GameDataMgr.S.GetPlayerData().unlockFoodItemIDs.Add(2);
            //GameDataMgr.S.GetPlayerData().SetDataDirty();
            //客人来访系统
            VisitorSystem.S.Init();

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
                if (AssetPreloaderMgr.S.IsPreloaderDone())
                {
                    m_IsGameStart = true;
                    //Timer.S.OnSingletonInit();

                    CheckMedalRefresh();
                    UIMgr.S.ClosePanelAsUIID(UIID.LogoPanel);
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);

                    MusicMgr.S.PlayMenuMusic();
                    MainGameMgr.S.OnInit();
                    FoodRecoverySystem.S.Init();

                    CountdownSystem.S.Init();

                    GameMgr.S.StartGuide();
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

          
            if (Input.GetKeyDown(KeyCode.J))
            {
                //GameDataMgr.S.GetPlayerData().AddCoinNum(100000);
                //GameDataMgr.S.GetPlayerData().AddFoodNum(100);
                GameDataMgr.S.GetPlayerData().ReduceFoodNum(100);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(0,600);

                for (int i = (int)ArmorType.ZiTenJia; i < (int)ArmorType.RuanWeiJia; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)i,(Step)1), 2000);
                }  
                for (int i = (int)ArmsType.ShaZhuDao; i < (int)ArmsType.YanYueDao; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)i,(Step)1), 2000);
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                for (int i = (int)RawMaterial.QingRock; i < (int)RawMaterial.SnakeTeeth; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 2000);
                }
            }
      

            if (Input.GetKeyDown(KeyCode.L))
            {
                for (int i = (int)KongfuType.TaiZuChangQuan; i < (int)KongfuType.ZuiQuan; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KongfuType)i), 2000);
                }
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(0, 200);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                MainGameMgr.S.InventoryMgr.GetAllInventoryItemList().Clear();
                for (int i = (int)HerbType.ChiDanZhuangQiWan; i < (int)HerbType.HuanHunDan; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)i), 2000);
                }
            }
        }

        /// <summary>
        /// 检查是否是第一次启动游戏，直接开启剧情
        /// </summary>
        public void CheckIsFirstGameStart()
        {
            if (GameDataMgr.S.GetPlayerData().isGuideStart)
                return;

            //锁定镜头
            EventSystem.S.Send(EventID.InGuideProgress, false);
            Action action = () =>
            {
                //开始第一个引导
                EventSystem.S.Send(EventID.OnGuideDialog1, 1);
                //播放打扫动画
                GameDataMgr.S.GetPlayerData().SetLobbyBuildeTime();
                GameObject effect = Resources.Load<GameObject>("Prefabs/EffectPrefab/BuildSmoke3_2");
                GameObject obj = Instantiate(effect);
                obj.transform.position = new Vector3(7.07f, 2.34f, 0);
                
                Timer.S.Post2Really(x =>
                {
                    Transform tz = obj.transform.Find("root/BuildingTZ");
                    tz.gameObject.SetActive(false);

                    EventSystem.S.Send(EventID.OnStartUnlockFacility, FacilityType.Lobby);
                    GameDataMgr.S.GetPlayerData().isGuideStart = true;
                    GameDataMgr.S.GetPlayerData().SetDataDirty();
                }, 1);
                Timer.S.Post2Really(x => 
                {
                    Destroy(obj);
                    //增加金币（暂时写在这里）
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem(RawMaterial.WuWood), 10);
                }, 2.5f);
            };
            UIMgr.S.OpenTopPanel(UIID.StoryPanel, null, "StoryPanel_Text_1,StoryPanel_Text_2,StoryPanel_Text_3,StoryPanel_Text_4", action);
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