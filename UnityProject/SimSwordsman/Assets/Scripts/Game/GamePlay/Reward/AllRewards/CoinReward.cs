using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
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
			GameDataMgr.S.GetPlayerData().AddCoin(m_Count);
		}

		public override string RewardName()
		{
			return "Coin";
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
	}
	
}