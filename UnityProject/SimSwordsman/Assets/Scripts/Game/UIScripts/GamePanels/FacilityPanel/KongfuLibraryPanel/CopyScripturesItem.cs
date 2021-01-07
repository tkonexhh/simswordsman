using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class CopyScripturesItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_CopyScripturesPos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurCopyScriptures;
        [SerializeField]
        private Image m_DiscipleImg;
        [SerializeField]
        private Button m_CopyScripturesBtn;


        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            throw new NotImplementedException();
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }

        
	}
	
}