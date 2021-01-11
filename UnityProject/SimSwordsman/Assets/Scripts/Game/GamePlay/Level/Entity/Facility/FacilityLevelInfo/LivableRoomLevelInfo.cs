using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class LivableRoomLevelInfo : FacilityLevelInfo
	{
        public int roomId;

        private int m_CurCapacity;
        private int m_NextCapacity;

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
            int capacity = TDFacilityLivableRoomTable.GetLevelInfo(roomId, realLevel).GetCurCapacity();
            return capacity;
        }
	}
	
}