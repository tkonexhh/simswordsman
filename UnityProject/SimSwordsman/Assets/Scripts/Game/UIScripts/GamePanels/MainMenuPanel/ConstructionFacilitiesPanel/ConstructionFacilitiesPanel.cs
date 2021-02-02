using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{

	public class ConstructionFacilitiesPanel : AbstractAnimPanel
	{
	    [SerializeField]
	    private Image m_TitleImg;
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

            Sprite facilityImg = GetFacilitySprite(m_FacilityType);
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
            m_TitleImg.sprite = GetTitleSprite(m_FacilityType);
            m_TitleImg.SetNativeSize();
            m_FacilityDescribe.text = m_CurFacilityConfigInfo.desc;
            m_CostItems = m_FacilityLevelInfo.GetUpgradeResCosts();
            m_ConstructionConditionValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + CommonUIMethod.GetGrade(m_FacilityLevelInfo.upgradeNeedLobbyLevel);
            // m_ConstructionConditionValue.text = Define.LECTURE_HALL + m_CurFacilityConfigInfo.GetNeedLobbyLevel() + Define.LEVEL;
            //m_CoinValue.text = m_CurFacilityConfigInfo.GetUnlockCoinCost().ToString();
            RefreshResInfo();
        }

        Sprite GetTitleSprite(FacilityType type)
        {
            string spritename = "";
            switch (type)
            {
                case FacilityType.Lobby:
                    spritename = "lobby_title";
                    break;
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                    spritename = "livableroomeast_title";
                    break;
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    spritename = "livableroomwest_title";
                    break;
                case FacilityType.Warehouse:
                    spritename = "warehouse_title";
                    break;
                case FacilityType.PracticeFieldEast:
                    spritename = "practicefieldeast_title";
                    break;
                case FacilityType.PracticeFieldWest:
                    spritename = "practicefieldwest_title";
                    break;
                case FacilityType.KongfuLibrary:
                    spritename = "kongfulibrary_title";
                    break;
                case FacilityType.Kitchen:
                    spritename = "kitchen_title";
                    break;
                case FacilityType.ForgeHouse:
                    spritename = "forgehouse_title";
                    break;
                case FacilityType.Baicaohu:
                    spritename = "baicaohu_title";
                    break;
                case FacilityType.PatrolRoom:
                    spritename = "patrolroom_title";
                    break;
            }
            return FindSprite(spritename);
        }

        Sprite GetFacilitySprite(FacilityType type)
        {
            string spritename = "";
            switch (type)
            {
                case FacilityType.Lobby:
                case FacilityType.Warehouse:
                case FacilityType.KongfuLibrary:
                case FacilityType.Kitchen:
                case FacilityType.ForgeHouse:
                case FacilityType.Baicaohu:
                case FacilityType.PatrolRoom:
                    spritename = type.ToString() + 1;
                    break;
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    spritename = "LivableRoom1";
                    break;
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    spritename = "PracticeField1";
                    break;
            }
            return FindSprite(spritename);
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
                m_Res1.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_FacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res2.sprite = FindSprite("Coin");
                m_Res1.gameObject.SetActive(true);
                m_Res2.gameObject.SetActive(true);
                m_Res3.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {
                m_Res1Value.text = m_CostItems[0].value.ToString();
                m_Res1.sprite = FindSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Value.text = m_CostItems[1].value.ToString();
                m_Res2.sprite = FindSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Value.text = m_FacilityLevelInfo.upgradeCoinCost.ToString();
                m_Res3.sprite = FindSprite("Coin");
                m_Res1.gameObject.SetActive(true);
                m_Res2.gameObject.SetActive(true);
                m_Res3.gameObject.SetActive(true);
            }
        }
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
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

                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);

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