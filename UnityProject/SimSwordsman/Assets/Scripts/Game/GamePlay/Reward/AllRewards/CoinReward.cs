using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class CoinReward : RewardBase
    {
        public CoinReward(int count) : base(RewardItemType.Coin, null, count)
        {
        }

        public override void AcceptReward(int bonus = 1)
        {
            GameDataMgr.S.GetPlayerData().AddCoinNum(Count * bonus);
        }

        public override string RewardName()
        {
            return "金钱";
        }

        public override string SpriteName()
        {
            return "Coin";
        }
    }
}