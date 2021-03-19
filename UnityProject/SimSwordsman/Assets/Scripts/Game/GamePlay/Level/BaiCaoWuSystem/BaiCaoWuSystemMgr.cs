using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class BaiCaoWuSystemMgr : TSingleton<BaiCaoWuSystemMgr>
    {
        public void Init()
        {
            List<BaiCaoWuData> dataList = GameDataMgr.S.GetClanData().BaiCaoWuDataList;

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    BaiCaoWuData data = dataList[i];

                    CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTime(),
                    () => {
                        GameDataMgr.S.GetClanData().UpdateBaiCaoWuData(data.HerbID, 1);
                    },
                    () =>
                    {
                        OnBaiCaoWuCountDownFinished(data.HerbID);
                    });
                    GameDataMgr.S.GetClanData().UpdateBaiCaoWuDataCountDownID(data.HerbID, countDownItem.GetCountDownID());
                }
            }
        }

        private void OnBaiCaoWuCountDownFinished(int herbID)
        {
            if (MainGameMgr.S.BattleFieldMgr.IsBattleing == false)
            {
                List<RewardBase> rewards = new List<RewardBase>();
                rewards.Add(new MedicineReward(herbID, 1));
                UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            }

            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)herbID, 1));

            GameDataMgr.S.GetClanData().RemoveBaiCaoWuData(herbID);
        }

        public BaiCaoWuData AddBaiCaoWuItemData(int herbID)
        {
            BaiCaoWuData data = GameDataMgr.S.GetClanData().AddBaiCaoWuData(herbID);

            CountDownItemTest countDownItem = CountDowntMgr.S.SpawnCountDownItemTest(data.GetRemainTime(), () =>
            {
                GameDataMgr.S.GetClanData().UpdateBaiCaoWuData(data.HerbID, 1);
            }, () =>
            {
                OnBaiCaoWuCountDownFinished(data.HerbID);
            });

            GameDataMgr.S.GetClanData().UpdateBaiCaoWuDataCountDownID(data.HerbID, countDownItem.GetCountDownID());
            return data;
        }

        /// <summary>
        /// 立刻完成倒计时
        /// </summary>
        /// <param name="herbID"></param>
        public void ImmediatelyCompleteBaiCaoWuCountDown(int herbID)
        {
            GameDataMgr.S.GetClanData().RemoveBaiCaoWuData(herbID);
        }
    }
}