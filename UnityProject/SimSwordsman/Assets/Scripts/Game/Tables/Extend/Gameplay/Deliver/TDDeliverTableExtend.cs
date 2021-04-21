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

        public static Dictionary<int, DeliverConfig>.ValueCollection GetDeliverConfigList()
        {
            return DeliverConfigDic.Values;
        }
        public static DeliverConfig GetDeliverConfig(int deliverID)
        {
            if (DeliverConfigDic.ContainsKey(deliverID))
            {
                return DeliverConfigDic[deliverID];
            }
            Log.e("Œ¥’“µΩ≥µ∂”ID,ID = " + deliverID);
            return null;
        }

    }
    public class DeliverConfig
    {
        public int level;
        public List<RewardBase> normalReward = new List<RewardBase>();
        public List<RewardBase> RareReward = new List<RewardBase>();
        public int duration;
        public string name;

        public DeliverConfig(TDDeliver tdData)
        {
            level = tdData.level;

            string[] normalRewardStr = tdData.normalReward.Split(';');
            for (int i = 0; i < normalRewardStr.Length; i++)
            {
                RewardBase levelReward = RewardMgr.S.GetRewardBase(normalRewardStr[i]);
                normalReward.Add(levelReward);
            }

            string[] specialRewardStr = tdData.rareReward.Split(';');
            for (int i = 0; i < specialRewardStr.Length; i++)
            {
                RewardBase levelReward = RewardMgr.S.GetRewardBase(specialRewardStr[i]);
                RareReward.Add(levelReward);
            }

            duration = tdData.duration;
            name = tdData.name;
        }
    }
}