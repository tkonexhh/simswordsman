using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class PracticeFieldLevelInfo : FacilityLevelInfo
	{
        private int m_CurCapacity;
        //private int m_NextCapacity;
        private int m_CurLevelUpSpeed;
        //private int m_NextLevelUpSpeed;
        private FacilityType m_HouseID;

        public void SetCurCapatity(int capacity)
        {
            m_CurCapacity = capacity;
        }

        public int GetCurCapacity()
        {
            return m_CurCapacity;
        }

        public int GetNextCapacity()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            int capacity = TDFacilityPracticeFieldTable.GetLevelInfo(m_HouseID,realLevel).GetCurCapacity();
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

        public int GetCurLevelUpSpeed()
        {
            return m_CurLevelUpSpeed;
        }

        public int GetNextLevelUpSpeed()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            int levelUpSpeed = TDFacilityPracticeFieldTable.GetLevelInfo(m_HouseID,realLevel).GetCurLevelUpSpeed();
            return levelUpSpeed;
        }
    }
	
}