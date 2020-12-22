using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class PracticeDisciple : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_PracticePos;
        [SerializeField]
        private Text m_CurPractice;
        [SerializeField]
        private Image m_PracticeImg;
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