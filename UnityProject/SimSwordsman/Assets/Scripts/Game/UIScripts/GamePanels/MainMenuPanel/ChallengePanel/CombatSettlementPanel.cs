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
        private Transform m_RewardContainer;
        [SerializeField]
        private GameObject m_RewardinfoItem;

        private bool m_IsSuccess;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private List<CharacterController> m_SelectedDiscipleList = null;
        private List<HerbType> m_SeletedHerb = null;
        private PanelType m_PanelType;
        private SimGameTask m_CurTaskInfo = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            m_SelectedDiscipleList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;

            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_ExitBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                PanelPool.S.DisplayPanel();
                EventSystem.S.Send(EventID.OnExitBattle);
                UIMgr.S.ClosePanelAsUIID(UIID.CombatInterfacePanel);
                OnPanelHideComplete();

                if (m_PanelType == PanelType.Task)
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
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
                    m_CurTaskInfo.ClaimReward((bool)args[2]);
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = (ChapterConfigInfo)args[1];
                    m_LevelConfigInfo = (LevelConfigInfo)args[2];
                    m_IsSuccess = (bool)args[3];
                    if (m_IsSuccess)
                    {
                        m_LevelConfigInfo.levelRewardList.ForEach(i => i.ApplyReward(1));
                        m_Font1.sprite = FindSprite("CombatSettlement_Font1");
                        m_Font2.sprite = FindSprite("CombatSettlement_Font2");
                    }
                    else
                    {
                        m_LevelConfigInfo.levelRewardList.ForEach(i => i.ApplyReward(2));
                        m_Font1.sprite = FindSprite("CombatSettlement_Font3");
                        m_Font2.sprite = FindSprite("CombatSettlement_Font4");
                    }
                    break;
                default:
                    break;
            }

            foreach (var item in m_SelectedDiscipleList)
                CreateRewardIInfoItem(item);
        }

        private void CreateRewardIInfoItem(CharacterController item)
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                    ItemICom taskRewardItemICom = Instantiate(m_RewardinfoItem, m_RewardContainer).GetComponent<ItemICom>();
                    taskRewardItemICom.OnInit(item, null, m_PanelType, m_CurTaskInfo, m_IsSuccess,this);
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
            OpenParentChallenge();
        }
    }
}