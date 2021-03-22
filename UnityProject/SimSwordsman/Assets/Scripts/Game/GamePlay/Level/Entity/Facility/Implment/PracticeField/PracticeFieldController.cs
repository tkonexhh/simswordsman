using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityCDController
    {
        public PracticeFieldController(FacilityType facilityType, FacilityView view) : base(facilityType, view)
        {
            EventSystem.S.Register(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);

            InitPracticeField();
        }

        ~PracticeFieldController()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshPracticeUnlock:
                    Refesh();
                    break;
            }
        }

        public BaseSlot GetIdlePracticeSlot(FacilityType facilityType)
        {
            return m_SlotList.FirstOrDefault(i => i.IsEmpty() && i.FacilityType.Equals(facilityType));
        }

        private void InitPracticeField()
        {
            List<PracticeSoltDBData> practiceFieldDBDatas = GameDataMgr.S.GetClanData().GetPracticeFieldData();

            if (practiceFieldDBDatas.Count == 0)
            {
                LoopInit(FacilityType.PracticeFieldEast);
                LoopInit(FacilityType.PracticeFieldWest);
                return;
            }

            foreach (var item in practiceFieldDBDatas)
                m_SlotList.Add(new PracticeField(item, m_View));
        }

        private void LoopInit(FacilityType facilityType)
        {
            List<PracticeFieldLevelInfo> eastInfos = GetPracticeFieldLevelInfoList(facilityType);
            for (int i = 0; i < eastInfos.Count; i++)
                m_SlotList.Add(new PracticeField(eastInfos[i], i + 1, i + 1, m_View));
        }

        /// <summary>
        /// ����ˢ�¿�λ״̬
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="facilityLevel"></param>
        public void RefreshPracticeUnlockInfo(FacilityType facilityType, int facilityLevel)
        {
            m_SlotList.ForEach(i =>
            {
                if (i.FacilityType == facilityType && i.UnlockLevel == facilityLevel)
                {
                    i.slotState = SlotState.Free;
                    GameDataMgr.S.GetClanData().RefresPracticeDBData(i as PracticeField);
                    EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, i);
                }
            });
        }
        /// <summary>
        /// �������ͻ�ȡ���е���������Ϣ
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        private List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList(FacilityType facilityType)
        {
            return TDFacilityPracticeFieldTable.GetPracticeFieldLevelInfoList(facilityType);
        }
    }
}