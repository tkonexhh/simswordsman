using Qarth;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class CollectSystem : TSingleton<CollectSystem>
    {
        Dictionary<int, int> m_CurrentCollcetCountDic = new Dictionary<int, int>();
       
        public void Init()
        {
            CheckUnlockItem(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, UnlockCheck);
            EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }


        private void OnStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
            {
                var tb = TDCollectConfigTable.dataList[cd.ID];
                SetDic(cd.ID, 0);
            }
        }

        private void OnTick(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
            {
                var tb = TDCollectConfigTable.dataList[cd.ID];
                SetDic(cd.ID, (int)(tb.maxStore * cd.GetProgress()));
            }
        }

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (m_CurrentCollcetCountDic.ContainsKey(cd.ID) && cd.stringID.Equals("CollectItem"))
            {
                var tb = TDCollectConfigTable.dataList[cd.ID];
                SetDic(cd.ID, tb.maxStore);
                //MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.itemId), tb.maxStore);
            }
        }

        void SetDic(int id, int value)
        {
            //向UI发送事件
            if (m_CurrentCollcetCountDic[id] != value)
            {
                EventSystem.S.Send(EventID.OnCollectCountChange, id, value);
                m_CurrentCollcetCountDic[id] = value;
            }
        }
        

        private void UnlockCheck(int key, object[] param)
        {
            FacilityType facilityType2 = (FacilityType)param[0];
            if (facilityType2 == FacilityType.Lobby)
            {
                CheckUnlockItem(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            }
        }

        void CheckUnlockItem(int lobbylevel)
        {
            TDCollectConfig temp;
            for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
            {
                temp = TDCollectConfigTable.dataList[i];
                if (!m_CurrentCollcetCountDic.ContainsKey(i) && temp.lobbyLevelRequired <= lobbylevel)
                    m_CurrentCollcetCountDic.Add(temp.id, 0);
            }
        }
        

        public void Collect(int id)
        {
            var tb = TDCollectConfigTable.dataList[id];
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
                {
                    //额外的ui反馈


                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.specialItemId), specialItemCount);
                }
               
            }
            //弹出UI反馈

            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.itemId), m_CurrentCollcetCountDic[id]);
            //重新计时
            CountdownSystem.S.StartCountdownerWithMin("CollectItem", id, tb.maxStore * tb.productTime);

        }

    }
    
}