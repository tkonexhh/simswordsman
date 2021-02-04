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

		private SelectedState m_SelelctedState = SelectedState.NotSelected;
		private bool IsSelected = false;

        internal void OnInit(CharacterItem characterItem)
        {
			m_CharacterItem = characterItem;
			BindAddListenerEvent();

			LoadClanPrefabs(GetLoadDiscipleName(m_CharacterItem));
			RefresPanelInfo();
		}
		public void LoadClanPrefabs(string prefabsName)
		{
			AddressableAssetLoader<Sprite> loader = new AddressableAssetLoader<Sprite>();
			loader.LoadAssetAsync(prefabsName, (obj) =>
			{
				//Debug.Log(obj);
				m_DiscipleHead.sprite = obj;
			});
		}
		private string GetLoadDiscipleName(CharacterItem characterItem)
		{
			return "head_"+characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
		}
		private void RefresPanelInfo()
        {
			switch (m_SelelctedState)
			{
				case SelectedState.Selected:
					m_SelectedImg.SetActive(true);
					m_DiscipleName.text = m_CharacterItem.name;
					break;
				case SelectedState.NotSelected:
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
				m_SelelctedState = SelectedState.Selected;
			}
			else
			{
				IsSelected = false;
				m_SelelctedState = SelectedState.NotSelected;
			}
			RefresPanelInfo();
		}


		private void BindAddListenerEvent()
		{
			m_ChoosePanelDisciple.onClick.AddListener(() => {
				AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

				IsSelected = !IsSelected;
                EventSystem.S.Send(EventID.OnSelectedEvent, m_CharacterItem, IsSelected);
            });
		}
	}
}