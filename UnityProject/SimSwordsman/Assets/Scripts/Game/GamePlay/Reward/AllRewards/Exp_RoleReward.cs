using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class Exp_RoleReward : RewardBase
    {
        public Exp_RoleReward(int id, int count) : base(RewardItemType.Exp_Role, id, count)
        {

        }

        public override void AcceptReward()
        {
            Log.e("��õ��Ӿ���?" + Count);
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