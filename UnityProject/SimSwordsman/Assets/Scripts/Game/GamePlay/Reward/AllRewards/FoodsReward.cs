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

        public override void AcceptReward(int bonus = 1)
        {
            GameDataMgr.S.GetPlayerData().AddFoodNum(Count * bonus);
        }

        public override string RewardName()
        {
            return "ʳ��";
            //return m_Info.Name;
        }

        public override string SpriteName()
        {
            return "Baozi";
        }
    }
}