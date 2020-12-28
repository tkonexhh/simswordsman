using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuLibraryLevelInfo : FacilityLevelInfo
    {
        private List<KungfuType> m_UnlockedKongfuType = new List<KungfuType>();

        public List<KungfuType> GetCurLevelUnlockedKongfuList()
        {
            return m_UnlockedKongfuType;
        }

        public void SetCurLevelUnlockedKongfuList(List<KungfuType> list)
        {
            m_UnlockedKongfuType = list;
        }

        public List<KungfuType> GetNextLevelUnlockedKongfuList()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            return ((KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.KongfuLibrary, realLevel)).GetCurLevelUnlockedKongfuList();
        }
    }

}