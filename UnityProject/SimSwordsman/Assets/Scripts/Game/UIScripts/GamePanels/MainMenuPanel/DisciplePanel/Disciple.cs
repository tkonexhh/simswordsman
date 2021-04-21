using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class Disciple : MonoBehaviour
    {
        [SerializeField]
        private Text m_DiscipleLevel;
        [SerializeField]
        private Image m_DiscipleLevelBg;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Image m_Line;
        [SerializeField]
        private Image m_DiscipleSmallHead;
        [SerializeField]
        private Image m_LineBottom;
        [SerializeField]
        private Image m_LineRightBottom;
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private Transform m_DiscipleState;  
        [SerializeField]
        private Transform m_LineTra;
        [SerializeField]
        private GameObject m_DiscipleRedPoint;

        private CharacterItem m_CurCharacter = null;
        private DisciplePanel m_ParentPanel;
        private void Start()
        {
            EventSystem.S.Register(EventID.OnMainMenuOrDiscipleRedPoint, HandAddListenerEvent);
            BindAddListenerEvent();
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnMainMenuOrDiscipleRedPoint:
                    m_CurCharacter = MainGameMgr.S.CharacterMgr.GetCharacterItem(m_CurCharacter.id);
                    m_DiscipleRedPoint.SetActive(m_CurCharacter.CheckDiscipelPanel());
                    break;
            }
        }

        public void SetShowLine(int num)
        {
            if (num == 0)
            {
                m_LineRightBottom.gameObject.SetActive(true);
                m_LineBottom.gameObject.SetActive(false);
            }
            else if (num == 1)
            {
                m_LineRightBottom.gameObject.SetActive(false);
                m_LineBottom.gameObject.SetActive(true);
            }
        }

        public void OnInit(CharacterItem characterItem, DisciplePanel disciple)
        {
            m_CurCharacter = characterItem;
            m_ParentPanel = disciple;

            m_DiscipleRedPoint.SetActive(characterItem.CheckDiscipelPanel());

            if (m_CurCharacter != null)
            {
                if (m_CurCharacter.IsFreeState())
                    m_DiscipleState.gameObject.SetActive(true);
                else
                    m_DiscipleState.gameObject.SetActive(false);
                m_DiscipleName.text = m_CurCharacter.name;
                m_DiscipleLevel.text = m_CurCharacter.level.ToString();

                switch (m_CurCharacter.quality)
                {
                    case CharacterQuality.Normal:
                        m_DiscipleLevelBg.sprite = m_ParentPanel.FindSprite("Disciple_FontBg_Blue");
                        m_Line.sprite = m_ParentPanel.FindSprite("Disciple_Line_Bule");
                        m_DiscipleSmallHead.sprite = m_ParentPanel.FindSprite("Disciple_SmallHead_Blue");
                        break;
                    case CharacterQuality.Good:
                        m_DiscipleLevelBg.sprite = m_ParentPanel.FindSprite("Disciple_FontBg_Yellow");
                        m_Line.sprite = m_ParentPanel.FindSprite("Disciple_Line_Yellow");
                        m_DiscipleSmallHead.sprite = m_ParentPanel.FindSprite("Disciple_SmallHead_Yellow");
                        break;
                    case CharacterQuality.Perfect:
                        m_DiscipleLevelBg.sprite = m_ParentPanel.FindSprite("Disciple_FontBg_Red");
                        m_Line.sprite = m_ParentPanel.FindSprite("Disciple_Line_Red");
                        m_DiscipleSmallHead.sprite = m_ParentPanel.FindSprite("Disciple_SmallHead_Red");
                        break;
                    default:
                        break;
                }
            }
            DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CurCharacter, m_LineTra, new Vector3(0, 87, 0), new Vector3(0.6f, 0.6f, 1));
        }


        /// <summary>
        /// ��ȡ��ǰ���ӵ�id
        /// </summary>
        /// <returns></returns>
        public int GetCurDiscipleId()
        {
            if (m_CurCharacter != null)
                return m_CurCharacter.id;
            return -1;
        }

        private void BindAddListenerEvent()
        {

        }

        public void SetButtonEvent(Action<object> action)
        {
            m_DiscipleBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                action?.Invoke(m_CurCharacter);
            });
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnMainMenuOrDiscipleRedPoint, HandAddListenerEvent);
        }
        private void OnDisable()
        {
        }
        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}