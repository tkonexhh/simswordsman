using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FoodsReward : RewardBase
    {

        public FoodsReward(int count) : base(RewardItemType.Food, null, count)
        {
        }

        public override void AcceptReward()
        {
            GameDataMgr.S.GetPlayerData().AddFoodNum(Count);
        }

        public override string RewardName()
        {
            return "Ê³Îï";
            //return m_Info.Name;
        }

        public override string SpriteName()
        {
            return "Baozi";
        }
    }
}