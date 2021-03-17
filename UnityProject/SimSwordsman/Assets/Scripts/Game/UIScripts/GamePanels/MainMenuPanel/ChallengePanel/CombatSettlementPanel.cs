using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
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
        private GameObject m_SuccessEffect;
        [SerializeField]
        private GameObject m_FailEffect;

        private bool m_IsSuccess;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private List<CharacterController> m_SelectedDiscipleList = null;
        private List<HerbType> m_SeletedHerb = null;
        private PanelType m_PanelType;
        private SimGameTask m_CurTaskInfo = null;
        private List<RewardBase> m_RewardList = new List<RewardBase>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnReciveRewardList, HandListenerEvent);
            EventSystem.S.Register(EventID.OnChallengeReward, HandListenerEvent);
            AudioMgr.S.PlaySound(Define.INTERFACE);
            m_SelectedDiscipleList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;

            BindAddListenerEvent();
        }

        protected override void OnClose()
        {
            base.OnClose();
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
                        UIMgr.S.OpenPanel(UIID.RewardPanel, RewardPanelCallback, m_RewardList);
                        return;
                    }
                }
                CloseEvent();
            });
        }

        private void RewardPanelCallback(AbstractPanel obj)
        {
            RewardPanel rewardPanel = obj as RewardPanel;
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
         
            if (m_PanelType == PanelType.Task)
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);

            if (m_PanelType == PanelType.Challenge)
                OpenParentChallenge();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Task:
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

                    m_LevelConfigInfo.levelRewardList.ForEach(i =>
                    {
                        if (i.RewardItem == RewardItemType.Exp_Kongfu || i.RewardItem == RewardItemType.Exp_Role)
                        {
                            i.SetCount(i.Count / m_SelectedDiscipleList.Count);
                        }
                    });
                    m_LevelConfigInfo.PrepareReward();
                    if (m_IsSuccess)
                        m_LevelConfigInfo.levelRewardList.ForEach(i => i.AcceptReward());
                    // else
                    //     m_LevelConfigInfo.levelRewardList.ForEach(i => i.AcceptReward(1));
                    break;
                default:
                    break;
            }
            RefreshFont();

            foreach (var item in m_SelectedDiscipleList)
                CreateRewardIInfoItem(item);
        }

        private void RefreshFont()
        {
            if (m_IsSuccess)
            {
                Instantiate(m_SuccessEffect, m_EffectParent).transform.localPosition = Vector3.zero;

                //m_Font1.sprite = FindSprite("CombatSettlement_Font1");
                //m_Font2.sprite = FindSprite("CombatSettlement_Font2");
            }
            else
            {
                Instantiate(m_FailEffect, m_EffectParent).transform.localPosition = Vector3.zero;
                //m_Font1.sprite = FindSprite("CombatSettlement_Font3");
                //m_Font2.sprite = FindSprite("CombatSettlement_Font4");
            }
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
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();

            //else
            //    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
        }
    }
}