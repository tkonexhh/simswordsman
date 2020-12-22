using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class WearableLearningPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Button m_ClsoeBtn;
        [SerializeField]
        private Transform m_WearableLearningTra;
        [SerializeField]
        private GameObject m_WearableLearningItem;

        private PropType m_CurPropType;
        //private List<EquipmentItem> m_CurAllEquip = null;
        private CharacterItem m_CurDisciple = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();

            // 测试代码：增加装备
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Armor, 1, 9));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Armor, 1, 2));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Arms, 1, 2));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Arms, 2, 5));


            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            //base.OnPanelOpen(args);
            //OpenDependPanel(EngineUI.MaskPanel, -1, null);

            //m_CurPropType = (PropType)args[0];
            //m_CurDisciple = args[1] as CharacterItem;
            //EquipmentItem chracEquip = m_CurDisciple.characterEquipment.Where(i => i.PropType == m_CurPropType).FirstOrDefault();
            //if (chracEquip!=null)
            //    MainGameMgr.S.InventoryMgr.AddEquipment(chracEquip);
            //m_CurAllEquip = MainGameMgr.S.InventoryMgr.GetAllEquipmentForType(m_CurPropType);

            //foreach (var item in m_CurAllEquip)
            //{
            //    CreateWearableLearningItem(item);
            //}
        }

        private void BindAddListenerEvent()
        {
            //m_ClsoeBtn.onClick.AddListener(() =>
            //{
            //    EquipmentItem chracEquip = m_CurDisciple.characterEquipment.Where(i => i.PropType == m_CurPropType).FirstOrDefault();
            //    if (chracEquip!=null)
            //        MainGameMgr.S.InventoryMgr.RemoveItem(chracEquip);
            //    HideSelfWithAnim();
            //});
        }

        //private void CreateWearableLearningItem(EquipmentItem equipment)
        //{
        //    ItemICom itemICom = Instantiate(m_WearableLearningItem, m_WearableLearningTra).GetComponent<ItemICom>();
        //    itemICom.OnInit(equipment);
        //    itemICom.SetButtonEvent(EquipBtnCallback);
        //}

        private void EquipBtnCallback(object obj)
        {
            //EquipmentItem equipmentItem = obj as EquipmentItem;
            //EquipmentItem chracEquip = m_CurDisciple.characterEquipment.Where(i => i.PropType == m_CurPropType).FirstOrDefault();
            //if (chracEquip != null)
            //{
            //    MainGameMgr.S.InventoryMgr.RemoveItem(chracEquip);  
            //    if (!chracEquip.IsHaveEquipment(equipmentItem))
            //        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple, equipmentItem);
            //}
            //else
            //    MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple, equipmentItem);  
            //OnPanelHideComplete();
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }

}