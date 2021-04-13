using Qarth;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameWish.Game
{
    public class CollectSystem : TSingleton<CollectSystem>
    {
        private TDCollectConfig m_TempCollectConfigDataTable;
        private CollectSystemItemData m_TempCollectItemData;

        private int m_GuideTimerID = -1;

        private int GetLobbyFacilityCurLevel
        {
            get {
                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            }
        }
        public void Init()
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, OnStartUpgradeFacilityCallBack);
            CheckData();

            OnCollectItemTypeCountChangedCallBack();

            CheckCollectSystemGuideIsFinished();
        }
        /// <summary>
        /// 检测收集系统引导是否完成
        /// </summary>
        private void CheckCollectSystemGuideIsFinished()
        {
            if (PlayerPrefs.GetInt(Define.IsClickCollectSytemBubble, -1) < 0) 
            {
                if (GuideMgr.S.IsGuideFinish(26) == false)
                {
                    m_TempCollectConfigDataTable = TDCollectConfigTable.GetData(1);

                    m_GuideTimerID = Timer.S.Post2Really((x) =>
                    {
                        if (GameDataMgr.S.GetClanData().GetCollectItemDataRewardCount(1) >= 5 &&
                        MainGameMgr.S.IsMainMenuPanelOpen && MainGameMgr.S.BattleFieldMgr.IsBattleing == false)
                        {
                            EventSystem.S.Send(EventID.OnGuideUnlockCollectSystem);

                            Timer.S.Cancel(m_GuideTimerID);
                        }
                    }, m_TempCollectConfigDataTable.productTime, -1);
                }
            }                        
        }

        private void OnCollectItemTypeCountChangedCallBack()
        {
            int typeCount = 0;
            List<CollectSystemItemData> dataList = GameDataMgr.S.GetClanData().CollectSystemItemDataList;
            for (int i = 0; i < dataList.Count; i++)
            {
                m_TempCollectItemData = dataList[i];
                if (m_TempCollectItemData != null && m_TempCollectItemData.IsCanShowBubbleIcon()) {
                    typeCount++;
                }
            }

            if (typeCount >= 2)
            {
                EventSystem.S.Send(EventID.OnChangeCollectLotusState2);
            }
            else {
                EventSystem.S.Send(EventID.OnChangeCollectLotusState1);
            }
        }
        public void CheckData()
        {
            for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
            {
                m_TempCollectConfigDataTable = TDCollectConfigTable.dataList[i];

                if (m_TempCollectConfigDataTable.lobbyLevelRequired <= GetLobbyFacilityCurLevel) 
                {
                    m_TempCollectItemData = GameDataMgr.S.GetClanData().GetCollectItemData(m_TempCollectConfigDataTable.id);

                    if (m_TempCollectItemData != null)
                    {
                        if (m_TempCollectItemData.IsCanShowBubbleIcon())
                        {
                            EventSystem.S.Send(EventID.OnCollectCountChange, m_TempCollectConfigDataTable.id);
                        }
                        else
                        {
                            StartCountDown(m_TempCollectConfigDataTable.id, m_TempCollectItemData.GetRemainTimeWhenBubbleShow());
                        }
                    }
                    else {
                        m_TempCollectItemData = GameDataMgr.S.GetClanData().AddOrUpdateCollectSystemItemData(m_TempCollectConfigDataTable.id, DateTime.Now);
                        StartCountDown(m_TempCollectItemData.ID, m_TempCollectItemData.GetRemainTimeWhenBubbleShow());
                    }
                }                
            }
        }
        private void StartCountDown(int collectID,int remaintTime) 
        {
            CountDowntMgr.S.SpawnCountDownItemTest(remaintTime, null, (remainTime) =>
            {
                EventSystem.S.Send(EventID.OnCollectCountChange, collectID);

                //EventSystem.S.Send(EventID.OnGuideUnlockCollectSystem);
                
                OnCollectItemTypeCountChangedCallBack();
            });
        }
        private void OnStartUpgradeFacilityCallBack(int key, object[] param)
        {
            FacilityType facilityType2 = (FacilityType)param[0];
            if (facilityType2 == FacilityType.Lobby)
            {
                List<TDCollectConfig> configDataList = TDCollectConfigTable.dataList.FindAll(x => x.lobbyLevelRequired == GetLobbyFacilityCurLevel);
                if (configDataList != null && configDataList.Count > 0) 
                {
                    for (int i = 0; i < configDataList.Count; i++)
                    {
                        m_TempCollectConfigDataTable = configDataList[i];
                        m_TempCollectItemData = GameDataMgr.S.GetClanData().AddOrUpdateCollectSystemItemData(m_TempCollectConfigDataTable.id, DateTime.Now);
                        StartCountDown(m_TempCollectItemData.ID, m_TempCollectItemData.GetRemainTimeWhenBubbleShow());
                    }
                }
            }
        }       
        public void Collect(int id)
        {
            var tb = TDCollectConfigTable.dataList.Find(x => x.id == id);
            if (tb == null) {
                Debug.LogError("collect配置表中未找到数据，id：" + id);
                return;
            }
            List<RewardBase> rewards = new List<RewardBase>();
            int count = GameDataMgr.S.GetClanData().GetCollectItemDataRewardCount(id);
            rewards.Add(RewardMgr.S.GetRewardBase(RewardItemType.Item, tb.itemId, count));

            DataAnalysisMgr.S.CustomEvent(DotDefine.collect, id.ToString() + ";" + count.ToString());

            //额外奖励(蜂针)
            if (tb.specialItemId != 0)
            {
                //计算数量
                int specialItemCount = 0;
                for (int i = 0; i < count; i++)
                {
                    if (RandomHelper.Range(0,10000) <= tb.specialRate)
                        specialItemCount++;
                }
                if (specialItemCount > 0)
                {
                    DataAnalysisMgr.S.CustomEvent(DotDefine.collect, id.ToString() + ";" + specialItemCount.ToString());

                    rewards.Add(RewardMgr.S.GetRewardBase(RewardItemType.Item, tb.specialItemId, specialItemCount));
                }
            }
            foreach (var item in rewards)
                item.AcceptReward();


            //弹出奖励UI反馈
            UIMgr.S.OpenTopPanel(UIID.RewardPanel, null, rewards);
            
            m_TempCollectItemData = GameDataMgr.S.GetClanData().RemoveCollectSystemItemData(id);
            if (m_TempCollectItemData != null) 
            {
                StartCountDown(m_TempCollectItemData.ID, m_TempCollectItemData.GetRemainTimeWhenBubbleShow());
            }
            OnCollectItemTypeCountChangedCallBack();
        }
    }
}