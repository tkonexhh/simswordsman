using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class ItemReward : RewardBase
	{
		public ItemReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            //Log.e("»ñµÃ" + m_TDItem.name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)m_KeyID), Count);

            AudioManager.S.PlayCollectSound(m_KeyID);
        }

		public override string RewardName()
		{
            return TDItemConfigTable.GetData(m_KeyID).name;
		}

		public override Sprite GetSprite()
		{
            Sprite sprite = Resources.Load<Sprite>("UI/Sprites/ItemIcon/" + TDItemConfigTable.GetData(m_KeyID).iconName);
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
            return TDItemConfigTable.GetData(m_KeyID).iconName;
        }
    }
}