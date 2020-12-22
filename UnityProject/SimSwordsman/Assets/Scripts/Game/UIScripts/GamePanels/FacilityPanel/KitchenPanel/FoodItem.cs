using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class FoodItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Image m_FoodImg;
        [SerializeField]
        private Text m_FoodNameTxt;
        [SerializeField]
        private Text m_FoodContTxt;
        [SerializeField]
        private Button m_MakeBtn;
        [SerializeField]
        private Text m_FoodEffecTxt;
        [SerializeField]
        private Text m_DurationTxt;

        [SerializeField]
        private Transform m_DurationTra;


        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        private void BindAddListenerEvent()
        {
            m_MakeBtn.onClick.AddListener(() => { });
        }
    }

}

