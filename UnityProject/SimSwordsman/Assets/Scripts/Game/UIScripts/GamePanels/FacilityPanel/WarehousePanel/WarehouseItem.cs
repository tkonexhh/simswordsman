using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;

namespace GameWish.Game
{
	public class WarehouseItem : MonoBehaviour,ItemICom
	{
		[SerializeField]
		private Button m_GoodsBtn;
		[SerializeField]
		private Text m_Nums;

		public ItemBase CurItemBase { set; get; }
		private bool m_IsHaveItem = false;
		public bool IsHaveItem {
			set 
			{
				m_IsHaveItem = value;
				if (!m_IsHaveItem)
					CurItemBase = null;
				m_GoodsBtn.gameObject.SetActive(m_IsHaveItem);
			}
			get
			{
				return m_IsHaveItem;
			}
		}

		public bool IsSameItemBase(ItemBase _itemBase)
		{
            if (_itemBase==null)
            {

            }
			if (_itemBase.PropType != CurItemBase.PropType)
				return false;
            switch (_itemBase.PropType)
            {
                case PropType.None:
                    break;
                case PropType.RawMaterial:
					PropItem propItem =(PropItem)CurItemBase;
					return propItem.IsHaveItem(_itemBase);
                case PropType.Armor:
					ArmorItem armorItem = (ArmorItem)CurItemBase;
					return armorItem.IsHaveItem(_itemBase);
                case PropType.Arms:
					ArmsItem armsItem = (ArmsItem)CurItemBase;
					return armsItem.IsHaveItem(_itemBase);
                default:
					return false;
            }
			return false;
        }

		public void AddItemToWarehouse(ItemBase itemBase)
		{
            if (itemBase!=null)
            {
				IsHaveItem = true;
				m_GoodsBtn.onClick.RemoveAllListeners();
				CurItemBase = itemBase;
				m_GoodsBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.ItemDetailsPanel, CurItemBase); });
				m_Nums.text = CurItemBase.Number.ToString();
			}
            else
				IsHaveItem = false;
		}

		public void RefreshNumber()
		{
			if (CurItemBase.Number <= 0)
			{
				IsHaveItem = false;
				return;
			}
			m_Nums.text = CurItemBase.Number.ToString();
		}


        public void SetButtonEvent(Action<object> action)
        {
        }

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
			CurItemBase = t as ItemBase;
			m_GoodsBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.ItemDetailsPanel, CurItemBase); });
			m_Nums.text = CurItemBase.Number.ToString();
		}

		public void GetSubType(ItemBase itemBase)
		{
           
        }

	}
}