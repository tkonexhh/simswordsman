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
        private Text m_ForgeHouseTitle;
        [SerializeField]
        private Text m_ForgeHouseCont;
        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Text m_NextLimitTxt;
        [SerializeField]
        private Text m_UpgradeRequiredCoinTxt;
        
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Transform m_ForgeHouseContTra;

        [SerializeField]
        private GameObject m_ForgeHouseItem;
        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private ForgeHouseInfo m_CurForgeHouseInfo = null;
        private List<EquipmentType> m_UnlockEquipmentTypeList = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(long.Parse(m_UpgradeRequiredCoinTxt.text));

                if (isReduceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelText();
                }
            });
        }

        private void RefreshPanelText()
        {
            m_NextLimitTxt.text = string.Empty;
            for (int i = 0; i < m_UnlockEquipmentTypeList.Count; i++)
            {
                m_NextLimitTxt.text += m_UnlockEquipmentTypeList[i].ToString();
            }
            m_CurLevelTxt.text = m_CurLevel.ToString();
            m_UpgradeRequiredCoinTxt.text = m_CurForgeHouseInfo.upgradeCoinCost.ToString();
          
        }

        private void RefreshPanelInfo()
        {
            m_ForgeHouseTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_FORGEHOUSE_NAME);
            m_ForgeHouseCont.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_FORGEHOUSE_DESCRIBLE);
            RefreshPanelText();
            for (int i = 0; i < 5; i++)
            {
                CreateForgeHouseItem();
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            m_CurFacilityType = (FacilityType)args[0];

            GetInformationForNeed();

            RefreshPanelInfo();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurForgeHouseInfo = (ForgeHouseInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_UnlockEquipmentTypeList = m_CurForgeHouseInfo.GetNextUnlockEquipmentType();

        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private void CreateForgeHouseItem()
        {
            ItemICom itemICom = Instantiate(m_ForgeHouseItem, m_ForgeHouseContTra).GetComponent<ItemICom>();
            //itemICom.OnInit();
        }
    }
	
}