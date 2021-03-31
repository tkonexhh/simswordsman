using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ForgeHousePanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_ForgeHouseCont;
        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Image m_FacilityIcon;

        [SerializeField]
        private Text m_NextUnlockName;  
        [SerializeField]
        private Text m_UpGradeCondition;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Transform m_ItemTra;

        [SerializeField]
        private GameObject m_RedPoint;
        [SerializeField]
        private GameObject m_ItemPrefab;

        [Header("Res")]
        [SerializeField]
        private Transform m_UpgradeResItemTra;
        [SerializeField]
        private GameObject m_UpgradeResItem;
        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private ForgeHouseInfo m_CurForgeHouseInfo = null;
        private List<CostItem> m_CostItems;
        private FacilityLevelInfo m_NextFacilityLevelInfo = null;
        private FacilityLevelInfo m_CurFacilityLevelInfo = null;
        private List<ForgeHouseItem> m_Items = new List<ForgeHouseItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }
        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=>{
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                HideSelfWithAnim();
            });
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (!CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems))
                    return;
                if (m_NextFacilityLevelInfo == null)
                    return;
                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextFacilityLevelInfo.upgradeCoinCost);
                if (isReduceSuccess)
                {
                    AudioMgr.S.PlaySound(Define.SOUND_BLEVELUP);

                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    //GetInformationForNeed();
                    //RefreshPanelInfo();
                    DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, m_CurFacilityType.ToString() + ";" + m_CurLevel);
                    HideSelfWithAnim();
                }
            });
        }

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextFacilityLevelInfo, m_CostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
            m_CurLevelTxt.text = CommonUIMethod.GetGrade(m_CurForgeHouseInfo.level);
            m_ForgeHouseCont.text = TDFacilityConfigTable.GetFacilityConfigInfo(m_CurFacilityType).desc;
            m_FacilityIcon.sprite = FindSprite("ForgeHouse" + m_CurLevel);

            RefreshPanelText();
            UpdateItems();
        }

        private void GetInformationForNeed()
        {
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurForgeHouseInfo = (ForgeHouseInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_NextUnlockName.text = Define.COMMON_DEFAULT_STR;
                m_UpGradeCondition.text = Define.COMMON_DEFAULT_STR;
            }
            else
            {
                m_NextFacilityLevelInfo = (ForgeHouseInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_CostItems = m_NextFacilityLevelInfo.GetUpgradeResCosts();
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();
        }

        private void RefreshPanelText()
        {
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_UpgradeResItemTra, m_UpgradeResItem, m_NextFacilityLevelInfo);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();
        }

        private void UpdateItems()
        {
            string[] temp = null;
            if (m_Items.Count == 0)
            {
                for (int i = 0; i < TDFacilityForgeHouseTable.dataList.Count; i++)
                {
                    temp = TDFacilityForgeHouseTable.dataList[i].unlockEquip.Split(';');
                    foreach (var id in temp)
                    {
                        GameObject obj = Instantiate(m_ItemPrefab, m_ItemTra);
                        ForgeHouseItem item = obj.GetComponent<ForgeHouseItem>();
                        item.m_ForgeHouseID = int.Parse(id);
                        item.UnlockLevel = TDFacilityForgeHouseTable.dataList[i].level;
                        m_Items.Add(item);
                    }
                }
            }
            for (int i = 0; i < m_Items.Count; i++)
            {
                ItemICom itemICom = m_Items[i].GetComponent<ItemICom>();
                itemICom.OnInit(this, null);
            }
        }

        protected override void OnClose()
        {
            base.OnClose();

            m_Items.ForEach(x => x.OnClose());
        }
    }
}