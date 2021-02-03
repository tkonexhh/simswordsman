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
        private Text m_EquipName;  
        [SerializeField]
        private Text m_Number;  
        [SerializeField]
        private Image m_EquipHead;
        [SerializeField]
        private Text m_Class;
        [SerializeField]
        private GameObject m_State;
        [SerializeField]
        private Button m_SelectedBtn;
        [SerializeField]
        private Transform m_Pos;
        private ItemBase m_ItemBase;
        private CharacterItem m_CurDisciple = null;
        private Sprite m_Sprite = null;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private bool isSelected = false;
        //private EquipmentItem m_CurEquipmentItem = null;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_ItemBase = t as ItemBase;
            m_CurDisciple = (CharacterItem)obj[0];
            m_Sprite = (Sprite)obj[1];
            BindAddListenerEvent();
            RefreshPanelInfo();
         
            //m_CurEquipmentItem = t as EquipmentItem;
            //m_ArticlesName.text = m_CurEquipmentItem.Name;

            //m_Number.text = CommonUIMethod.GetItemNumber(m_CurEquipmentItem.Number);
        }
        private void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.SetActive(true);
                    //RefreshEquipInfo();
                    break;
                case SelectedState.NotSelected:
                    m_State.SetActive(false);
                    break;
                default:
                    break;
            }
            m_EquipHead.sprite = m_Sprite;
            m_EquipName.text = m_ItemBase.Name;
            m_Number.text = m_ItemBase.Number.ToString();
        }
        public void IsSame(ItemBase itemBase)
        {
            if (itemBase.GetSortId() != m_ItemBase.GetSortId())
            {
                m_SelelctedState = SelectedState.NotSelected;
                isSelected = false;
                RefreshPanelInfo();
            }
        }

        private void BindAddListenerEvent()
        {
            m_SelectedBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                isSelected = !isSelected;
                if (isSelected)
                    m_SelelctedState = SelectedState.Selected;
                else
                    m_SelelctedState = SelectedState.NotSelected;
                RefreshPanelInfo();
                EventSystem.S.Send(EventID.OnSelectedEquipEvent, isSelected, m_ItemBase, m_Pos);
              
            });
        }

       

        public void SetButtonEvent(Action<object> action)
        {
           
        }
	}
}