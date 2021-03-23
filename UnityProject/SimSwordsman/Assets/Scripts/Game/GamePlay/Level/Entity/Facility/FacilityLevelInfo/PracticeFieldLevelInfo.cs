using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PracticeFieldLevelInfo : FacilityLevelInfo
    {
        private int m_CurCapacity;
        private int m_Duration;
        //private int m_NextCapacity;
        private int m_CurLevelUpSpeed;
        private int m_LevelUpSpeed;
        //private int m_NextLevelUpSpeed;
        private FacilityType m_HouseID;

        public void SetCurData(TDFacilityPracticeField tdData)
        {
            m_CurCapacity = tdData.capability;
            m_Duration = tdData.duration;
            m_CurLevelUpSpeed = tdData.levelUpSpeed;
            m_HouseID = (FacilityType)tdData.houseId;
            m_LevelUpSpeed = tdData.levelUpSpeed;
        }

        public int GetCurExp()
        {
            return m_CurLevelUpSpeed;
        }

        public int GetCurCapacity()
        {
            return m_CurCapacity;
        }

        public int GetCurLevelUpSpeed()
        {
            return m_LevelUpSpeed;
        }

        public int GetDuration()
        {
            return m_Duration;
        }

        public int GetNextCapacity()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_PRACTIVEFIELD);
            int capacity = TDFacilityPracticeFieldTable.GetLevelInfo(m_HouseID, realLevel).GetCurCapacity();
            return capacity;
        }

        public void SetCurLevelUpSpeed(int levelUpSpeed)
        {
            m_CurLevelUpSpeed = levelUpSpeed;
        }

        public void SetHouseID(FacilityType facilityType)
        {
            m_HouseID = facilityType;
        }

        public FacilityType GetHouseID()
        {
            return m_HouseID;
        }

        public int GetNextLevelUpSpeed()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_PRACTIVEFIELD);
            int levelUpSpeed = TDFacilityPracticeFieldTable.GetLevelInfo(m_HouseID, realLevel).GetCurLevelUpSpeed();
            return levelUpSpeed;
        }
    }

}