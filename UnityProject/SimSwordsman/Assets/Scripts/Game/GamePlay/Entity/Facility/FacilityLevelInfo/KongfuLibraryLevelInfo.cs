using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuLibraryLevelInfo : FacilityLevelInfo
    {
        private List<KongfuType> m_UnlockedKongfuType = new List<KongfuType>();

        public List<KongfuType> GetCurLevelUnlockedKongfuList()
        {
            return m_UnlockedKongfuType;
        }

        public void SetCurLevelUnlockedKongfuList(List<KongfuType> list)
        {
            m_UnlockedKongfuType = list;
        }

        public List<KongfuType> GetNextLevelUnlockedKongfuList()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            return ((KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.KongfuLibrary, realLevel)).GetCurLevelUnlockedKongfuList();
        }
    }

}