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
                AddFacility(facilityType, 1, FacilityState.Locked);
            }
        }

        public List<FacilityItemDbData> GetUnlockFacilityList()
        {
            List<FacilityItemDbData> dataList = new List<FacilityItemDbData>();

            for (int i = 0; i < facilityList.Count; i++)
            {
                FacilityItemDbData data = facilityList[i];
                if (data != null && data.facilityState == FacilityState.Unlocked)
                {
                    dataList.Add(data);
                }
            }

            return dataList;
        }

        public void UpgradeFacility(FacilityType facilityType, int deltaLevel/*, int subId*/)
        {
            FacilityItemDbData facilityDbData = GetFacilityData(facilityType/*, subId*/);
            if (facilityDbData != null)
            {
                facilityDbData.AddLevel(deltaLevel);
            }

            EventSystem.S.Send(EventID.OnUpgradeFacility, facilityType);
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

        public FacilityItemDbData AddFacility(FacilityType facilityType, int subId, FacilityState state)
        {
            int id = (int)facilityType;

            FacilityItemDbData data = facilityList.Find(i => i.id == id && i.subId == subId);
            if (data == null)
            {
                data = new FacilityItemDbData(id, subId, 1, state);
                facilityList.Add(data);
            }
            return data;
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
            
            if (facilityDbData == null) 
            {
                facilityDbData = AddFacility(facilityType, 1, FacilityState.Locked);                
            }
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