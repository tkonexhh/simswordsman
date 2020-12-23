using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{

    public enum LivableRoomState
    {
        NotBuilt,
        Built,
        FullLevel,
    }
    public class LivableRoomItem : MonoBehaviour, ItemICom
    {

        [SerializeField]
        private Text m_LivableRoomName;
        [SerializeField]
        private Text m_LivableRoomLevel;
        [SerializeField]
        private Text m_HabitablePopulationValue;
        [SerializeField]
        private Text m_NextHabitablePopulationValue;
        [SerializeField]
        private Text m_UpgradeConditions;
        [SerializeField]
        private Text m_UpgradeResourcesValue;
        [SerializeField]
        private Text m_HabitablePopulationTitle;
        [SerializeField]
        private Text m_NextHabitablePopulation;
        [SerializeField]
        private Text m_UpgradeResourcesTitle;
        [SerializeField]
        private Text m_UpgradeBtnText;

        [SerializeField]
        private Image m_LivableRoomImg;
        [SerializeField]
        private Button m_UpgradeBtn;

        private LivableRoomLevelInfo m_LivableRoomLevelInfo = null;
        private FacilityConfigInfo m_FacilityConfigInfo = null;
        private FacilityType m_CurFacilityType;
        private int m_SubID;
        private int m_CurLevel;
        private string m_CurLivableRoomName = string.Empty;
        private LivableRoomState m_LivableRoomState;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurFacilityType = (FacilityType)obj[0];
            m_SubID = (int)obj[1];
            BindAddListenerEvent();

            GetInformationForNeed();
            RefreshPanelInfo();
        }

        private void BindAddListenerEvent()
        {
            m_UpgradeBtn.onClick.AddListener(() =>
            {
                switch (m_LivableRoomState)
                {
                    case LivableRoomState.NotBuilt:
                        m_LivableRoomState = LivableRoomState.Built;
                        MainGameMgr.S.FacilityMgr.SetFacilityState(m_CurFacilityType, FacilityState.State1/*, m_SubID*/);
                        break;
                    case LivableRoomState.Built:
                        int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
                        if (m_LivableRoomLevelInfo.GetUpgradeCondition() > lobbyLevel)
                            return;

                        bool isReduceSuccess = GameDataMgr.S.GetPlayerData().ReduceCoinNum(double.Parse(m_UpgradeResourcesValue.text));

                        if (isReduceSuccess)
                        {
                            EventSystem.S.Send(EventID.OnStartUpgradeFacility, m_CurFacilityType, m_SubID, 1);
                            GetInformationForNeed();
                        }
                        break;
                    case LivableRoomState.FullLevel:
                        break;
                    default:
                        break;
                }
                RefreshPanelInfo();
            });
        }

        private void GetInformationForNeed()
        {

            m_FacilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType/*, m_SubID*/);
            m_LivableRoomLevelInfo = (LivableRoomLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_CurFacilityType, m_CurLevel);
            m_CurLivableRoomName = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType).name;

            if (m_CurLevel == Define.FACILITY_MAX_LEVEL)
            {
                m_LivableRoomState = LivableRoomState.FullLevel;
                return;
            }

            FacilityState facilityState = GameDataMgr.S.GetClanData().GetFacilityData(m_CurFacilityType/*, m_SubID*/).facilityState;

            if (facilityState == FacilityState.ReadyToUnlock)
            {
                m_LivableRoomState = LivableRoomState.NotBuilt;
            }
            else
            {
                m_LivableRoomState = LivableRoomState.Built;
            }

        }

        private void RefreshPanelText()
        {
        }

        public string GetLivableRoomName(int m_SubID)
        {
            switch (m_SubID)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                default:
                    return null;
            }
        }

        private void RefreshPanelInfo()
        {
            m_UpgradeBtnText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
            RefreshPanelText();

            m_LivableRoomName.text = m_CurLivableRoomName + GetLivableRoomName(m_SubID);
            m_LivableRoomLevel.text = m_LivableRoomLevelInfo.level.ToString();

            switch (m_LivableRoomState)
            {
                case LivableRoomState.NotBuilt:
                    m_HabitablePopulationTitle.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTBUILD);
                    m_HabitablePopulationValue.text = Define.COMMON_DEFAULT_STR;
                    m_NextHabitablePopulationValue.text = Define.COMMON_DEFAULT_STR;
                    m_NextHabitablePopulation.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDINFODESC) + m_FacilityConfigInfo.GetNeedLobbyLevel() + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE);
                    m_UpgradeResourcesTitle.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILDRESOURCES);
                    m_UpgradeResourcesValue.text = m_FacilityConfigInfo.GetUnlockCoinCost().ToString();
                    m_UpgradeBtnText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_BUILD);
                    break;
                case LivableRoomState.Built:
                    
                    m_HabitablePopulationTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
                    m_HabitablePopulationValue.text = m_LivableRoomLevelInfo.GetCurCapacity()+ CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_PEOPLE);
                    m_NextHabitablePopulation.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_NEXTHABITABLE);
                    m_NextHabitablePopulationValue.text = m_LivableRoomLevelInfo.GetNextCapacity() + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE);
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADEINFODESC) + m_LivableRoomLevelInfo.GetUpgradeCondition() + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE);
                    m_UpgradeResourcesTitle.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADERESOURCES);
                    m_UpgradeResourcesValue.text = m_LivableRoomLevelInfo.upgradeResCosts.ToString();
                    m_UpgradeBtnText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_UPGRADE);
                    break;
                case LivableRoomState.FullLevel:
                    m_HabitablePopulationTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_CURRENTLYHABITABLE);
                    m_HabitablePopulationValue.text = m_LivableRoomLevelInfo.GetCurCapacity() + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_PEOPLE);
                    m_NextHabitablePopulationValue.text = Define.COMMON_DEFAULT_STR;
                    m_NextHabitablePopulation.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeResourcesTitle.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeResourcesValue.text = Define.COMMON_DEFAULT_STR;
                    m_UpgradeConditions.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_FULLLEVEL);
                    m_UpgradeBtn.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }
    }
}