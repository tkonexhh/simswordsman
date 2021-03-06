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
        [SerializeField] private Button m_Btn;
        [SerializeField] private Image m_LevelBg;
        [SerializeField] private Text m_Level;
        [SerializeField] private Text m_DiscipleName;
        [SerializeField] private Image m_Plus;
        [SerializeField] private Image m_DiscipleLevelBg;
        [SerializeField] private Image m_Line;

        protected CharacterItem m_CharacterItem;
        private SendDisciplesPanel m_SendDisciplesPanel;
        private PanelType m_PanelType;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private GameObject m_DiscipleHeadObj;
        public void Init(PanelType panelType, SendDisciplesPanel sendDisciplesPanel)
        {
            m_SendDisciplesPanel = sendDisciplesPanel;
            m_PanelType = panelType;
            BindAddListenerEvent();
            OnInit();
            RefreshPanelInfo();
        }

        private void RefreshDiscipleColor()
        {
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_DiscipleLevelBg.sprite = m_SendDisciplesPanel.FindSprite("Disciple_FontBg_Blue");
                    m_Line.sprite = m_SendDisciplesPanel.FindSprite("Disciple_Line_Bule");
                    break;
                case CharacterQuality.Good:
                    m_DiscipleLevelBg.sprite = m_SendDisciplesPanel.FindSprite("Disciple_FontBg_Yellow");
                    m_Line.sprite = m_SendDisciplesPanel.FindSprite("Disciple_Line_Yellow");
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleLevelBg.sprite = m_SendDisciplesPanel.FindSprite("Disciple_FontBg_Red");
                    m_Line.sprite = m_SendDisciplesPanel.FindSprite("Disciple_Line_Red");
                    break;
                default:
                    break;
            }
        }
        private void BindAddListenerEvent()
        {
            m_Btn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                EventSystem.S.Send(EventID.OnSendDiscipleDicEvent, m_PanelType);
            });
        }
        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    if (m_DiscipleHeadObj != null)
                        m_DiscipleHeadObj.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = m_CharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    break;
                case SelectedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    if (m_DiscipleHeadObj != null)
                        m_DiscipleHeadObj.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            OnRefreshPanelInfo();
        }

        public void RefreshSelectedDisciple(CharacterItem characterItem)
        {
            m_CharacterItem = characterItem;
            if (m_CharacterItem == null)
            {
                m_SelelctedState = SelectedState.NotSelected;
            }
            else
            {

                LoadClanPrefabs(CharacterMgr.GetLoadDiscipleName(m_CharacterItem));
                m_SelelctedState = SelectedState.Selected;
            }
            RefreshPanelInfo();
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            if (m_DiscipleHeadObj != null)
                DestroyImmediate(m_DiscipleHeadObj);
            m_DiscipleHeadObj = DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CharacterItem, m_LevelBg.transform, new Vector3(45.9f, -29, 0), new Vector3(0.4f, 0.4f, 1));
        }

        protected virtual void OnInit() { }
        protected virtual void OnRefreshPanelInfo() { }
    }

}