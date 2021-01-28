using Qarth;
using System;
using UnityEngine;


namespace GameWish.Game
{
	public class ArmorReward : RewardBase
	{
		public ArmorReward(RewardItemType type, int id, int count) : base(type, id, count)
        {

        }

		public override void AcceptReward()
		{
            //Log.e("»ñµÃ" + m_Equip.Name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new ArmorItem((ArmorType)m_KeyID, Step.One), Count);
        }
        
		public override string RewardName()
		{
			return TDEquipmentConfigTable.GetData(m_KeyID).name;
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
            return TDEquipmentConfigTable.GetData(m_KeyID).iconName;
        }
    }
	
}