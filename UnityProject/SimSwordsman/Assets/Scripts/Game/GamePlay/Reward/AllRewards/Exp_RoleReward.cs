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

        public override void AcceptReward(int bonus = 1)
        {
            if (!m_KeyID.HasValue)
                return;

            var characterItem = MainGameMgr.S.CharacterMgr.GetCharacterController(m_KeyID.Value);
            characterItem.AddExp(Count * bonus);
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