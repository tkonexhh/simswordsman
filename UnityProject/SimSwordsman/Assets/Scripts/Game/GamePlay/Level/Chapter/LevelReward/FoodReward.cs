using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class FoodReward : LevelReward
    {
        private int m_FoodValue;

        public FoodReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_FoodValue = int.Parse(paramStrs[1]);
        }

        public override void ApplyReward(int par)
        {
            GameDataMgr.S.GetPlayerData().AddFoodNum(m_FoodValue);
        }
    }

}