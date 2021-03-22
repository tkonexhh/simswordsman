using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
    public class RawMatCollectSystem : MonoBehaviour, IMgr
    {
        private Dictionary<CollectedObjType, RawMatItem> m_RawMatItemDic = new Dictionary<CollectedObjType, RawMatItem>();
        private float m_RefreshInterval = 1;
        private float m_RefreshTime = 0;

        #region IMgr

        public void OnInit()
        {
            RawMatItem[] allRawMatItemList = FindObjectsOfType<RawMatItem>();
            allRawMatItemList.ToList().ForEach(i =>
            {
                m_RawMatItemDic.Add(i.collectedObjType, i);
                i.OnInit();
            });
        }

        public void InitState()
        {
            m_RawMatItemDic.Values.ToList().ForEach(i =>
            {
                i.InitState();
            });
        }

        public void OnUpdate()
        {
            //m_RefreshTime += Time.deltaTime;
            //if (m_RefreshTime > m_RefreshInterval)
            //{
            //    m_RefreshTime = 0;
            //    m_RawMatItemDic.Values.ToList().ForEach(i => i.Refresh());
            //}
            m_RawMatItemDic.Values.ToList().ForEach(i => i.OnUpdate());
        }

        public void OnDestroyed()
        {
        }

        #endregion

        public RawMatItem GetRawMatItem(CollectedObjType collectedObjType)
        {
            if (m_RawMatItemDic.ContainsKey(collectedObjType))
                return m_RawMatItemDic[collectedObjType];

            return null;
        }

    }

}