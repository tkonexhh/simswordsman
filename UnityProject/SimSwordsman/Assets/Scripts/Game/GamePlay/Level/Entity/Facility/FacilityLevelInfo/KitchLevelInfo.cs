using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class KitchLevelInfo : FacilityLevelInfo
	{
        private int m_CurFoodLimit;
        private int m_CurFoodAddSpeed;

        public void SetCurFoodLimit(int foodLimit)
        {
            m_CurFoodLimit = foodLimit;
        }

        public int GetCurFoodLimit()
        {
            return m_CurFoodLimit;
        }

        public void SetCurAddSpeed(int addSpeed)
        {
            m_CurFoodAddSpeed = addSpeed;
        }

        public int GetCurFoodAddSpeed()
        {
            return m_CurFoodAddSpeed;
        }

        public int GetNextFoodLimit()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            int capacity = TDFacilityKitchenTable.GetLevelInfo(realLevel).GetCurFoodLimit();
            return capacity;
        }

        public int GetNextFoodAddSpeed()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            int capacity = TDFacilityKitchenTable.GetLevelInfo(realLevel).GetCurFoodAddSpeed();
            return capacity;
        }
    }
	
}