using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ChallengeSelectedDisciple : MonoBehaviour
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
        private SelelctedState m_SelelctedState = SelelctedState.NotSelected;

        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelelctedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_ChooseSelectedDisciple.enabled = true;
                    m_SelectedImg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
                    break;
                case SelelctedState.NotSelected:
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
        public void OnInit(ChallengeChooseDisciple challengeChooseDisciple)
        {
            RefreshPanelInfo();
            m_ChooseSelectedDisciple.onClick.AddListener(() => {
                EventSystem.S.Send(EventID.OnSelectedDiscipleEvent, m_CharacterItem, false);
            });
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

        public void SetSelectedDisciple(CharacterItem characterItem, bool isSelected)
        {
            m_CharacterItem = characterItem;
            if (isSelected)
                m_SelelctedState = SelelctedState.Selected;
            else
                m_SelelctedState = SelelctedState.NotSelected;
            RefreshPanelInfo();
        }

        public SelelctedState GetSelelctedState()
        {
            return m_SelelctedState;
        }
    }
	
}