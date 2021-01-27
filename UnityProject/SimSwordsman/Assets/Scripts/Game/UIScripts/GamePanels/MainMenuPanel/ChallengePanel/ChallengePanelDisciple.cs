using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ChallengePanelDisciple : MonoBehaviour
	{
		[SerializeField]
		private Button m_ChoosePanelDisciple;
		[SerializeField]
		private Text m_Level;
		[SerializeField]
		private Image m_DiscipleHead;
		[SerializeField]
		private Text m_DiscipleName;
		[SerializeField]
		private GameObject m_SelectedImg;

		private CharacterItem m_CharacterItem;

		private SelelctedState m_SelelctedState = SelelctedState.NotSelected;
		private bool IsSelected = false;

        internal void OnInit(CharacterItem characterItem)
        {
			m_CharacterItem = characterItem;
			BindAddListenerEvent();

			RefresPanelInfo();
		}

        private void RefresPanelInfo()
        {
			switch (m_SelelctedState)
			{
				case SelelctedState.Selected:
					m_SelectedImg.SetActive(true);
					m_DiscipleName.text = m_CharacterItem.name;
					break;
				case SelelctedState.NotSelected:
					m_SelectedImg.SetActive(false);
					m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
					m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
					break;
				default:
					break;
			}
		}

		public bool IsHavaSameDisciple(CharacterItem characterItem)
		{
			if (characterItem.id == m_CharacterItem.id)
				return true;
			return false;
		}

		public void SetItemState(bool isHave)
		{
			if (isHave)
			{
				IsSelected = true;
				m_SelelctedState = SelelctedState.Selected;
			}
			else
			{
				IsSelected = false;
				m_SelelctedState = SelelctedState.NotSelected;
			}
			RefresPanelInfo();
		}


		private void BindAddListenerEvent()
		{
			m_ChoosePanelDisciple.onClick.AddListener(() => {
				IsSelected = !IsSelected;
                EventSystem.S.Send(EventID.OnSelectedDiscipleEvent, m_CharacterItem, IsSelected);
            });
		}
	}
}