using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum ClickType
    {
        None,
        First,
        RecruitmentOrder,
        LookAdvertisement,
        Over,
    }

    public class LobbyPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_FacilitiesName;
        [SerializeField]
        private Text m_BriefIntroduction;

        [SerializeField]
        private Text m_CurLevelValueText;
        [SerializeField]
        private Text m_NextLevelValueText;
        [SerializeField]
        private Text m_UpgradeRewardValueText;
        [SerializeField]
        private Text m_UpgradeNeedCoinValueText;
        [SerializeField]
        private Text m_RecruitmentGoldTxt;
        [SerializeField]
        private Text m_RecruitmentSilverTxt;

        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Button m_CloseBtn;
        [Header("银牌招募令")]
        [SerializeField]
        private Button m_SilverRecruitBtn;
        [SerializeField]
        private Text m_SilverRecruitmentTimes;
        [SerializeField]
        private Transform m_SilverRecruitmentTimesTra;

        [Header("金牌招募令")]
        [SerializeField]
        private Button m_GoldRecruitBtn;
        [SerializeField]
        private Text m_GoldRecruitmentTimes;
        [SerializeField]
        private Transform m_GoldRecruitmentTimesTra;




        private int m_CurLevel = 1;
        private FacilityLevelInfo m_CurFacilityLevelInfo = null;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        private FacilityConfigInfo m_CurFacilityConfigInfo = null;

        private int RecruitmentGoldCrder = 2;
        private int RecruitmentSilverCrder = 2;

        private RecruitDiscipleMgr m_RecruitDiscipleMgr = null;
        private FacilityMgr m_FacilityMgr = null;

        private ClickType m_CurrentGoldClickType = ClickType.First;
        private ClickType m_CurrentSilverClickType = ClickType.First;

        private Dictionary<RecruitType, ClickType> m_RecruitDic = new Dictionary<RecruitType, ClickType>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_RecruitDiscipleMgr = MainGameMgr.S.RecruitDisciplerMgr;
            m_FacilityMgr = MainGameMgr.S.FacilityMgr;

            m_RecruitDic.Add(RecruitType.SilverMedal, ClickType.First);
            m_RecruitDic.Add(RecruitType.GoldMedal, ClickType.First);

            BindAddListenerEvent();

        }

        /// <summary>
        /// 绑定监听函数
        /// </summary>
        private void BindAddListenerEvent()
        {
            m_UpgradeBtn.onClick.AddListener(() => { OnClickUpgradeBtn(); });

            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_SilverRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.SilverMedal); });
            m_GoldRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.GoldMedal); });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            EventSystem.S.Register(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);


            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            RefreshLevelInfo();

            RefreshPanelInfo();
            RefreshText();

        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
            EventSystem.S.UnRegister(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);

        }
        /// <summary>
        /// 事件监听回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void HandlingListeningEvents(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshPanelInfo:
                    RefreshPanelInfo();
                    break;
                case EventID.OnRefreshRecruitmentOrder:
                    RefreshRecruitmentOrder((RecruitType)param[0]);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 刷新招募令数量
        /// </summary>
        /// <param name="recruitType"></param>
        private void RefreshRecruitmentOrder(RecruitType recruitType)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    RecruitmentGoldCrder--;
                    break;
                case RecruitType.SilverMedal:
                    RecruitmentSilverCrder--;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 刷新按钮文本信息
        /// </summary>
        public void RefreshPanelInfo()
        {
            m_FacilitiesName.text = m_CurFacilityConfigInfo.name;
            m_BriefIntroduction.text = m_CurFacilityConfigInfo.desc;
            bool goldMedalFirst = m_RecruitDiscipleMgr.GetIsFirstMedal(RecruitType.GoldMedal);
            bool silverMedalFirst = m_RecruitDiscipleMgr.GetIsFirstMedal(RecruitType.SilverMedal);
            m_GoldRecruitmentTimesTra.gameObject.SetActive(false);
            m_SilverRecruitmentTimesTra.gameObject.SetActive(false);
            if (silverMedalFirst)
                m_RecruitmentSilverTxt.text = Define.RECRUIT_FREE;
            else
            {
                if (RecruitmentSilverCrder > 0)
                    m_RecruitmentSilverTxt.text = Define.SILVER_ORDER;
                else
                {
                    int silverMedalCount = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.SilverMedal);
                    m_SilverRecruitmentTimes.text = silverMedalCount.ToString()
                       + "/3";
                    m_RecruitDic[RecruitType.SilverMedal] = ClickType.LookAdvertisement;
                    m_RecruitmentSilverTxt.text = Define.LOOKING_AT_SILVER_ADVERTISEMENT;

                    m_SilverRecruitmentTimesTra.gameObject.SetActive(true);
                    if (silverMedalCount == 0)
                        m_RecruitDic[RecruitType.SilverMedal] = ClickType.Over;
                }
            }

            if (goldMedalFirst)
                m_RecruitmentGoldTxt.text = Define.RECRUIT_FREE;
            else
            {
                if (RecruitmentGoldCrder > 0)
                    m_RecruitmentGoldTxt.text = Define.GOLD_MEDAL_RECRUITt_ORDER;
                else
                {
                    int goldMedalCount = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.GoldMedal);
                    m_GoldRecruitmentTimes.text = goldMedalCount.ToString()
                       + "/3";
                    m_RecruitDic[RecruitType.GoldMedal] = ClickType.LookAdvertisement;
                    m_RecruitmentGoldTxt.text = Define.WATCH_GOLD_MEDALS;

                    m_GoldRecruitmentTimesTra.gameObject.SetActive(true);
                    if (goldMedalCount == 0)
                        m_RecruitDic[RecruitType.GoldMedal] = ClickType.Over;
                }
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            //CloseDependPanel(EngineUI.MaskPanel);
        }

        /// <summary>
        /// 升级按钮
        /// </summary>
        private void OnClickUpgradeBtn()
        {
            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeNeedCoinValueText.text));

            if (isReduceSuccess)
            {
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Lobby, 1, 1);
                RefreshLevelInfo();
                RefreshText();
            }
        }
        /// <summary>
        /// 招募点击事件
        /// </summary>
        private void OnClickRecruitBtn(RecruitType type)
        {
            switch (m_RecruitDic[type])
            {
                case ClickType.First:
                    UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.First);
                    m_RecruitDic[type] = ClickType.RecruitmentOrder;
                    break;
                case ClickType.RecruitmentOrder:
                    UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.RecruitmentOrder);
                    break;
                case ClickType.LookAdvertisement:
                    UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.LookAdvertisement);
                    break;
                case ClickType.Over:
                    UIMgr.S.OpenPanel(UIID.LogPanel, "招募标题", "招募次数用尽");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 刷新等级信息
        /// </summary>
        private void RefreshLevelInfo()
        {
            m_CurLevel = m_FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            m_CurFacilityLevelInfo = m_FacilityMgr.GetFacilityLevelInfo(FacilityType.Lobby, m_CurLevel);
            m_CurFacilityConfigInfo = m_FacilityMgr.GetFacilityConfigInfo(FacilityType.Lobby);
        }

        private void RefreshText()
        {
            m_CurLevelValueText.text = m_CurLevel.ToString();
            m_NextLevelValueText.text = (m_CurLevel + 1).ToString();
            m_UpgradeRewardValueText.text = m_CurFacilityLevelInfo.upgradeCosts.GetContent();
            m_UpgradeNeedCoinValueText.text = m_CurFacilityLevelInfo.upgradeCosts.ToString();

            m_SilverRecruitmentTimes.text = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.SilverMedal).ToString() + "/3";

            m_GoldRecruitmentTimes.text = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.GoldMedal).ToString() + "/3";

        }
    }
}

