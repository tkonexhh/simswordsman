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
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_RuleBtn;

        [SerializeField]
        private Text m_CountDownNumber;
        [SerializeField]
        private Button m_FinishTrialBtn;
        [SerializeField]
        private Slider m_CountDownSlider;
        [SerializeField]
        private Image m_CharacterIconBefore;
        [SerializeField]
        private Text m_CharacterNameBefore;
        [SerializeField]
        private Image m_CharacterIconAfter;
        [SerializeField]
        private Text m_CharacterNameAfter;
        [SerializeField]
        private Text m_Appellation;

        private CharacterItem TrialDisciple;
        protected override void OnUIInit()
        {
            base.OnUIInit();


            GetInfomationForNeed();
            RefreshPanelInfo();
            BindAddListenerEvent();
        }

        private void GetInfomationForNeed()
        {
            TrialDisciple = MainGameMgr.S.CharacterMgr.GetCharacterItem(MainGameMgr.S.HeroTrialMgr.TrialDiscipleID);
        }

        private void HandAdListenerEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnRefreshTrialPanel:
                    GetInfomationForNeed();
                    RefreshPanelInfo();
                    break;
                case (int)EventID.OnCountDownRefresh:
                    RefreshProgress((double)param[0]);
                    break;
            }
        }

        private void RefreshProgress(double second)
        {
            m_CountDownSlider.value = (float)second / (float)MainGameMgr.S.HeroTrialMgr.TrialTotalTime;
            m_CountDownNumber.text = CommonMethod.SplicingTime(second);
        }

        private void RefreshPanelInfo()
        {
            if (TrialDisciple == null)
                return;
            m_CharacterIconBefore.sprite = CommonMethod.GetDiscipleSprite(TrialDisciple);
            m_CharacterNameBefore.text = TrialDisciple.name;
            m_CharacterIconAfter.sprite = CommonMethod.GetDiscipleSprite(TrialDisciple);
            m_CharacterNameAfter.text = TrialDisciple.name;
            m_Appellation.text = CommonMethod.GetAppellation(MainGameMgr.S.HeroTrialMgr.TrialClan);
        }

        public override void OnBecomeHide()
        {
            base.OnBecomeHide();
            EventSystem.S.UnRegister(EventID.OnRefreshTrialPanel, HandAdListenerEvent);
            EventSystem.S.UnRegister(EventID.OnCountDownRefresh, HandAdListenerEvent);
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

            m_FinishTrialBtn.onClick.AddListener(() =>
            {
                MainGameMgr.S.HeroTrialMgr.Reset();
                MainGameMgr.S.HeroTrialMgr.OnExitHeroTrial();

                HideSelfWithAnim();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnRefreshTrialPanel, HandAdListenerEvent);
            EventSystem.S.Register(EventID.OnCountDownRefresh, HandAdListenerEvent);

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