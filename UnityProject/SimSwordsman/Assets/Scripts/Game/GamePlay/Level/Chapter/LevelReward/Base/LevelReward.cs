using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public abstract class LevelReward
    {
        public LevelRewardType rewardType;
        public string[] paramStrs;

        public LevelReward(LevelRewardType rewardType, string[] paramStrs)
        {
            this.rewardType = rewardType;
            this.paramStrs = paramStrs;
        }

        public abstract void ApplyReward();
    }
}