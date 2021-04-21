using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BulletinBoardDisciple : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private Image m_LevelBg;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_Plus;
        [SerializeField]
        private Image m_DiscipleLevelBg;
        [SerializeField]
        private Image m_Line;
        private CharacterItem m_CharacterItem;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;

        private BulletinBoardPanel m_BulletinBoardPanel;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurTaskInfo = t as SimGameTask;
            m_BulletinBoardPanel = (BulletinBoardPanel)obj[0];
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_Btn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                DataAnalysisMgr.S.CustomEvent(DotDefine.c_task_enter, m_CurTaskInfo.TaskId.ToString());
                EventSystem.S.Send(EventID.OnBulletinSendDiscipleDicEvent, m_CurTaskInfo);
            });
        }

        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.sprite = m_BulletinBoardPanel.FindSprite(CharacterMgr.GetLoadDiscipleName(m_CharacterItem));
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = m_CharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    break;
                case SelectedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
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
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Blue");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Bule");
                    break;
                case CharacterQuality.Good:
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Yellow");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Yellow");
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Red");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Red");
                    break;
                default:
                    break;
            }
        }

        public void SetBtnClick(bool IsClick)
        {
            m_Btn.enabled = IsClick;
        }

        public void RefreshSelectedDisciple(CharacterItem characterItem)
        {
            m_CharacterItem = characterItem;
            if (m_CharacterItem == null)
                m_SelelctedState = SelectedState.NotSelected;
            else
            {
                m_SelelctedState = SelectedState.Selected;
            }
            RefreshPanelInfo();
        }
        public void SetButtonEvent(Action<object> action)
        {
        }
    }
}