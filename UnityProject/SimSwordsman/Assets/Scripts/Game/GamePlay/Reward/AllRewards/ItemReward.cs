using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class ItemReward : RewardBase
	{
		private PropConfigInfo m_TDItem;

		public ItemReward(RewardItemType type, int id, int count) : base(type, id, count)
        {
            m_isInitSuccess = false;
            m_TDItem = TDItemConfigTable.GetPropConfigInfo(id);
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
			return m_TDItem.name;
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

		//private void CheckMyBoostType()
		//{
		//	switch (m_KeyID)
		//	{
		//		case "Bottle_s":
		//			m_BoostType = BoostType.Item_HorizontalAndVertical;
		//			break;
		//		case "Bottle_b":
		//			m_BoostType = BoostType.Item_BigBomb;
		//			break;
		//		case "Umbrella":
		//			m_BoostType = BoostType.Item_Umbrella;
		//			break;
		//		case "Hammer":
		//			m_BoostType = BoostType.ExplodeArea;
		//			break;
		//		case "Scissors":
		//			m_BoostType = BoostType.CrossClear;
		//			break;
		//		case "Hand":
		//			m_BoostType = BoostType.FreeMove;
		//			break;
		//		default:
		//			m_BoostType = BoostType.Item_SmallBomb;//给了默认奖励
		//			Log.e("可使用道具读取出错");
		//			break;
		//	}
		//	m_TDItem = TDBoostItemsTable.GetBoostItemByType(m_BoostType);
		//}

	}
	
}