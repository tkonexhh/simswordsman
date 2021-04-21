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
        [SerializeField] protected Button m_ChoosePanelDisciple;
        [SerializeField] private Text m_Level;
        [SerializeField] private Image m_DiscipleHead;
        [SerializeField] private Text m_DiscipleName;
        [SerializeField] private GameObject m_SelectedImg;
        [SerializeField] private Image m_DiscipleLevelBg;
        [SerializeField] private Image m_Line;


        protected CharacterItem m_CharacterItem;
        private ChallengeChooseDisciple m_ChallengeChooseDisciple;

        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        protected bool IsSelected = false;

        internal void Init(CharacterItem characterItem, ChallengeChooseDisciple challengeChoose)
        {
            m_CharacterItem = characterItem;
            m_ChallengeChooseDisciple = challengeChoose;
            BindAddListenerEvent();
            m_DiscipleName.text = m_CharacterItem.name;
            LoadClanPrefabs();
            OnInit();
            RefresPanelInfo();
        }

        void LoadClanPrefabs()
        {
            DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CharacterItem, transform, new Vector3(0, 20, 0), new Vector3(0.4f, 0.4f, 1));
        }

        private void RefresPanelInfo()
        {
            RefreshDiscipleColor();
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_SelectedImg.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_SelectedImg.SetActive(false);
                    m_Level.text = m_CharacterItem.level.ToString();
                    break;
                default:
                    break;
            }

            OnRefreshPanelInfo();
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


        protected virtual void BindAddListenerEvent()
        {
            m_ChoosePanelDisciple.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                IsSelected = !IsSelected;
                EventSystem.S.Send(EventID.OnSelectedEvent, m_CharacterItem, IsSelected);
            });
        }


        protected virtual void OnInit() { }
        protected virtual void OnRefreshPanelInfo() { }
    }
}