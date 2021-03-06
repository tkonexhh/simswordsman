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
        [Header("Res")]
        [SerializeField]
        private Transform m_UpgradeResItemTra;
        [SerializeField]
        private GameObject m_UpgradeResItem;

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
            EventSystem.S.Register(EventID.OnUpgradeRefreshEvent, HandAddListenerEvent);

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
            EventSystem.S.UnRegister(EventID.OnUpgradeRefreshEvent, HandAddListenerEvent);

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
                switch (m_LivableRoomState)
                {
                    case LivableRoomState.ReadyBuilt:
                        if (!CommonUIMethod.CheackIsBuild(m_CurLivableRoomLevelInfo, m_CurCostItems))
                            return;

                        m_LivableRoomState = LivableRoomState.Upgrade;
                        MainGameMgr.S.FacilityMgr.SetFacilityState(m_CurFacilityType, FacilityState.Unlocked/*, m_SubID*/);

                        DataAnalysisMgr.S.CustomEvent(DotDefine.facility_build, m_CurFacilityType.ToString());

                        ReduceItem(m_CurLivableRoomLevelInfo);
                        GetInformationForNeed();
                        break;
                    case LivableRoomState.Upgrade:
                        if (!CommonUIMethod.CheackIsBuild(m_NextLivableRoomLevelInfo, m_NextCostItems))
                            return;

                        AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                        bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_NextLivableRoomLevelInfo.upgradeCoinCost);
                        if (isReduceSuccess)
                        {
                            ReduceItem(m_NextLivableRoomLevelInfo);
                            EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                            GetInformationForNeed();

                            DataAnalysisMgr.S.CustomEvent(DotDefine.facility_upgrade, m_CurFacilityType.ToString() + ";" + m_CurLevel);
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
            List<CostItem> costItems = livableRoomLevelInfo.GetUpgradeResCosts();
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
            m_CurCostItems = m_CurLivableRoomLevelInfo.GetUpgradeResCosts();
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
                Log.w("livableRoomLevel id is = " + livableRoomLevel.roomId);
                return null;
            }
        }

        private void RefreshPanelInfo()
        {
            switch (m_LivableRoomState)
            {
                case LivableRoomState.ReadyBuilt:
                    List<CostItem> costItemList = GetCostItem(m_CurLivableRoomLevelInfo);
                    if (CommonUIMethod.CheackIsBuild(m_CurLivableRoomLevelInfo, costItemList, false))
                        m_RedPoint.SetActive(true);
                    else
                        m_RedPoint.SetActive(false);
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_CurLevel);
                    m_UpperMiddle.SetActive(false);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTBUILD);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    m_CurPeopleValue.text = Define.COMMON_DEFAULT_STR;
                    SetUpgradeConditions(LivableRoomState.ReadyBuilt);
                    RefreshResInfo(m_CurLivableRoomLevelInfo, costItemList);
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILD);

                    if (!CommonUIMethod.CheackIsBuild(m_CurLivableRoomLevelInfo, costItemList, false))
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
                    if (CommonUIMethod.CheackIsBuild(m_NextLivableRoomLevelInfo, m_NextCostItems, false))
                        m_RedPoint.SetActive(true);
                    else
                        m_RedPoint.SetActive(false);
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_CurLivableRoomLevelInfo.level);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_CurLivableRoomLevelInfo.GetCurCapacity()));
                    m_NextPeopleValue.text = CommonUIMethod.GetStrForColor("#AD7834", CommonUIMethod.GetPeople(m_CurLivableRoomLevelInfo.GetNextCapacity()));
                    m_UpperMiddle.SetActive(true);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    SetUpgradeConditions(LivableRoomState.Upgrade);
                    //m_UpgradeConditions.text = "升级需要讲武堂达到" + Define.SPACE
                    //    + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_NextLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    RefreshResInfo(m_NextLivableRoomLevelInfo, GetCostItem(m_NextLivableRoomLevelInfo));
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
                    m_UpgradeBtnImg.sprite = FindSprite("LivableRoomPanel_BgBtn2");

                    m_LivableRoomImg.sprite = m_ParentPanel.FindSprite("LivableRoom" + m_CurLevel);
                    m_LivableRoomImg.gameObject.SetActive(true);
                    m_LivableRoomImgLock.gameObject.SetActive(false);
                    break;
                case LivableRoomState.FullLevel:
                    m_LivableRoomLevel.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_FULLLEVEL);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_CurLivableRoomLevelInfo.GetCurCapacity()));
                    m_UpperMiddle.SetActive(false);
                    m_FullScale.text = CommonUIMethod.GetStrForColor("#AD7834", Define.COMMON_FULLEDLEVEL, true);
                    m_UpgradeConditions.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeBtn.gameObject.SetActive(false);
                    m_LivableRoomImg.sprite = m_ParentPanel.FindSprite("LivableRoom" + m_CurLevel);
                    m_LivableRoomImg.gameObject.SetActive(true);
                    m_LivableRoomImgLock.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

        }

        private void SetUpgradeConditions(LivableRoomState livableRoomState)
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            switch (livableRoomState)
            {
                case LivableRoomState.ReadyBuilt:
                    if (lobbyLevel < m_CurLivableRoomLevelInfo.GetNeedLobbyLevel())
                    {
                        m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_CurLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    }
                    else
                    {
                        m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#4C6AA5", CommonUIMethod.GetGrade(m_CurLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    }
                    break;
                case LivableRoomState.Upgrade:
                    if (lobbyLevel < m_NextLivableRoomLevelInfo.GetNeedLobbyLevel())
                    {
                        m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_NextLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    }
                    else
                    {
                        m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#4C6AA5", CommonUIMethod.GetGrade(m_NextLivableRoomLevelInfo.GetNeedLobbyLevel()));
                    }
                    break;
            }
        }

        private void RefreshResInfo(LivableRoomLevelInfo livableRoomLevel, List<CostItem> costItems)
        {
            CommonUIMethod.RefreshUpgradeResInfo(costItems, m_UpgradeResItemTra, m_UpgradeResItem, livableRoomLevel);
        }
        public void SetButtonEvent(Action<object> action)
        {
        }
    }
}
