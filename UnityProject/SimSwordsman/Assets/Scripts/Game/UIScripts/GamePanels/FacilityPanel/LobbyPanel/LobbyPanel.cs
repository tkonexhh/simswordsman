﻿using System.Collections;
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
        private List<CostItem> m_CostItems;
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
            RefreshResInfo();
        }

        private void RefreshResInfo()
        {
            if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_NextFacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res2Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1Img.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2Img.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = m_NextFacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res3Img.sprite = FindSprite("Coin");
                m_Res3Img.gameObject.SetActive(true);
            }
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
            {
                FloatMessage.S.ShowMsg("未达到升级条件");
                return;
            }
            if (m_NextFacilityLevelInfo == null)
                return;
            bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
            if (isReduceSuccess)
            {
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
                    return false;
            }

            return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_NextFacilityLevelInfo.upgradeCoinCost);
        }
        /// <summary>
        /// 刷新等级信息
        /// </summary>
        private void RefreshLevelInfo()
        {
            m_CurLevel = m_FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            m_NextFacilityLevelInfo = m_FacilityMgr.GetFacilityLevelInfo(FacilityType.Lobby, m_CurLevel+1);
            m_CurFacilityConfigInfo = m_FacilityMgr.GetFacilityConfigInfo(FacilityType.Lobby);
            m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
        }
    }
}

