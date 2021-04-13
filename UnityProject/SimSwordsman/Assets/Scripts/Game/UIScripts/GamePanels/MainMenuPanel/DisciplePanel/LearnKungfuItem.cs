using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class LearnKungfuItem : MonoBehaviour
	{
        [SerializeField]
        private Text m_EquipName;  
        [SerializeField]
        private Text m_Number;  
        [SerializeField]
        private Image m_EquipBg;
        [SerializeField]
        private Image m_EquipNameBg;
        [SerializeField]
        private Text m_Class;
        [SerializeField]
        private GameObject m_State;
        [SerializeField]
        private Button m_SelectedBtn;
        [SerializeField]
        private Transform m_Pos;
        private KungfuItem m_KungfuItem;
        private CharacterItem m_CurDisciple = null;
        private List<Sprite> m_Sprites = null;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private bool isSelected = false;
        //private EquipmentItem m_CurEquipmentItem = null;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_KungfuItem = t as KungfuItem;

            m_CurDisciple = (CharacterItem)obj[0];
            m_Sprites = (List<Sprite>)obj[1];
            BindAddListenerEvent();
            RefreshPanelInfo();
            //m_CurEquipmentItem = t as EquipmentItem;
            //m_ArticlesName.text = m_CurEquipmentItem.Name;

            //m_Number.text = CommonUIMethod.GetItemNumber(m_CurEquipmentItem.Number);
        }

        private Sprite GetSprite(string name)
        {
           return  m_Sprites.Where(i => i.name.Equals(name)).FirstOrDefault();
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

            switch (GetKungfuQuality(m_KungfuItem.KungfuType))
            {
                case KungfuQuality.Normal:
                    m_EquipBg.sprite = GetSprite("Introduction");
                    break;
                case KungfuQuality.Master:
                    m_EquipBg.sprite = GetSprite("Advanced");
                    break;
                case KungfuQuality.Super:
                    m_EquipBg.sprite = GetSprite("Excellent");
                    break;
                default:
                    break;
            }
            m_EquipNameBg.sprite = GetSprite(GetIconName(m_KungfuItem.KungfuType));

            m_EquipName.text = m_KungfuItem.Name;
            m_Number.text = m_KungfuItem.Number.ToString();
        }
        public void IsSame(ItemBase itemBase)
        {
            if (itemBase.GetSortId() != m_KungfuItem.GetSortId())
            {
                m_SelelctedState = SelectedState.NotSelected;
                isSelected = false;
                RefreshPanelInfo();
            }
        }
        private string GetIconName(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
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
                EventSystem.S.Send(EventID.OnSelectedKungfuEvent, isSelected, m_KungfuItem, m_Pos);
              
            });
        }
	}
}