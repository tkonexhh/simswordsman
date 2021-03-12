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
        private Button m_BlackBtn;
        [SerializeField]
        private Image m_TitleImg;
        [SerializeField]
        private Text m_FacilityDescribe;
        [SerializeField]
        private Text m_ConstructionConditionValue;
        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;


        [SerializeField]
        private Image m_FacilityPhotoImg;

        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_CloseBtn;   
        [SerializeField]
        private GameObject m_RedPoint;

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
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
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

        }

        private void RefreshPanelInfo()
        {
            m_CurFacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_FacilityType);
            m_FacilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, 1);
            m_TitleImg.sprite = GetTitleSprite(m_FacilityType);
            m_TitleImg.SetNativeSize();
            m_FacilityDescribe.text = CommonUIMethod.TextEmptyOne() + m_CurFacilityConfigInfo.desc;
            m_CostItems = m_FacilityLevelInfo.GetUpgradeResCosts();
            m_ConstructionConditionValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + CommonUIMethod.GetStrForColor("#7B3735", CommonUIMethod.GetGrade(m_FacilityLevelInfo.upgradeNeedLobbyLevel));
            RefreshResInfo();
            if (CommonUIMethod.CheackIsBuild(m_FacilityLevelInfo, m_CostItems,false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
        }

        Sprite GetTitleSprite(FacilityType type)
        {
            string spritename = "";
            switch (type)
            {
                case FacilityType.Lobby:
                    spritename = "LobbyPanel_lobby_title";
                    break;
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                    spritename = "LivableRoomPanel_EastName";
                    break;
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    spritename = "LivableRoomPanel_WeatName";
                    break;
                case FacilityType.Warehouse:
                    spritename = "WareHouse_title";
                    break;
                case FacilityType.PracticeFieldEast:
                    spritename = "PracticeFieldEast";
                    break;
                case FacilityType.PracticeFieldWest:
                    spritename = "PracticeFieldWest";
                    break;
                case FacilityType.KongfuLibrary:
                    spritename = "kongfulibrary_title";
                    break;
                case FacilityType.Kitchen:
                    spritename = "kitchen_title";
                    break;
                case FacilityType.ForgeHouse:
                    spritename = "forgehourse_title";
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
            CommonUIMethod.RefreshUpgradeResInfo(m_CostItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, m_FacilityLevelInfo,this);
        }
        private void BindAddListenerEvent()
        {

            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_BlackBtn.onClick.AddListener(() =>
            {
                HideSelfWithAnim();
            });
            m_AcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (!CommonUIMethod.CheackIsBuild(m_FacilityLevelInfo, m_CostItems))
                    return;

                if (GameDataMgr.S.GetGameData().playerInfoData.ReduceCoinNum(m_CurFacilityConfigInfo.GetUnlockCoinCost()))
                {
                    //#region 不开新手引导时需要存在
                    //if (m_FacilityType == FacilityType.Lobby)
                    //    GameDataMgr.S.GetPlayerData().SetLobbyBuildeTime();
                    //#endregion
                    for (int i = 0; i < m_CostItems.Count; i++)
                        MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);

                    FacilityController facilityController =  MainGameMgr.S.FacilityMgr.GetFacilityController(m_FacilityType);
                    facilityController.BuildClosedRedDot(false);
                    EventSystem.S.Send(EventID.OnStartUnlockFacility, m_FacilityType);
                    EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
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