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
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Image m_LineBottom;
        [SerializeField]
        private Image m_LineRightBottom;
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private Transform m_DiscipleState;
        private AddressableAssetLoader<Sprite> m_Loader;

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
                if (m_CurCharacter.characterStateId == CharacterStateID.EnterClan || m_CurCharacter.characterStateId == CharacterStateID.Wander)
                    m_DiscipleState.gameObject.SetActive(true);
                else
                    m_DiscipleState.gameObject.SetActive(false);
                m_DiscipleName.text = m_CurCharacter.name;
                m_DiscipleLevel.text = CommonUIMethod.GetGrade(m_CurCharacter.level);
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
            m_Loader?.Release();
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