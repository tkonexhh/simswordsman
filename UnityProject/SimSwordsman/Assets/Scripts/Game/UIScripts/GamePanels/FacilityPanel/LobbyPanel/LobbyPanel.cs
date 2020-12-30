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
        /// <summary>
        /// 第一次招募
        /// </summary>
        First,
        /// <summary>
        /// 招募令招募
        /// </summary>
        RecruitmentOrder,
        /// <summary>
        /// 看广告招募
        /// </summary>
        LookAdvertisement,
        Over,
    }

    public class LobbyPanel : AbstractAnimPanel
    {

        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BriefIntroduction;
        [SerializeField]
        private Image m_LobbyImg;
        [SerializeField]
        private Text m_LvellValue;
        [SerializeField]
        private Transform m_FacilityTra;
        [SerializeField]
        private GameObject m_FacilityInfoItem;
        [SerializeField]
        private Text m_UpgradeTitle;
        [SerializeField]
        private Text m_CoinValue;
        [SerializeField]
        private Text m_BaoziValue;
        [SerializeField]
        private Button m_UpgradeBtn; 
        [SerializeField]
        private Text m_UpgradeBtnValue;

        [Header("Buttom")]
        [SerializeField]
        private Transform m_Bottom;
        [SerializeField]
        private GameObject m_RecruitmentOrderItem;

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
        private void InitFixedInfo()
        {
            m_BriefIntroduction.text = m_CurFacilityConfigInfo.desc;
            m_UpgradeTitle.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADENEEDS);
            m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
        }
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

            //m_SilverRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.SilverMedal); });
            //m_GoldRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.GoldMedal); });
        }

        protected override void OnOpen()
        {
            base.OnOpen();


            RefreshLevelInfo();

            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            InitFixedInfo();

            RefreshPanelInfo();

            CreateRecruitmentOrder(RecruitType.SilverMedal, FindSprite("SilverOlder"));
            CreateRecruitmentOrder(RecruitType.GoldMedal, FindSprite("GoldOlder"));
        }

        private void CreateRecruitmentOrder(RecruitType Medal,Sprite sprite)
        {
            ItemICom itemICom = Instantiate(m_RecruitmentOrderItem, m_Bottom).GetComponent<ItemICom>();
            itemICom.OnInit(this,null, Medal, sprite);
        }

        protected override void OnClose()
        {
            base.OnClose();
        

        }


        /// <summary>
        /// 刷新按钮文本信息
        /// </summary>
        public void RefreshPanelInfo()
        {
            m_LvellValue.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_CoinValue.text = Define.RIDE + m_CurFacilityLevelInfo.upgradeCoinCost;


            //m_FacilitiesName.text = m_CurFacilityConfigInfo.name;
            //m_BriefIntroduction.text = m_CurFacilityConfigInfo.desc;
            //bool goldMedalFirst = m_RecruitDiscipleMgr.GetIsFirstMedal(RecruitType.GoldMedal);
            //bool silverMedalFirst = m_RecruitDiscipleMgr.GetIsFirstMedal(RecruitType.SilverMedal);
            //m_GoldRecruitmentTimesTra.gameObject.SetActive(false);
            //m_SilverRecruitmentTimesTra.gameObject.SetActive(false);
            //if (silverMedalFirst)
            //    m_RecruitmentSilverTxt.text = Define.RECRUIT_FREE;
            //else
            //{
            //    if (RecruitmentSilverCrder > 0)
            //        m_RecruitmentSilverTxt.text = Define.SILVER_ORDER;
            //    else
            //    {
            //        int silverMedalCount = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.SilverMedal);
            //        m_SilverRecruitmentTimes.text = silverMedalCount.ToString()
            //           + "/3";
            //        m_RecruitDic[RecruitType.SilverMedal] = ClickType.LookAdvertisement;
            //        m_RecruitmentSilverTxt.text = Define.LOOKING_AT_SILVER_ADVERTISEMENT;

            //        m_SilverRecruitmentTimesTra.gameObject.SetActive(true);
            //        if (silverMedalCount == 0)
            //            m_RecruitDic[RecruitType.SilverMedal] = ClickType.Over;
            //    }
            //}

            //if (goldMedalFirst)
            //    m_RecruitmentGoldTxt.text = Define.RECRUIT_FREE;
            //else
            //{
            //    if (RecruitmentGoldCrder > 0)
            //        m_RecruitmentGoldTxt.text = Define.GOLD_MEDAL_RECRUITt_ORDER;
            //    else
            //    {
            //        int goldMedalCount = m_RecruitDiscipleMgr.GetCurRecruitCount(RecruitType.GoldMedal);
            //        m_GoldRecruitmentTimes.text = goldMedalCount.ToString()
            //           + "/3";
            //        m_RecruitDic[RecruitType.GoldMedal] = ClickType.LookAdvertisement;
            //        m_RecruitmentGoldTxt.text = Define.WATCH_GOLD_MEDALS;

            //        m_GoldRecruitmentTimesTra.gameObject.SetActive(true);
            //        if (goldMedalCount == 0)
            //            m_RecruitDic[RecruitType.GoldMedal] = ClickType.Over;
            //    }
            //}
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
            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_CoinValue.text));

            if (isReduceSuccess)
            {
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Lobby, 1, 1);
                RefreshLevelInfo();
                RefreshPanelInfo();
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

       
    }
}

