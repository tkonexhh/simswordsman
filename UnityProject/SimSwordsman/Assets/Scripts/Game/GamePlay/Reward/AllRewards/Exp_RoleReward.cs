using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
	public class Exp_RoleReward : RewardBase
	{
		public Exp_RoleReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            Log.e("��õ��Ӿ��飺" + m_Count);
			//GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
		}
        
		public override string RewardName()
		{
			return "���Ӿ���";
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