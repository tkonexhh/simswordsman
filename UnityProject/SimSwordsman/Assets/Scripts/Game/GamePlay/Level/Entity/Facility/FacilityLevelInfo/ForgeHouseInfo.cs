using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class ForgeHouseInfo : FacilityLevelInfo
	{
        private List<EquipmentType> m_CurUnlockEquipmentTypeList;

        public void SetCurEquipmentType(List<EquipmentType> equipmentType)
        {
            m_CurUnlockEquipmentTypeList = equipmentType;
        }

        public List<EquipmentType> GetCurEquipmentType()
        {
            return m_CurUnlockEquipmentTypeList;
        }

        public List<EquipmentType> GetNextUnlockEquipmentType()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_PRACTIVEFIELD);
            List<EquipmentType> type = TDFacilityForgeHouseTable.GetLevelInfo(realLevel).GetCurEquipmentType();
            return type;
        }
    }
	
}