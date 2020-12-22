using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class HerbalMedicineItem : MonoBehaviour,ItemICom
    {
        [SerializeField]    
        private Text m_HerbalMedicineNumber;
        [SerializeField]
        private Text m_HerbalMedicineName;

        [SerializeField]
        private Button m_HerbalMedicineBtn;

        private PlayerDataHerb m_CurHerb = null;

        private bool m_IsSelected = false;
        private void Start()
        {
            BindAddListenerEvent();

        }
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurHerb = t as PlayerDataHerb;
            if (m_CurHerb != null)
            {
                m_HerbalMedicineName.text = m_CurHerb.Name;
                m_HerbalMedicineNumber.text = m_CurHerb.Number.ToString();
            }
        }
        /// <summary>
        /// 获取当前草药的id
        /// </summary>
        /// <returns></returns>
        public int GetCurHerbId()
        {
            if (m_CurHerb != null)
                return m_CurHerb.ID;
            return -1;
        }

        public bool GetHerbStatue()
        {
            return m_IsSelected;
        }

        public void SetStateSelected()
        {
            if (m_IsSelected)
                GetComponent<Image>().color = Color.white;
            else
                GetComponent<Image>().color = Color.red;
            m_IsSelected = !m_IsSelected;
        }


        private void BindAddListenerEvent()
        {
           
        }

        public void SetButtonEvent(Action<object> action)
        {
            m_HerbalMedicineBtn.onClick.AddListener(() => {
                action?.Invoke(this);
            });
        }

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}