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
            Log.e("??¨´????ï“" + Count);
            //GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
        }

        public override string RewardName()
        {
            return "¹¦·ò¾­Ñé";
        }

        public override string SpriteName()
        {
            return "";
        }
    }
}