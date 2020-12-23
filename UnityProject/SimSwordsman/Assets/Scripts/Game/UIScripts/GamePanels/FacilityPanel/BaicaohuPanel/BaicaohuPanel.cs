using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BaicaohuPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_BaicaohuTitle;
        [SerializeField]
        private Text m_BaicaohuCont;
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
        private Transform m_BaicaohuContTra;

        [SerializeField]
        private GameObject m_BaicaohuItem;

        private FacilityType m_CurFacilityType = FacilityType.None;

        private int m_CurLevel;
        private BaicaohuInfo m_CurBaicaohuInfo = null;
        private List<MedicinalPowderType> m_MedicinalPowderTypes = null;
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

                bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeRequiredCoinTxt.text));

                if (isReduceSuccess)
                {
                    EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                    GetInformationForNeed();
                    RefreshPanelText();
                }
            });
        }

        private void RefreshPanelInfo()
        {
            m_BaicaohuTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_BAICAOHU_NAME);
            m_BaicaohuCont.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_BAICAOHU_DESCRIBLE);

            RefreshPanelText();
            for (int i = 0; i < 5; i++)
            {
                CreateForgeHouseItem();
            }
        }
        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            m_CurBaicaohuInfo = (BaicaohuInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_MedicinalPowderTypes = m_CurBaicaohuInfo.GetNextUnlockMedicinalPowderType();
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
            m_NextLimitTxt.text = string.Empty;
            for (int i = 0; i < m_MedicinalPowderTypes.Count; i++)
            {
                m_NextLimitTxt.text += m_MedicinalPowderTypes[i].ToString();
            }

            m_UpgradeRequiredCoinTxt.text = m_CurBaicaohuInfo.upgradeResCosts.ToString();
            m_CurLevelTxt.text = m_CurLevel.ToString();
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private void CreateForgeHouseItem()
        {
            ItemICom itemICom = Instantiate(m_BaicaohuItem, m_BaicaohuContTra).GetComponent<ItemICom>();
            //itemICom.OnInit();
        }
    }

}