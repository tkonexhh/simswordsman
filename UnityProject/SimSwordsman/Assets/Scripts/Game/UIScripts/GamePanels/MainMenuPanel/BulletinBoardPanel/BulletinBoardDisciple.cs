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
        private CharacterItem m_CharacterItem;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private SelelctedState m_SelelctedState = SelelctedState.NotSelected;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {

            m_CurTaskInfo = t as SimGameTask;
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_Btn.onClick.AddListener(() => {
                EventSystem.S.Send(EventID.OnSendDiscipleDicEvent, m_CurTaskInfo);
            });
        }
        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelelctedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
                    break;
                case SelelctedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
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
            if (m_CharacterItem==null)
                m_SelelctedState = SelelctedState.NotSelected;
            else
                m_SelelctedState = SelelctedState.Selected;
            RefreshPanelInfo();
        }
        public void SetButtonEvent(Action<object> action)
        {
        }
	}
}