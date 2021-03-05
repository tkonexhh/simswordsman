using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class Exp_KongfuRweard : RewardBase
    {

        public Exp_KongfuRweard(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            Log.e("??��????�" + Count);
            //GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
        }

        public override string RewardName()
        {
            return "������";
        }

        public override string SpriteName()
        {
            return "";
        }
    }
}