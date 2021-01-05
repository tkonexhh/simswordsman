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
            //Log.e("���" + m_TDItem.name + m_Count);
            MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)m_KeyID), Count);
        }

		public override string RewardName()
		{
            return TDItemConfigTable.GetData(m_KeyID).name;
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
		//			m_BoostType = BoostType.Item_SmallBomb;//����Ĭ�Ͻ���
		//			Log.e("��ʹ�õ��߶�ȡ����");
		//			break;
		//	}
		//	m_TDItem = TDBoostItemsTable.GetBoostItemByType(m_BoostType);
		//}

	}
	
}