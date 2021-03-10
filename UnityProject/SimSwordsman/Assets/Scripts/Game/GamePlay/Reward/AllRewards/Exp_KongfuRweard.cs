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

        public override void AcceptReward(int bonus = 1)
        {
            if (!m_KeyID.HasValue)
                return;

            var characterItem = MainGameMgr.S.CharacterMgr.GetCharacterController(m_KeyID.Value);
            characterItem?.CharacterModel.DistributionKungfuExp(Count * bonus);
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