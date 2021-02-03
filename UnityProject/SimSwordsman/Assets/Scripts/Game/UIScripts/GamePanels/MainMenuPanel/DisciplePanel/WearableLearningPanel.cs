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
        private Button m_ArrangeBtn;
        [SerializeField]
        private Transform m_WearableLearningTra;
        [SerializeField]
        private GameObject m_WearableLearningItem;
        private ItemBase m_SelectedItemBase;
        private bool IsSelected = false;
        private PropType m_CurPropType;
        private Transform m_Pos;
        private List<ItemBase> m_ItemBaseList = null;
        private CharacterItem m_CurDisciple = null;
        private List<WearableLearningItem> m_WearableLearningItemDic = new List<WearableLearningItem>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectedEquipEvent, HandleAddListenerEvevt);
            // 测试代码：增加装备
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Armor, 1, 9));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Armor, 1, 2));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Arms, 1, 2));
            //MainGameMgr.S.InventoryMgr.AddEquipment(new EquipmentItem(PropType.Arms, 2, 5));

            BindAddListenerEvent();
        }
        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEquipEvent:
                    RefreshEquipInfo((bool)param[0], (ItemBase)param[1], (Transform)param[2]);
                    break;
                default:
                    break;
            }
        }

        private void RefreshEquipInfo(bool isSelected, ItemBase itemBase, Transform transform)
        {
            IsSelected = isSelected;
            ItemBase selected = itemBase;
            m_Pos = transform;
            m_ArrangeBtn.gameObject.SetActive(true);
            if (m_SelectedItemBase != null && m_SelectedItemBase.GetSortId() == selected.GetSortId())
            {
                if (!IsSelected)
                {
                    m_SelectedItemBase = null;
                    m_ArrangeBtn.gameObject.SetActive(false);
                    return;
                }
            }
            m_SelectedItemBase = selected;
            foreach (var item in m_WearableLearningItemDic)
                item.IsSame(m_SelectedItemBase);
        }
        private void Update()
        {
            if (IsSelected)
                m_ArrangeBtn.transform.position = m_Pos.position;
        }
        private void GeInformationForNeed()
        {
            m_ItemBaseList = MainGameMgr.S.InventoryMgr.GetAllEquipmentForType(m_CurPropType);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            m_CurPropType = (PropType)args[0];
            m_CurDisciple = (CharacterItem)args[1];
            GeInformationForNeed();

            foreach (var item in m_ItemBaseList)
                CreateWearableLearningItem(item);
        }

        private void BindAddListenerEvent()
        {
            m_ArrangeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                switch (m_SelectedItemBase.PropType)
                {
                    case PropType.Arms:
                        MainGameMgr.S.InventoryMgr.AddItem(m_CurDisciple.GetEquipmentForType(PropType.Arms));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArms(m_SelectedItemBase));
                        break;
                    case PropType.Armor:
                        MainGameMgr.S.InventoryMgr.AddItem(m_CurDisciple.GetEquipmentForType(PropType.Armor));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArmor(m_SelectedItemBase));
                        break;
                    default:
                        break;
                }
                MainGameMgr.S.InventoryMgr.RemoveItem(m_SelectedItemBase);
                EventSystem.S.Send(EventID.OnSelectedEquipSuccess);
                UIMgr.S.ClosePanelAsUIID(UIID.WearableLearningPanel);
            });

            m_ClsoeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                //EquipmentItem chracEquip = m_CurDisciple.characterEquipment.Where(i => i.PropType == m_CurPropType).FirstOrDefault();
                //if (chracEquip != null)
                //    MainGameMgr.S.InventoryMgr.RemoveItem(chracEquip);
                HideSelfWithAnim();
            });
        }

        private void CreateWearableLearningItem(ItemBase itemBase)
        {
            
            WearableLearningItem itemICom = Instantiate(m_WearableLearningItem, m_WearableLearningTra).GetComponent<WearableLearningItem>();
            itemICom.OnInit(itemBase, null, m_CurDisciple, FindSprite(GetEquipName(itemBase.GetSubName())));

            m_WearableLearningItemDic.Add(itemICom);
        }


        public string GetEquipName(int id)
        {
            return TDEquipmentConfigTable.GetIconName(id);
        }

        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnSelectedEquipEvent, HandleAddListenerEvevt);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}