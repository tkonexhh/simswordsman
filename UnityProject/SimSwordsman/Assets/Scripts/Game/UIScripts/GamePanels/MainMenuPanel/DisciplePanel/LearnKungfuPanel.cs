using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class LearnKungfuPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Transform m_SelectedList;
        [SerializeField]
        private GameObject m_Disciple;
        private int m_CurIndex;
        private List<ItemBase> m_ItemBase = null;
        private CharacterItem m_CharacterItem = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            BindAddListenerEvent();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CharacterItem = (CharacterItem)args[0];
            m_CurIndex = (int)args[1];
            GetInformationForNeed();

            for (int i = 0; i < m_ItemBase.Count; i++)
            {
                CreateDisciple(m_ItemBase[i]);
            }
        }

        private void GetInformationForNeed()
        {
            m_ItemBase = MainGameMgr.S.InventoryMgr.GetAllEquipmentForType(PropType.Kungfu);
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

        private void CreateDisciple(ItemBase itemBase)
        {
            GameObject disciple = Instantiate(m_Disciple, m_SelectedList);
            ItemICom discipleItem = disciple.GetComponent<ItemICom>();

            discipleItem.OnInit(itemBase,null, m_CharacterItem, m_CurIndex);
        }
    }
}