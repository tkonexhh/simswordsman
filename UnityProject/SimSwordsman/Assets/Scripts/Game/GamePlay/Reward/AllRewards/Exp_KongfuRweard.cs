using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class Exp_KongfuRweard : RewardBase
    {

        public Exp_KongfuRweard(int id, int count) : base(RewardItemType.Exp_Kongfu, id, count)
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