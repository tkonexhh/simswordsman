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

        private CharacterItem m_CurCharacter = null;
        private DisciplePanel m_ParentPanel;
        private void Start()
        {
            BindAddListenerEvent();
        }

        public void SetShowLine(int num)
        {
            if (num==0)
            {
                m_LineRightBottom.gameObject.SetActive(true);
                m_LineBottom.gameObject.SetActive(false);
            }
            else if(num ==1)
            {
                m_LineRightBottom.gameObject.SetActive(false);
                m_LineBottom.gameObject.SetActive(true);
            }
        }

        public void OnInit(CharacterItem characterItem, DisciplePanel disciple)
        {
            m_CurCharacter = characterItem;
            m_ParentPanel = disciple;
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
            m_DiscipleImg.sprite = m_ParentPanel.FindSprite(GetLoadDiscipleName(m_CurCharacter));
        }

        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        /// <summary>
        /// 获取当前弟子的id
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
            m_DiscipleBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                action?.Invoke(m_CurCharacter);
            });
        }

        private void OnDestroy()
        {
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