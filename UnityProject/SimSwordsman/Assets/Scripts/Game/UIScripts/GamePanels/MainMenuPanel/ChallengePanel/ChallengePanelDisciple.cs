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
		[SerializeField]
		private Image m_DiscipleLevelBg;
		[SerializeField]
		private Image m_Line;
		private AddressableAssetLoader<Sprite> m_Loader;

		private CharacterItem m_CharacterItem;
		private ChallengeChooseDisciple m_ChallengeChooseDisciple;

		private SelectedState m_SelelctedState = SelectedState.NotSelected;
		private bool IsSelected = false;

        internal void OnInit(CharacterItem characterItem, ChallengeChooseDisciple challengeChoose)
        {
			m_CharacterItem = characterItem;
			m_ChallengeChooseDisciple = challengeChoose;
			BindAddListenerEvent();

			LoadClanPrefabs(GetLoadDiscipleName(m_CharacterItem));
			RefresPanelInfo();
		}   
		public void LoadClanPrefabs(string prefabsName)
		{
			m_DiscipleHead.sprite = m_ChallengeChooseDisciple.FindSprite(prefabsName);
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
					RefreshDiscipleColor();
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

        private void RefreshDiscipleColor()
        {
			switch (m_CharacterItem.quality)
			{
				case CharacterQuality.Normal:
					m_DiscipleLevelBg.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_FontBg_Blue");
					m_Line.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_Line_Bule");
					break;
				case CharacterQuality.Good:
					m_DiscipleLevelBg.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_FontBg_Yellow");
					m_Line.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_Line_Yellow");
					break;
				case CharacterQuality.Perfect:
					m_DiscipleLevelBg.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_FontBg_Red");
					m_Line.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_Line_Red");
					break;
				default:
					break;
			}
		}

        private void OnDestroy()
        {
			if (m_Loader != null)
			{
				m_Loader.Release();
			}

		}
		private void OnDisable()
        {
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