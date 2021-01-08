using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{

    [Serializable]
    public class FacilityDbData
    {
        public List<FacilityItemDbData> facilityList = new List<FacilityItemDbData>();

        public FacilityDbData()
        {

        }

        public void SetDefaultValue()
        {
            AddFacility(FacilityType.Lobby, 1, FacilityState.ReadyToUnlock);
            for (int i = (int)FacilityType.Lobby + 1; i < (int)FacilityType.TotalCount; i++)
            {
                FacilityType facilityType = (FacilityType)i;
                //if (facilityType == FacilityType.LivableRoomEast)
                //{
                //    AddFacility(FacilityType.LivableRoomEast, 1, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomEast, 2, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomEast, 3, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomEast, 4, FacilityState.Locked);
                //}
                //else if (facilityType == FacilityType.LivableRoomWest)
                //{
                //    AddFacility(FacilityType.LivableRoomWest, 1, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomWest, 2, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomWest, 3, FacilityState.Locked);
                //    AddFacility(FacilityType.LivableRoomWest, 4, FacilityState.Locked);
                //}
                //else
                {
                    AddFacility(facilityType, 1, FacilityState.Locked);
                }

            }
        }

        public void UpgradeFacility(FacilityType facilityType, int deltaLevel/*, int subId*/)
        {
            FacilityItemDbData facilityDbData = GetFacilityData(facilityType/*, subId*/);
            if (facilityDbData != null)
            {
                facilityDbData.AddLevel(deltaLevel);
            }
        }

        public int GetFacilityLevel(FacilityType facilityType/*, int subId*/)
        {
            int level = -1;
            FacilityItemDbData facilityDbData = GetFacilityData(facilityType/*, subId*/);
            if (facilityDbData != null)
            {
                level = facilityDbData.level;
            }

            return level;
        }

        public bool IsLocked(FacilityType facilityType)
        {
            FacilityItemDbData facilityDbData = GetFacilityData(facilityType);
            if (facilityDbData != null)
            {
                if (facilityDbData.facilityState == FacilityState.Locked || facilityDbData.facilityState == FacilityState.ReadyToUnlock)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public void AddFacility(FacilityType facilityType, int subId, FacilityState state)
        {
            int id = (int)facilityType;

            bool isOwned = facilityList.Any(i => i.id == id && i.subId == subId);
            if (!isOwned)
            {
                facilityList.Add(new FacilityItemDbData(id, subId, 1, state));
            }
        }

        public void SetFacilityState(FacilityType facilityType, FacilityState state/*, int subId*/)
        {
            FacilityItemDbData facility = GetFacilityData(facilityType/*, subId*/);
            if (facility != null)
            {
                facility.facilityState = state;
            }
        }

        //public void RemoveFacility(FacilityType facilityType)
        //{
        //    int id = (int)facilityType;

        //    facilityList = facilityList.Except(facilityList.Where(i => i.id == id)).ToList();
        //}

        public FacilityItemDbData GetFacilityData(FacilityType facilityType/*, int subId*/)
        {
            FacilityItemDbData facilityDbData = facilityList.Where(i => i.id == (int)facilityType/* && i.subId == subId*/).FirstOrDefault();
            return facilityDbData;
        }
    }

    [Serializable]
    public class FacilityItemDbData
    {
        public int id;
        public int subId;
        public int level;
        public FacilityState facilityState;

        public FacilityItemDbData()
        {

        }

        public FacilityItemDbData(int id, int subId, int level, FacilityState state)
        {
            this.id = id;
            this.subId = subId;
            this.level = level;
            this.facilityState = state;
        }

        public void AddLevel(int deltaLevel)
        {
            level += deltaLevel;
            EventSystem.S.Send(EventID.OnEndUpgradeFacility, id);
        }
    }


}