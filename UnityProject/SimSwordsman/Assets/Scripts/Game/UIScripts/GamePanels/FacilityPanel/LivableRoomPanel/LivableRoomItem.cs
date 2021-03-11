using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{

    public enum LivableRoomState
    {
        /// <summary>
        /// 准备建造
        /// </summary>
        ReadyBuilt,
        /// <summary>
        /// 升级
        /// </summary>
        Upgrade,
        /// <summary>
        /// 满级
        /// </summary>
        FullLevel,
    }
    public class LivableRoomItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Image m_LivableRoomImg;
        [SerializeField]
        private Image m_LivableRoomImgLock;
        [Header("DividingLine")]
        [Header("MiddleBottom")]
        [SerializeField]
        private Text m_FullScale;
        [SerializeField]
        private Text m_UpgradeConditions;
        [Header("Bottom")]
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Text m_Res3Value;
        [SerializeField]
        private Text m_Res1Value;
        [SerializeField]
        private Image m_Res1Img;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Button m_UpgradeBtn;
        [SerializeField]
        private Text m_UpgradeBtnValue;
        [SerializeField]
        private Image m_UpgradeBtnImg;
        [Header("Upper")]
        [SerializeField]
        private Text m_LivableRoomName;
        [SerializeField]
        private Text m_LivableRoomLevel;
        [Header("UpperMiddle")]
        [SerializeField]
        private Text m_CurPeopleCount;
        [SerializeField]
        private Text m_CurPeopleValue;
        [Header("UpperMiddle")]
        [SerializeField]
        private Text m_NextPeopleCount;
        [SerializeField]
        private Text m_NextPeopleValue;
        [SerializeField]
        private GameObject m_UpperMiddle; 
        [SerializeField]
        private GameObject m_RedPoint;

        private LivableRoomLevelInfo m_CurLivableRoomLevelInfo = null;
        private LivableRoomLevelInfo m_NextLivableRoomLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private string m_CurLivableRoomName = string.Empty;
        private LivableRoomState m_LivableRoomState;
        private List<CostItem> m_NextCostItems;
        private List<CostItem> m_CurCostItems;

        private AbstractAnimPanel m_ParentPanel;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            EventSystem.S.Register(EventID.OnUpgradeRefreshEvent,HandAddListenerEvent);

            BindAddListenerEvent();
            m_ParentPanel = t as AbstractAnimPanel;
            m_CurFacilityType = (FacilityType)obj[0];
            GetInformationForNeed();
            RefreshPanelText();
            RefreshPanelInfo();
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnUpgradeRefreshEvent:
                    RefreshPanelInfo();
                    break;
                default:
                    break;
            }
        }
        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnUpgradeRefreshEvent,HandAddListenerEvent);

        }

        private Sprite FindSprite(string name)
        {
            return m_ParentPanel.FindSprite(name);
        }

        private void BindAddListenerEvent()
        {
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (!CommonUIMethod.CheackIsBuild(m_NextLivableRoomLevelInfo, m_NextCostItems))
                    return;

                switch (m_LivableRoomState)
                {
                    case LivableRoomState.ReadyBuilt:
                        m_LivableRoomState = LivableRoomState.Upgrade;
                        MainGameMgr.S.FacilityMgr.SetFacilityState(m_CurFacilityType, FacilityState.Unlocked/*, m_SubID*/);
                        ReduceItem(m_CurLivableRoomLevelInfo);
                        GetInformationForNeed();
                        break;
                    case LivableRoomState.Upgrade:
                        AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                        bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextLivableRoomLevelInfo.upgradeCoinCost);
                        if (isReduceSuccess)
                        {
                            ReduceItem(m_NextLivableRoomLevelInfo);
                            EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                            GetInformationForNeed();
                        }
                        break;
                    case LivableRoomState.FullLevel:
                        break;
                    default:
                        break;
                }
                //for (int i = 0; i < m_NextCostItems.Count; i++)
                //    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_NextCostItems[i].itemId), m_NextCostItems[i].value);

                EventSystem.S.Send(EventID.OnUpgradeRefreshEvent);
                EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
                UIMgr.S.ClosePanelAsUIID(UIID.LivableRoomPanel);
            });
        }

        private void ReduceItem(LivableRoomLevelInfo livableRoomLevelInfo)
        {
            List<CostItem>  costItems = livableRoomLevelInfo.GetUpgradeResCosts();
            for (int i = 0; i < costItems.Count; i++)
            {
                MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)costItems[i].itemId), costItems[i].value);
            }
           

        }

        private bool CheckPropIsEnough(bool isShow = true)
        {
            for (int i = 0; i < m_NextCostItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_NextCostItems[i].itemId, m_NextCostItems[i].value);
                if (!isHave)
                {
                    if (isShow)
                        FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_MATERIALS));
                    return false;
                }
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_CurLivableRoomLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
            {
                if (isShow)
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_COIN));
                return false;
            }
        }
        private bool CheackIsBuild(bool isShow = true)
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_CurLivableRoomLevelInfo.GetUpgradeCondition() > lobbyLevel)
            {
                if (isShow)
                    FloatMessage.S.ShowMsg(CommonUIMethod.GetStringForTableKey(Define.COMMON_POPUP_NEEDLOBBY));
                return false;
            }

            if (CheckPropIsEnough(isShow))
                return true;
            return false;
        }

        private void GetInformationForNeed()
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType/*, m_SubID*/);
            int maxLevel = MainGameMgr.S.FacilityMgr.GetFacilityMaxLevel(m_CurFacilityType);
            m_CurLivableRoomLevelInfo = (LivableRoomLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            FacilityState facilityState = GameDataMgr.S.GetClanData().GetFacilityData(m_CurFacilityType/*, m_SubID*/).facilityState;

            if (facilityState == FacilityState.ReadyToUnlock || facilityState == FacilityState.Locked)
                m_LivableRoomState = LivableRoomState.ReadyBuilt;
            else
                m_LivableRoomState = LivableRoomState.Upgrade;
            if (m_CurLevel == maxLevel)
            {
                m_UpgradeBtn.gameObject.SetActive(false);
                m_LivableRoomState = LivableRoomState.FullLevel;
                //m_UnlockContentValue.text = "无";
            }
            else
            {
                m_NextLivableRoomLevelInfo = (LivableRoomLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel + 1);
                m_NextCostItems = m_NextLivableRoomLevelInfo.GetUpgradeResCosts();
            }
        }

        private void RefreshPanelText()
        {
            m_LivableRoomName.text = m_FacilityConfigInfo.name;
            m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
            m_NextPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_NEXTHABITABLE);
        }

        private List<CostItem> GetCostItem(LivableRoomLevelInfo livableRoomLevel)
        {
            if (livableRoomLevel != null)
                return livableRoomLevel.GetUpgradeResCosts();
            else
            {
                Log.w("livableRoomLevel id is = "+ livableRoomLevel.roomId);
                return null;
            }
        }    

        private void RefreshPanelInfo()
        {
            if (CommonUIMethod.CheackIsBuild(m_NextLivableRoomLevelInfo, m_NextCostItems, false))
                m_RedPoint.SetActive(true);
            else
                m_RedPoint.SetActive(false);
        
            switch (m_LivableRoomState)
            {
                case LivableRoomState.ReadyBuilt:
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_CurLevel);
                    m_UpperMiddle.SetActive(false);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTBUILD);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    m_CurPeopleValue.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_NextLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    RefreshResInfo(m_NextLivableRoomLevelInfo, GetCostItem(m_NextLivableRoomLevelInfo));
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILD);

                    if (!CommonUIMethod.CheackIsBuild(m_NextLivableRoomLevelInfo, m_NextCostItems, false))
                    {
                        m_UpgradeBtnImg.sprite = FindSprite("LivableRoomPanel_BgBtn3");
                        m_UpgradeBtn.interactable = false;

                        m_LivableRoomImg.gameObject.SetActive(false);
                        m_LivableRoomImgLock.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_UpgradeBtnImg.sprite = FindSprite("LivableRoomPanel_BgBtn1");
                        m_UpgradeBtn.interactable = true;

                        m_LivableRoomImg.sprite = m_ParentPanel.FindSprite("LivableRoom" + m_CurLevel);
                        m_LivableRoomImg.gameObject.SetActive(true);
                        m_LivableRoomImgLock.gameObject.SetActive(false);
                    }
                    break;
                case LivableRoomState.Upgrade:
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_CurLivableRoomLevelInfo.level);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_NextLivableRoomLevelInfo.GetCurCapacity()));
                    m_NextPeopleValue.text = CommonUIMethod.GetStrForColor("#AD7834", CommonUIMethod.GetPeople(m_NextLivableRoomLevelInfo.GetNextCapacity()));
                    m_UpperMiddle.SetActive(true);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = "升级需要讲武堂达到" + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_NextLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    RefreshResInfo(m_NextLivableRoomLevelInfo, GetCostItem(m_NextLivableRoomLevelInfo));
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
                    m_UpgradeBtnImg.sprite= FindSprite("LivableRoomPanel_BgBtn2");

                    m_LivableRoomImg.sprite = m_ParentPanel.FindSprite("LivableRoom" + m_CurLevel);
                    m_LivableRoomImg.gameObject.SetActive(true);
                    m_LivableRoomImgLock.gameObject.SetActive(false);
                    break;
                case LivableRoomState.FullLevel:
                    m_LivableRoomLevel.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_FULLLEVEL);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_CurLivableRoomLevelInfo.GetCurCapacity()));
                    m_UpperMiddle.SetActive(false);
                    m_FullScale.text = CommonUIMethod.GetStrForColor("#AD7834", Define.COMMON_FULLEDLEVEL);
                    m_UpgradeConditions.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeBtn.gameObject.SetActive(false);
                    m_Res3Img.gameObject.SetActive(false);
                    m_Res1Img.gameObject.SetActive(false);
                    m_Res2Img.gameObject.SetActive(false);

                    m_LivableRoomImg.sprite = m_ParentPanel.FindSprite("LivableRoom" + m_CurLevel);
                    m_LivableRoomImg.gameObject.SetActive(true);
                    m_LivableRoomImgLock.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

        }

        private void RefreshResInfo(LivableRoomLevelInfo livableRoomLevel, List<CostItem> costItems)
        {
            CommonUIMethod.RefreshUpgradeResInfo(costItems, m_Res1Value, m_Res1Img, m_Res2Value, m_Res2Img, m_Res3Value, m_Res3Img, livableRoomLevel, m_ParentPanel);

            //if (costItems == null)
            //    return;

            //if (costItems.Count == 1)
            //{
            //    int havaItem = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);
            //    m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItem, costItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[0].value);
            //    m_Res1Img.sprite = FindSprite(GetIconName(costItems[0].itemId));
            //    m_Res2Value.text = GetCurCoin(livableRoomLevel) + Define.SLASH + CommonUIMethod.GetTenThousand(livableRoomLevel.upgradeCoinCost);
            //    m_Res2Img.sprite = FindSprite("Coin");
            //    m_Res3Img.gameObject.SetActive(false);
            //}
            //else if (costItems.Count == 2)
            //{
            //    int havaItemFirst = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[0].itemId);
            //    int havaItemSec = MainGameMgr.S.InventoryMgr.GetRawMaterialNumberForID(costItems[1].itemId);
            //    m_Res1Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemFirst, costItems[0].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[0].value);
            //    m_Res1Img.sprite = FindSprite(GetIconName(costItems[0].itemId));
            //    m_Res2Value.text = CommonUIMethod.GetTenThousand(GetCurItem(havaItemSec, costItems[1].value)) + Define.SLASH + CommonUIMethod.GetTenThousand(costItems[1].value);
            //    m_Res2Img.sprite = FindSprite(GetIconName(costItems[1].itemId));
            //    m_Res3Value.text = GetCurCoin(livableRoomLevel) + Define.SLASH + CommonUIMethod.GetTenThousand(livableRoomLevel.upgradeCoinCost);
            //    m_Res3Img.sprite = FindSprite("Coin");
            //    m_Res3Img.gameObject.SetActive(true);
            //}
        }
        private int GetCurItem(int hava, int number)
        {
            if (hava >= number)
                return number;
            return hava;
        }
        private string GetCurCoin(LivableRoomLevelInfo livableRoomLevel)
        {
            long coin = GameDataMgr.S.GetPlayerData().GetCoinNum();
            if (coin >= livableRoomLevel.upgradeCoinCost)
                return CommonUIMethod.GetTenThousand(livableRoomLevel.upgradeCoinCost);
            return CommonUIMethod.GetTenThousand((int)coin);
        }
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }


        public void SetButtonEvent(Action<object> action)
        {
        }
    }
}
