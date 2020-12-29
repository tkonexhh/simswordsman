using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class ArmsReward : RewardBase
	{
		public ArmsReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            MainGameMgr.S.InventoryMgr.AddItem(new ArmsItem((Arms)m_KeyID, Step.One), m_Count);
        }
        
		public override string RewardName()
		{
			return ((Arms)m_KeyID).ToString();
		}

		public override Sprite GetSprite()
		{
            Sprite sprite = null;// Resources.Load("UI/BoostItem/" + m_TDItem.icon, typeof(Sprite)) as Sprite;
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