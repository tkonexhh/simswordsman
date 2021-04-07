using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDDeliverTable
    {
        public static Dictionary<int, DeliverConfig> DeliverConfigDic = new Dictionary<int, DeliverConfig>();
        static void CompleteRowAdd(TDDeliver tdData)
        {
            DeliverConfigDic.Add(tdData.level,new DeliverConfig(tdData));
        }

        public class DeliverConfig
        {
            public int level;
            public List<RewardBase> normalReward = new List<RewardBase>();
            public List<RewardBase> RareReward = new List<RewardBase>();
            public int Duration;

            public DeliverConfig(TDDeliver tdData)
            {
                level = tdData.level;

                RewardBase levelReward = RewardMgr.S.GetRewardBase(rewards[i]);
                normalReward.Add(levelReward);
            }
        }
    }
}