using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;
using static UnityEngine.Application;
using System.Linq;

namespace GameWish.Game
{
    public class MainMenuPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_HideBtn;

        [SerializeField]
        private Text m_VillaName;
        [SerializeField]
        private Text m_CoinValue;
        [SerializeField]
        private Text m_BaoziValue;
        [SerializeField]
        private GameObject m_BaoziCountDownObj;
        [SerializeField]
        private Text m_BaoziCountDown;
        [SerializeField]
        private Text m_WareHouseNumber;


        [SerializeField]
        private Button m_VillaBtn;
        [SerializeField]
        private Button m_CreateCoinBtn;
        [SerializeField]
        private Button m_CreateBaoziBtn;
        [SerializeField]
        private Button m_SignInBtn;
        [SerializeField]
        private Button m_BulletinBoardBtn;
        [SerializeField] private GameObject m_ObjBulletinBoardRed;
        [SerializeField]
        private Button m_WareHouseBtn;

        [Header("菜单按钮")]
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private GameObject m_DiscipleRedPoint;
        [SerializeField]
        private Button TrialRunBtn;
        [SerializeField]
        private Button m_ChallengeBtn;
        [SerializeField]
        private GameObject m_Challenging;
        [SerializeField]
        private Button m_VoldemortTowerBtn;
        [SerializeField] private GameObject m_ObjTowerRed;
        [SerializeField]
        private Button m_MythicalAnimalsBtn;
        [SerializeField]
        private Button m_HeroTrialBtn;
        [SerializeField] private Button m_BtnArena;

        [Header("战斗任务")]
        [SerializeField]
        private GameObject m_TacticalFunctionBtn;
        [SerializeField]
        private Transform m_TacticalFunctionTra;

        [Header("访客")]
        [SerializeField]
        private Button m_VisitorBtn1;
        [SerializeField]
        private Image m_VisitorImg1;
        [SerializeField]
        private Button m_VisitorBtn2;
        [SerializeField]
        private Image m_VisitorImg2;
        [SerializeField]
        private Button m_SettingBtn;

        public Transform[] tras;
        private bool isHide = false;

        [SerializeField] private MainTaskUIHandler m_MainTaskUIHandler;

        private List<TacticalFunctionBtn> m_CommonTaskList = new List<TacticalFunctionBtn>();

        protected override void OnUIInit()
        {
            //RegisterEvents();
            base.OnUIInit();
            m_MainTaskUIHandler.Init(this);
        }

        private void HandListenerEvent(int key, object[] param)
        {
            if ((EventID)key == EventID.OnMainMenuOrDiscipleRedPoint)
            {
                List<CharacterItem> characterItemList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
                foreach (var item in characterItemList)
                {
                    if (item.CheckDiscipelPanel())
                    {
                        m_DiscipleRedPoint.SetActive(true);
                        return;
                    }
                }
                m_DiscipleRedPoint.SetActive(false);
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            //ClearTacticalFunctionBtn();
            OpenDependPanel(UIID.WorldUIPanel, -1);
            RegisterEvents();
            m_MainTaskUIHandler.OnOpen();

            MainGameMgr.S.TaskMgr.dailyTaskController.FirstCheck();

            m_HideBtn.onClick.AddListener(() =>
            {
                Transform[] children = GetComponentsInChildren<Transform>();
                foreach (var item in tras)
                {
                    item.gameObject.SetActive(isHide);
                }
                isHide = !isHide;
            });
            m_SettingBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenTopPanel(UIID.UserAccountPanel, null);
                UIMgr.S.OpenPanel(UIID.UserAccountPanel);
            });

            int limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;

            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
            {
                m_BaoziCountDownObj.SetActive(false);
            }

            if (string.IsNullOrEmpty(GameDataMgr.S.GetClanData().GetClanName()))
                m_VillaName.transform.parent.gameObject.SetActive(false);
            else
            {
                m_VillaName.transform.parent.gameObject.SetActive(true);
                m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
            }

            m_CreateBaoziBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= GetFoodUpperLimit())
                {
                    FloatMessage.S.ShowMsg("当前食物已经满了哦。");
                    return;
                }
                DataAnalysisMgr.S.CustomEvent(DotDefine.food_supply_open);
                UIMgr.S.OpenPanel(UIID.SupplementFoodPanel);
            });

            m_VillaBtn.onClick.AddListener(() =>
            {
                if (PlatformHelper.isPlanMode)
                {
                    GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(50000);
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
                    GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(50000);
                }
                if (PlatformHelper.isTestMode)
                {
                    GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(50000);
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
                    GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(500000);
                }
            });
            m_BulletinBoardBtn.onClick.AddListener(() =>
            {
                m_ObjBulletinBoardRed.SetActive(false);
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                UIMgr.S.OpenPanel(UIID.DailyTaskPanel);
                // UIMgr.S.OpenPanel(UIID.BulletinBoardPanel);
            });
            m_WareHouseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.WarehousePanel);
            });
            m_DiscipleBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_open);

                UIMgr.S.OpenPanel(UIID.DisciplePanel);
            });
            TrialRunBtn.onClick.AddListener(() =>
            {

            });
            m_SignInBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SignInPanel);
            });
            m_ChallengeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                int needLobbyLevel = TDSystemConfigTable.GetLobbyLevelRequired(GameFunctionSystem.SysChallenge);

                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
                if (lobbyLevel >= needLobbyLevel)
                    UIMgr.S.OpenPanel(UIID.ChallengePanel);
                else
                    FloatMessage.S.ShowMsg("讲武堂" + needLobbyLevel + "级后可解锁");
            });
            m_VoldemortTowerBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_ObjTowerRed.SetActive(false);
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
                int needLobbyLevel = TowerDefine.ENTER_LEVEL;
                if (lobbyLevel >= needLobbyLevel || PlatformHelper.isTestMode)
                {
                    UIMgr.S.OpenPanel(UIID.TowerPanel);
                }
                else
                {
                    FloatMessage.S.ShowMsg("讲武堂" + needLobbyLevel + "级后可解锁");
                }
            });
            bool showTowerRed = !DataRecord.S.GetBool(TowerDefine.SAVEKEY_NEWDAYSHOW, false) && MainGameMgr.S.FacilityMgr.GetLobbyCurLevel() >= TowerDefine.ENTER_LEVEL;
            m_ObjTowerRed.SetActive(showTowerRed);
            m_CreateCoinBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

            });

            m_VisitorBtn1.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                Visitor visitor = VisitorSystem.S.CurrentVisitor[0];
                DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_tap, visitor.Reward.KeyID.ToString());

                UIMgr.S.OpenPanel(UIID.VisitorPanel, 0);
            });
            m_VisitorBtn2.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                Visitor visitor = VisitorSystem.S.CurrentVisitor[1];
                DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_tap, visitor.Reward.KeyID.ToString());

                UIMgr.S.OpenPanel(UIID.VisitorPanel, 1);
            });

            m_HeroTrialBtn.onClick.AddListener(() =>
            {
                string msg = string.Empty;
                if (!PlatformHelper.isTestMode)
                {
                    if (!MainGameMgr.S.HeroTrialMgr.IsUnlocked(ref msg))
                    {
                        FloatMessage.S.ShowMsg(msg);
                        return;
                    }
                }

                UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);

                UIMgr.S.OpenPanel(UIID.HeroTrialPanel);

                if (MainGameMgr.S.HeroTrialMgr.DbData.state == HeroTrialStateID.Idle && MainGameMgr.S.HeroTrialMgr.CheckIsTrialReady())
                {
                    UIMgr.S.OpenPanel(UIID.HeroTrialTipPanel);
                }

                MainGameMgr.S.HeroTrialMgr.OnEnterHeroTrial();
            });

            m_BtnArena.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
                int needLobbyLevel = ArenaDefine.ENTER_LEVEL;
                if (lobbyLevel >= needLobbyLevel || PlatformHelper.isTestMode)
                {
                    UIMgr.S.OpenPanel(UIID.ArenaPanel);
                }
                else
                {
                    FloatMessage.S.ShowMsg("讲武堂" + needLobbyLevel + "级后可解锁");
                }
            });


            RefreshChallenging();
            MainGameMgr.S.IsMainMenuPanelOpen = true;
            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            //GameDataMgr.S.GetGameData().playerInfoData.AddCoinNum(5000);
            RefreshPanelInfo();
        }
        protected override void OnClose()
        {
            base.OnClose();
            CloseDependPanel(UIID.WorldUIPanel);
            UnregisterEvents();
            m_MainTaskUIHandler.OnClose();

            MainGameMgr.S.IsMainMenuPanelOpen = false;
        }

        private void RefreshPanelInfo()
        {
            m_CoinValue.text = CommonUIMethod.GetTenThousandOrMillion(GameDataMgr.S.GetPlayerData().GetCoinNum());
            m_BaoziValue.text = GameDataMgr.S.GetPlayerData().GetFoodNum() + Define.SLASH + GetFoodUpperLimit().ToString();
            m_WareHouseNumber.text = GetWareHouseAllPeopleNumber();
        }

        private int GetCurBaoziNum()
        {
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() > GetFoodUpperLimit())
            {
                GameDataMgr.S.GetPlayerData().SetFoodNum(GetFoodUpperLimit());
                return GetFoodUpperLimit();
            }
            return GameDataMgr.S.GetPlayerData().GetFoodNum();
        }

        private int GetFoodUpperLimit()
        {
            int foodMaxLimit = 0;
            FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen);
            if (facility.GetState() == FacilityState.Unlocked)
            {
                int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
                foodMaxLimit = TDFacilityKitchenTable.GetFoodMaxLimit(curLevel);
            }
            return foodMaxLimit;
        }

        private string GetWareHouseAllPeopleNumber()
        {
            int CurLevelMaxNumber = 0;
            int CurLevelNumber = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Count;
            //for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            //    CurLevelMaxNumber+=TDFacilityLivableRoomTable.GetCapability(i, TDFacilityLivableRoomTable.GetMaxLevel(i));

            for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            {
                FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i);
                if (facility.GetState() == FacilityState.Unlocked)
                    CurLevelMaxNumber += TDFacilityLivableRoomTable.GetCapability(i, MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i));
            }

            return CurLevelNumber.ToString() + Define.SLASH + CurLevelMaxNumber.ToString();
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnSendBulletinBoardFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.Register(EventID.OnClanNameChange, ChangeClanName);
            EventSystem.S.Register(EventID.OnFoodRefreshEvent, HandleEvent);
            EventSystem.S.Register(EventID.OnMainMenuOrDiscipleRedPoint, HandListenerEvent);
            EventSystem.S.Register(EventID.OnDeleteTaskBtn, HandleEvent);
            EventSystem.S.Register(EventID.OnMainMenuDailyTaskRedPoint, HandleEvent);
            EventSystem.S.Register(EventID.OnRefeshDailyTaskPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnMainMenuChallenging, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshMainMenuPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnSendBulletinBoardFacility, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCheckVisitorBtn, CheckVisitorBtn);
            EventSystem.S.UnRegister(EventID.OnClanNameChange, ChangeClanName);
            EventSystem.S.UnRegister(EventID.OnFoodRefreshEvent, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnMainMenuOrDiscipleRedPoint, HandListenerEvent);
            EventSystem.S.UnRegister(EventID.OnDeleteTaskBtn, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnMainMenuDailyTaskRedPoint, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnRefeshDailyTaskPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnMainMenuChallenging, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshMainMenuPanel:
                    RefreshPanelInfo();
                    break;
                case EventID.OnDeleteTaskBtn:

                    TacticalFunctionBtn tacticalFunctionBtn = m_CommonTaskList.Where(i => i.TaskID == (int)param[0]).FirstOrDefault();
                    if (tacticalFunctionBtn != null)
                    {
                        m_CommonTaskList.Remove(tacticalFunctionBtn);
                        DestroyImmediate(tacticalFunctionBtn.gameObject);
                    }
                    break;
                case EventID.OnCloseParentPanel:
                    CloseSelfPanel();
                    break;
                case EventID.OnSendBulletinBoardFacility:
                    RefreshTacticalInfo();
                    break;
                case EventID.OnFoodRefreshEvent:
                    if ((bool)param[1])
                    {
                        m_BaoziCountDownObj.gameObject.SetActive(false);
                    }
                    else
                    {
                        m_BaoziCountDown.text = (string)param[0];
                        m_BaoziCountDownObj.gameObject.SetActive(true);
                    }
                    break;
                case EventID.OnMainMenuDailyTaskRedPoint:
                    m_ObjBulletinBoardRed.SetActive(true);
                    break;
                case EventID.OnRefeshDailyTaskPanel:
                    m_ObjBulletinBoardRed.SetActive(false);
                    break;
                case EventID.OnMainMenuChallenging:
                    RefreshChallenging();
                    break;
                case EventID.OnStartUpgradeFacility:
                    if (param == null || param.Length <= 0)
                        return;
                    FacilityType facilityType = (FacilityType)param[0];
                    if (facilityType == FacilityType.Lobby)
                    {
                        int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
                        if (lobbyLevel == TowerDefine.ENTER_LEVEL)
                        {
                            m_ObjTowerRed.SetActive(true);
                        }
                    }
                    break;
            }
        }

        private void ClearTacticalFunctionBtn()
        {
            for (int i = 0; i < m_CommonTaskList.Count; i++)
            {
                DestroyImmediate(m_CommonTaskList[i].gameObject);
            }
        }

        private void RefreshChallenging()
        {
            List<CharacterItem> characterItems = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            if (characterItems.Count >= 5)
            {
                CommonUIMethod.BubbleSortForType(characterItems, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
                float allAtkValue = 0;
                for (int i = 0; i < 5; i++)
                    allAtkValue += characterItems[i].atkValue;

                ChapterDbItem chapterDbItem = MainGameMgr.S.ChapterMgr.GetLatestChapter();
                if (chapterDbItem != null)
                {
                    LevelConfigInfo levelConfigInfo = MainGameMgr.S.ChapterMgr.GetLevelInfo(chapterDbItem.chapter, chapterDbItem.level);
                    //Debug.LogError("推荐功力 = "+ levelConfigInfo.recommendAtkValue);
                    if (levelConfigInfo != null)
                    {
                        if (allAtkValue >= levelConfigInfo.recommendAtkValue * 1.5f)
                            m_Challenging.SetActive(true);
                        else
                            m_Challenging.SetActive(false);
                    }
                    else
                    {
                        Log.e("levelConfigInfo not found: " + chapterDbItem.chapter + " " + chapterDbItem.level);
                    }
                }
            }
        }

        private void ChangeClanName(int key, object[] param)
        {
            if (!m_VillaName.transform.parent.gameObject.activeSelf)
                m_VillaName.transform.parent.gameObject.SetActive(true);
            m_VillaName.text = GameDataMgr.S.GetClanData().GetClanName();
        }

        private void CheckVisitorBtn(int key, params object[] param)
        {
            switch ((int)param[0])
            {
                case 0:
                    m_VisitorBtn1.transform.parent.gameObject.SetActive(false);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(false);
                    break;
                case 1:
                    RefreshVisitorImg(1);

                    m_VisitorBtn1.transform.parent.gameObject.SetActive(true);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(false);
                    break;
                case 2:
                    RefreshVisitorImg(2);

                    m_VisitorBtn1.transform.parent.gameObject.SetActive(true);
                    m_VisitorBtn2.transform.parent.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void RefreshVisitorImg(int numer)
        {
            Visitor visitor = VisitorSystem.S.CurrentVisitor[0];
            TDVisitorConfig tb = TDVisitorConfigTable.GetData(visitor.VisitorCfgID);
            m_VisitorImg1.sprite = FindSprite(tb.roleRes);
            m_VisitorImg1.SetNativeSize();
            if (numer == 2)
            {
                Visitor visitor2 = VisitorSystem.S.CurrentVisitor[1];
                TDVisitorConfig tb1 = TDVisitorConfigTable.GetData(visitor2.VisitorCfgID);
                m_VisitorImg2.sprite = FindSprite(tb1.roleRes);
                m_VisitorImg2.SetNativeSize();
            }
        }


        #region 战斗任务相关

        private void RefreshTacticalInfo()
        {
            List<SimGameTask> curTaskList = MainGameMgr.S.CommonTaskMgr.CurTaskList;
            curTaskList.ForEach(i =>
            {
                if (!m_CommonTaskList.Any(j => j.SimGameTask.TaskId == i.TaskId))
                {
                    // Spawn new pop
                    TacticalFunctionBtn tacticalFunctionBtn = Instantiate(m_TacticalFunctionBtn, m_TacticalFunctionTra).GetComponent<TacticalFunctionBtn>();
                    tacticalFunctionBtn.OnInit(this, i);
                    m_CommonTaskList.Add(tacticalFunctionBtn);
                }
            });
        }
        #endregion

    }
}

