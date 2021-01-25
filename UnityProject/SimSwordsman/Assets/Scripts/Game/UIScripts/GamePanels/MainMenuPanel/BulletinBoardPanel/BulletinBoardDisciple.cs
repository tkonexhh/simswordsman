using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class BulletinBoardDisciple : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Button m_LevelBtn;
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

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_LevelBtn.onClick.AddListener(()=>{ 

            });
        }

        public void SetButtonEvent(Action<object> action)
        {
        }
	}
}