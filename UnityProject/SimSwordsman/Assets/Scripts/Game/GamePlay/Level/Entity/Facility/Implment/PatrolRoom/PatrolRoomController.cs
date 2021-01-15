using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PatrolRoomController : FacilityController
    {
        public List<PatrolRoomSlot> m_PatrolRoomSlotList = new List<PatrolRoomSlot>();

        public PatrolRoomController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            InitPatrolRoom();
        }

        private void InitPatrolRoom()
        {
            List<PatrolRoomSoltDBData> patrolRoomSoltDBData = GameDataMgr.S.GetClanData().GetPatrolRoomData();

            if (patrolRoomSoltDBData.Count == 0)
            {
                LoopInit(FacilityType.PatrolRoom);
                return;
            }

            foreach (var item in patrolRoomSoltDBData)
                m_PatrolRoomSlotList.Add(new PatrolRoomSlot(item));
        }

        public List<PatrolRoomSlot> GetReadingSlotList()
        {
            return m_PatrolRoomSlotList;
        }
        public void RefreshSlotInfo(int facilityLevel)
        {
            m_PatrolRoomSlotList.ForEach(i =>
            {

                List<PatrolRoomInfo> infos = TDFacilityPatrolRoomTable.GetSameSoltList(i);

                foreach (var item in infos)
                {
                    if (item.level == facilityLevel)
                    {
                        i.Warp(item);
                        GameDataMgr.S.GetClanData().RefresPatrolRoomDBData(i);
                        EventSystem.S.Send(EventID.OnRefresPatrolSoltInfo, i);
                    }
                }
            });
        }
        private void LoopInit(FacilityType facilityType)
        {
            int count = 0;
            List<PatrolRoomInfo> eastInfos = MainGameMgr.S.FacilityMgr.GetPatrolRoomLevelInfoList(facilityType);
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
                        m_PatrolRoomSlotList.Add(new PatrolRoomSlot(eastInfos[i], i + 1 - count, i + 1));

                    //int lastPracticePosCount = eastInfos[i - 1].GetCurCapacity();
                    //int curPracticePosCount = eastInfos[i].GetCurCapacity();

                    //int delta = curPracticePosCount - lastPracticePosCount;
                    //if (delta == 0)
                    //{
                    //    m_ReadingSlotList.Add(new KungfuLibraySlot(eastInfos[i], i, i + 1));
                    //    count++;
                    //}
                    //else if (delta == 1)
                    //    m_ReadingSlotList.Add(new KungfuLibraySlot(eastInfos[i], i + 1 - count, i + 1));
                }
                else
                    m_PatrolRoomSlotList.Add(new PatrolRoomSlot(eastInfos[i], i + 1, i + 1));
            }
        }

    }
}