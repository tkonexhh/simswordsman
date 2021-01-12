using Qarth;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class CollectSystem : TSingleton<CollectSystem>
    {

        Dictionary<int, int> m_CurrentCollcetCountDic = new Dictionary<int, int>();

        List<int> m_RewardItems { get { return GameDataMgr.S.GetPlayerData().rewardCollectItemIDs; } }
       
        public void Init()
        {
            //m_RewardItems.Clear();
            //GameDataMgr.S.GetPlayerData().SetDataDirty();
            CheckUnlockItem(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, UnlockCheck);
            EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }

        public void CheckData()
        {
            foreach (var item in m_RewardItems)
                SetDic(item, TDCollectConfigTable.GetData(item).maxStore);

            //���δ�������ռ���
            foreach (var item in TDCollectConfigTable.dataList)
            {
                if (m_CurrentCollcetCountDic.ContainsKey(item.id) && !m_RewardItems.Contains(item.id) && CountdownSystem.S.GetCountdowner("CollectItem", item.id) == null)
                    CountdownSystem.S.StartCountdownerWithMin("CollectItem", item.id, item.maxStore * item.productTime);
            }
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
                if (!m_RewardItems.Contains(cd.ID))
                {
                    m_RewardItems.Add(cd.ID);
                    GameDataMgr.S.GetPlayerData().SetDataDirty();
                }
            }
        }

        void SetDic(int id, int value)
        {
            //��UI�����¼�
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
                TDCollectConfig temp;
                for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
                {
                    temp = TDCollectConfigTable.dataList[i];
                    if (!m_CurrentCollcetCountDic.ContainsKey(i) && temp.lobbyLevelRequired <= MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby))
                    {
                        m_CurrentCollcetCountDic.Add(temp.id, 0);
                        //���� ��ʼ�ռ���ʱ
                        CountdownSystem.S.StartCountdownerWithMin("CollectItem", temp.id, temp.maxStore * temp.productTime);
                    }
                }
            }
        }
        
        void CheckUnlockItem(int lobbylevel)
        {
            TDCollectConfig temp;
            for (int i = 0; i < TDCollectConfigTable.dataList.Count; i++)
            {
                temp = TDCollectConfigTable.dataList[i];
                if (!m_CurrentCollcetCountDic.ContainsKey(i) && temp.lobbyLevelRequired <= lobbylevel)
                {
                    m_CurrentCollcetCountDic.Add(temp.id, 0);
                }
            }
        }
       

        public void Collect(int id)
        {
            var tb = TDCollectConfigTable.dataList[id];
            //���⽱��(����)
            if (tb.specialItemId != 0)
            {
                //��������
                int specialItemCount = 0;
                for (int i = 0; i < m_CurrentCollcetCountDic[id]; i++)
                {
                    if (RandomHelper.Range(0,10000) <= tb.specialRate)
                        specialItemCount++;
                }
                if (specialItemCount > 0)
                {
                    //�����ui����


                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.specialItemId), specialItemCount);
                }
               
            }
            //����UI����
            UIMgr.S.OpenPanel(UIID.LogPanel, "����", string.Format("���{0}", m_CurrentCollcetCountDic[id]));
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)tb.itemId), m_CurrentCollcetCountDic[id]);
            SetDic(id, 0);
            //�浵�Ƴ�
            if (m_RewardItems.Contains(id))
            {
                m_RewardItems.Remove(id);
                GameDataMgr.S.GetPlayerData().SetDataDirty();
            }
            //���¼�ʱ
            CountdownSystem.S.StartCountdownerWithMin("CollectItem", id, tb.maxStore * tb.productTime);
           
        }

    }
    
}