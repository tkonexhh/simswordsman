using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class Disciple : MonoBehaviour,ItemICom
    {
        [SerializeField]
        private Text m_DiscipleLevel;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_DiscipleImg;  
        [SerializeField]
        private Button m_DiscipleBtn;
        [SerializeField]
        private Transform m_DiscipleState;

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
                m_DiscipleLevel.text = CommonUIMethod.GetGrade(m_CurCharacter.level);
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
                action?.Invoke(m_CurCharacter);
            });
        }

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}