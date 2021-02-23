using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class KongfuLibraryMgr : MonoBehaviour, IMgr
    {
        private List<KungfuType> m_UnlockedKongfuList = new List<KungfuType>();

        #region IMgr
        public void OnInit()
        {
            m_UnlockedKongfuList = GameDataMgr.S.GetClanData().kongfuData.unlockedKongfuTypeList;
        }

        public void OnDestroyed()
        {
        }

        public void OnUpdate()
        {
        }

        #endregion

        #region Public

        public List<KungfuType> GetAllUnlockedKongfuList()
        {
            return m_UnlockedKongfuList;
        }

        public void UnlockKongfu(KungfuType kongfuType)
        {
            if (!m_UnlockedKongfuList.Contains(kongfuType))
            {
                m_UnlockedKongfuList.Add(kongfuType);
            }

            GameDataMgr.S.GetClanData().UnlockKongfu(kongfuType);
        }

        #endregion
    }

}