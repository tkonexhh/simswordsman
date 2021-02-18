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
        private Coroutine m_CoroutineID;
        private int m_CountDownCount;

        int limit;
        private KitchLevelInfo m_CurKitchLevelInfo;

        private string m_StartTime;
        private const string m_StartTimeName = "StartTime";
        private const string m_Second = "Second";

        ~FoodRecoverySystem()
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
            if ((int)param[0]== limit)
            {
                PlayerPrefs.DeleteKey(m_StartTimeName);
                m_StartTime = string.Empty;
                m_CoroutineID = null;
            }
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() < limit)
                RefreshCountDown();
        }
        private void AddFood(int key, object[] param)
        {
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
            {
                if (m_CoroutineID!=null)
                  StopCoroutine(m_CoroutineID);
                GameDataMgr.S.GetPlayerData().SetFoodNum(limit);
                EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(0), true);
            }
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
            //PlayerPrefs.SetString(m_StartTimeName, m_StartTime);

        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            if ((EngineEventID)key == EngineEventID.OnAfterApplicationFocusChange)
            {
                if (m_CurKitchLevelInfo == null)
                {
                    int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
                    m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, curLevel);
                }

                if ((bool)param[0])
                {
                    int second = ComputingTime(m_StartTime);
                    if (m_CountDownCount - second<=0)
                    {
                        int foodAddSpeed = m_CurKitchLevelInfo.GetCurFoodAddSpeed();
                        int foodNumber = second / foodAddSpeed;
                        for (int i = 0; i < foodNumber; i++)
                            GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                        int surplus = second % foodAddSpeed;
                        m_CountDownCount = foodAddSpeed - surplus;
                        return;
                    }
                    m_CountDownCount -= second;
                }
                else
                    m_StartTime = DateTime.Now.ToString();
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
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            int curLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
            m_CurKitchLevelInfo = (KitchLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.Kitchen, curLevel);
            if (GameDataMgr.S.GetPlayerData().GetFoodNum() >= limit)
            {
                PlayerPrefs.DeleteKey(m_StartTimeName);
                m_StartTime = string.Empty;
                m_CoroutineID = null;
                PlayerPrefs.SetString(m_StartTimeName, DateTime.Now.ToString());
                PlayerPrefs.SetInt(m_Second, m_CountDownCount);
                return;
            }

            m_StartTime = PlayerPrefs.GetString(m_StartTimeName);
            if (string.IsNullOrEmpty(m_StartTime))
            {
                PlayerPrefs.SetString(m_StartTimeName, DateTime.Now.ToString());
                PlayerPrefs.SetInt(m_Second, m_CountDownCount);
                if (m_CoroutineID == null)
                    m_CoroutineID = StartCoroutine("CountDown", m_CurKitchLevelInfo.GetCurFoodAddSpeed());
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
                    if (foodNumber == 0)
                    {
                        m_CountDownCount = PlayerPrefs.GetInt(m_Second, m_CountDownCount);
                        m_CountDownCount -= surplus;
                    }  
                    else
                        m_CountDownCount = foodAddSpeed - surplus;
                    if (m_CoroutineID==null)
                          m_CoroutineID = StartCoroutine("CountDown", m_CountDownCount);
                }
            }
        }


        private IEnumerator CountDown(int second)
        {
            m_CountDownCount = second;
            EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(m_CountDownCount), false);
            limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
            while (true)
            {
                yield return new WaitForSeconds(1);
                PlayerPrefs.SetString(m_StartTimeName, DateTime.Now.ToString());
                PlayerPrefs.SetInt(m_Second, m_CountDownCount);
                m_CountDownCount--;
                EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(m_CountDownCount), false);
                if (m_CountDownCount <= 0)
                {
                    if (GameDataMgr.S.GetPlayerData().GetFoodNum() + 1 <= limit)
                        GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                    EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(0), false);
                    yield return new WaitForSeconds(1);
                    if (GameDataMgr.S.GetPlayerData().GetFoodNum() + 1 < limit)
                        m_CoroutineID = StartCoroutine("CountDown", m_CurKitchLevelInfo.GetCurFoodAddSpeed());
                    else if(GameDataMgr.S.GetPlayerData().GetFoodNum() + 1 >= limit)
                        EventSystem.S.Send(EventID.OnFoodRefreshEvent, SplicingTime(0), true);
                    break;
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
                //RefreshKitchInfo();
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