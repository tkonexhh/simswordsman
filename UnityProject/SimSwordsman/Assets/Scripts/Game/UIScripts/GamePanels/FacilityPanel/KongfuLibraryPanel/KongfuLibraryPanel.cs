using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
	public class KongfuLibraryPanel : AbstractAnimPanel
	{
	    [SerializeField]
	    private Button m_CloseBtn;
	    [SerializeField]
	    private Text m_KongfuLibraryNameTxt;
	    [SerializeField]
	    private Text m_BriefIntroductionTxt;
	    [SerializeField]
	    private Button m_UpgradeBtn;
	    [SerializeField]
	    private Text m_UpgradeCostCoinValueTxt;
	    [SerializeField]
	    private Text m_CurLevelValueTxt;
	    [SerializeField]
	    private Text m_MartialArtsContTxt;
	    [SerializeField]
	    private Transform m_MartialArtsContTra;
		[SerializeField]
		private GameObject m_CopyScripturesItem;

		private FacilityType m_CurFacilityType = FacilityType.None;

		private int m_CurLevel;
		private KongfuLibraryLevelInfo m_CurKongfuLibraryLevelInfo = null;

		protected override void OnUIInit()
	    {
	        base.OnUIInit();
			BindAddListenerEvent();
	    }

        private void RefreshPanelInfo()
        {
			m_KongfuLibraryNameTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KONGFULIVRARY_NAME);
			m_BriefIntroductionTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_KONGFULIVRARY_DESCRIBLE);

			RefreshPanelText();

			for (int i = 0; i < 4; i++)
            {
				CreateCopyScripturesItem();
			}
        }

		private void GetInformationForNeed()
		{
			m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
			m_CurKongfuLibraryLevelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
		}

		private void RefreshPanelText()
		{
			m_CurLevelValueTxt.text = m_CurLevel.ToString();

			m_UpgradeCostCoinValueTxt.text = m_CurKongfuLibraryLevelInfo.upgradeCoinCost.ToString();

			List<KungfuType> KongfuList = m_CurKongfuLibraryLevelInfo.GetNextLevelUnlockedKongfuList();

			m_MartialArtsContTxt.text = "";
			for (int i = 0; i < KongfuList.Count; i++)
            {
				m_MartialArtsContTxt.text += KongfuList[0].ToString();
			}
		}

		private void BindAddListenerEvent()
        {
			m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

			m_UpgradeBtn.onClick.AddListener(() => {

				bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(long.Parse(m_UpgradeCostCoinValueTxt.text));

				if (isReduceSuccess)
				{
					EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
					GetInformationForNeed();
					RefreshPanelText();
				}
			});
		}

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
			m_CurFacilityType = (FacilityType)args[0];
			GetInformationForNeed();

			RefreshPanelInfo();
		}

        protected override void OnPanelHideComplete()
	    {
	        base.OnPanelHideComplete();
			CloseSelfPanel();
	    }

		private void CreateCopyScripturesItem()
		{
			ItemICom  itemICom = Instantiate(m_CopyScripturesItem, m_MartialArtsContTra).GetComponent<ItemICom>();
			//itemICom.OnInit();
		}
	}
	
}