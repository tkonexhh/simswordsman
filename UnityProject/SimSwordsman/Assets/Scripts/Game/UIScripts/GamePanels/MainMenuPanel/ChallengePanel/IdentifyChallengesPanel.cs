using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class IdentifyChallengesPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_ChallengeTitle;
        [SerializeField]
        private Text m_ChallengeCont;
        [SerializeField]
        private Text m_ChallengeRewardValue;

        [SerializeField]
        private Image m_ChallengePhoto;

        [SerializeField]
        private Button m_ChallengeBtn;
        [SerializeField]
        private Button m_CloseBtn;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_ChallengeBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel,PanelType.Challenge, m_CurChapterConfigInfo, m_LevelConfigInfo);
                CloseSelfPanel();
                CloseDependPanel(EngineUI.MaskPanel);
                EventSystem.S.Send(EventID.OnCloseParentPanel);
            });
        }



        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;
            m_LevelConfigInfo = MainGameMgr.S.ChapterMgr.GetLevelInfo(m_CurChapterConfigInfo.chapterId, (int)args[1]);
            RefreshPanelInfo();
        }

        private void RefreshPanelInfo()
        {
            m_ChallengeTitle.text = CommonUIMethod.GetChallengeTitle(m_CurChapterConfigInfo, m_LevelConfigInfo.level);
            m_ChallengeCont.text = m_LevelConfigInfo.desc;
            //m_ChallengeRewardValue.text = m_LevelConfigInfo.levelReward.value.ToString();
        }



        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}