using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class KongfuReward : RewardBase
	{
		public KongfuReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            //Log.e("»ñµÃ" + m_Info.Name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KongfuType)m_KeyID), Count);
        }
        
		public override string RewardName()
		{
            return TDKongfuConfigTable.GetData(m_KeyID).kongfuName;
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
            return TDKongfuConfigTable.GetIconName((KongfuType)m_KeyID);
        }
    }
}