using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ChooseDisciplePanel : AbstractAnimPanel
	{
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Transform m_SelectedList;
        [SerializeField]
        private GameObject m_Disciple;

        private List<CharacterItem> m_CharacterItem = null;
        private PracticeField m_PracticeFieldInfo = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
           
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PracticeFieldInfo = (PracticeField)args[0];
            GetInformationForNeed();

            BindAddListenerEvent();

            for (int i = 0; i < m_CharacterItem.Count; i++)
            {
                if (m_CharacterItem[i].characterStateData.parentState == CharacterStateID.Wander 
                    && m_CharacterItem[i].level<Define.CHARACTER_MAX_LEVEL)
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
            m_PracticeFieldInfo.SetCharacterItem(characterItem, PracticeFieldState.Practice);
            EventSystem.S.Send(EventID.OnSelectDisciple, m_PracticeFieldInfo);
            OnPanelHideComplete();
        }
    }
}