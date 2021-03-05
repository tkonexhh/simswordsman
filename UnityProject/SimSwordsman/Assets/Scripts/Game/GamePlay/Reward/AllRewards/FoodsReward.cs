using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class FoodsReward : RewardBase
    {
        public FoodsReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            Log.e("?????? -- ??????");
            //GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
        }

        public override string RewardName()
        {
            return "ʳ��";
            //return m_Info.Name;
        }

        public override string SpriteName()
        {
            return "";
        }
    }
}