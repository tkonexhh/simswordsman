using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class CollectSystemMgr : MonoBehaviour, IMgr
    {
        Dictionary<int, int> m_CurrentCollcetCountDic = new Dictionary<int, int>();
        #region IMgr
        public void OnInit()
        {
            CheckUnlockItem(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockCheck);
            EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

        }

        #endregion


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
                //MainGameMgr.S.InventoryMgr.AddItem(new PropItem(   (RawMaterial)tb.itemId), tb.maxStore);
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
            //弹出UI反馈

            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.itemId), tb.maxStore);
            //重新计时
            CountdownSystem.S.StartCountdownerWithMin("CollectItem", id, tb.maxStore * tb.productTime);

        }

    }
    
}