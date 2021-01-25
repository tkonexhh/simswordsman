using System;
using UnityEngine;


namespace GameWish.Game
{
	public class MedicineReward : RewardBase
	{
		public MedicineReward(RewardItemType type, int id, int count) : base(type, id, count)
        {
            //m_isInitSuccess = false;
        }

		public override void AcceptReward()
		{
            //Log.e("»ñµÃ" + m_Info.Name + m_Count);
            //MainGameMgr.S.MedicinalPowderMgr.AddHerb(m_KeyID, Count);
            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)m_KeyID, Count));
        }

		public override string RewardName()
		{
			return TDHerbConfigTable.GetData(m_KeyID).name;
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