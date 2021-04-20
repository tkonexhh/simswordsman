using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameWish.Game
{
    public class CollectItemSystem : MonoBehaviour, IMgr
    {
        private Dictionary<CollectedObjType, ItemTipeMono> m_ItemTipDic = new Dictionary<CollectedObjType, ItemTipeMono>();

        public void OnInit()
        {
            ItemTipeMono[] allRawMatItemList = FindObjectsOfType<ItemTipeMono>();
            allRawMatItemList.ToList().ForEach(i =>
            {
                m_ItemTipDic.Add(i.m_ItemTipsType, i);
            });
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

        }


        public ItemTipeMono GetRawMatItem(CollectedObjType collectedObjType)
        {
            if (m_ItemTipDic.ContainsKey(collectedObjType))
                return m_ItemTipDic[collectedObjType];

            return null;
        }
    }

}