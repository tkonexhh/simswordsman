using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class TacticalFunctionDisciple : MonoBehaviour
    {
        [SerializeField]
        private Image m_LevelBg;
        [SerializeField]
        private Image m_HeadImg;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private GameObject m_Plus;


        private CharacterItem m_CurCharacterItem;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private TacticalFunctionPanel m_TacticalFunctionPanel;

        #region Public

        public void OnInit(TacticalFunctionPanel tacticalFunctionPanel)
        {
            m_TacticalFunctionPanel = tacticalFunctionPanel;
            RefreshInfo();
        }

        public void SelectedDisciple(CharacterItem characterItem)
        {
            m_CurCharacterItem = characterItem;
            m_SelelctedState = SelectedState.Selected;
            RefreshInfo();
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        public void RefreshInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_HeadImg.sprite = m_TacticalFunctionPanel.FindSprite(GetLoadDiscipleName(m_CurCharacterItem));
                    m_Level.text = m_CurCharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    m_Plus.SetActive(false);
                    m_LevelBg.gameObject.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_Plus.SetActive(true);
                    m_LevelBg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

        }
        #endregion
        #region Private
        private void Start()
        {
            m_Btn.onClick.AddListener(() =>
            {
                EventSystem.S.Send(EventID.OnDiscipleButtonOnClick, PanelType.Task);

            });
        }

        private void RefreshDiscipleColor()
        {
            switch (m_CurCharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_LevelBg.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_FontBg_Blue");
                    //m_Line.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_Line_Bule");
                    break;
                case CharacterQuality.Good:
                    m_LevelBg.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_FontBg_Yellow");
                    //m_Line.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_Line_Yellow");
                    break;
                case CharacterQuality.Perfect:
                    m_LevelBg.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_FontBg_Red");
                    //m_Line.sprite = m_TacticalFunctionPanel.FindSprite("Disciple_Line_Red");
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}