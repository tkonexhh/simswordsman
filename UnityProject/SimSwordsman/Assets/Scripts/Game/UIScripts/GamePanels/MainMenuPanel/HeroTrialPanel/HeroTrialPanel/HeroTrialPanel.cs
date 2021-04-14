using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameWish.Game
{

    public class HeroTrialPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_SelectCharacterBtn;
        [SerializeField]
        private Button m_FinishBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_RuleBtn;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();

        }

        private void BindAddListenerEvent()
        {
            m_RuleBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.IntroductionRulesPanel);
            });

            m_CloseBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.HeroTrialMgr.OnExitHeroTrial();

                HideSelfWithAnim();
            });

            m_SelectCharacterBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, PanelType.HeroTrial);
            });

            m_FinishBtn.onClick.AddListener(() =>
            {
                //MainGameMgr.S.HeroTrialMgr.
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);


            //OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            UIMgr.S.OpenPanel(UIID.MainMenuPanel);

            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();

            //CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}