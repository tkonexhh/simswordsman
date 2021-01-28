using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class HerbalMedicineItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_HerbHead;
        [SerializeField]
        private Image m_State;
        [SerializeField]
        private Text m_HerbName;

        [SerializeField]
        private Button m_HerbalMedicineBtn;

        //private PlayerDataHerb m_CurHerb = null;
        private HerbItem m_HerbItem = null;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private bool m_IsSelected = false;
        private void Start()
        {
            BindAddListenerEvent();
        }
        public void OnInit(int id)
        {
            m_HerbItem = MainGameMgr.S.InventoryMgr.GetHerbForID(id) as HerbItem;

            RefreshPanelInfo();
        }

        private void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.gameObject.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_State.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            if (m_HerbItem != null)
            {
                m_HerbName.text = m_HerbItem.Name;
                //m_HerbalMedicineNumber.text = m_HerbItem.Number.ToString();
            }
        }

        /// <summary>
        /// 获取当前草药的id
        /// </summary>
        /// <returns></returns>
        public int GetCurHerbId()
        {
            if (m_HerbItem != null)
                return (int)m_HerbItem.HerbID;
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
            m_HerbalMedicineBtn.onClick.AddListener(() =>
            {
                m_IsSelected = !m_IsSelected;
                if (m_IsSelected)
                {
                    if (m_HerbItem == null || m_HerbItem.Number - 1 <= 0)
                    {
                        FloatMessage.S.ShowMsg("草药数量不足!");
                        return;
                    }
                    m_SelelctedState = SelectedState.Selected;
                }
                else
                    m_SelelctedState = SelectedState.NotSelected;

                EventSystem.S.Send(EventID.OnSendHerbEvent, m_IsSelected, m_HerbItem);
                RefreshPanelInfo();
            });
        }

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}