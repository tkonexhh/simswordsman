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
        public List<KungfuType> unlockedKongfuTypeList = new List<KungfuType>();

        public KongfuData()
        {

        }

        public void UnlockKongfu(KungfuType kongfuType)
        {
            if (!unlockedKongfuTypeList.Contains(kongfuType))
            {
                unlockedKongfuTypeList.Add(kongfuType);
            }
        }
    }
}