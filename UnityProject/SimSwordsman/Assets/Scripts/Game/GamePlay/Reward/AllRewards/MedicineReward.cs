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
            //Log.e("���" + m_Info.Name + m_Count);
            //MainGameMgr.S.MedicinalPowderMgr.AddHerb(m_KeyID, Count);
            MainGameMgr.S.InventoryMgr.AddItem(new HerbItem((HerbType)m_KeyID, Count));
        }

		public override string RewardName()
		{
			return TDHerbConfigTable.GetData(m_KeyID).name;
		}

		public override Sprite GetSprite()
		{
            Sprite sprite = Resources.Load<Sprite>("UI/Sprites/ItemIcon/" + TDHerbConfigTable.GetData(m_KeyID).icon);
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
            return TDHerbConfigTable.GetData(m_KeyID).icon;
        }
    }
}