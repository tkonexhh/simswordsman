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
        const string lastKeepTimeKey = "FoodLastKeepTimeKey";
        string lastKeepTime;
        
        int interval = 60;

        int limit;
        int secondCount = 0;
        int timerID = 0;

        public void Init()
        {
            EventSystem.S.Register(EventID.OnEndUpgradeFacility, OnUpgradeKitchen);
            EventSystem.S.Register(EventID.OnReduceFoodNum, OnReduceFood);
            if (GameDataMgr.S.GetClanData().IsLocked(FacilityType.Kitchen))
            {
                EventSystem.S.Register(EventID.OnStartUnlockFacility, OnUnlockKitchen);
            }
            else
            {
                limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
                string value = PlayerPrefs.GetString(lastKeepTimeKey, "");
                if (string.IsNullOrEmpty(value))
                {
                    lastKeepTime = DateTime.Now.ToString();
                    PlayerPrefs.SetString(lastKeepTimeKey, lastKeepTime);
                }
                else
                {
                    lastKeepTime = value;
                    CalculateFoodNum();
                }
                StartCountdown();
            }
        }

        void CalculateFoodNum()
        {
            DateTime lasttime = DateTime.Parse(lastKeepTime);
            TimeSpan offset = DateTime.Now - lasttime;
            int count = (int)(offset.TotalSeconds/interval);
            if (GameDataMgr.S.GetPlayerData().foodNum + count >= limit)
            {
                GameDataMgr.S.GetPlayerData().AddFoodNum(limit - GameDataMgr.S.GetPlayerData().foodNum);
                lastKeepTime = DateTime.Now.ToString();
            }
            else
            {
                GameDataMgr.S.GetPlayerData().AddFoodNum(count);
                secondCount = (int)(offset.TotalSeconds % interval);
            }
        }
        void StartCountdown()
        {
            timerID = Timer.S.Post2Really(count =>
            {
                secondCount++;
                if (secondCount % interval == 0)
                {
                    secondCount = 0;
                    if (GameDataMgr.S.GetPlayerData().foodNum + 1 <= limit)
                    {
                        GameDataMgr.S.GetPlayerData().AddFoodNum(1);
                        lastKeepTime = DateTime.Now.ToString();
                        PlayerPrefs.SetString(lastKeepTimeKey, lastKeepTime);
                    }
                    else
                    {
                        Timer.S.Cancel(timerID);
                        timerID = -1;
                    }
                }
            }, 1, -1);
        }


        private void OnUnlockKitchen(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Kitchen)
            {
                limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
                secondCount = 0;
                lastKeepTime = DateTime.Now.ToString();
                PlayerPrefs.SetString(lastKeepTimeKey, lastKeepTime);
                StartCountdown();
            }
        }

        private void OnUpgradeKitchen(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Kitchen)
            {
                limit = TDFacilityKitchenTable.GetData(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen)).foodLimit;
                if (timerID == -1)
                {
                    secondCount = 0;
                    StartCountdown();
                }
            }
        }

        private void OnReduceFood(int key, object[] param)
        {
            if (timerID == -1 && GameDataMgr.S.GetPlayerData().foodNum < limit)
            {
                secondCount = 0;
                lastKeepTime = DateTime.Now.ToString();
                PlayerPrefs.SetString(lastKeepTimeKey, lastKeepTime);
                StartCountdown();
            }
        }
    }


}