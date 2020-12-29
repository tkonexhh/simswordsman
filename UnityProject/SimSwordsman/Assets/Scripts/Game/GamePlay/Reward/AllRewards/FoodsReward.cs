using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class FoodsReward : RewardBase
	{
		public FoodsReward(RewardItemType type, int id, int count) : base(type, id, count)
        {
            m_isInitSuccess = false;
        }

		public override void AcceptReward()
		{
			//GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
		}

		public override string RewardCount()
		{
			return m_Count.ToString();
		}

		public override string RewardName()
		{
            return null;
            //return m_Info.Name;
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