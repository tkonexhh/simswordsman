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
        private List<PracticeField> m_PracticeField = new List<PracticeField>();
        private IFacilityClickedHandler m_ClickHandler = null;

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
        public void ExrInitData()
        {
            InitPracticeField();
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

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Facility"));
            if (hit.collider != null)
            {
                m_ClickHandler = hit.collider.GetComponent<IFacilityClickedHandler>();                
            }
        }

        public void On_TouchUp(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Facility"));
            if (hit.collider != null)
            {
                IFacilityClickedHandler handler = hit.collider.GetComponent<IFacilityClickedHandler>();
                if (handler != null && m_ClickHandler == handler)
                {
                    handler.OnClicked();
                }
            }
        }

        #endregion

        #region Public Get    

        /// <summary>
        /// Get facility current level
        /// </summary>
        public int GetFacilityCurLevel(FacilityType facilityType/*, int subId = 1*/)
        {
            int level = GameDataMgr.S.GetClanData().GetFacilityLevel(facilityType/*, subId*/);
            return Mathf.Min(level, Define.FACILITY_MAX_LEVEL);
        }

        /// <summary>
        /// 获得主城等级
        /// </summary>
        /// <returns></returns>
        public int GetLobbyCurLevel()
        {
            return GetFacilityCurLevel(FacilityType.Lobby);
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
                case FacilityType.BartizanEast:
                case FacilityType.BartizanWest:
                    facilityLevelInfo = TDFacilityBartizanTable.GetLevelInfo(level);
                    break;
            }

            return facilityLevelInfo;
        }

        #region PracticeField
        /// <summary>
        /// 根据类型获取所有的练功房信息
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        public List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList(FacilityType facilityType)
        {
            return TDFacilityPracticeFieldTable.GetPracticeFieldLevelInfoList(facilityType);
        }
        /// <summary>
        /// 获取配置文件中所有的练兵场信息
        /// </summary>
        /// <returns></returns>
        public List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList()
        {
            return TDFacilityPracticeFieldTable.GetPracticeFieldLevelInfoList();
        }

        /// <summary>
        /// 获取应用层练兵场信息
        /// </summary>
        /// <returns></returns>
        public List<PracticeField> GetPracticeField()
        {
            return m_PracticeField;
        }

        /// <summary>
        /// 升级刷新坑位状态
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="facilityLevel"></param>
        public void RefreshPracticeUnlockInfo(FacilityType facilityType, int facilityLevel)
        {
            m_PracticeField.ForEach(i =>
            {
                if (i.FacilityType == facilityType && i.UnlockLevel == facilityLevel)
                {
                    i.PracticeFieldState = PracticeFieldState.Free;
                    GameDataMgr.S.GetClanData().RefresDBData(i);
                    EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, i);
                }
            });
        }

        /// <summary>
        /// 获取训练时间
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDurationForLevel(FacilityType facilityType, int level)
        {
            return TDFacilityPracticeFieldTable.GetDurationForLevel(facilityType, level);
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
                    break;
            }
        }

        private void InitPracticeField()
        {
            List<PracticeFieldDBData> practiceFieldDBDatas = GameDataMgr.S.GetClanData().GetPracticeFieldData();

            if (practiceFieldDBDatas.Count == 0)
            {
                LoopInit(FacilityType.PracticeFieldEast);
                LoopInit(FacilityType.PracticeFieldWest);
                return;
            }

            foreach (var item in practiceFieldDBDatas)
                m_PracticeField.Add(new PracticeField(item));

        }

        private void LoopInit(FacilityType facilityType)
        {
            List<PracticeFieldLevelInfo> eastInfos = GetPracticeFieldLevelInfoList(facilityType);
            for (int i = 0; i < eastInfos.Count; i++)
            {
                if (i - 1 >= 0)
                {
                    int lastPracticePosCount = eastInfos[i - 1].GetCurCapacity();
                    int curPracticePosCount = eastInfos[i].GetCurCapacity();
                    for (int j = 0; j < curPracticePosCount - lastPracticePosCount; j++)
                        m_PracticeField.Add(new PracticeField(eastInfos[i], i + 1));
                }
                else
                    m_PracticeField.Add(new PracticeField(eastInfos[i], i + 1));
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


    public class PracticeField
    {
        private PracticeFieldLevelInfo practiceFieldLevelInfo;

        public FacilityType FacilityType { set; get; }
        public int Index { set; get; }
        public int UnlockLevel { set; get; }
        public PracticeFieldState PracticeFieldState { set; get; }

        public CharacterItem CharacterItem { set; get; }
        public string StartTime { set; get; }

        public PracticeField(PracticeFieldLevelInfo item, int index)
        {
            FacilityType = item.GetHouseID();
            Index = index;
            UnlockLevel = index;
            int practiceFieldLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            if (practiceFieldLevel >= item.level)
                PracticeFieldState = PracticeFieldState.Free;
            else
                PracticeFieldState = PracticeFieldState.NotUnlocked;
            CharacterItem = null;
            StartTime = string.Empty;

            GameDataMgr.S.GetClanData().AddPracticeFieldData(this);
        }
        public PracticeField(PracticeFieldDBData item)
        {
            FacilityType = item.facilityType;
            Index = item.pitPositionID;
            UnlockLevel = item.unlockLevel;
            PracticeFieldState = item.practiceFieldState;
            if (item.characterID != -1)
                CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(item.characterID);
            StartTime = item.startTime;
            if (PracticeFieldState == PracticeFieldState.Practice)
                InitTimerUpdate();
        }

        private void InitTimerUpdate()
        {
            CountDownItem countDownMgr = new CountDownItem(FacilityType.ToString() + Index, GetDurationTime());
            countDownMgr.OnCountDownOverEvent = overAction;

            TimeUpdateMgr.S.AddObserver(countDownMgr);
        }

        public void overAction()
        {
            if (CharacterItem!=null)
            {
                AddExperience(CharacterItem);
                TrainingIsOver();
            }
        }
        private void AddExperience(CharacterItem characterItem)
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            int exp = MainGameMgr.S.FacilityMgr.GetExpValue(FacilityType, level);
            characterItem.AddExp(exp);
        }
        public int GetDurationTime()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType);
            int duration = MainGameMgr.S.FacilityMgr.GetDurationForLevel(FacilityType, level);
            int takeTime = ComputingTime(StartTime);
            return duration - takeTime;
        }
        private int ComputingTime(string time)
        {
            DateTime dateTime;
            DateTime.TryParse(time, out dateTime);
            if (dateTime != null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalSeconds;
            }
            return 0;
        }
        public void TrainingIsOver()
        {
            SetCharacterItem(CharacterItem, PracticeFieldState.Free);
            CharacterItem = null;
            StartTime = string.Empty;
            GameDataMgr.S.GetClanData().TrainingIsOver(this);
            EventSystem.S.Send(EventID.OnDisciplePracticeOver, this);
        }

        public void SetCharacterItem(CharacterItem characterItem, PracticeFieldState practiceFieldState)
        {

            //StartTime = MainGameMgr.S.FacilityMgr.GetDurationForLevel(curFacilityType, curLevel);
           
            CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(characterItem.id);
            switch (practiceFieldState)
            {
                case PracticeFieldState.Free:
                    characterController.SetState(CharacterStateID.Wander);
                    break;
                case PracticeFieldState.CopyScriptures:
                    break;
                case PracticeFieldState.Practice:
                    StartTime = DateTime.Now.ToString();
                    CharacterItem = characterItem;
                    characterController.SetState(CharacterStateID.Practice);
                    break;
                default:
                    break;
            }
            PracticeFieldState = practiceFieldState;
            GameDataMgr.S.GetClanData().RefresDBData(this);
        }
    }
}