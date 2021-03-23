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
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleAddListenerEvent);
            InitSolt();
        }

        ~PracticeFieldController()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleAddListenerEvent);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshPracticeUnlock:
                    Refesh();
                    break;
                case EventID.OnStartUpgradeFacility:
                    FacilityType facilityType = (FacilityType)param[0];
                    if (facilityType == this.facilityType)//升级的时候重新生成slot
                    {
                        InitSolt();
                    }
                    break;
            }
        }

        private void InitSolt()
        {
            int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType);
            PracticeFieldLevelInfo info = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(facilityType, level) as PracticeFieldLevelInfo;
            int seat = info.GetCurCapacity();
            int nowSeat = m_SlotList.Count;
            int delta = seat - nowSeat;
            if (delta > 0)
            {
                for (int i = nowSeat; i < seat; i++)
                {
                    m_SlotList.Add(new PracticeField(info, i, 1, m_View));
                }
            }
        }
    }
}