using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class TowerCoinReward : RewardBase
    {
        public TowerCoinReward(int count) : base(RewardItemType.TowerCoin, null, count)
        {
        }

        public override void AcceptReward(int bonus = 1)
        {
            GameDataMgr.S.GetPlayerData().towerData.AddCoin(Count * bonus);
        }

        public override string RewardName()
        {
            return "伏魔币";
        }

        public override string SpriteName()
        {
            return "towercoin";
        }
    }

}