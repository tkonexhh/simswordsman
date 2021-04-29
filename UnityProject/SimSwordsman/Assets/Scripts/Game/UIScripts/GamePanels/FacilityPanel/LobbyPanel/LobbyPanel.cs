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
        /// 免费招募
        /// </summary>
        Free,
        /// <summary>
        /// 招募令招募
        /// </summary>
        RecruitmentOrder,
        /// <summary>
        /// 看广告招募
        /// </summary>
        LookAdvertisement,
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
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeBtnValue;
        [SerializeField]
        private RectTransform m_UnlockBg;
        [SerializeField]
        private Text m_NextLevelUnlockText;
        [SerializeField]
        private Text m_UnlockContentValue;
        [SerializeField]
        private GameObject m_RedPoint;



        [Header("Buttom")]
        [SerializeField]
        private Transform m_Bottom;
        [SerializeField]
        private GameObject m_RecruitmentOrderItem;
        [Header("Res")]
        [SerializeField]
        private Transform m_UpgradeResItemTra;
        [SerializeField]
        private GameObject m_UpgradeResItem;


        private int m_CurLevel = 1;
        private const int NextFontSize = 20;
        private const int FontWight = 305;
        private List<CostItem> m_CostItems;
        private List<string> m_UnlockContent = null;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        private FacilityConfigInfo m_CurFacilityConfigInfo = null;
        private LobbyLevelInfo m_LobbyLevelInfo = null;

        private RecruitDiscipleMgr m_RecruitDiscipleMgr = null;
        private FacilityMgr m_FacilityMgr = null;

        private List<RecruitmentOrderItem> m_RecruitmentItemList = new List<RecruitmentOrderItem>();

        private void InitFixedInfo()
        {
            m_BriefIntroduction.text = m_CurFacilityConfigInfo.desc;
            m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
        }
        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_RecruitDiscipleMgr = MainGameMgr.S.RecruitDisciplerMgr;
            m_FacilityMgr = MainGameMgr.S.FacilityMgr;

            BindAddListenerEvent();
        }

        /// <summary>
        /// 绑定监听函数
        /// </summary>
        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_UpgradeBtn.onClick.AddListener(() => { OnClickUpgradeBtn(); });
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            GetInformationForNeed();

            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            InitFixedInfo();

            RefreshPanelInfo();

            m_RecruitmentItemList.Clear();
            CreateRecruitmentOrder(RecruitType.SilverMedal, FindSprite("SilverOrderImg"));
            CreateRecruitmentOrder(RecruitType.GoldMedal, FindSprite("GoldOrderImg"));
        }

        private void CreateRecruitmentOrder(RecruitType Medal, Sprite sprite)
        {
            RecruitmentOrderItem item = Instantiate(m_RecruitmentOrderItem, m_Bottom).GetComponent<RecruitmentOrderItem>();
            if (item != null)
            {
                m_RecruitmentItemList.Add(item);
                item.OnInit(this, null, Medal, sprite);
            }

            //ItemICom itemICom = Instantiate(m_RecruitmentOrderItem, m_Bottom).GetComponent<ItemICom>();
            //itemICom.OnInit(this, null, Medal, sprite);
        }

        protected override void OnClose()
        {
            base.OnClose();

            m_RecruitmentItemList.ForEach(x => x.OnClose());
        }

        /// <summary>
        /// 刷新按钮文本信息
        /// </summary>
        public void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems, false) && CommonMethod.CheckEnoughDiscipleLevel(m_LobbyLevelInfo))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            m_LvellValue.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_LobbyImg.sprite = FindSprite("Lobby" + m_CurLevel);
            if (m_LobbyLevelInfo != null)
                m_UpgradeTitle.text = "需要拥有" + CheckFontColor() + m_LobbyLevelInfo.upgradePreconditions.DiscipleLevel + "级弟子";
            else
                m_UpgradeTitle.text = string.Empty;

            RefreshResInfo();
            SetNextLobbyStr();
        }

        private string CheckFontColor()
        {
            if (CommonMethod.CheckEnoughDiscipleLevel(m_LobbyLevelInfo))
                return CommonUIMethod.GetStrForColor(ColorDefine.BULE, m_LobbyLevelInfo.upgradePreconditions.DiscipleNumber + "名");
            else
                return CommonUIMethod.GetStrForColor(ColorDefine.RED, m_LobbyLevelInfo.upgradePreconditions.DiscipleNumber + "名");
        }
        private void SetNextLobbyStr()
        {
            if (m_UnlockContent == null)
                return;

            m_UnlockBg.sizeDelta = new Vector2(FontWight, 0);
            m_UnlockContentValue.rectTransform.sizeDelta = new Vector2(FontWight, 0);
            string str = string.Empty;
            Vector2 deltaData = Vector2.zero;
            str += m_UnlockContent[0];
            deltaData += new Vector2(0, NextFontSize);
            for (int i = 1; i < m_UnlockContent.Count; i++)
            {
                str += "\n";
                str += m_UnlockContent[i];
                deltaData += new Vector2(0, NextFontSize);
            }
            m_UnlockContentValue.rectTransform.sizeDelta += deltaData;
            deltaData += new Vector2(0, NextFontSize);
            m_UnlockBg.sizeDelta += deltaData;
            m_UnlockContentValue.text = str;
            //记录
            m_UnlockContentValue.text = m_UnlockContentValue.text.Replace("\\n", "\n");
        }
        private void RefreshResInfo()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_UpgradeResItemTra, m_UpgradeResItem, m_NextFacilityLevelInfo);
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
            AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            if (!CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems))
                return;
            //if (!CheckDiscipleCondition())
            //{
            //    FloatMessage.S.ShowMsg("需要拥有" + m_LobbyLevelInfo.upgradePreconditions.DiscipleNumber + "名" + m_LobbyLevelInfo.upgradePreconditions.DiscipleLevel + "级弟子");
            //    return;
            //}
            if (!PlatformHelper.isTestMode)
            {
                if (!CheckDiscipleCondition())
                {
                    FloatMessage.S.ShowMsg("需要拥有" + m_LobbyLevelInfo.upgradePreconditions.DiscipleNumber + "名" + m_LobbyLevelInfo.upgradePreconditions.DiscipleLevel + "级弟子");
                    return;
                }
            }

            //CheckDiscipleCondition();

            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isReduceSuccess)
            {
                AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                for (int i = 0; i < m_CostItems.Count; i++)
                    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Lobby, 1, 1);
                GetInformationForNeed();
                RefreshPanelInfo();
                DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, FacilityType.Lobby.ToString() + ";" + m_CurLevel);
                HideSelfWithAnim();
            }
        }

        private bool CheckDiscipleCondition()
        {
            return CommonMethod.CheckEnoughDiscipleLevel(m_LobbyLevelInfo);
        }

        /// <summary>
        /// 刷新等级信息
        /// </summary>
        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(FacilityType.Lobby);
            m_CurLevel = m_FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            m_CurFacilityConfigInfo = m_FacilityMgr.GetFacilityConfigInfo(FacilityType.Lobby);
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_UnlockContentValue.text = "无";
            }
            else
            {
                m_NextFacilityLevelInfo = m_FacilityMgr.GetFacilityLevelInfo(FacilityType.Lobby, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo?.GetUpgradeResCosts();
                m_LobbyLevelInfo = (LobbyLevelInfo)m_NextFacilityLevelInfo;
                m_UnlockContent = MainGameMgr.S.FacilityMgr.GetUnlockContent(m_CurLevel + 1);
            }
        }
    }
}

