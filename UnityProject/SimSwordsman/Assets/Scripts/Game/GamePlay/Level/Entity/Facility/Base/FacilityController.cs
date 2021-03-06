using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
    public class FacilityController : IEntityController
    {
        protected FacilityView m_View = null;
        protected FacilityModel m_Model = null;

        protected FacilityType m_FacilityType;
        protected FacilityState m_FacilityState;
        protected int m_SubId; // TODO: delete

        private bool UpGradeRedPoint = false;
        private bool SubRedPoint = false;

        public FacilityType facilityType => m_FacilityType;
        public FacilityView view => m_View;

        #region IEntityController
        public virtual void Init()
        {

        }

        public virtual void Release()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void Update()
        {
        }
        #endregion

        private List<FacilityType> m_HavaWorkingBubbleFacility = new List<FacilityType>();
        ~FacilityController()
        {
            EventSystem.S.UnRegister(EventID.OnRawMaterialChangeEvent, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSendWorkingBubbleFacility, HandleAddListenerEvent);
        }
        public FacilityController(FacilityType facilityType, FacilityView view)
        {
            EventSystem.S.Register(EventID.OnRawMaterialChangeEvent, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendWorkingBubbleFacility, HandleAddListenerEvent);

            m_FacilityType = facilityType;

            m_View = view;
            m_View.SetController(this);

            FacilityItemDbData dbItem = GameDataMgr.S.GetClanData().GetFacilityItem(m_FacilityType);
            m_Model = new FacilityModel(this, dbItem);

            SetState(dbItem.facilityState, true);

            if (m_FacilityState == FacilityState.Unlocked)
            {
                m_View.SetViewByLevel(true);
            }
        }
        public void RefreshExclamatoryMark(bool active)
        {
            m_View.SetTips(active);
        }

        public void BuildClosedRedDot(bool active)
        {
            m_View.SetTips(active);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRawMaterialChangeEvent:
                    if (m_FacilityType == FacilityType.BulletinBoard)
                        break;
                    CheckUpgradeConditions();
                    break;
                case EventID.OnSendWorkingBubbleFacility:
                    if ((FacilityType)param[0] == m_FacilityType)
                    {
                        if ((bool)param[1] && !m_HavaWorkingBubbleFacility.Contains((FacilityType)param[0]))
                            m_HavaWorkingBubbleFacility.Add((FacilityType)param[0]);
                        else if (!(bool)param[1] && m_HavaWorkingBubbleFacility.Contains((FacilityType)param[0]))
                            m_HavaWorkingBubbleFacility.Remove((FacilityType)param[0]);
                        CheckUpgradeConditions();
                    }
                    break;
            }
        }

        private void CheckUpgradeConditions()
        {
            int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_FacilityType);
            FacilityLevelInfo facilityLevelInfo = null;
            switch (m_FacilityState)
            {
                case FacilityState.Locked:
                    break;
                case FacilityState.ReadyToUnlock:
                    facilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, curLevel);
                    break;
                case FacilityState.Unlocked:
                    facilityLevelInfo = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(m_FacilityType, curLevel + 1);
                    break;
                default:
                    break;
            }

            List<CostItem> costItems = null;

            if (m_HavaWorkingBubbleFacility.Contains(m_FacilityType))
            {
                if (m_FacilityType == FacilityType.Lobby)
                    EventSystem.S.Send(EventID.OnSendRecruitable, false);
                UpGradeRedPoint = false;
                SubRedPoint = false;
                RefreshRedPoint();
                return;
            }

            if (facilityLevelInfo != null)
                costItems = facilityLevelInfo.GetUpgradeResCosts();
            if (facilityLevelInfo == null || costItems == null)
                UpGradeRedPoint = false;
            else
            {
                if (CheckIsBuild(facilityLevelInfo, costItems) && CheckEnoughDiscipleLevel(facilityLevelInfo))
                {
                    UpGradeRedPoint = true;
                }
                else
                {
                    UpGradeRedPoint = false;
                }
            }

            SubRedPoint = CheckSubFunc();
            RefreshRedPoint();
        }

        private bool CheckEnoughDiscipleLevel(FacilityLevelInfo facilityLevelInfo)
        {
            if (m_FacilityType == FacilityType.Lobby)
            {
                LobbyLevelInfo lobbyLevelInfo =(LobbyLevelInfo)facilityLevelInfo;
                return CommonMethod.CheckEnoughDiscipleLevel(lobbyLevelInfo);
            }
            else
                return true;
        }

        private void RefreshRedPoint()
        {
            if (m_FacilityType == FacilityType.Lobby)
            {
                if (!SubRedPoint && UpGradeRedPoint)
                    m_View.SetTips(true);
                else
                    m_View.SetTips(false);
            }
            else
            {
                if (SubRedPoint || UpGradeRedPoint)
                    m_View.SetTips(true);
                else
                    m_View.SetTips(false);
            }
        }

        /// <summary>
        /// ???????????
        /// </summary>
        protected virtual bool CheckSubFunc()
        {
            SubRedPoint = false;
            return SubRedPoint;
        }

        private bool CheckIsBuild(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems)
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            if (facilityLevelInfo.GetUpgradeCondition() > lobbyLevel)
                return false;

            if (CheckPropIsEnough(facilityLevelInfo, costItems))
                return true;
            return false;
        }
        private bool CheckPropIsEnough(FacilityLevelInfo facilityLevelInfo, List<CostItem> costItems)
        {
            for (int i = 0; i < costItems.Count; i++)
            {
                bool isHave = MainGameMgr.S.InventoryMgr.CheckItemInInventory((RawMaterial)costItems[i].itemId, costItems[i].value);
                if (!isHave)
                    return false;
            }
            bool isHaveCoin = GameDataMgr.S.GetPlayerData().CheckHaveCoin(facilityLevelInfo.upgradeCoinCost);
            if (isHaveCoin)
                return true;
            else
                return false;
        }
        public virtual void SetState(FacilityState facilityState, bool isFile = false)
        {
            m_FacilityState = facilityState;

            m_View?.SetViewByState(isFile);
        }

        public void OnUpgrade()
        {
            m_View.SetViewByLevel();
        }

        public FacilityState GetState()
        {
            return m_FacilityState;
        }

        public FacilityType GetFacilityType()
        {
            return m_FacilityType;
        }

        public int GetSubId()
        {
            return m_SubId;
        }

        public Vector3 GetDoorPos()
        {
            return m_View.GetDoorPos();
        }

        #region work system ???
        private FacilityWorkingStateEnum m_FacilityWorkingState = FacilityWorkingStateEnum.Idle;
        public FacilityWorkingStateEnum FacilityWorkingState { get { return m_FacilityWorkingState; } }
        private int m_AutoStartWorkTimerID = -1;
        public bool IsIdleState()
        {
            return FacilityWorkingState == FacilityWorkingStateEnum.Idle;
        }
        public bool IsWorking()
        {
            return FacilityWorkingState == FacilityWorkingStateEnum.Working;
        }
        public bool IsShowBubble()
        {
            return FacilityWorkingState == FacilityWorkingStateEnum.Bubble;
        }
        public void ChangeFacilityWorkingState(FacilityWorkingStateEnum state)
        {
            this.m_FacilityWorkingState = state;

            if (IsShowBubble() == false)
            {
                Timer.S.Cancel(m_AutoStartWorkTimerID);
            }
        }
        /// <summary>
        /// ?????????????
        /// </summary>
        /// <returns></returns>
        public bool DispatchDiscipleStartWork(bool isAuto = false)
        {
            if (IsShowBubble())
            {
                CharacterMgr characterMgr = MainGameMgr.S.CharacterMgr;

                List<CharacterController> characterControllerList = characterMgr.CharacterControllerList;

                characterControllerList = characterControllerList.Where(x => x.CurState == CharacterStateID.Wander).ToList();

                if (characterControllerList != null && characterControllerList.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, characterControllerList.Count);

                    CharacterController m_WorkingCharacterController = characterControllerList[index];

                    TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());

                    GameDataMgr.S.GetClanData().SetWorkData(GetFacilityType(), m_WorkingCharacterController.CharacterId, lobbyData.workTime);

                    ChangeFacilityWorkingState(FacilityWorkingStateEnum.Working);

                    m_WorkingCharacterController.SetState(CharacterStateID.Working, GetFacilityType());

                    return true;
                }
                else
                {
                    if (!isAuto)
                        FloatMessage.S.ShowMsg("??????????????????");
                    return false;
                }
            }
            else
            {
                //Debug.LogError("is not show bubble state:");

                Timer.S.Cancel(m_AutoStartWorkTimerID);
                return false;
            }
        }
        /// <summary>
        /// ??????????
        /// </summary>
        /// <param name="characterID"></param>
        public void DispatchDiscipleStartWork(int characterID)
        {
            CharacterController controller = MainGameMgr.S.CharacterMgr.GetCharacterController(characterID);

            if (controller != null)
            {
                TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());

                GameDataMgr.S.GetClanData().SetWorkData(GetFacilityType(), controller.CharacterId, lobbyData.workTime);

                ChangeFacilityWorkingState(FacilityWorkingStateEnum.Working);

                controller.SetState(CharacterStateID.Working, GetFacilityType());
            }
            else
            {
                Debug.LogError("dispatch disciple is null");
            }
        }
        /// <summary>
        /// ???????????????
        /// </summary>
        public void CoundDownAutoStartWork(Action<bool> endCallBack = null, bool isAuto = false)
        {
            Timer.S.Cancel(m_AutoStartWorkTimerID);

            TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());

            int autoWorkWaitTime = lobbyData.autoWorkWait;

            m_AutoStartWorkTimerID = Timer.S.Post2Really((x) =>
            {
                if (!(KongfuLibraryPanel.isOpened || PracticeFieldPanel.isOpened))
                {
                    if ((DateTime.Now - GameplayMgr.resumeTime).TotalSeconds > 5)
                    {
                        if (endCallBack != null)
                        {
                            endCallBack(isAuto);
                        }
                        m_AutoStartWorkTimerID = -1;
                    }
                }
            }, autoWorkWaitTime, -1);
        }
        #endregion
    }
}