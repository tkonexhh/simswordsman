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

        private GameObject m_HeadObj = null;
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

        public void RefreshInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    if (m_HeadObj != null)
                        DestroyImmediate(m_HeadObj);
                    m_HeadObj = DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CurCharacterItem, transform, new Vector3(5f, 5f, 0), new Vector3(0.4f, 0.4f, 1));
                    m_Level.text = m_CurCharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    m_Plus.SetActive(false);
                    //m_HeadImg.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_Plus.SetActive(true);
                    m_LevelBg.gameObject.SetActive(false);
                    m_HeadImg.gameObject.SetActive(false);
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