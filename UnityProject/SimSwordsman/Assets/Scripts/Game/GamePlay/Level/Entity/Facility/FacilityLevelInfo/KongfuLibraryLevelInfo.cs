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

        private List<KongfuType> m_UnlockedKongfuType = new List<KongfuType>();
        private List<KungFuPoolConfig> m_KungFuPool = new List<KungFuPoolConfig>();

        public int GetCurCapacity()
        {
            return m_SeatOfCopying;
        }

        public int GetDurationOfCopying()
        {
            return m_DurationOfCopying;
        }

        public List<KungFuPoolConfig> GetKungFuPoolConfig()
        {
            return m_KungFuPool;
        }


        public List<KongfuType> GetCurLevelUnlockedKongfuList()
        {
            return m_UnlockedKongfuType;
        }

        public void SetInitData(List<KongfuType> list, KungFuPoolConfig kungFuPoolConfig, int duration, int seat)
        {
            m_UnlockedKongfuType = list;
            m_KungFuPool.Add(kungFuPoolConfig);
            m_DurationOfCopying = duration;
            m_SeatOfCopying = seat;
        }

        public List<KongfuType> GetNextLevelUnlockedKongfuList()
        {
            int realLevel = Mathf.Min(level + 1, Define.FACILITY_MAX_KUNGFULIBRARY);
            return ((KongfuLibraryLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(FacilityType.KongfuLibrary, realLevel)).GetCurLevelUnlockedKongfuList();
        }
    }

    public class KungFuPoolConfig
    {
        public KongfuType Kungfu { set; get; }
        public int Weight { set; get; }

        public KungFuPoolConfig(KongfuType kungfu, int number)
        {
            Kungfu = kungfu;
            Weight = number;
        }
    }
}