using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace GameWish.Game
{

    [Serializable]
    public class RawMatItemData
    {
        public CollectedObjType collectObjType;
        public string lastShowBubbleTime;
        public int collectTime;

        public RawMatItemData()
        {

        }

        public RawMatItemData(CollectedObjType collectedObj, string time)
        {
            collectObjType = collectedObj;
            lastShowBubbleTime = time;
        }

    }
}