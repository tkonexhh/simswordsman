using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;

namespace GameWish.Game
{
    public class KongfuLibraryController : FacilityCDController//FacilityController
    {
        public KongfuLibraryController(FacilityType facilityType, FacilityView view) : base(facilityType, view)
        {
            EventSystem.S.Register(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
            //InitKungfuField();
        }

        ~KongfuLibraryController()
        {
            EventSystem.S.UnRegister(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefresKungfuSoltInfo:
                    Refesh();
                    break;
                default:
                    break;
            }
        }

        public BaseSlot GetIdlePracticeSlot()
        {
            return m_SlotList.FirstOrDefault(i => i.IsEmpty());
        }

        public void InitKungfuField()
        {
            List<KungfuSoltDBData> kungfuLibraryDBDatas = GameDataMgr.S.GetClanData().GetKungfuLibraryData();

            if (kungfuLibraryDBDatas.Count == 0)
            {
                LoopInit(FacilityType.KongfuLibrary);
                return;
            }

            foreach (var item in kungfuLibraryDBDatas)
                m_SlotList.Add(new KungfuLibraySlot(item, m_View));

        }
        public void RefreshSlotInfo(int facilityLevel)
        {
            m_SlotList.ForEach(i =>
            {
                List<KongfuLibraryLevelInfo> infos = TDFacilityKongfuLibraryTable.GetSameSoltList(i as KungfuLibraySlot);

                foreach (var item in infos)
                {
                    if (item.level == facilityLevel)
                    {
                        (i as KungfuLibraySlot).Warp(item);
                        GameDataMgr.S.GetClanData().RefresKungfuDBData(i as KungfuLibraySlot);
                        EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, i);
                    }
                }
            });
        }

        /// <summary>
        /// 第一次打开时初始化坑位信息
        /// </summary>
        /// <param name="facilityType"></param>
        private void LoopInit(FacilityType facilityType)
        {
            int count = 0;
            List<KongfuLibraryLevelInfo> eastInfos = MainGameMgr.S.FacilityMgr.GetKungfuLibraryLevelInfoList(facilityType);
            for (int i = 0; i < eastInfos.Count; i++)
            {
                if (i - 1 >= 0)
                {

                    int lastPracticePosCount = eastInfos[i - 1].GetCurCapacity();
                    int curPracticePosCount = eastInfos[i].GetCurCapacity();

                    int delta = curPracticePosCount - lastPracticePosCount;
                    if (delta == 0)
                    {
                        count++;
                        continue;
                    }
                    else if (delta == 1)
                        m_SlotList.Add(new KungfuLibraySlot(eastInfos[i], i + 1 - count, i + 1, m_View));
                }
                else
                    m_SlotList.Add(new KungfuLibraySlot(eastInfos[i], i + 1, i + 1, m_View));
            }
        }
    }
}