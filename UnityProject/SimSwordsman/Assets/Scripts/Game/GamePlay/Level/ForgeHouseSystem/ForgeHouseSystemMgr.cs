using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class ForgeHouseSystemMgr : TSingleton<ForgeHouseSystemMgr>
    {
        public void Init()
        {
            List<ForgeHouseItemData> dataList = GameDataMgr.S.GetClanData().ForgeHouseItemDataList;

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    ForgeHouseItemData data = dataList[i];

                    CountDown countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTime(),
                    (remainTime) =>
                    {
                        GameDataMgr.S.GetClanData().UpdateForgeHouseItemData(data.ForgeHouseItemID, 1);
                    },
                    (remainTime) =>
                    {
                        OnForgeHouseItemDataCountDownFinished(data.ForgeHouseItemID);
                    });
                    GameDataMgr.S.GetClanData().UpdateForgeHouseItemDataCountDownID(data.ForgeHouseItemID, countDownItem.GetCountDownID());
                }
            }
        }
        private void OnForgeHouseItemDataCountDownFinished(int forgeHouseID)
        {
            bool isArmor = false;
            if (MainGameMgr.S.BattleFieldMgr.IsBattleing == false)
            {
                if (forgeHouseID > 500)
                {
                    List<RewardBase> rewards = new List<RewardBase>();
                    rewards.Add(new ArmorReward(forgeHouseID, 1));
                    UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
                    isArmor = true;
                }
                else
                {
                    List<RewardBase> rewards = new List<RewardBase>();
                    rewards.Add(new ArmsReward(forgeHouseID, 1));
                    UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
                    isArmor = false;
                }
            }

            if (isArmor)
            {
                MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)forgeHouseID, Step.One), 1);
            }
            else
            {
                MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((ArmsType)forgeHouseID, Step.One), 1);
            }

            GameDataMgr.S.GetClanData().RemoveForgeHouseItemData(forgeHouseID);
        }
        public ForgeHouseItemData AddForgeHouseItemData(int forgeHouseID)
        {
            ForgeHouseItemData data = GameDataMgr.S.GetClanData().AddForgeHouseItemData(forgeHouseID);

            CountDown countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTime(), (remainTime) =>
            {
                GameDataMgr.S.GetClanData().UpdateForgeHouseItemData(data.ForgeHouseItemID, 1);
            }, (remainTime) =>
            {
                OnForgeHouseItemDataCountDownFinished(data.ForgeHouseItemID);
            });

            GameDataMgr.S.GetClanData().UpdateForgeHouseItemDataCountDownID(data.ForgeHouseItemID, countDownItem.GetCountDownID());
            return data;
        }
        /// <summary>
        /// 立刻完成倒计时
        /// </summary>
        /// <param name="forgeHouseID"></param>
        public void ImmediatelyCompleteBaiCaoWuCountDown(int forgeHouseID)
        {
            GameDataMgr.S.GetClanData().RemoveForgeHouseItemData(forgeHouseID);
        }
    }

    public class ForgeHouseItemData
    {
        public int ForgeHouseItemID;
        public DateTime StartTime;
        public DateTime EndTime;
        public int TotalSecondsTime;
        public int AlreadyPassTime;
        public int m_CountDownID = -1;
        public ForgeHouseItemData() { }
        public ForgeHouseItemData(int ForgeHouseItemID, DateTime StartTime, DateTime EndTime)
        {
            this.ForgeHouseItemID = ForgeHouseItemID;
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            TotalSecondsTime = (int)(EndTime - StartTime).TotalSeconds;
        }
        public void UpdateData(DateTime StartTime, DateTime EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;

            TotalSecondsTime = (int)(EndTime - StartTime).TotalSeconds;
            AlreadyPassTime = 0;
        }
        public int GetRemainTime()
        {
            return Mathf.Max(0, TotalSecondsTime - AlreadyPassTime);
        }
        public string GetRemainTimeStr()
        {
            return (DateTime.Now.AddSeconds(GetRemainTime()) - DateTime.Now).ToString(@"hh\:mm\:ss");
        }
        public void SetCouontDownID(int countDownID)
        {
            m_CountDownID = countDownID;
        }
        public int GetCountDownID()
        {
            return m_CountDownID;
        }
        public float GetProgress()
        {
            return Mathf.Clamp01((AlreadyPassTime * 1.0f / TotalSecondsTime));
        }
    }
}
