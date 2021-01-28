using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
	public class Exp_KongfuRweard : RewardBase
	{

		public Exp_KongfuRweard(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            Log.e("获得功夫经验：" + Count);
            //GameDataMgr.S.GetPropsDbData().AddCountFromType(m_BoostType, count);
        }
        
		public override string RewardName()
		{
			return "功夫经验";
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
        public override string SpriteName()
        {
            return "";
        }
    }
}