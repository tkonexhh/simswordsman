using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
	public class PracticeFieldPanel : AbstractAnimPanel
	{
	    [SerializeField]
	    private Text m_PracticeFieldName;
		[SerializeField]
	    private Text m_BriefIntroductionTxt;
	    [SerializeField]
	    private Text m_UpgradeCostCoinValueTxt;
	    [SerializeField]
	    private Text m_CurLevelTxt;
	    [SerializeField]
	    private Text m_CurTrainingPositionTxt;
	    [SerializeField]
	    private Text m_NextTrainingPositionTxt;
	    [SerializeField]
	    private Text m_UpgradeConditionsTxt;
	    [SerializeField]
	    private Text m_CurUpgradeSpeedTxt;
	    [SerializeField]
	    private Text m_NextUpgradeSpeedTxt;

		[SerializeField]
		private Button m_UpgradeBtn;
		[SerializeField]
		private Button m_CloseBtn;

		[SerializeField]
		private Transform m_PracticeDiscipleContTra;
		[SerializeField]
		private GameObject m_PracticeDisciple;

		private FacilityType m_CurFacilityType;

		private int m_CurLevel;
		private PracticeFieldLevelInfo m_CurPracticeFieldLevelInfo = null;

		protected override void OnUIInit()
	    {
	        base.OnUIInit();


			BindAddListenerEvent();
	    }

        private void BindAddListenerEvent()
        {
			m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
			m_UpgradeBtn.onClick.AddListener(()=> {

				bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeCostCoinValueTxt.text));

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

        private void GetInformationForNeed()
        {
			m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
			m_CurPracticeFieldLevelInfo = (PracticeFieldLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
		}

        private void RefreshPanelInfo() {
            switch (m_CurFacilityType)
            {
                case FacilityType.PracticeFieldEast:
					m_PracticeFieldName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELDEAST_NAME);
                    break;
                case FacilityType.PracticeFieldWest:
					m_PracticeFieldName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELDWEST_NAME);
                    break;
            }
			m_BriefIntroductionTxt.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_PRACTICEFIELD_DESCRIBLE);


			RefreshPanelText();
			
			for (int i = 0; i < 4; i++)
            {
				CreatePracticeDisciple();
			}
        }

        private void RefreshPanelText()
        {
			
			m_UpgradeCostCoinValueTxt.text = m_CurPracticeFieldLevelInfo.upgradeCost.ToString();
			m_CurLevelTxt.text = m_CurLevel.ToString();
			m_CurTrainingPositionTxt.text = m_CurPracticeFieldLevelInfo.GetCurCapacity().ToString();
			m_NextTrainingPositionTxt.text = m_CurPracticeFieldLevelInfo.GetNextCapacity().ToString();
			m_CurUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetCurLevelUpSpeed().ToString();
			m_NextUpgradeSpeedTxt.text = m_CurPracticeFieldLevelInfo.GetNextLevelUpSpeed().ToString();
			//m_UpgradeConditionsTxt.text = m_CurPracticeFieldLevelInfo.preconditions

		}

		private void CreatePracticeDisciple()
		{
			ItemICom itemICom = Instantiate(m_PracticeDisciple, m_PracticeDiscipleContTra).GetComponent<ItemICom>();
			//itemICom.OnInit();
		}


		protected override void OnPanelHideComplete()
	    {
	        base.OnPanelHideComplete();
			CloseSelfPanel();
	    }
	}
	
}