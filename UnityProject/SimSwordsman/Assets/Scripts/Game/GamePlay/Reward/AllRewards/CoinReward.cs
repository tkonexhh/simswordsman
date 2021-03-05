using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
    public class CoinReward : RewardBase
    {
        public CoinReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

        public override void AcceptReward()
        {
            GameDataMgr.S.GetPlayerData().AddCoinNum(Count);
        }

        public override string RewardName()
        {
            return "ͭǮ";
        }

        public override string SpriteName()
        {
            return "Coin";
        }
    }

}