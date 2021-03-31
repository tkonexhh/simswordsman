using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;
using BitBenderGames;
using UnityEngine.SceneManagement;
using Int64 = System.Int64;
using Random = UnityEngine.Random;
using Unity.Notifications.Android;

namespace GameWish.Game
{
    public partial class GameplayMgr : TMonoSingleton<GameplayMgr>
    {
        [SerializeField] private Transform m_EntityRoot;

        public Transform EntityRoot { get => m_EntityRoot; set => m_EntityRoot = value; }
        public MonoBehaviour Mono { get => m_Mono; set => m_Mono = value; }

        public static DateTime resumeTime = DateTime.Now;

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
            GameDataMgr.S.GetPlayerData().SetLastPlayTime(GameExtensions.GetTimeStamp());

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

            AudioManager.S.OnInit();

            if (string.IsNullOrEmpty(GameDataMgr.S.GetPlayerData().firstPlayTime))
            {
                GameDataMgr.S.GetPlayerData().firstPlayTime = DateTime.Now.ToString();
            }

            RemoteConfigMgr.S.StartChecker(null);

            m_IsLoadingBarFinished = true;

            SendSplitChannelData();

            WeChatShareMgr.S.Init();

            RandomBattleAdIntervalRemoteMgr.S.Init();

            yield return null;
        }

        private void SendSplitChannelData()
        {
            string channelName = CustomExtensions.GetSDKChannel();
            //Debug.LogError("split channel:" + channelName);
#if UNITY_ANDROID
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("channel", channelName);
            DataAnalysisMgr.S.CustomEventDic("split_channel", dic);
#else
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("channel", channelName);
            DataAnalysisMgr.S.CustomEventDic("split_channel", dic);
#endif
        }

        private void OnGamePauseChange(int key, params object[] args)
        {
            bool pause = (bool)args[0];

            if (!pause)
            {
                TimeUpdateMgr.S.Resume();

                resumeTime = DateTime.Now;
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

            string[] strList = new string[]
            {
                "师父，又有人上门挑战我们了，快来带我们迎战吧！",
                "掌门，有位年轻貌美的女子想来拜师，您不来看看吗？",
                "不好了师父，师娘在后山挖野菜被蛇咬了，快来救救她吧！",
                "报告掌门，弟子们都已经吃饱喝足了，快来给我们安排任务吧！",
                "好消息！大师兄在后山打猎捡到一件宝贝，师父快来看看是什么"
            };
            string str = strList[Random.Range(0, strList.Length)];

            DateTime nowTime = DateTime.Now;
            DateTime notifiTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 12, 0, 0);
            NotificationMgr.S.SendNotification("最强门派", str, notifiTime, true);

            DateTime notifiTimeSecond = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 20, 0, 0);
            NotificationMgr.S.SendNotification("最强门派", str, notifiTime, true);
        }

        private void ApplicationQuit(int key, params object[] args)
        {
            //GameDataMgr.S.GetPlayerInfoData().SetLoginTime();
        }

        private void FirstLogin()
        {
            int num = PlayerPrefs.GetInt("test");
            if (num != 1)
            {
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)1001), 20);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)1002), 20);
                PlayerPrefs.SetInt("test", 1);
            }
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
                    UIMgr.S.OpenPanel(UIID.WorldUIPanel);
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel, MainMenuPanelCallback);

                    MusicMgr.S.PlayMenuMusic();
                    MainGameMgr.S.OnInit();
                    FoodRecoverySystem.S.Init();
                    VisitorSystem.S.Init();

                    WorkSystemMgr.S.Init();

                    BaiCaoWuSystemMgr.S.Init();
                    ForgeHouseSystemMgr.S.Init();
                    CollectSystem.S.Init();

                    if (PlatformHelper.isEditor)
                    {
                        Application.runInBackground = true;
                    }
                    GameMgr.S.StartGuide();

                    //Application.runInBackground = true;
                    FirstLogin();
                }
            }
            else
            {
                MainGameMgr.S.OnUpdate();
                VisitorSystem.S.Update();
            }

            ////For test
            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    GameDataMgr.S.GetPlayerInfoData().AddCoinNum(1E100);
            //}

            #region 测试代码
            if (Input.GetKeyDown(KeyCode.W))
            {
                //MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)101, Step.One), 10);

                EventSystem.S.Send(EventID.OnChallengeSystemTrigger_IntroduceTrigger);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                for (int i = (int)RawMaterial.SilverToken; i <= (int)RawMaterial.GoldenToken; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                for (int i = (int)HerbType.ChiDanZhuangQiWan; i <= (int)HerbType.HuanHunDan; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)i), 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                for (int i = (int)RawMaterial.QingRock; i < (int)RawMaterial.SnakeTeeth; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)i), 5000);
                }
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)2002), 5000);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)3001), 5000);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)3002), 5000);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)3003), 5000);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)3101), 5000);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)3102), 5000);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.ZiTenJia, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.ZiTenJia, Step.Four), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.ZiTenJia, Step.Seven), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.ZiTenJia, Step.Nine), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.ZiTenJia, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.YinYeJia, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.YinYeJia, Step.Four), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.YinYeJia, Step.Seven), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.YinYeJia, Step.Nine), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem(ArmorType.YinYeJia, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.MengGuBaoDao, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.MengGuBaoDao, Step.Four), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.MengGuBaoDao, Step.Seven), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.MengGuBaoDao, Step.Nine), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.MengGuBaoDao, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.DaGouBang, Step.Eight), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.DaGouBang, Step.Four), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.DaGouBang, Step.Seven), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.DaGouBang, Step.Nine), 1);
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem(ArmsType.DaGouBang, Step.Eight), 1);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(5000000000000);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(0, 50);
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(1, 50);
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(2, 50);
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(3, 50);
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(4, 50);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                MainGameMgr.S.CharacterMgr.AddCharacterLevel(0, 500);
                for (int i = (int)KungfuType.TaiZuChangQuan; i < (int)KungfuType.ZuiQuan; i++)
                {
                    MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KungfuType)i), 2);
                }
            }


            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameDataMgr.S.GetPlayerData().AddFoodNum(1);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameDataMgr.S.GetPlayerData().ReduceFoodNum(2);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameDataMgr.S.GetPlayerData().ReduceFoodNum(100);
            }


            if (Input.GetKeyDown(KeyCode.V))
            {
                List<RewardBase> rewards = new List<RewardBase>();
                rewards.Add(new ArmsReward((int)ArmsType.ShaZhuDao, 1));
                rewards.Add(new ArmsReward((int)ArmsType.DaHuanDao, 1));
                rewards.Add(new ArmsReward((int)ArmsType.MengGuBaoDao, 1));
                rewards.Add(new KongfuReward((int)KungfuType.DuGuJiuJian, 1));
                UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                EventSystem.S.Send(EventID.OnBattleSuccessed);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                EventSystem.S.Send(EventID.OnBattleFailed);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                PanelPool.S.AddPromotion(new DiscipleRiseStage(0, 2, 1212));
                PanelPool.S.DisplayPanel();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PanelPool.S.AddPromotion(new WugongBreakthrough(0, new CharacterKongfuDBData(1, KungfuLockState.Learned, KungfuType.DuGuJiuJian, 10, 1), 1212));
                PanelPool.S.DisplayPanel();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                PanelPool.S.AddPromotion(new EquipAmror(1, 2000, new ArmorItem(ArmorType.MingGuangKai,Step.Five)));
                PanelPool.S.DisplayPanel();
            }
            #endregion
        }

        private void MainMenuPanelCallback(AbstractPanel obj)
        {
            EventSystem.S.Send(EventID.OnMainMenuOrDiscipleRedPoint);
            EventSystem.S.Send(EventID.OnSendBulletinBoardFacility);
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
                }, 0.5f);
                Timer.S.Post2Really(x =>
                {
                    Destroy(obj);
                    //增加金币（暂时写在这里）
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem(RawMaterial.WuWood), 10);
                }, 0.7f);
            };
            //UIMgr.S.OpenTopPanel(UIID.StoryPanel, null, "StoryPanel_Text_1,StoryPanel_Text_2,StoryPanel_Text_3,StoryPanel_Text_4", action);
            UIMgr.S.OpenPanel(UIID.StoryPanel, "StoryPanel_Text_1,StoryPanel_Text_2,StoryPanel_Text_3,StoryPanel_Text_4", action);
        }

        private void CheckMedalRefresh()
        {
            int hour = System.DateTime.Now.Hour;
            int minute = System.DateTime.Now.Minute;
            switch (hour)
            {
                case 23:
                    if (minute >= 59)
                        EventSystem.S.Send(EventID.OnGoldMedalRefresh);
                    break;
                case 00:
                    if (minute <= 1)
                        EventSystem.S.Send(EventID.OnGoldMedalRefresh);
                    break;
                case 11:
                    if (minute >= 59)
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