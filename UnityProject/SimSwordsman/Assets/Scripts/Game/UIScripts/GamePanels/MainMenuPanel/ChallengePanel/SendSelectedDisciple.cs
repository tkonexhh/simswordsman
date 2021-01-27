using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class SendSelectedDisciple : MonoBehaviour
	{
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private Image m_LevelBg;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_Plus;
        private GameObject m_SelectedImg;
        private CharacterItem m_CharacterItem;
        private PanelType m_PanelType;
		private SelelctedState m_SelelctedState = SelelctedState.NotSelected;
		public void OnInit(PanelType panelType)
		{
			m_PanelType = panelType;
			BindAddListenerEvent();
			RefreshPanelTypeInfo();
		}

        private void BindAddListenerEvent()
        {
            m_Btn.onClick.AddListener(()=> {
                switch (m_PanelType)
                {
                    case PanelType.Task:
                        break;
                    case PanelType.Challenge:
                        EventSystem.S.Send(EventID.OnSendDiscipleDicEvent);
                        break;
                    default:
                        break;
                }
            });
		}
        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelelctedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
                    break;
                case SelelctedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void RefreshSelectedDisciple(CharacterItem characterItem)
        {
            m_CharacterItem = characterItem;
            if (m_CharacterItem == null)
                m_SelelctedState = SelelctedState.NotSelected;
            else
                m_SelelctedState = SelelctedState.Selected;
            RefreshPanelInfo();
        }
        private void RefreshPanelTypeInfo()
		{
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_Btn.enabled = false;
                    break;
                case PanelType.Challenge:
                    RefreshPanelInfo();
                    break;
                default:
                    break;
            }
        }
	}
	
}