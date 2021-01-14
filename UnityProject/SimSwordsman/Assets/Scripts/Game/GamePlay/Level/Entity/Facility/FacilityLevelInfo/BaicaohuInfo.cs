using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class BaicaohuInfo : FacilityLevelInfo
	{
        private List<MedicinalPowderType> m_CurUnlockMedicinalPowderTypeList;

        public void SetCurMedicinalPowderType(List<MedicinalPowderType> equipmentType)
        {
            m_CurUnlockMedicinalPowderTypeList = equipmentType;
        }

        public List<MedicinalPowderType> GetCurMedicinalPowderType()
        {
            return m_CurUnlockMedicinalPowderTypeList;
        }

        public List<MedicinalPowderType> GetNextUnlockMedicinalPowderType()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_BAICAOHU);
            List<MedicinalPowderType> type = TDFacilityBaicaohuTable.GetLevelInfo(realLevel).GetCurMedicinalPowderType();
            return type;
        }
    }
	
}