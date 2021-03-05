using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class Exp_RoleReward : RewardBase
    {
        public Exp_RoleReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            Log.e("��õ��Ӿ��飺" + Count);
            //GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
        }

        public override string RewardName()
        {
            return "弟子经验";
        }

        public override string SpriteName()
        {
            return "";
        }
    }
}