using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{

	public class ConstructionFacilitiesPanel : AbstractAnimPanel
	{
	    [SerializeField]
	    private Text m_TitleTxt;
	    [SerializeField]
	    private Text m_FacilityDescribe; 
		[SerializeField]
		private Text m_ConstructionConditionValue;
        [SerializeField]
        private Text m_BaoziValue;
        [SerializeField]
        private Text m_CoinValue;


        [SerializeField]
		private Image m_FacilityPhotoImg;

		[SerializeField]
		private Button m_AcceptBtn;
		[SerializeField]
		private Button m_CloseBtn;

        private FacilityType m_FacilityType;
        private int m_SubId;

        private FacilityConfigInfo m_CurFacilityConfigInfo = null;

        protected override void OnUIInit()
	    {
	        base.OnUIInit();
           
			BindAddListenerEvent();
	    }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if (args.Length < 2)
            {
                Log.e("Construct facility panel, args pattern wrong");
                return;
            }

            m_FacilityType = (FacilityType)args[0];

            Sprite facilityImg =  FindSprite(m_FacilityType.ToString());
            if (facilityImg != null)
                m_FacilityPhotoImg.sprite = facilityImg;
            else
                Log.w("Facility Image is null,it is = {0} ", facilityImg);
            m_SubId = (int)args[1];
            RefreshPanelInfo();

            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }

        private void RefreshPanelInfo()
        {
            m_CurFacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_FacilityType);
            m_TitleTxt.text = m_CurFacilityConfigInfo.name;
            m_FacilityDescribe.text = m_CurFacilityConfigInfo.desc;

           // m_ConstructionConditionValue.text = Define.LECTURE_HALL + m_CurFacilityConfigInfo.GetNeedLobbyLevel() + Define.LEVEL;
            m_CoinValue.text = m_CurFacilityConfigInfo.GetUnlockCoinCost().ToString();

        }
       

        private void BindAddListenerEvent()
        {

			m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_AcceptBtn.onClick.AddListener(()=> 
            {
                if (GameDataMgr.S.GetGameData().playerInfoData.ReduceCoinNum(double.Parse(m_CoinValue.text)))
                {
                    EventSystem.S.Send(EventID.OnStartUnlockFacility, m_FacilityType, m_SubId);

                    HideSelfWithAnim();
                }
            });
		}


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }

}