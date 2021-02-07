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
    public class FoodRecoverySystem : TSingleton<FoodRecoverySystem>
    {
        int limit;
        private KitchLevelInfo m_CurKitchLevelInfo;

        private string m_StartTime;
        private const string m_StartTimeName = "StartTime";

        ~FoodRecoverySystem()
        {
            EventSystem.S.UnRegister(EventID.OnEndUpgradeFacility, OnUpgradeKitchen);
            EventSystem.S.UnRegister(EventID.OnStartUnlockFacility, OnUnlockKitchen);
            EventSystem.S.UnRegister(EventID.OnReduceFood, OnUnlockKitchen);
        }


        public void Init()
        {

            EventSystem.S.Register(EventID.OnEndUpgradeFacility, OnUpgradeKitchen);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, OnUnlockKitchen);
            EventSystem.S.Register(EventID.OnReduceFood, ReduceFood);
            if (!RefreshKitchInfo())
                return;



            RefreshCountDown();
            //PlayerPrefs.SetString(m_StartTimeName, m_StartTime);

        }
        private void ReduceFood(int key, object[] param)
        {
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            if (GameDataMgr.S.GetPlayerData().foodNum < limit)
                StartCountdown();
        }
        private void RefreshCountDown()
        {
            int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, curLevel);
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
            {
                GameDataMgr.S.GetPlayerData().SetFoodNum(limit);
                return;
            }

            string m_StartTime = PlayerPrefs.GetString(m_StartTimeName);
            if (string.IsNullOrEmpty(m_StartTime))
            {
                m_StartTime = DateTime.Now.ToString();
                StartCountdown();
            }
            else
            {
                DateTime dateTime;
                DateTime.TryParse(m_StartTime, out dateTime);
                if (dateTime != null)
                {
                    TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                    int second = (int)timeSpan.TotalSeconds;
                    int foodAddSpeed = m_CurKitchLevelInfo.GetCurFoodAddSpeed();
                    int foodNumber = second / foodAddSpeed;
                    for (int i = 0; i < foodNumber; i++)
                        GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                    int surplus = second % foodAddSpeed;
                    CountDownItem countDown = new CountDownItem(FacilityType.Kitchen.ToString(), surplus);
                    TimeUpdateMgr.S.AddObserver(countDown);
                    countDown.OnCountDownOverEvent += CountDownOver;
                    countDown.OnSecondRefreshEvent += SecondRefresh;
                }
            }
        }

        private bool RefreshKitchInfo()
        {
            FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen);
            if (facility.GetState() == FacilityState.Unlocked)
                return true;
            return false;
        }

        void StartCountdown()
        {
            CountDownItem countDown = new CountDownItem(FacilityType.Kitchen.ToString(), m_CurKitchLevelInfo.GetCurFoodAddSpeed());
            TimeUpdateMgr.S.AddObserver(countDown);
            countDown.OnCountDownOverEvent += CountDownOver;
            countDown.OnSecondRefreshEvent += SecondRefresh;
        }

        private void SecondRefresh(string obj)
        {
            EventSystem.S.Send(EventID.OnFoodRefreshEvent, obj);
            PlayerPrefs.SetString(m_StartTimeName, DateTime.Now.ToString());
        }



        private void CountDownOver()
        {
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            if (GameDataMgr.S.GetPlayerData().foodNum + 1 <= limit)
            {
                GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                StartCountdown();
            }
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
                RefreshCountDown();
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

                RefreshKitchInfo();
            }
        }
    }
}