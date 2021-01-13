using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuLibraryLevelInfo : FacilityLevelInfo
    {
        private int m_DurationOfCopying;
        private int m_SeatOfCopying;

        private List<KungfuType> m_UnlockedKongfuType = new List<KungfuType>();
        private List<KungFuPoolConfig> m_KungFuPool = new List<KungFuPoolConfig>();


        public List<KungfuType> GetCurLevelUnlockedKongfuList()
        {
            return m_UnlockedKongfuType;
        }

        public void SetInitData(List<KungfuType> list, KungFuPoolConfig kungFuPoolConfig, int duration, int seat)
        {
            m_UnlockedKongfuType = list;
            m_KungFuPool.Add(kungFuPoolConfig);
            m_DurationOfCopying = duration;
            m_SeatOfCopying = seat;
        }

        public List<KungfuType> GetNextLevelUnlockedKongfuList()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_LEVEL);
            return ((KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.KongfuLibrary, realLevel)).GetCurLevelUnlockedKongfuList();
        }
    }

    public class KungFuPoolConfig
    {
        public KungfuType Kungfu { set; get; }
        public int Number { set; get; }

        public KungFuPoolConfig(KungfuType kungfu, int number)
        {
            Kungfu = kungfu;
            Number = number;
        }
    }
}