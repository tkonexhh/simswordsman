using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class Kungfu : MonoBehaviour,ItemICom
    {
        [SerializeField]
        private Text m_DiscipleLevel;
        [SerializeField]
        private Text m_DiscipleName;

        [SerializeField]
        private Button m_DiscipleBtn;
        private int m_CurIndex;
        private CharacterItem m_CharacterItem = null;
        private KungfuItem m_CurKungfuItem = null;

        private void Start()
        {
            BindAddListenerEvent();
        }
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurKungfuItem = t as KungfuItem;
            m_CharacterItem = (CharacterItem)obj[0];
            m_CurIndex = (int)obj[1];
            if (m_CurKungfuItem != null)
            {
                m_DiscipleName.text = m_CurKungfuItem.Name;
                m_DiscipleLevel.text = Define.COMMON_DEFAULT_STR;
            }
        }

        private void BindAddListenerEvent()
        {
            m_DiscipleBtn.onClick.AddListener(()=>{
                MainGameMgr.S.CharacterMgr.LearnKungfu(m_CharacterItem.id, m_CurIndex, m_CurKungfuItem);
                EventSystem.S.Send(EventID.OnRefreshDisciple, m_CurIndex);
                UIMgr.S.ClosePanelAsUIID(UIID.LearnKungfuPanel);
                //m_KungfuLibraySlotInfo.SetCharacterItem(characterItem, SlotState.CopyScriptures, m_CurFacilityType);
                //EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, m_KungfuLibraySlotInfo);
                //OnPanelHideComplete();
            });
        }
     
        public void SetButtonEvent(Action<object> action)
        {
            m_DiscipleBtn.onClick.AddListener(() => {
                action?.Invoke(m_CurKungfuItem);
            });
        }

        public void DestroySelf()
        {
            DestroyImmediate(gameObject);
        }
    }
}