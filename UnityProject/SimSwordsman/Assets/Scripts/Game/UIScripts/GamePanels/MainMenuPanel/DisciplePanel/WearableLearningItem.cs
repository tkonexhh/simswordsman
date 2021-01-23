using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class WearableLearningItem : MonoBehaviour,ItemICom
	{

        [SerializeField]
        private Text m_ArticlesName;
        [SerializeField]
        private Text m_Class;
        [SerializeField]
        private Text m_Number;

        [SerializeField]
        private Button m_SelectedBtn;
        private ItemBase m_ItemBase;
        private CharacterItem m_CurDisciple = null;
        //private EquipmentItem m_CurEquipmentItem = null;


        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_ItemBase = t as ItemBase;
            m_CurDisciple = (CharacterItem)obj[0];
            BindAddListenerEvent();
            switch (m_ItemBase.PropType)
            {
                case PropType.Arms:
                    ArmsItem armsItem = m_ItemBase as ArmsItem;
                    m_Class.text = CommonUIMethod.GetClass((int)armsItem.ClassID);
                    break;
                case PropType.Armor:
                    ArmorItem armorItem = m_ItemBase as ArmorItem;
                    m_Class.text = CommonUIMethod.GetClass((int)armorItem.ClassID);
                    break;
                default:
                    break;
            }
            m_Number.text = m_ItemBase.Name;
            m_ArticlesName.text = m_ItemBase.Number.ToString() ;
            //m_CurEquipmentItem = t as EquipmentItem;
            //m_ArticlesName.text = m_CurEquipmentItem.Name;
         
            //m_Number.text = CommonUIMethod.GetItemNumber(m_CurEquipmentItem.Number);
        }

        private void BindAddListenerEvent()
        {
            m_SelectedBtn.onClick.AddListener(()=> {
                switch (m_ItemBase.PropType)
                {
                    case PropType.Arms:
                        MainGameMgr.S.InventoryMgr.AddItem(m_CurDisciple.GetEquipmentForType(PropType.Arms));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArms(m_ItemBase)); ;
                        break;
                    case PropType.Armor:
                        MainGameMgr.S.InventoryMgr.AddItem(m_CurDisciple.GetEquipmentForType(PropType.Armor));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArmor(m_ItemBase)); ;
                        break;
                    default:
                        break;
                }
                MainGameMgr.S.InventoryMgr.RemoveItem(m_ItemBase);
                EventSystem.S.Send(EventID.OnSelectedEquipSuccess);
                UIMgr.S.ClosePanelAsUIID(UIID.WearableLearningPanel);
            });
        }

        public void SetButtonEvent(Action<object> action)
        {
           
        }
	}
}