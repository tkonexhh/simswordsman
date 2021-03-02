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

        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;

        [Header("Buttom")]
        [SerializeField]
        private Transform m_Bottom;
        [SerializeField]
        private GameObject m_RecruitmentOrderItem; 
    

        private int m_CurLevel = 1;
        private const int NextFontSize = 38;
        private const int FontWight = 305;
        private List<CostItem> m_CostItems;
        private List<string> m_UnlockContent = null;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        private FacilityConfigInfo m_CurFacilityConfigInfo = null;

        private RecruitDiscipleMgr m_RecruitDiscipleMgr = null;
        private FacilityMgr m_FacilityMgr = null;


        private void InitFixedInfo()
        {
            m_BriefIntroduction.text = m_CurFacilityConfigInfo.desc;
            m_UpgradeTitle.text = "升级所需资源:";
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

            //m_SilverRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.SilverMedal); });
            //m_GoldRecruitBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.GoldMedal); });
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            GetInformationForNeed();

            //OpenDependPanel(EngineUI.MaskPanel, -1, null);
            InitFixedInfo();

            RefreshPanelInfo();

            CreateRecruitmentOrder(RecruitType.SilverMedal, FindSprite("SilverOrderImg"));
            CreateRecruitmentOrder(RecruitType.GoldMedal, FindSprite("GoldOrderImg"));
        }

        private void CreateRecruitmentOrder(RecruitType Medal, Sprite sprite)
        {
            ItemICom itemICom = Instantiate(m_RecruitmentOrderItem, m_Bottom).GetComponent<ItemICom>();
            itemICom.OnInit(this, null, Medal, sprite);
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
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo,m_CostItems,false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            m_LvellValue.text = CommonUIMethod.GetGrade(m_CurLevel);
            m_LobbyImg.sprite = FindSprite("Lobby" + m_CurLevel);

            RefreshResInfo();
            SetNextLobbyStr();
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
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_NextFacilityLevelInfo, this);
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
            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isReduceSuccess)
            {
                AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                for (int i = 0; i < m_CostItems.Count; i++)
                    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Lobby, 1, 1);
                GetInformationForNeed();
                RefreshPanelInfo();
                HideSelfWithAnim();
            }
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
                m_Res1Img.gameObject.SetActive(false);
                m_Res2Img.gameObject.SetActive(false);
                m_Res3Img.gameObject.SetActive(false);
                m_UnlockContentValue.text = "无";
            }
            else
            {
                m_NextFacilityLevelInfo = m_FacilityMgr.GetFacilityLevelInfo(FacilityType.Lobby, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo?.GetUpgradeResCosts();
                m_UnlockContent = MainGameMgr.S.FacilityMgr.GetUnlockContent(m_CurLevel + 1);
            }
        }
    }
}

