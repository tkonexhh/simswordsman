using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class WarehouseLevelInfo : FacilityLevelInfo
    {
        private int m_Reserves;

        public int GetCurReserves()
        {
            return m_Reserves;
        }

        public void SetReserves(int reserves)
        {
            m_Reserves = reserves;
        }

    }

}