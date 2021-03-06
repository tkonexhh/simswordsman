using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PracticeDisciple : MonoBehaviour
    {
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_LevelBg;
        //[SerializeField]
        //private Image m_DiscipleHead;
        [SerializeField]
        private Image m_State;
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private Transform m_Pos;
        [SerializeField]
        private Image m_DiscipleLevelBg;
        [SerializeField]
        private Image m_Line;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private CharacterItem m_CharacterItem;

        private ChooseDisciplePanel m_ChooseDisciplePanel;
        private bool isSelected = false;
        public void OnInit(CharacterItem characterItem, ChooseDisciplePanel chooseDisciplePanel)
        {
            m_CharacterItem = characterItem;
            m_ChooseDisciplePanel = chooseDisciplePanel;
            m_Btn.onClick.AddListener(() =>
            {

                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                isSelected = !isSelected;
                if (isSelected)
                    m_SelelctedState = SelectedState.Selected;
                else
                    m_SelelctedState = SelectedState.NotSelected;
                RefreshPanelInfo();
                EventSystem.S.Send(EventID.OnSelectedEvent, isSelected, m_CharacterItem, m_Pos);
            });
            DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CharacterItem, m_LevelBg.transform, new Vector3(45.9f, -29, 0), new Vector3(0.4f, 0.4f, 1));

            //m_DiscipleHead.sprite = m_ChooseDisciplePanel.FindSprite(CharacterMgr.GetLoadDiscipleName(m_CharacterItem));
            RefreshPanelInfo();
        }

        private void RefreshPanelInfo()
        {
            m_Level.text = m_CharacterItem.level.ToString();
            m_DiscipleName.text = m_CharacterItem.name;
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_DiscipleLevelBg.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_FontBg_Blue");
                    m_Line.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_Line_Bule");
                    break;
                case CharacterQuality.Good:
                    m_DiscipleLevelBg.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_FontBg_Yellow");
                    m_Line.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_Line_Yellow");
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleLevelBg.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_FontBg_Red");
                    m_Line.sprite = m_ChooseDisciplePanel.FindSprite("Disciple_Line_Red");
                    break;
                default:
                    break;
            }
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.gameObject.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_State.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void IsSame(CharacterItem characterItem)
        {
            if (characterItem.id != m_CharacterItem.id)
            {
                m_SelelctedState = SelectedState.NotSelected;
                isSelected = false;
                RefreshPanelInfo();
            }
        }
        // Start is called before the first frame update
    }

}