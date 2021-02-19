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
		private Image m_GoodsImg;
		[SerializeField]
		private Button m_GoodsBtn;
		[SerializeField]
		private Text m_Nums;	
		[SerializeField]
		private Image m_KungfuName;
		private WarehousePanel m_WarehousePanel;
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
		public void SetItemSprite(Sprite sprite)
		{
			m_GoodsImg.sprite = sprite;
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
				case PropType.Kungfu:
					KungfuItem kunfuItem = (KungfuItem)CurItemBase;
					return kunfuItem.IsHaveItem(_itemBase);
				default:
					return false;
            }
			return false;
        }
		private string GetIconName(KongfuType kungfuType)
		{
			return TDKongfuConfigTable.GetIconName(kungfuType);
		}
		private KungfuQuality GetKungfuQuality(KongfuType kungfuType)
		{
			return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
		}
		public void AddItemToWarehouse(ItemBase itemBase, Sprite sprite)
		{
			if (sprite != null)
			{
				if (itemBase.PropType == PropType.Kungfu)
				{
					switch (GetKungfuQuality((KongfuType)itemBase.GetSubName()))
					{
						case KungfuQuality.Normal:
							m_GoodsImg.sprite = m_WarehousePanel.FindSprite("Introduction");
							break;
						case KungfuQuality.Super:
							m_GoodsImg.sprite = m_WarehousePanel.FindSprite("Advanced");
							break;
						case KungfuQuality.Master:
							m_GoodsImg.sprite = m_WarehousePanel.FindSprite("Excellent");
							break;
						default:
							break;
					}
					m_KungfuName.sprite = m_WarehousePanel.FindSprite(GetIconName((KongfuType)itemBase.GetSubName()));
					m_KungfuName.gameObject.SetActive(true);
				}
                else
                {
					m_KungfuName.gameObject.SetActive(false);
					m_GoodsImg.sprite = sprite;
				}
			}
			if (itemBase != null)
			{
				IsHaveItem = true;
				m_GoodsBtn.onClick.RemoveAllListeners();
				CurItemBase = itemBase;
				m_GoodsBtn.onClick.AddListener(() => {
					AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

					UIMgr.S.OpenPanel(UIID.ItemDetailsPanel, CurItemBase, m_WarehousePanel); 
				});
				m_Nums.text = CurItemBase.Number.ToString();
			}
            else
				IsHaveItem = false;
		}

		public void RefreshNumber(int delta)
		{
			CurItemBase.Number -= delta;
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
			//CurItemBase = t as ItemBase;
			//m_GoodsBtn.onClick.AddListener(() => { UIMgr.S.OpenPanel(UIID.ItemDetailsPanel, CurItemBase); });
			//m_Nums.text = CurItemBase.Number.ToString();
			m_WarehousePanel = t as WarehousePanel;
		}

		public void GetSubType(ItemBase itemBase)
		{
           
        }

	}
}