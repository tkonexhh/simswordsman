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
            //m_UpgradeTitle.text = CommonUIMethod.GetUpgradeCondition(m_NextFacilityLevelInfo.upgradeNeedLobbyLevel);
            m_UpgradeTitle.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADENEEDS);
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

            CreateRecruitmentOrder(RecruitType.SilverMedal, FindSprite("silverolder"));
            CreateRecruitmentOrder(RecruitType.GoldMedal, FindSprite("goldolder"));
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
            if (m_CostItems == null)
                return;

            if (m_CostItems.Count == 1)
            {
                int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[0].itemId);
                m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItem, m_CostItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[0].value);
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                int havaItemFirst = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[0].itemId);
                int havaItemSec = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(m_CostItems[1].itemId);
                m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemFirst, m_CostItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[0].value);
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemSec, m_CostItems[1].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(m_CostItems[1].value);
                m_Res2Img.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = GetCurCoin() + Define.SLASH + CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
        }
        private int GetCurItem(int hava, int number)
        {
            if (hava >= number)
                return number;
            return hava;
        }
        private string GetCurCoin()
        {
            long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coin >= m_NextFacilityLevelInfo.upgradeCoinCost)
                return CommonUIMethod.GetTenThousand(m_NextFacilityLevelInfo.upgradeCoinCost);
            return CommonUIMethod.GetTenThousand((int)coin);
        }

        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
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
            if (!CheackIsBuild())
                return;
            if (m_NextFacilityLevelInfo == null)
                return;
            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isReduceSuccess)
            {
                for (int i = 0; i < m_CostItems.Count; i++)
                    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                EventSystem.S.Send(EventID.OnStartUpgradeFacility, FacilityType.Lobby, 1, 1);
                RefreshLevelInfo();
                RefreshPanelInfo();
            }
        }
        private bool CheackIsBuild()
        {
            if (CheckPropIsEnough())
                return true;
            return false;
        }
        private bool CheckPropIsEnough()
        {
            for (int i = 0; i < m_CostItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_CostItems[i].itemId, m_CostItems[i].value);
                if (!isHave)
                {
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return false;
                }
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
            {
                FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_COIN));
                return false;
            }
        }
        /// <summary>
        /// 刷新等级信息
        /// </summary>
        private void RefreshLevelInfo()
        {
            m_CurLevel = m_FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            m_NextFacilityLevelInfo = m_FacilityMgr.GetFacilityLevelInfo(FacilityType.Lobby, m_CurLevel + 1);
            m_CurFacilityConfigInfo = m_FacilityMgr.GetFacilityConfigInfo(FacilityType.Lobby);
            m_UnlockContent = MainGameMgr.S.FacilityMgr.GetUnlockContent(m_CurLevel + 1);
            if (m_NextFacilityLevelInfo != null)
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
        }
    }
}

