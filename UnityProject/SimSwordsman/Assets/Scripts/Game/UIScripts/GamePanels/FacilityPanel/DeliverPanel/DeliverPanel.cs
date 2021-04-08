using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameWish.Game.TDDeliverTable;

namespace GameWish.Game
{
    public class DeliverPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;
        [Header("Middle")]
        [SerializeField]
        private Text m_Desc;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Text m_NextUnlockCont;
        [SerializeField]
        private Text m_UpgradeCondition;
        [SerializeField]
        private Image m_FacilityPhoto;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Transform m_ResTra;
        [SerializeField]
        private GameObject m_ResItem; 
        [SerializeField]
        private GameObject m_RedPoint;
        [Header("Down")]
        [SerializeField]
        private Transform m_DeliverTra;
        [SerializeField]
        private GameObject m_DeliverItem;


        private FacilityType m_CurFacilityType = FacilityType.None;
        private int m_CurLevel;
        private DeliverLevelInfo m_CurDeliverInfo = null;
        private List<CostItem> m_CostItems;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        public List<SingleDeliverDetailData> DaliverDetailDataList = null;
        private List<DeliverConfig> m_DeliverConfigList = new List<DeliverConfig>();
        private List<DeliverItem> m_DeliverItemList = new List<DeliverItem>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_CurFacilityType = FacilityType.Deliver;
            BindAddListenerEvent();

            GetInformationForNeed();
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
         
            RefreshPanelInfo();

            foreach (var item in DaliverDetailDataList)
            {
                CreateDeliverItem(item);
            }
        }

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            m_Level.text = CommonUIMethod.GetGrade(m_CurDeliverInfo.level);
            m_Desc.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityPhoto.sprite = SpriteHandler.S.GetSprite(AtlasDefine.FacilityIconAtlas, "Deliver"+ m_CurDeliverInfo.level);
            if (m_NextFacilityLevelInfo != null)
                m_UpgradeCondition.text = "升级需要讲武堂达到" + CommonUIMethod.GetGrade(m_NextFacilityLevelInfo.upgradeNeedLobbyLevel);
            if (m_NextFacilityLevelInfo != null)
                m_NextUnlockCont.text = /*m_CurDeliverInfo.GetCurMedicinalPowderName();*/"";
            else
                m_NextUnlockCont.text = "无";

            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_ResTra, m_ResItem, m_NextFacilityLevelInfo);
        }

        private void BindAddListenerEvent()
        {
            m_UpgradeBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (!CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems))
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);

                    DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, m_CurFacilityType.ToString() + ";" + m_CurLevel);

                    HideSelfWithAnim();
                }
            });
            m_CloseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        private void CreateDeliverItem(SingleDeliverDetailData item)
        {
            DeliverItem deliverItem = Instantiate(m_DeliverItem, m_DeliverTra).GetComponent<DeliverItem>();
            deliverItem.OnInit(item);
            m_DeliverItemList.Add(deliverItem);
        }

        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurDeliverInfo = (DeliverLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_NextUnlockCont.text = Define.COMMON_DEFAULT_STR;
                m_UpgradeCondition.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextFacilityLevelInfo = (DeliverLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
            }
            m_DeliverConfigList.AddRange(GetDeliverConfigList());
            DaliverDetailDataList = GameDataMgr.S.GetClanData().DeliverData.GetSingleDeliverDetailDataList();
        }
    }
}