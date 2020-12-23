using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public class FacilityMgr : MonoBehaviour, IMgr, IInputObserver
    {
        private List<FacilityController> m_FacilityList = new List<FacilityController>();

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

        #region InputObserver

        public void On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
        }

        public void On_LongTap(Gesture gesture)
        {
        }

        public void On_Swipe(Gesture gesture)
        {
        }

        public void On_TouchDown(Gesture gesture)
        {

        }

        public void On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return;


            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero,1000, 1 << 11);
            if (hit.collider != null)
            {
                IFacilityClickedHandler handler = hit.collider.GetComponent<IFacilityClickedHandler>();
                if (handler != null)
                {
                    handler.OnClicked();
                }
            }
        }

        public void On_TouchUp(Gesture gesture)
        {
        }

        #endregion

        #region Public Get    

        /// <summary>
        /// Get facility current level
        /// </summary>
        public int GetFacilityCurLevel(FacilityType facilityType, int subId = 1)
        {
            int level = GameDataMgr.S.GetClanData().GetFacilityLevel(facilityType, subId);
            return Mathf.Min(level,Define.FACILITY_MAX_LEVEL) ;
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
                    facilityLevelInfo = TDFacilityLivableRoomTable.GetLevelInfo((int)facilityType - 1, level);
                    break;
                case FacilityType.Warehouse:
                    facilityLevelInfo = TDFacilityWarehouseTable.GetLevelInfo(level);
                    break;
                case FacilityType.PracticeFieldEast:
                case FacilityType.PracticeFieldWest:
                    facilityLevelInfo = TDFacilityPracticeFieldTable.GetLevelInfo(level);
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
            }

            return facilityLevelInfo;
        }

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

        public FacilityController GetFacilityController(FacilityType facilityType, int subId)
        {
            return m_FacilityList.FirstOrDefault(i => i.GetFacilityType() == facilityType && i.GetSubId() == subId);
        }

        public Vector3 GetDoorPos(FacilityType facilityType, int subId = 1)
        {
            FacilityController controller = GetFacilityController(facilityType, subId);
            if (controller != null)
            {
                return controller.GetDoorPos();
            }

            Log.e("GetDoorPos, can't find the facility controller");

            return Vector3.zero;
        }
        public void SetFacilityState(FacilityType type, FacilityState state, int subId = 1)
        {
            GameDataMgr.S.GetClanData().SetFacilityState(type, state, subId);

            FacilityController controller = GetFacilityController(type, subId);
            controller?.SetState(state);

            if (state == FacilityState.State1) // Some facility unlocked
            {
                RefreshFacilityUnlockState();
            }
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
                    int subId2 = (int)param[1];
                    SetFacilityState(facilityType2, FacilityState.State1, subId2);
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
            GameDataMgr.S.GetClanData().UpgradeFacility(type, deltaLevel, subId);

            RefreshFacilityUnlockState();
        }

        private void RefreshFacilityUnlockState()
        {
            List<FacilityItemDbData> allFacilityDataList = GameDataMgr.S.GetClanData().GetAllFacility();
            allFacilityDataList.ForEach(i =>
            {
                if (i.facilityState == FacilityState.Locked)
                {
                    FacilityConfigInfo configInfo = GetFacilityConfigInfo((FacilityType)(i.id));
                    bool isSatisfied = IsUnlockPreconditionSatisfied(configInfo);
                    if (isSatisfied)
                    {
                        SetFacilityState((FacilityType)(i.id), FacilityState.ReadyToUnlock, i.subId);
                    }
                }
            });
        }

        private bool IsUnlockPreconditionSatisfied(FacilityConfigInfo configInfo)
        {
            bool isPrefacilityUnlocked = configInfo.prefacilityType == FacilityType.None ? true : IsFacilityUnlocked(configInfo.prefacilityType, 1);
            bool isSatisfied = GetFacilityCurLevel(FacilityType.Lobby, 1) >= configInfo.needLobbyLevel && isPrefacilityUnlocked;
            return isSatisfied;
        }

        private bool IsFacilityUnlocked(FacilityType facilityType, int subId)
        {
            FacilityItemDbData dbItem = GameDataMgr.S.GetClanData().GetFacilityItem(facilityType, subId);
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