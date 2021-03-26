using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// 食物（体力/包子）恢复
    /// </summary>
    public class FoodRecoverySystem : TMonoSingleton<FoodRecoverySystem>
    {
        private int m_CountDownCount = 0;
        private int m_TimerId;
        private int limit;
        private KitchLevelInfo m_CurKitchLevelInfo;

        private bool isOne = true;
        private bool isSwitch = false;
        private int m_Remainder = 0;

        private void Start()
        {
            RefreshKitchenInfo();
            HandleOfflineProfit();
            CheckFoodIsFull();
        }
        private void RefreshKitchenInfo()
        {
            int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
            limit = TDFacilityKitchenTable.GetData(curLevel).foodLimit;
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, curLevel);
        }

        private void HandleOfflineProfit()
        {
            string preTime = GameDataMgr.S.GetPlayerData().GetFoodCoundDownTime();
            if (string.IsNullOrEmpty(preTime))
                return;
            int seconds = ComputingTime(preTime);
            int foodNumber = seconds / m_CurKitchLevelInfo.GetCurFoodAddSpeed();
            m_Remainder = seconds % m_CurKitchLevelInfo.GetCurFoodAddSpeed();
         
            GameDataMgr.S.GetPlayerData().SetFoodCoundDownTime(DateTime.Now.ToString());
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
                return;
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() + foodNumber >= limit)
                GameDataMgr.S.GetPlayerData().SetFoodNum(limit);
            else
            {
                if (foodNumber>=0)
                    GameDataMgr.S.GetPlayerData().AddFoodNum(foodNumber);
            }

            EventSystem.S.Send(EventID.OnRefreshMainMenuPanel);
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnEndUpgradeFacility, OnUpgradeKitchen);
            EventSystem.S.UnRegister(EventID.OnStartUnlockFacility, OnUnlockKitchen);
            EventSystem.S.UnRegister(EventID.OnReduceFood, ReduceFood);
            EventSystem.S.UnRegister(EventID.OnAddFood, AddFood);
            EventSystem.S.UnRegister(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);
        }
        private void ReduceFood(int key, object[] param)
        {
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen);
            if (facility.GetState() != FacilityState.Unlocked)
                return;
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() < limit)
                isSwitch = true;
        }
        private void AddFood(int key, object[] param)
        {
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
                isSwitch = false;
        }

        public void Init()
        {
            EventSystem.S.Register(EventID.OnEndUpgradeFacility, OnUpgradeKitchen);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, OnUnlockKitchen);
            EventSystem.S.Register(EventID.OnReduceFood, ReduceFood);
            EventSystem.S.Register(EventID.OnAddFood, AddFood);
            EventSystem.S.Register(EngineEventID.OnAfterApplicationFocusChange, HandleAddListenerEvent);

            if (!RefreshKitchInfo())
                return;

            RefreshCountDown();
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            if ((EngineEventID)key == EngineEventID.OnAfterApplicationFocusChange)
            {
                if ((bool)param[0])
                {
                    HandleOfflineProfit();
                }
                //ReduceTickTime(ComputingTime(startTime));
                else
                    GameDataMgr.S.GetPlayerData().SetFoodCoundDownTime(DateTime.Now.ToString());
            }
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

        private void RefreshCountDown()
        {
            isSwitch = true;

        }
        private void Update()
        {
            if (isOne && isSwitch)
            {
                isOne = false;
                StartCountDown(m_CurKitchLevelInfo.GetCurFoodAddSpeed() + 1, (i) =>
                {
                    m_CountDownCount = i;
                    EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(m_CountDownCount), false);
                    GameDataMgr.S.GetPlayerData().SetFoodCoundDownTime(DateTime.Now.ToString());
                }, () =>
                {
                    GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                    CheckFoodIsFull();
                    isOne = true;
                });
            }
        }
        /// <summary>
        /// 检测食物有没有满
        /// </summary>
        private void CheckFoodIsFull()
        {
            if (GameDataMgr.S.GetPlayerData().GetFoodNum()>= limit)
            {
                EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(0), true);
                isSwitch = false;
            }
        }
        private void StartCountDown(int second, Action<int> action, Action finesh)
        {
            m_TimerId = Timer.S.Post2Really((i) =>
            {
                action?.Invoke(second - i);
                if ((second - i) == 0)
                {
                    finesh?.Invoke();
                }
            }, 1, second);
        }

        private bool RefreshKitchInfo()
        {
            FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen);
            if (facility.GetState() == FacilityState.Unlocked)
                return true;
            return false;
        }
        private void OnUnlockKitchen(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Kitchen)
            {
                limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
                int num = limit - GameDataMgr.S.GetPlayerData().foodNum;
                if (num > 0)
                    GameDataMgr.S.GetPlayerData().AddFoodNum(num);
            }
        }

        private void OnUpgradeKitchen(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Kitchen)
            {
                limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
                int num = limit - GameDataMgr.S.GetPlayerData().foodNum;
                if (num > 0)
                    GameDataMgr.S.GetPlayerData().AddFoodNum(num);
                //RefreshKitchInfo();
                Timer.S.Cancel(m_TimerId);
                isOne = true;
                isSwitch = false;
                RefreshKitchenInfo();
                EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(0), true);
            }
        }
        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
    }
}