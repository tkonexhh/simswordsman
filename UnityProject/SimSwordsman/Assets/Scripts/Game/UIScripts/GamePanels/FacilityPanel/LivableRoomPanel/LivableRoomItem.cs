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
        [Header("DividingLine")]
        [Header("MiddleBottom")]
        [SerializeField]
        private Text m_FullScale;
        [SerializeField]
        private Text m_UpgradeConditions;
        [Header("Bottom")]
        [SerializeField]
        private Image m_Res3;
        [SerializeField]
        private Text m_Res3Consume;
        [SerializeField]
        private Text m_Res1Consume;
        [SerializeField]
        private Image m_Res1;
        [SerializeField]
        private Text m_Res2Consume;
        [SerializeField]
        private Image m_Res2;
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

        private LivableRoomLevelInfo m_LivableRoomLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private string m_CurLivableRoomName = string.Empty;
        private LivableRoomState m_LivableRoomState;
        private List<Sprite> m_Sprites;
        private List<CostItem> m_CostItems;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            m_CurFacilityType = (FacilityType)obj[0];
            m_Sprites = (List<Sprite>)obj[1];
            GetInformationForNeed();
            RefreshPanelText();
            RefreshPanelInfo();
        }

        private Sprite GetSprite(string name)
        { 
            return m_Sprites.Where(i => i.name.Equals(name)).FirstOrDefault();
        }

        private void BindAddListenerEvent()
        {
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                if (!CheackIsBuild())
                {
                    FloatMessage.S.ShowMsg("未达到升级条件");
                    return;
                }

                switch (m_LivableRoomState)
                {
                    case LivableRoomState.ReadyBuilt:
                        m_LivableRoomState = LivableRoomState.Upgrade;
                        MainGameMgr.S.FacilityMgr.SetFacilityState(m_CurFacilityType, FacilityState.Unlocked/*, m_SubID*/);
                        break;
                    case LivableRoomState.Upgrade:
                        bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(m_LivableRoomLevelInfo.upgradeCoinCost);
                        if (isReduceSuccess)
                        {
                            EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, 1, 1);
                            GetInformationForNeed();
                        }
                        break;
                    case LivableRoomState.FullLevel:
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < m_CostItems.Count; i++)
                    MainGameMgr.S.InventoryMgr.RemoveItem(new PropItem((RawMaterial)m_CostItems[i].itemId), m_CostItems[i].value);

                RefreshPanelInfo();
            });
        }

        private bool CheackIsBuild()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (m_LivableRoomLevelInfo.GetUpgradeCondition() <= lobbyLevel && CheckPropIsEnough())
                return true;
            return false;
        }

        private bool CheckPropIsEnough()
        {
            for (int i = 0; i < m_CostItems.Count; i++)
            {
              bool isHave =  MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)m_CostItems[i].itemId, m_CostItems[i].value);
                if (!isHave)
                    return false;
            }

           return GameDataMgr.S.GetPlayerData().CheckHaveCoin(m_LivableRoomLevelInfo.upgradeCoinCost);
        }

        private void GetInformationForNeed()
        {

            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType/*, m_SubID*/);
            m_LivableRoomLevelInfo = (LivableRoomLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel+1);
            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CostItems = m_LivableRoomLevelInfo.GetUpgradeResCosts();
            if (m_CurLevel == Define.FACILITY_MAX_LIVABLEROOM)
            {
                m_LivableRoomState = LivableRoomState.FullLevel;
                return;
            }

            FacilityState facilityState = GameDataMgr.S.GetClanData().GetFacilityData(m_CurFacilityType/*, m_SubID*/).facilityState;

            if (facilityState == FacilityState.ReadyToUnlock || facilityState== FacilityState.Locked)
                m_LivableRoomState = LivableRoomState.ReadyBuilt;
            else
                m_LivableRoomState = LivableRoomState.Upgrade;

        }

        private void RefreshPanelText()
        {
            m_LivableRoomName.text = m_FacilityConfigInfo.name;
            m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
            m_NextPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_NEXTHABITABLE);
        }

        private void RefreshPanelInfo()
        {
            switch (m_LivableRoomState)
            {
                case LivableRoomState.ReadyBuilt:
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_CurLevel);
                    m_UpperMiddle.SetActive(false);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTBUILD);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    m_CurPeopleValue.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_LivableRoomLevelInfo.GetNeedLobbyLevel()));
                    RefreshResInfo();
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILD);

                    if (!CheackIsBuild())
                    {
                        m_UpgradeBtnImg.sprite = GetSprite("BgBtn3");
                        m_UpgradeBtn.interactable = false;
                    }
                    else
                    {
                        m_UpgradeBtnImg.sprite = GetSprite("BgBtn1");
                        m_UpgradeBtn.interactable = true;
                    }
                    break;
                case LivableRoomState.Upgrade:
                    m_LivableRoomLevel.text = CommonUIMethod.GetGrade(m_LivableRoomLevelInfo.level);
                    m_CurPeopleCount.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_LivableRoomLevelInfo.GetCurCapacity()));
                    m_NextPeopleValue.text = CommonUIMethod.GetStrForColor("#AD7834", CommonUIMethod.GetPeople(m_LivableRoomLevelInfo.GetNextCapacity()));
                    m_UpperMiddle.SetActive(true);
                    m_FullScale.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + Define.SPACE
                        + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetGrade(m_LivableRoomLevelInfo.GetNeedLobbyLevel()));
                    RefreshResInfo();
                    m_UpgradeBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
                    m_UpgradeBtnImg.sprite= GetSprite("BgBtn2");
                    break;
                case LivableRoomState.FullLevel:
                    m_LivableRoomLevel.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_FULLLEVEL);
                    m_CurPeopleValue.text = CommonUIMethod.GetStrForColor("#365387", CommonUIMethod.GetPeople(m_LivableRoomLevelInfo.GetCurCapacity()));
                    m_UpperMiddle.SetActive(false);
                    m_FullScale.text = CommonUIMethod.GetStrForColor("#AD7834", Define.COMMON_FULLEDLEVEL);
                    m_UpgradeConditions.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeBtn.gameObject.SetActive(false);
                    m_Res3.gameObject.SetActive(false);
                    m_Res1.gameObject.SetActive(false);
                    m_Res2.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

        }

        private void RefreshResInfo()
        {

            if (m_CostItems.Count==1)
            {
                m_Res1Consume.text = m_CostItems[0].value.ToString();
                m_Res1.sprite = GetSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Consume.text = m_LivableRoomLevelInfo.upgradeCoinCost.ToString();
                m_Res2.sprite = GetSprite("Coin");
                m_Res3.gameObject.SetActive(false);
            }
            else if (m_CostItems.Count == 2)
            {

                m_Res1Consume.text = m_CostItems[0].value.ToString();
                m_Res1.sprite = GetSprite(GetIconName(m_CostItems[0].itemId));
                m_Res2Consume.text = m_CostItems[1].value.ToString();
                m_Res2.sprite = GetSprite(GetIconName(m_CostItems[1].itemId));
                m_Res3Consume.text = m_LivableRoomLevelInfo.upgradeCoinCost.ToString();
                m_Res3.sprite = GetSprite("Coin");
                m_Res3.gameObject.SetActive(true);
            }
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
