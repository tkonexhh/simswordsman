using Qarth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{

    public class FailItemData
    {
        public string icon;
        public string name;
        public string color;
        public List<string> conts;

        public FailItemData(string v1, string v2, string v3, List<string> list)
        {
            this.icon = v1;
            this.name = v2;
            this.color = v3;
            this.conts = list;
        }
    }

    public class CombatSettlementPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_ExitBtn;
        [SerializeField]
        private Image m_Font1;
        [SerializeField]
        private Image m_Font2;
        [SerializeField]
        private Transform m_EffectParent;

        [SerializeField]
        private Transform m_RewardContainer;
        [SerializeField]
        private GameObject m_RewardinfoItem;
        [SerializeField]
        private Transform m_FailRewardContainer;
        [SerializeField]
        private GameObject m_FailRewardinfoItem;
        [SerializeField]
        private GameObject m_SuccessEffect;
        [SerializeField]
        private GameObject m_FailEffect;
        [SerializeField]
        private GameObject m_FailTitle;  
        [SerializeField]
        private GameObject m_Line;

        private bool m_IsSuccess;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private TowerLevelConfig m_TowerLevelConfig = null;
        private ArenaLevelConfig m_ArenaLevelConfig = null;
        private List<CharacterController> m_SelectedDiscipleList = null;
        private List<HerbType> m_SeletedHerb = null;
        private PanelType m_PanelType;
        private SimGameTask m_CurTaskInfo = null;
        private List<RewardBase> m_RewardList = new List<RewardBase>();


        public List<FailItemData> failItemDatas = new List<FailItemData>() { };

        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnReciveRewardList, HandListenerEvent);
            EventSystem.S.Register(EventID.OnChallengeReward, HandListenerEvent);
            AudioMgr.S.PlaySound(Define.INTERFACE);
            m_SelectedDiscipleList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;

            BindAddListenerEvent();
            m_FailTitle.SetActive(false);
            m_Line.SetActive(false);
            m_RewardContainer.gameObject.SetActive(false);

            failItemDatas.Add(new FailItemData ("CombatSettlement_Bg7", "??????", "#8E9FB4",new List<string>() { "?????????","????????????"}));
            failItemDatas.Add(new FailItemData ("CombatSettlement_Bg8", "??????", "#A78279", new List<string>() { "?????????","????????????","????????????"}));
            failItemDatas.Add(new FailItemData ("CombatSettlement_Bg9", "??????", "#8F89A6", new List<string>() { "?????????","????????????"}));
            failItemDatas.Add(new FailItemData ("CombatSettlement_Bg10", "??????", "#8F89A6", new List<string>() { "????????????"}));
        }

        protected override void OnClose()
        {
            base.OnClose();
            //m_LevelConfigInfo = null;
            ////m_CurChapterConfigInfo = null;
            //m_TowerLevelConfig = null;
            //m_CurTaskInfo = null;

            //CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnReciveRewardList, HandListenerEvent);
            EventSystem.S.UnRegister(EventID.OnChallengeReward, HandListenerEvent);
        }

        private void HandListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnReciveRewardList:
                    m_RewardList.AddRange((List<RewardBase>)param[0]);
                    break;
                case EventID.OnChallengeReward:
                    HandChallengeReward(param);
                    break;
            }
        }


        private void HandChallengeReward(object[] param)
        {
            switch ((RewardItemType)param[0])
            {
                case RewardItemType.Item:
                    m_RewardList.Add(new ItemReward((int)param[1], (int)param[2]));
                    break;
                case RewardItemType.Armor:
                    m_RewardList.Add(new ArmorReward((int)param[1], (int)param[2]));
                    break;
                case RewardItemType.Arms:
                    m_RewardList.Add(new ArmsReward((int)param[1], (int)param[2]));
                    break;
                case RewardItemType.Kongfu:
                    m_RewardList.Add(new KongfuReward((int)param[1], (int)param[2]));
                    break;
                case RewardItemType.Food:
                    m_RewardList.Add(new FoodsReward((int)param[2]));
                    break;
                case RewardItemType.Coin:
                    m_RewardList.Add(new CoinReward((int)param[2]));
                    break;
                default:
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
                if (m_IsSuccess)
                {
                    if (m_RewardList.Count > 0)
                    {
                        if (m_PanelType == PanelType.Challenge)
                        {
                            if (m_LevelConfigInfo != null)
                            {
                                bool isBossLevel = TDLevelConfigTable.IsBossLevel(m_LevelConfigInfo.level);

                                UIMgr.S.OpenPanel(UIID.RewardPanel, RewardPanelCallback, m_RewardList, m_LevelConfigInfo.level);
                            }
                        }
                        else
                        {
                            UIMgr.S.OpenPanel(UIID.RewardPanel, RewardPanelCallback, m_RewardList);
                        }
                        return;
                    }
                }
                else
                {
                    CheckIsStartTowerShopGuide();
                }
                CloseEvent();
            });
        }

        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        private void CheckIsStartTowerShopGuide()
        {
            if (GuideMgr.S.IsGuideFinish(41))
            {
                return;
            }

            EventSystem.S.Send(EventID.OnTowerTrigger_FightFinishedClickShopBtnTrigger);
        }

        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        private void CheckIsStartArenaShopGuide()
        {
            if (GuideMgr.S.IsGuideFinish(51))
            {
                return;
            }

            EventSystem.S.Send(EventID.OnArenaSystemTrigger_FightFinishedClickShopBtnTrigger);
        }

        private void RewardPanelCallback(AbstractPanel obj)
        {
            RewardPanel rewardPanel = obj as RewardPanel;
            if (m_LevelConfigInfo != null)
                rewardPanel.SetLevelID(m_LevelConfigInfo.level);
            rewardPanel.OnBtnCloseEvent += CloseEvent;
        }

        private void CloseEvent()
        {
            EventSystem.S.Send(EventID.OnExitBattle);
            UIMgr.S.ClosePanelAsUIID(UIID.CombatInterfacePanel);
            PanelPool.S.DisplayPanel();

            if (m_CurTaskInfo != null)
            {
                EventSystem.S.Send(EventID.OnCloseFightingPanel, m_CurTaskInfo.TaskId);
            }
            switch (m_PanelType)
            {
                case PanelType.Task:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel, MainMenuPanelCallBack);
                    RefreshInterAdTimes();
                    if (GameDataMgr.S.GetPlayerData().isPlayMaxTimes())
                        return;

                    if (GameDataMgr.S.GetPlayerData().GetIsNewUser())
                    {
                        PlayerInterAD(5);
                    }
                    else
                        PlayerInterAD(RandomBattleAdIntervalRemoteMgr.S.InterADInterval);
                    break;
                case PanelType.Challenge:
                    OpenParentChallenge();
                    break;
                case PanelType.Tower:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                    UIMgr.S.OpenPanel(UIID.TowerPanel);
                    if (m_IsSuccess)
                    {
                        // if (towerConf.CanRevive() && !GameDataMgr.S.GetPlayerData().recordData.HasLevelRevived(MainGameMgr.S.TowerSystem.maxLevel))
                        if (GameDataMgr.S.GetPlayerData().recordData.towerRevive.dailyCount < TowerDefine.REVIVE_COUNT)
                        {
                            var characterLst = GameDataMgr.S.GetPlayerData().towerData.towerCharacterLst;
                            bool canRevive = false;
                            for (int i = 0; i < characterLst.Count; i++)
                            {
                                if (characterLst[i].IsDead() && characterLst[i].revive == false)
                                {
                                    canRevive = true;
                                    break;
                                }
                            }
                            if (canRevive)
                            {
                                UIMgr.S.OpenPanel(UIID.TowerRevivePanel);
                            }
                        }

                    }

                    TowerBattleOverToGuide tempStruct = new TowerBattleOverToGuide();
                    tempStruct.isSuccess = m_IsSuccess;
                    tempStruct.level = m_TowerLevelConfig.level;
                    int characterNum = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Count;
                    int towerNum = GameDataMgr.S.GetPlayerData().towerData.towerCharacterLst.Count;
                    tempStruct.remain = Mathf.Min(characterNum - towerNum, TowerDefine.MAX_CHARACT_NUM - towerNum);
                    EventSystem.S.Send(EventID.OnTowerBattleOver, tempStruct);

                    CheckIsStartTowerShopGuide();
                    break;

                case PanelType.Arena:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                    UIMgr.S.OpenPanel(UIID.ArenaPanel);

                    CheckIsStartArenaShopGuide();
                    break;
            }
        }

        private void MainMenuPanelCallBack(AbstractPanel obj)
        {
            EventSystem.S.Send(EventID.OnSendBulletinBoardFacility);
        }

        private void RefreshInterAdTimes()
        {
            string recordTime = GameDataMgr.S.GetPlayerData().GetNoBroadcastTimesTime();
            int house = CommonUIMethod.GetDeltaTime(recordTime);
            int count = house / 24;
            int refreshCount = GameDataMgr.S.GetPlayerData().GetRefreshInterTimes();
            if (count > refreshCount)
            {
                GameDataMgr.S.GetPlayerData().RefreshInterAdData();
                GameDataMgr.S.GetPlayerData().SetRefreshInterTimes(count);
            }
        }

        private void PlayerInterAD(int number)
        {
            if (GameDataMgr.S.GetPlayerData().GetBattleTimes() > number)
            {
                GameDataMgr.S.GetPlayerData().SetBattleTimes(-(number + 1));
                if (number == 5)
                    GameDataMgr.S.GetPlayerData().SetIsNewUser();
                if (GameDataMgr.S.GetPlayerData().GetNoBroadcastTimes() > 0)
                {
                    ///????????????????????????
                    GameDataMgr.S.GetPlayerData().SetNoBroadcastTimes(-1);
                    return;
                }
                else
                {
                    AdsManager.S.PlayInterAD("BattleMask", LookInterADSuccessCallBack);
                    GameDataMgr.S.GetPlayerData().SetPlayInterADTimes();
                }
            }
        }

        private void LookInterADSuccessCallBack(bool obj)
        {
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PanelType = (PanelType)args[0];

            switch (m_PanelType)
            {
                case PanelType.Task:
                    GameDataMgr.S.GetPlayerData().SetBattleTimes();
                    m_CurTaskInfo = (SimGameTask)args[1];
                    m_IsSuccess = (bool)args[2];
                    m_CurTaskInfo.ClaimReward(m_IsSuccess);
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = (ChapterConfigInfo)args[1];
                    m_LevelConfigInfo = (LevelConfigInfo)args[2];
                    m_IsSuccess = (bool)args[3];
                    foreach (var item in m_LevelConfigInfo.levelRewardList)
                        if (item.RewardItem != RewardItemType.Exp_Kongfu && item.RewardItem != RewardItemType.Exp_Role)
                            m_RewardList.Add(item);
                    m_LevelConfigInfo.PrepareReward();
                    //if (m_IsSuccess)
                    //    m_LevelConfigInfo.levelRewardList.ForEach(i => i.AcceptReward());
                    if (m_IsSuccess)
                    {
                        m_LevelConfigInfo.AcceptReward();

                        GameDataMgr.S.GetPlayerData().recordData.AddChanllenge();
                    }
                    break;
                case PanelType.Tower:
                    m_TowerLevelConfig = (TowerLevelConfig)args[1];

                    m_IsSuccess = (bool)args[2];
                    if (m_IsSuccess)
                    {
                        m_TowerLevelConfig.PrepareReward();
                        MainGameMgr.S.TowerSystem.PassLevel();
                    }
                    DataAnalysisMgr.S.CustomEvent(m_IsSuccess ? DotDefine.Tower_Battle_Win : DotDefine.Tower_Battle_Fail);
                    break;
                case PanelType.Arena:
                    m_ArenaLevelConfig = (ArenaLevelConfig)args[1];
                    m_IsSuccess = (bool)args[2];

                    if (m_IsSuccess)
                    {
                        m_ArenaLevelConfig.PrepareReward();
                        MainGameMgr.S.ArenaSystem.PassLevel(m_ArenaLevelConfig.level);
                    }
                    DataAnalysisMgr.S.CustomEvent(m_IsSuccess ? DotDefine.Arena_Battle_Win : DotDefine.Arena_Battle_Fail);
                    break;
                default:
                    break;
            }
            RefreshFont();

            if (m_IsSuccess)
            {
                m_RewardContainer.gameObject.SetActive(true);
                m_FailRewardContainer.gameObject.SetActive(false);

                //??????
                foreach (var item in m_SelectedDiscipleList)
                    CreateRewardIInfoItem(item);
            }
            else
            {
                m_FailTitle.SetActive(true);
                m_Line.SetActive(true);
                m_RewardContainer.gameObject.SetActive(false);
                //??????
                foreach (var item in failItemDatas)
                {
                    FailRewardIInfoItem failRewardIInfoItem = Instantiate(m_FailRewardinfoItem, m_FailRewardContainer).GetComponent<FailRewardIInfoItem>();
                    failRewardIInfoItem.OnInit(item);
                }
            }
        }

        private void RefreshFont()
        {
            Instantiate(m_IsSuccess ? m_SuccessEffect : m_FailEffect, m_EffectParent).transform.localPosition = Vector3.zero;
        }

        private void CreateRewardIInfoItem(CharacterController item)
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                    ItemICom taskRewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
                    taskRewardItemICom.OnInit(item, null, m_PanelType, m_CurTaskInfo, m_IsSuccess, this);
                    break;
                case PanelType.Challenge:
                    ItemICom ChaRewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
                    ChaRewardItemICom.OnInit(item, null, m_PanelType, m_LevelConfigInfo, m_IsSuccess, this);
                    break;
                case PanelType.Tower:
                    ItemICom towerRewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
                    towerRewardItemICom.OnInit(item, null, m_PanelType, m_TowerLevelConfig, m_IsSuccess, this);
                    break;
                case PanelType.Arena:
                    ItemICom arenaRewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
                    arenaRewardItemICom.OnInit(item, null, m_PanelType, m_ArenaLevelConfig, m_IsSuccess, this);
                    break;
                default:
                    break;
            }

        }
        private void OpenParentChallenge()
        {
            UIMgr.S.OpenPanel(UIID.ChallengePanel, m_CurChapterConfigInfo.clanType);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();

            //else
            //    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
        }
    }


    public class TowerBattleOverToGuide
    {
        public bool isSuccess;
        public int level;
        public int remain;
    }
}
