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
            // List<PatrolRoomSoltDBData> patrolRoomSoltDBData = GameDataMgr.S.GetClanData().GetPatrolRoomData();

            // if (patrolRoomSoltDBData.Count == 0)
            // {
            //     LoopInit(FacilityType.PatrolRoom);
            //     return;
            // }

            // foreach (var item in patrolRoomSoltDBData)
            //     m_PatrolRoomSlotList.Add(new PatrolRoomSlot(item,m_View));
        }

        public List<PatrolRoomSlot> GetReadingSlotList()
        {
            return m_PatrolRoomSlotList;
        }
        public void RefreshSlotInfo(int facilityLevel)
        {
            // m_PatrolRoomSlotList.ForEach(i =>
            // {

            //     List<PatrolRoomInfo> infos = TDFacilityPatrolRoomTable.GetSameSoltList(i);

            //     foreach (var item in infos)
            //     {
            //         if (item.level == facilityLevel)
            //         {
            //             i.Warp(item);
            //             GameDataMgr.S.GetClanData().RefresPatrolRoomDBData(i);
            //             EventSystem.S.Send(EventID.OnRefresPatrolSoltInfo, i);
            //         }
            //     }
            // });
        }


    }
}