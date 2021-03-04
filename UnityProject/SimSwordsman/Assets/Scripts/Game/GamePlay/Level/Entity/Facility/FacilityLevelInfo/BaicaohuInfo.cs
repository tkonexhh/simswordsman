using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class BaicaohuInfo : FacilityLevelInfo
	{
        private HerbType m_CurUnlockHerbType;

        public void SetCurMedicinalPowderType(HerbType equipmentType)
        {
            m_CurUnlockHerbType = equipmentType;
        }

        public string GetCurMedicinalPowderName()
        {
            return TDHerbConfigTable.GetHerbNameById((int)m_CurUnlockHerbType);
        }
        public HerbType GetCurMedicinalPowderType()
        {
            return m_CurUnlockHerbType;
        }
    }
	
}