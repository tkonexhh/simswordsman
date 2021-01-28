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
			return "铜钱";
		}

		public override Sprite GetSprite()
		{
            Sprite sprite = Resources.Load("Sprites/MainMenu/Coin", typeof(Sprite)) as Sprite;
            return sprite;
        }

        public override void GetSpriteAsyn(GetSpriteCallBack callBack)
        {
            
        }

        public override void SetCallBackAction(Action action)
		{

		}

        public override string SpriteName()
        {
            return "Coin";
        }
    }
	
}