using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;

namespace GameWish.Game
{
    public class KongfuLibraryController : FacilityCDController
    {
        public KongfuLibraryController(FacilityView view) : base(FacilityType.KongfuLibrary, view)
        {
            EventSystem.S.Register(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleAddListenerEvent);
            InitSolt();
        }

        ~KongfuLibraryController()
        {
            EventSystem.S.UnRegister(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleAddListenerEvent);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefresKungfuSoltInfo:
                    Refesh();
                    break;
                case EventID.OnStartUpgradeFacility:
                    FacilityType facilityType = (FacilityType)param[0];
                    if (facilityType == FacilityType.KongfuLibrary)//升级的时候重新生成slot
                    {
                        InitSolt();
                    }
                    break;
                default:
                    break;
            }
        }

        public BaseSlot GetIdlePracticeSlot()
        {
            return m_SlotList.FirstOrDefault(i => i.IsEmpty());
        }

        private void InitSolt()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.KongfuLibrary);
            KongfuLibraryLevelInfo levelInfo = (KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.KongfuLibrary, level);
            int seat = levelInfo.GetCurCapacity();//当前席位

            int nowSeat = m_SlotList.Count;
            int delta = seat - nowSeat;
            if (delta > 0)
            {
                for (int i = nowSeat; i < seat; i++)
                {
                    m_SlotList.Add(new KungfuLibraySlot(levelInfo, i, 1, m_View));
                }
            }
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
                        Debug.LogError(item.level);
                        (i as KungfuLibraySlot).Warp(item);
                        // GameDataMgr.S.GetClanData().ownedKungfuLibraryData.RefresDBData(i as KungfuLibraySlot);
                        EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, i);
                    }
                }
            });
        }
    }
}