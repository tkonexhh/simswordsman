using Qarth;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class CollectSystem : TSingleton<CollectSystem>
    {

        Dictionary<int, int> m_CurrentCollcetCountDic = new Dictionary<int, int>();

        List<int> m_RewardItems { get { return GameDataMgr.S.GetPlayerData().rewardCollectItemIDs; } }

        TDCollectConfig m_TempTable;

        public void Init()
        {
            CheckUnlockItem(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, UnlockCheck);
            //EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }

        public void CheckData()
        {
            foreach (var item in m_RewardItems)
                SetDic(item, TDCollectConfigTable.GetData(item).maxStore);

            //检查未开启的收集物
            foreach (var item in TDCollectConfigTable.dataList)
            {
                if (m_CurrentCollcetCountDic.ContainsKey(item.id) && !m_RewardItems.Contains(item.id) && CountdownSystem.S.GetCountdowner("CollectItem", item.id) == null)
                    CountdownSystem.S.StartCountdownerWithMin("CollectItem", item.id, item.maxStore * item.productTime);
            }
        }

        //private void OnStart(int key, object[] param)
        //{
        //    Countdowner cd = (Countdowner)param[0];
        //    if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
        //    {
        //        var tb = TDCollectConfigTable.dataList[cd.ID];
        //        SetDic(cd.ID, 0);
        //    }
        //}

        private void OnTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
            {
                m_TempTable = TDCollectConfigTable.dataList[cd.ID];
                OnTickAddCount(cd.ID, (int)(m_TempTable.maxStore * cd.GetProgress()));
                //SetDic(cd.ID, (int)(tb.maxStore * cd.GetProgress()));
            }
        }

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
            {
                m_TempTable = TDCollectConfigTable.dataList[cd.ID];
                SetDic(cd.ID, m_TempTable.maxStore);
                if (!m_RewardItems.Contains(cd.ID))
                {
                    m_RewardItems.Add(cd.ID);
                    GameDataMgr.S.GetPlayerData().SetDataDirty();
                }
            }
        }

        void OnTickAddCount(int id,int value)
        {
            if(m_CurrentCollcetCountDic[id] < value)
            {
                m_CurrentCollcetCountDic[id] = value;
                EventSystem.S.Send(EventID.OnCollectCountChange, id, value);
                CheckLotusState();
            }
        }

        void SetDic(int id, int value, bool isGuide = false)
        {
            //向UI发送事件
            if (m_CurrentCollcetCountDic[id] != value)
            {
                m_CurrentCollcetCountDic[id] = value;
                if (isGuide)
                    EventSystem.S.Send(EventID.OnCollectCountChange, id, value, isGuide);
                else
                    EventSystem.S.Send(EventID.OnCollectCountChange, id, value);
                CheckLotusState();
            }
        }

        /// <summary>
        /// 检查莲花的状态（莲藕，莲花，荷叶有一项超过最大数量的一半，则设置图片为状态2，否则为状态1）
        /// </summary>
        void CheckLotusState()
        {
            foreach (var item in m_CurrentCollcetCountDic)
            {
                m_TempTable = TDCollectConfigTable.dataList[item.Key];
                if (item.Value != 3 && item.Value >= (m_TempTable.maxStore * 0.5f))
                {
                    EventSystem.S.Send(EventID.OnChangeCollectLotusState2);
                    return;
                }
            }
            EventSystem.S.Send(EventID.OnChangeCollectLotusState1);
        }
        

        private void UnlockCheck(int key, object[] param)
        {
            FacilityType facilityType2 = (FacilityType)param[0];
            if (facilityType2 == FacilityType.Lobby)
            {
                for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
                {
                    m_TempTable = TDCollectConfigTable.dataList[i];
                    if (!m_CurrentCollcetCountDic.ContainsKey(m_TempTable.id) && m_TempTable.lobbyLevelRequired <= MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby))
                    {
                        m_CurrentCollcetCountDic.Add(m_TempTable.id, 0);
                        CountdownSystem.S.StartCountdownerWithMin("CollectItem", m_TempTable.id, m_TempTable.maxStore * m_TempTable.productTime);
                        //处理新手引导
                        if (m_CurrentCollcetCountDic.Count == 1)
                        {
                            SetDic(m_TempTable.id, 1, true);
                            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
                            EventSystem.S.Send(EventID.OnGuideUnlockCollectSystem);
                        }
                    }
                }
            }
        }
        
        void CheckUnlockItem(int lobbylevel)
        {
            for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
            {
                m_TempTable = TDCollectConfigTable.dataList[i];
                if (!m_CurrentCollcetCountDic.ContainsKey(i) && m_TempTable.lobbyLevelRequired <= lobbylevel)
                {
                    m_CurrentCollcetCountDic.Add(m_TempTable.id, 0);
                }
            }
        }
       
        public void Collect(int id)
        {
            var tb = TDCollectConfigTable.dataList[id];
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(RewardMgr.S.GetRewardBase(RewardItemType.Item, tb.itemId, m_CurrentCollcetCountDic[id]));
            //额外奖励(蜂针)
            if (tb.specialItemId != 0)
            {
                //计算数量
                int specialItemCount = 0;
                for (int i = 0; i < m_CurrentCollcetCountDic[id]; i++)
                {
                    if (RandomHelper.Range(0,10000) <= tb.specialRate)
                        specialItemCount++;
                }
                if (specialItemCount > 0)
                    rewards.Add(RewardMgr.S.GetRewardBase(RewardItemType.Item, tb.specialItemId, specialItemCount));
            }
            foreach (var item in rewards)
                item.AcceptReward();

            //弹出奖励UI反馈
            UIMgr.S.OpenPanel(UIID.RewardPanel, rewards);
            //存档移除
            if (m_RewardItems.Contains(id))
            {
                m_RewardItems.Remove(id);
                GameDataMgr.S.GetPlayerData().SetDataDirty();
            }
            //重新计时
            SetDic(id, 0);
            CountdownSystem.S.StartCountdownerWithMin("CollectItem", id, tb.maxStore * tb.productTime);
        }
    }
}