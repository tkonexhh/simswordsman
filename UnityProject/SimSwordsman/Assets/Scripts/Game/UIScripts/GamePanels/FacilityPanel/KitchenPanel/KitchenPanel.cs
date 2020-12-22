using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public class KitchenPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_KitchenTitleTxt;
        [SerializeField]
        private Text m_KitchenContTxt;
        [SerializeField]
        private Text m_UpgradeRequiredCoinTxt;
        [SerializeField]
        private Text m_CurLevelTxt;
        [SerializeField]
        private Text m_CurFoodLimitTxt;
        [SerializeField]
        private Text m_NextFoodLimitTxt;
        [SerializeField]
        private Text m_CurRecoverySpeedTxt;
        [SerializeField]
        private Text m_NextRecoverySpeedTxt;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_UpgradeBtn;

        [SerializeField]
        private Transform m_KitchenContTra;

        [SerializeField]
        private GameObject m_FoodItem;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private KitchLevelInfo m_CurKitchLevelInfo = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
        }

        private void RefreshPanelInfo()
        {

            m_KitchenTitleTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KITCHEN_NAME);
            m_KitchenContTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KITCHEN_DESCRIBLE);

            RefreshPanelText();
            for (int i = 0; i < 5; i++)
            {
                CreateFood();
            }
        }
        private void RefreshPanelText()
        {
            m_CurLevelTxt.text = m_CurLevel.ToString();
            m_CurFoodLimitTxt.text = m_CurKitchLevelInfo.GetCurFoodAddSpeed().ToString();
            m_CurRecoverySpeedTxt.text = m_CurKitchLevelInfo.GetCurFoodAddSpeed().ToString();

            m_NextFoodLimitTxt.text = m_CurKitchLevelInfo.GetNextFoodLimit().ToString();
            m_NextRecoverySpeedTxt.text = m_CurKitchLevelInfo.GetNextFoodAddSpeed().ToString();

            m_UpgradeRequiredCoinTxt.text = m_CurKitchLevelInfo.upgradeCost.ToString();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
            GetInformationForNeed();
            RefreshPanelInfo();

        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_UpgradeBtn.onClick.AddListener(() =>
            {

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeRequiredCoinTxt.text));

                if (isReduceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelText();
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        public void CreateFood()
        {
            ItemICom itemICom = Instantiate(m_FoodItem, m_KitchenContTra).GetComponent<ItemICom>();
            itemICom.OnInit(this);
        }
    }

}