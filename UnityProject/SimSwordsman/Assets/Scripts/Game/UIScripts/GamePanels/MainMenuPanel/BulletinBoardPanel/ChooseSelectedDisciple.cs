using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ChooseSelectedDisciple : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Button m_ChooseSelectedDisciple;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private GameObject m_SelectedImg;    
        [SerializeField]
        private GameObject m_Plus;
        [SerializeField]
        private GameObject m_LevelBg;
        private CharacterItem m_CharacterItem;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            RefreshPanelInfo();
            m_ChooseSelectedDisciple.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                EventSystem.S.Send(EventID.OnSelectedEvent, m_CharacterItem, false);
            });
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        public SelectedState GetSelelctedState()
        {
            return m_SelelctedState;
        }

        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_ChooseSelectedDisciple.enabled = true;
                    m_SelectedImg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
                    break;
                case SelectedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    m_SelectedImg.gameObject.SetActive(false);
                    m_ChooseSelectedDisciple.enabled = false;
                    break;
                default:
                    break;
            }
        }

        public bool IsHavaSame(CharacterItem characterItem)
        {
            if (m_CharacterItem != null && m_CharacterItem.id == characterItem.id)
                return true;
            else
            {
                Log.w("当前弟子未选中");
                return false;
            }
        }

        public void SetSelectedDisciple(CharacterItem characterItem,bool isSelected)
        {
            m_CharacterItem = characterItem;
            if (isSelected)
                m_SelelctedState = SelectedState.Selected;
            else
                m_SelelctedState = SelectedState.NotSelected;
            RefreshPanelInfo();
        }

        private void OnDisable()
        {

        }
    }
	
}	