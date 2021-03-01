using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class FacilityMgr : MonoBehaviour, IMgr, IInputObserver
    {
        private List<FacilityController> m_FacilityList = new List<FacilityController>();
        private IClickedHandler m_ClickHandler = null;

        #region IMgr
        public void OnInit()
        {
            RegisterEvents();
            InitFacilityList();
            InputMgr.S.AddTouchObserver(this);
        }

        public void OnUpdate()
        {
        }

        public void OnDestroyed()
        {
            UnregisterEvents();

            InputMgr.S.RemoveTouchObserver(this);
        }
        #endregion

        #region Public Set
        public void SetFacilityState(FacilityType type, FacilityState state/*, int subId = 1*/)
        {
            GameDataMgr.S.GetClanData().SetFacilityState(type, state/*, subId*/);

            FacilityController controller = GetFacilityController(type/*, subId*/);
            controller?.SetState(state);

            if (state == FacilityState.Unlocked) // Some facility unlocked
            {
                RefreshFacilityUnlockState();
            }
        }

        #endregion

        #region InputObserver

        private float m_TouchStartTime = 0;
        public bool On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
            return false;
        }

        public bool On_LongTap(Gesture gesture)
        {
            return false;
        }

        public bool On_Swipe(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchDown(Gesture gesture)
        {
            return false;
        }

        public bool BlockInput()
        {
            return true;
        }

        public int GetSortingLayer()
        {
            return 1;
        }

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            m_TouchStartTime = Time.time;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Facility"));
            if (hit.collider != null)
            {
                m_ClickHandler = hit.collider.GetComponent<IClickedHandler>();

                return true;
            }

            return false;
        }

        public bool On_TouchUp(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Facility"));
            if (hit.collider != null)
            {
                IClickedHandler handler = hit.collider.GetComponent<IClickedHandler>();
                if (handler != null && m_ClickHandler == handler && Time.time - m_TouchStartTime < 0.3f)
                {
                    handler.OnClicked();

                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public Get    
        public FacilityState GetFacilityState(FacilityType type)
        {
            FacilityController controller = GetFacilityController(type);
            return controller.GetState();
        }
        /// <summary>
        /// Get facility current level
        /// </summary>
        public int GetFacilityCurLevel(FacilityType facilityType/*, int subId = 1*/)
        {
            int level = GameDataMgr.S.GetClanData().GetFacilityLevel(facilityType/*, subId*/);

            switch (facilityType)
            {
                case FacilityType.Lobby:
                    return Mathf.Min(level, Define.FACILITY_MAX_LOBBY);
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    return Mathf.Min(level, Define.FACILITY_MAX_LIVABLEROOM);
                case FacilityType.Warehouse:
                    return Mathf.Min(level, Define.FACILITY_MAX_WAREHOUSE);
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    return Mathf.Min(level, Define.FACILITY_MAX_PRACTIVEFIELD);
                case FacilityType.KongfuLibrary:
                    return Mathf.Min(level, Define.FACILITY_MAX_KUNGFULIBRARY);
                case FacilityType.Kitchen:
                    return Mathf.Min(level, Define.FACILITY_MAX_KITCHEN);
                case FacilityType.ForgeHouse:
                    return Mathf.Min(level, Define.FACILITY_MAX_FORGEHOUSE);
                case FacilityType.Baicaohu:
                    return Mathf.Min(level, Define.FACILITY_MAX_BAICAOHU);
                case FacilityType.PatrolRoom:
                    return Mathf.Min(level, Define.FACILITY_MAX_PATROLROOM);
                case FacilityType.BulletinBoard:
                    return Mathf.Min(level, Define.FACILITY_MAX_BULLETInBOARD);
                case FacilityType.TotalCount:
                    return Mathf.Min(level, Define.FACILITY_MAX_TOTALCOUNT);
                default:
                    break;
            }
            return 0;
        }

        /// <summary>
        /// 获得主城等级
        /// </summary>
        /// <returns></returns>
        public int GetLobbyCurLevel()
        {
            return GetFacilityCurLevel(FacilityType.Lobby);
        }

        public int GetFacilityMaxLevel(FacilityType facilityType)
        {
            switch (facilityType)
            {
                case FacilityType.Lobby:
                    return Define.FACILITY_MAX_LOBBY;
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    return Define.FACILITY_MAX_LIVABLEROOM;
                case FacilityType.Warehouse:
                    return Define.FACILITY_MAX_WAREHOUSE;
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    return Define.FACILITY_MAX_PRACTIVEFIELD;
                case FacilityType.KongfuLibrary:
                    return Define.FACILITY_MAX_KUNGFULIBRARY;
                case FacilityType.Kitchen:
                    return Define.FACILITY_MAX_KITCHEN;
                case FacilityType.ForgeHouse:
                    return Define.FACILITY_MAX_FORGEHOUSE;
                case FacilityType.Baicaohu:
                    return Define.FACILITY_MAX_BAICAOHU;
                case FacilityType.PatrolRoom:
                    return Define.FACILITY_MAX_PATROLROOM;
            }
            return -1;
        }
        /// <summary>
        /// Get facility info by level
        /// </summary> 
        public FacilityLevelInfo GetFacilityLevelInfo(FacilityType facilityType, int level)
        {
            FacilityLevelInfo facilityLevelInfo = null;

            switch (facilityType)
            {
                case FacilityType.Lobby:
                    facilityLevelInfo = TDFacilityLobbyTable.GetLevelInfo(level);
                    break;
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    facilityLevelInfo = TDFacilityLivableRoomTable.GetLevelInfo((int)facilityType, level);
                    break;
                case FacilityType.Warehouse:
                    facilityLevelInfo = TDFacilityWarehouseTable.GetLevelInfo(level);
                    break;
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    facilityLevelInfo = TDFacilityPracticeFieldTable.GetLevelInfo(facilityType, level);
                    break;
                case FacilityType.Kitchen:
                    facilityLevelInfo = TDFacilityKitchenTable.GetLevelInfo(level);
                    break;
                case FacilityType.KongfuLibrary:
                    facilityLevelInfo = TDFacilityKongfuLibraryTable.GetLevelInfo(level);
                    break;
                case FacilityType.ForgeHouse:
                    facilityLevelInfo = TDFacilityForgeHouseTable.GetLevelInfo(level);
                    break;
                case FacilityType.Baicaohu:
                    facilityLevelInfo = TDFacilityBaicaohuTable.GetLevelInfo(level);
                    break;
                case FacilityType.PatrolRoom:
                    facilityLevelInfo = TDFacilityPatrolRoomTable.GetLevelInfo(level);
                    break;
            }

            return facilityLevelInfo;
        }

        public  List<string> GetUnlockContent(int level)
        {
            return TDFacilityLobbyTable.GetUnlockContent(level);
        }


        #region PracticeField

        /// <summary>
        /// 获取配置文件中所有的练兵场信息
        /// </summary>
        /// <returns></returns>
        public List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList()
        {
            return TDFacilityPracticeFieldTable.GetPracticeFieldLevelInfoList();
        }

        /// <summary>
        /// 获取训练时间
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDurationForLevel(FacilityType facilityType, int level)
        {
            switch (facilityType)
            {
            
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    return TDFacilityPracticeFieldTable.GetDurationForLevel(facilityType, level);
                case FacilityType.KongfuLibrary:
                    return TDFacilityKongfuLibraryTable.GetDurationForLevel(level);
            }
            return 0;
        }

        /// <summary>
        /// 根据等级获取练功经验
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetExpValue(FacilityType facilityType, int level)
        {
            return TDFacilityPracticeFieldTable.GetExpValue(facilityType, level);
        }

        public void StartCountDown(int second, Action<string> refresAction, Action overAction)
        {
            CountDownItem countDownMgr = new CountDownItem("PracticeField", second);
            TimeUpdateMgr.S.AddObserver(countDownMgr);
            TimeUpdateMgr.S.Start();
            countDownMgr.OnSecondRefreshEvent = refresAction;
            countDownMgr.OnCountDownOverEvent = overAction;
        }

        #endregion

        #region KungfuLibrary

        public List<KongfuLibraryLevelInfo> GetKungfuLibraryLevelInfoList(FacilityType facilityType)
        {
            return TDFacilityKongfuLibraryTable.GetKongfuLibraryLevelInfoList(facilityType);
        }
        /// <summary>
        /// 根据藏经阁等级获取相应的功夫
        /// </summary>
        /// <param name="kungfuLibraryLevel"></param>
        /// <returns></returns>
        public KungfuType GetKungfuForWeightAndLevel(int kungfuLibraryLevel)
        {
            return TDFacilityKongfuLibraryTable.GetKungfuForWeightAndLevel(kungfuLibraryLevel);
        }

        #endregion

        #region PatrolRoom
        public List<PatrolRoomInfo> GetPatrolRoomLevelInfoList(FacilityType facilityType)
        {
            return TDFacilityPatrolRoomTable.GetPatrolRoomLevelInfoList(facilityType);
        }
        #endregion
        /// <summary>
        /// Get facility config info
        /// </summary>
        public FacilityConfigInfo GetFacilityConfigInfo(FacilityType facilityType)
        {
            FacilityConfigInfo facilityConfigInfo = TDFacilityConfigTable.GetFacilityConfigInfo(facilityType);

            if (facilityConfigInfo == null)
            {
                Log.e("Facility config info not found, facility type is: " + facilityType.ToString());
            }

            return facilityConfigInfo;
        }

        public FacilityController GetFacilityController(FacilityType facilityType/*, int subId*/)
        {
            return m_FacilityList.FirstOrDefault(i => i.GetFacilityType() == facilityType /*&& i.GetSubId() == subId*/);
        }

        public Vector3 GetDoorPos(FacilityType facilityType/*, int subId = 1*/)
        {
            FacilityController controller = GetFacilityController(facilityType/*, subId*/);
            if (controller != null)
            {
                return controller.GetDoorPos();
            }

            Log.e("GetDoorPos, can't find the facility controller");

            return Vector3.zero;
        }

        #endregion

        #region Private Methods
        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnStartUnlockFacility, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnStartUpgradeFacility:
                    FacilityType facilityType = (FacilityType)param[0];
                    int subId = (int)param[1];
                    int deltaLevel = (int)param[2];
                    UpgradeFacility(facilityType, subId, deltaLevel);
                    break;
                case (int)EventID.OnStartUnlockFacility:
                    FacilityType facilityType2 = (FacilityType)param[0];
                    //int subId2 = (int)param[1];
                    SetFacilityState(facilityType2, FacilityState.Unlocked/*, subId2*/);

                    if (facilityType2 == FacilityType.Lobby) //BulletinBoard unlocked when lobby unlocked
                    {
                        SetFacilityState(FacilityType.BulletinBoard, FacilityState.Unlocked);
                    }
                    break;
            }
        }

        private void InitFacilityList()
        {
            FacilityView[] allFacilityViewList = FindObjectsOfType<FacilityView>();
            allFacilityViewList.ToList().ForEach(i =>
            {
                FacilityController facility = i.GenerateContoller();
                if (facility != null)
                {
                    m_FacilityList.Add(facility);
                }
            });
        }

        private void UpgradeFacility(FacilityType type, int subId, int deltaLevel)
        {
            GameDataMgr.S.GetClanData().UpgradeFacility(type, deltaLevel/*, subId*/);
            GetFacilityController(type)?.OnUpgrade();

            RefreshFacilityUnlockState();
        }

        private void RefreshFacilityUnlockState()
        {
            List<FacilityItemDbData> allFacilityDataList = GameDataMgr.S.GetClanData().GetAllFacility();
            allFacilityDataList.ForEach(i =>
            {
                if (i.facilityState == FacilityState.Locked && i.id != (int)FacilityType.BulletinBoard)
                {
                    FacilityConfigInfo configInfo = GetFacilityConfigInfo((FacilityType)(i.id));
                    bool isSatisfied = IsUnlockPreconditionSatisfied(configInfo);
                    if (isSatisfied)
                    {
                        SetFacilityState((FacilityType)(i.id), FacilityState.ReadyToUnlock/*, i.subId*/);
                    }
                }
            });
        }

        private bool IsUnlockPreconditionSatisfied(FacilityConfigInfo configInfo)
        {
            //仓库必须通过引导来建造，否则不能变为可建造状态
            if (configInfo.facilityType == FacilityType.Warehouse)
                return false;

            bool isPrefacilityUnlocked = configInfo.prefacilityType == FacilityType.None ? true : IsFacilityUnlocked(configInfo.prefacilityType, 1);
            bool isSatisfied = GetFacilityCurLevel(FacilityType.Lobby) >= configInfo.GetNeedLobbyLevel() && isPrefacilityUnlocked;
            return isSatisfied;
        }

        private bool IsFacilityUnlocked(FacilityType facilityType, int subId)
        {
            FacilityItemDbData dbItem = GameDataMgr.S.GetClanData().GetFacilityItem(facilityType/*, subId*/);
            if (dbItem != null)
            {
                FacilityState facilityState = dbItem.facilityState;
                bool isUnlocked = facilityState != FacilityState.Locked && facilityState != FacilityState.ReadyToUnlock;

                return isUnlocked;
            }
            else
            {
                Log.e("GetFacilityItem return null, facility type: " + facilityType.ToString() + " sub id: " + subId);
                return false;
            }
        }
        #endregion
    }
}