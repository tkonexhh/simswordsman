using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public abstract class LevelReward
    {
        public RewardItemType rewardType;
        public string[] paramStrs;

        public LevelReward(RewardItemType rewardType, string[] paramStrs)
        {
            this.rewardType = rewardType;
            this.paramStrs = paramStrs;
        }

        public abstract void ApplyReward(int pas);

        public abstract int GetRewardValue();
    }
}