using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class EquipBelongingsItem : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_Plus;
		[SerializeField]
		private GameObject m_Lock;
		[SerializeField]
		private Image m_EquipBelongingsItemImg;
		[SerializeField]
		private Image m_ItemIcon;
		[SerializeField]
		private GameObject m_RedPoint;

		private CharacterItem m_CharacterItem = null;
		private bool m_IsUnlock = false;
		private CharaceterEquipment m_CharaceterEquipment;
		private BgColorType m_BgColorType;
		private PropType m_PropType;

		// Start is called before the first frame update
		public void OnInit(CharacterItem characterItem, bool isArmorUnlock, CharaceterEquipment characeterEquipment, BgColorType bgColorType, PropType propType)
		{
			m_CharacterItem = characterItem;
			m_IsUnlock = isArmorUnlock;
			m_BgColorType = bgColorType;
			m_PropType = propType;
			m_CharaceterEquipment = characeterEquipment;
			RefreshPanelInfo();
			RefreshBgColor(m_BgColorType);
			CheckEquipRedPoint();
		}

		private void CheckEquipRedPoint()
		{
			switch (m_PropType)
			{
				case PropType.Arms:
					m_RedPoint.SetActive(m_CharacterItem.CheckArms() || m_CharacterItem.CheckEquipStrengthen(m_CharaceterEquipment, false));
					break;
				case PropType.Armor:
					m_RedPoint.SetActive(m_CharacterItem.CheckArmor() || m_CharacterItem.CheckEquipStrengthen(m_CharaceterEquipment, false));
					break;
			}
			//m_RedPoint.SetActive(m_CurDisciple.CheckEquipStrengthen(m_CharaceterEquipment, false));
		}
		private void OnDestroy()
		{
		}
		private void RefreshPanelInfo()
        {
            if (m_IsUnlock)
            {
				//解锁
				

				if (m_CharaceterEquipment.IsHaveEquip())
				{
					//未装备
					m_Plus.SetActive(true);
				}
                else
                {
					//装备
					m_ItemIcon.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EquipmentAtlas, m_CharaceterEquipment.GetIconName());
				}
			}
            else
            {
				//未解锁
				m_Lock.SetActive(true);
			}
        }

		public void RefreshBgColor(BgColorType m_BgColorType)
        {
			switch (m_BgColorType)
			{
				case BgColorType.Black:
					m_EquipBelongingsItemImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg21");
					break;
				case BgColorType.White:
					m_EquipBelongingsItemImg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DisciplePanelAtlas, "DisciplePanel_Bg28");
					break;
			}
		}
    }
	
}