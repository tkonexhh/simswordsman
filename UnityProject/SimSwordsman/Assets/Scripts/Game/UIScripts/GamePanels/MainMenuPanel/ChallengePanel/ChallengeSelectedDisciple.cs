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
        [SerializeField] private Button m_ChooseSelectedDisciple;
        [SerializeField] private Text m_Level;
        [SerializeField] private Image m_DiscipleHead;
        [SerializeField] private Text m_DiscipleName;
        [SerializeField] private GameObject m_SelectedImg;
        [SerializeField] private GameObject m_Plus;
        [SerializeField] private GameObject m_LevelBg;
        [SerializeField] private Image m_DiscipleLevelBg;
        [SerializeField] private Image m_Line;

        protected CharacterItem m_CharacterItem;
        private ChallengeChooseDisciple m_ChallengeChooseDisciple;

        private SelectedState m_SelelctedState = SelectedState.NotSelected;

        internal void Init(ChallengeChooseDisciple challengeChooseDisciple)
        {
            m_ChallengeChooseDisciple = challengeChooseDisciple;

            m_ChooseSelectedDisciple.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                EventSystem.S.Send(EventID.OnSelectedEvent, m_CharacterItem, false);
            });

            OnInit();
            RefreshPanelInfo();
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
                    m_Level.text = m_CharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    break;
                case SelectedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    m_SelectedImg.gameObject.SetActive(false);
                    m_ChooseSelectedDisciple.enabled = false;
                    m_Line.sprite = m_ChallengeChooseDisciple.FindSprite("Disciple_Line_Bule");
                    break;
                default:
                    break;
            }

            OnRefreshPanelInfo();
        }

        void LoadClanPrefabs(string prefabsName)
        {
            m_DiscipleHead.sprite = m_ChallengeChooseDisciple.FindSprite(prefabsName);
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
            {
                LoadClanPrefabs(CharacterMgr.GetLoadDiscipleName(m_CharacterItem));
                m_SelelctedState = SelectedState.Selected;
            }
            else
                m_SelelctedState = SelectedState.NotSelected;
            RefreshPanelInfo();
        }

        public SelectedState GetSelelctedState()
        {
            return m_SelelctedState;
        }

        protected virtual void OnInit() { }
        protected virtual void OnRefreshPanelInfo() { }
    }

}