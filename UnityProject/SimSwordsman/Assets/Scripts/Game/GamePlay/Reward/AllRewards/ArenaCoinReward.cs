using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ArenaCoinReward : RewardBase
    {
        public ArenaCoinReward(int count) : base(RewardItemType.TowerCoin, null, count)
        {
        }

        public override void AcceptReward(int bonus = 1)
        {
            GameDataMgr.S.GetPlayerData().arenaData.AddCoin(Count * bonus);
        }

        public override string RewardName()
        {
            return "擂台币";
        }

        public override string SpriteName()
        {
            return "towercoin";
        }

        public override string RewardTips()
        {
            return "可以用来在竞技场商店中购买道具";
        }
    }

}