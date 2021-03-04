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
        private Text m_Count;  
        [SerializeField]
        private Image m_Lock;

        [SerializeField]
        private Button m_HerbalMedicineBtn;

        //private PlayerDataHerb m_CurHerb = null;
        private HerbItem m_HerbItem = null;
        private int m_HerbItemID;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private bool m_IsSelected = false;
        private SendDisciplesPanel m_SendDisciplesPanel;
        private void Start()
        {
            BindAddListenerEvent();
        }
        public void OnInit(int id, SendDisciplesPanel sendDisciplesPanel)
        {
            m_HerbItemID = id;
            m_SendDisciplesPanel = sendDisciplesPanel;
            m_HerbItem = MainGameMgr.S.InventoryMgr.GetHerbForID(id) as HerbItem;
            RefreshPanelInfo();
            //解锁
            if (RefreshHerbUnlockLevel())
            {
                m_Lock.gameObject.SetActive(false);
                m_HerbHead.gameObject.SetActive(true);
                m_State.gameObject.SetActive(true);
            }
            else
            //未解锁
            {
                m_Lock.gameObject.SetActive(true);
                m_HerbHead.gameObject.SetActive(false);
                m_State.gameObject.SetActive(false);
            }
        }

        private bool RefreshHerbUnlockLevel()
        {
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            int unlockLevel = TDHerbConfigTable.GetUnlockLobbyLevel(m_HerbItemID);
            if (lobbyLevel >= unlockLevel)
                return true;
            return false;
        }

        private string GetIconName(int herbType)
        {
            return TDHerbConfigTable.GetHerbIconNameById(herbType);
        }

        private HerbConfig GetHerbById(int herbType)
        {
            return TDHerbConfigTable.GetHerbById(herbType);
        }
        private void RefreshPanelInfo()
        {
            m_HerbHead.sprite = m_SendDisciplesPanel.FindSprite(GetIconName(m_HerbItemID));
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.sprite = m_SendDisciplesPanel.FindSprite("SendDisciplePanel_Bg4");
                    m_Count.text = Define.COMMON_DEFAULT_STR;
                    break;
                case SelectedState.NotSelected:
                    m_State.sprite = m_SendDisciplesPanel.FindSprite("SendDisciplePanel_Bg3");
                    if (m_HerbItem != null)
                        m_Count.text = m_HerbItem.Number.ToString();
                    else
                        m_Count.text = Define.DEFAULT_NUMBER_ZERO;
                    break;
                default:
                    break;
            }
            m_HerbName.text = GetHerbById(m_HerbItemID).Name;
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
                    if (!RefreshHerbUnlockLevel())
                    {
                        FloatMessage.S.ShowMsg("草药未解锁!");
                        return;
                    }
                    else
                    {
                        if (m_HerbItem == null || m_HerbItem.Number <= 0)
                        {
                            FloatMessage.S.ShowMsg("草药数量不足!");
                            return;
                        }
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