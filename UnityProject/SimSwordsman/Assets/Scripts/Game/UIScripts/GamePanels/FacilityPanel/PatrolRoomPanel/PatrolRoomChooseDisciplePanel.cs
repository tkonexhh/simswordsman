using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class PatrolRoomChooseDisciplePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Transform m_SelectedList;
        [SerializeField]
        private GameObject m_Disciple;

        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private List<CharacterItem> m_CharacterItem = null;
        private PatrolRoomSlot m_PatrolRoomSlotInfo = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            BindAddListenerEvent();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PatrolRoomSlotInfo = (PatrolRoomSlot)args[0];
            m_CurFacilityType = (FacilityType)args[1];
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(m_CurFacilityType);
            GetInformationForNeed();

            for (int i = 0; i < m_CharacterItem.Count; i++)
            {
                if (m_CharacterItem[i].IsFreeState() && m_CharacterItem[i].level < Define.CHARACTER_MAX_LEVEL)
                    CreateDisciple(m_CharacterItem[i]);
            }
        }

        private void GetInformationForNeed()
        {
            m_CharacterItem = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }

        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject disciple = Instantiate(m_Disciple, m_SelectedList);
            ItemICom discipleItem = disciple.GetComponent<ItemICom>();

            discipleItem.OnInit(characterItem);
            discipleItem.SetButtonEvent(AddListenerBtn);
        }

        private void AddListenerBtn(object obj)
        {
            CharacterItem characterItem = obj as CharacterItem;
            m_PatrolRoomSlotInfo.SetCharacterItem(characterItem, SlotState.Patrol, m_CurFacilityType);
            EventSystem.S.Send(EventID.OnRefresPatrolSoltInfo, m_PatrolRoomSlotInfo);
            OnPanelHideComplete();
        }
    }
}