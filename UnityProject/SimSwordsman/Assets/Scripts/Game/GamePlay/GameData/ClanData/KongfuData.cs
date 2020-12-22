using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{

    [Serializable]
    public class KongfuData
    {
        public int kongfuLibraryLevel = 1;
        public List<KongfuType> unlockedKongfuTypeList = new List<KongfuType>();

        public KongfuData()
        {

        }

        public void UnlockKongfu(KongfuType kongfuType)
        {
            if (!unlockedKongfuTypeList.Contains(kongfuType))
            {
                unlockedKongfuTypeList.Add(kongfuType);
            }
        }
    }
}