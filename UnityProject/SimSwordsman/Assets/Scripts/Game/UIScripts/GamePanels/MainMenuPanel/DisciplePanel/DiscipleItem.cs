using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class DiscipleItem : MonoBehaviour,ItemICom
    {
        [SerializeField]
        private Text m_DiscipleLevel;
        [SerializeField]
        private Text m_DiscipleName;

        [SerializeField]
        private Button m_DiscipleBtn;

        private CharacterItem m_CurCharacter = null;

        private void Start()
        {
            BindAddListenerEvent();
        }
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurCharacter = t as CharacterItem;
            if (m_CurCharacter != null)
            {
                m_DiscipleName.text = m_CurCharacter.name;
                m_DiscipleLevel.text = m_CurCharacter.level.ToString();
            }
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

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}