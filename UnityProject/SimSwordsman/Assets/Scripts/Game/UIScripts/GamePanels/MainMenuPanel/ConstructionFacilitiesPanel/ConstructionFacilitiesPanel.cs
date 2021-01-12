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
        private Image m_Res1;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3;
        [SerializeField]
        private Text m_Res3Value;


        [SerializeField]
		private Image m_FacilityPhotoImg;

		[SerializeField]
		private Button m_AcceptBtn;
		[SerializeField]
		private Button m_CloseBtn;

        private FacilityType m_FacilityType;
        private int m_SubId;

        private List<CostItem> m_CostItems;
        private FacilityConfigInfo m_CurFacilityConfigInfo = null;
        private FacilityLevelInfo m_FacilityLevelInfo = null;

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
            m_FacilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, 1);
            m_TitleTxt.text = m_CurFacilityConfigInfo.name;
            m_FacilityDescribe.text = m_CurFacilityConfigInfo.desc;
            m_CostItems = m_FacilityLevelInfo.GetUpgradeResCosts();
            m_ConstructionConditionValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + CommonUIMethod.GetGrade(m_FacilityLevelInfo.upgradeNeedLobbyLevel);
            // m_ConstructionConditionValue.text = Define.LECTURE_HALL + m_CurFacilityConfigInfo.GetNeedLobbyLevel() + Define.LEVEL;
            //m_CoinValue.text = m_CurFacilityConfigInfo.GetUnlockCoinCost().ToString();
            RefreshResInfo();
        }

        private void RefreshResInfo()
        {
            if (m_CostItems.Count==0)
            {
                m_Res1.gameObject.SetActive(false);
                m_Res2.gameObject.SetActive(false);
                m_Res3.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 1)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1.sprite = FindSprite(RawMaterial.cyanrock.ToString());
                m_Res2Value.text = m_FacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res2.sprite = FindSprite("Coin");
                m_Res1.gameObject.SetActive(true);
                m_Res2.gameObject.SetActive(true);
                m_Res3.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {

                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1.sprite = FindSprite(RawMaterial.cyanrock.ToString());
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2.sprite = FindSprite(RawMaterial.silverwood.ToString());
                m_Res3Value.text = m_FacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res3.sprite = FindSprite("Coin");
                m_Res1.gameObject.SetActive(true);
                m_Res2.gameObject.SetActive(true);
                m_Res3.gameObject.SetActive(true);
            }
        }

        private void BindAddListenerEvent()
        {

			m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_AcceptBtn.onClick.AddListener(()=> 
            {
                if (!CheackIsBuild())
                {
                    FloatMessage.S.ShowMsg("未达到升级条件");
                    return;
                }

                if (GameDataMgr.S.GetGameData().playerInfoData.ReduceCoinNum(m_CurFacilityConfigInfo.GetUnlockCoinCost()))
                {
                    if (m_FacilityType == FacilityType.Lobby)
                        GameDataMgr.S.GetPlayerData().SetLobbyBuildeTime();

                    EventSystem.S.Send(EventID.OnStartUnlockFacility, m_FacilityType, m_SubId);

                    OnPanelHideComplete();
                }
            });
		}
        private bool CheackIsBuild()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_FacilityLevelInfo.GetUpgradeCondition() <= lobbyLevel && CheckPropIsEnough())
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

            return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_FacilityLevelInfo.upgradeCoinCost);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }

}