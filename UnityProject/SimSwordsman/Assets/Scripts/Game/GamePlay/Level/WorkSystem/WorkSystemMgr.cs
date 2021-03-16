using Qarth;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GameWish.Game
{
    public class WorkCharacter
    {
        public int CharacterID;
        public int FaciityTypeID;

        private FacilityType facilityType;
        public FacilityType GetFacilityType()
        {
            if (facilityType == 0)
                facilityType = (FacilityType)FaciityTypeID;
            return facilityType;
        }
    }

    public class WorkSystemMgr :TSingleton<WorkSystemMgr>
    {
        /// <summary>
        /// �����ѽ����Ľ����б�
        /// </summary>
        private List<FacilityController> m_AllUnlockFacilityList = new List<FacilityController>();
        /// <summary>
        /// �����ñ������
        /// </summary>
        private TDFacilityLobby m_LobbyTableData { get { return TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()); } }
        /// <summary>
        /// ��ǰ����ܹ����Ľ�������
        /// </summary>
        private int m_MaxCanWorkFacilityLimit
        {
            get
            {
                return m_LobbyTableData.workMaxAmount;
            }
        }

        private int m_WorkTimerID = -1;

        private bool m_IsInitData = false;

        public void Init()
        {
            if (GameDataMgr.S.GetPlayerData().IsUnlockWorkSystem())
            {
                InitData();
            }
            else
            {
                EventSystem.S.Register(EventID.OnUnlockWorkSystem, OnUnlockWorkSystemCallBack);
            }
        }
        private void InitData() 
        {
            if (m_IsInitData) return;

            m_IsInitData = true;

            CheckData();

            BindingEvents();

            StartShowWorkBubble();
        }

        private void BindingEvents()
        {
            EventSystem.S.Register(EventID.OnStartUnlockFacility, OnStartUnlockFacilityCallBack);
        }

        private void OnStartUnlockFacilityCallBack(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];

            FacilityController controller = MainGameMgr.S.FacilityMgr.GetFacilityController(type);

            if (IsCanShowWorkBubble(type) && m_AllUnlockFacilityList.Contains(controller) == false)
            {
                m_AllUnlockFacilityList.Add(controller);
            }
        }

        private void OnUnlockWorkSystemCallBack(int key, object[] param)
        {
            GameDataMgr.S.GetPlayerData().SetWorkSystem(true);

            InitData();
        }
        /// <summary>
        /// ��ʼ��ʾ��������
        /// </summary>
        private void StartShowWorkBubble()
        {
            Timer.S.Cancel(m_WorkTimerID);

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);

            TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(lobbyLevel);

            if (lobbyData == null)
            {
                lobbyData = TDFacilityLobbyTable.GetData(0);
            }

            if (lobbyData != null)
            {
                m_WorkTimerID = Timer.S.Post2Really((time) =>
                {
                    try
                    {
                        List<FacilityController> tmpControllerList = m_AllUnlockFacilityList.Where(x => x.IsWorking() || x.IsShowBubble()).ToList();

                        if (tmpControllerList != null && tmpControllerList.Count < m_MaxCanWorkFacilityLimit)
                        {
                            tmpControllerList = m_AllUnlockFacilityList.Where(y => y.IsIdleState()).ToList();

                            if (tmpControllerList != null && tmpControllerList.Count > 0)
                            {
                                int index = UnityEngine.Random.Range(0, tmpControllerList.Count);

                                FacilityController controller = tmpControllerList[index];

                                if (controller != null)
                                {
                                    EventSystem.S.Send(EventID.OnAddCanWorkFacility, controller.GetFacilityType());
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogError("error:" + ex.ToString());
                    }
                }, lobbyData.workInterval, -1);
            }
            else
            {
                UnityEngine.Debug.LogError("error:");
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        private void CheckData()
        {
            m_AllUnlockFacilityList.Clear();

            //��ȡ���н�������
            var unlockFacilityDataList = GameDataMgr.S.GetClanData().ownedFacilityData.GetUnlockFacilityList();
            for (int i = 0; i < unlockFacilityDataList.Count; i++)
            {
                FacilityItemDbData data = unlockFacilityDataList[i];

                FacilityType type = (FacilityType)data.id;

                FacilityController controller = MainGameMgr.S.FacilityMgr.GetFacilityController(type);

                if (IsCanShowWorkBubble(type) && m_AllUnlockFacilityList.Contains(controller) == false)
                {
                    m_AllUnlockFacilityList.Add(controller);
                }
            }

            //��Ᵽ�����ݣ��Ƿ���δ������ɵĽ���
            List<WorkItemData> workDataList = GameDataMgr.S.GetClanData().WorkItemDataList;
            if (workDataList != null && workDataList.Count > 0) 
            {
                for (int i = 0; i < workDataList.Count; i++)
                {
                    WorkItemData data = workDataList[i];

                    FacilityController facilityController = MainGameMgr.S.FacilityMgr.GetFacilityController(data.FacilityType);

                    if (facilityController != null) 
                    {
                        facilityController.DispatchDiscipleStartWork(data.CharacterID);
                    }
                }
            }
        }
        /// <summary>
        /// ��ǰ�����Ƿ��ܹ���ʾ��������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsCanShowWorkBubble(FacilityType type)
        {
            if (type == FacilityType.None || type == FacilityType.BulletinBoard || type == FacilityType.TotalCount || type == FacilityType.PatrolRoom)
            {
                return false;
            }
            return true;
        }
    }
}