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
            int capacity = TDFacilityLivableRoomTable.GetLevelInfo(realLevel).GetCurCapacity();
            return capacity;
        }

        public void SetCurLevelUpSpeed(int levelUpSpeed)
        {
            m_CurLevelUpSpeed = levelUpSpeed;
        }

        public int GetCurLevelUpSpeed()
        {
            return m_CurLevelUpSpeed;
        }

        public int GetNextLevelUpSpeed()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            int levelUpSpeed = TDFacilityPracticeFieldTable.GetLevelInfo(realLevel).GetCurLevelUpSpeed();
            return levelUpSpeed;
        }
    }
	
}