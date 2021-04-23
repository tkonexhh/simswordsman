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
        private Text m_Title;     
        [SerializeField]
        private Image m_NotAvailable;
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

        private const int Rows = 4;
        private const float EquipHeight = 169.4f;
        private const float BtnHeight = 43f;
        private List<WearableLearningItem> m_WearableLearningItemDic = new List<WearableLearningItem>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            EventSystem.S.Register(EventID.OnSelectedEquipEvent, HandleAddListenerEvevt);

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
        private void CalculateContainerHeight()
        {
            int rows = m_WearableLearningItemDic.Count / Rows;
            if ((m_WearableLearningItemDic.Count % Rows) != 0)
                rows += 1;

            float height = EquipHeight * rows;
            m_WearableLearningTra.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + BtnHeight);
        }

        private void RefreshEquipInfo(bool isSelected, ItemBase itemBase, Transform transform)
        {
            IsSelected = isSelected;
            ItemBase selected = itemBase;
            m_Pos = transform;
            //m_ArrangeBtn.gameObject.SetActive(true);
            if (m_SelectedItemBase != null && m_SelectedItemBase.GetSortId() == selected.GetSortId())
            {
                if (!IsSelected)
                {
                    m_SelectedItemBase = null;
                    //m_ArrangeBtn.gameObject.SetActive(false);
                    return;
                }
            }
            m_SelectedItemBase = selected;
            foreach (var item in m_WearableLearningItemDic)
                item.IsSame(m_SelectedItemBase);
        }
        private void Update()
        {
            //if (IsSelected)
            //    m_ArrangeBtn.transform.position = m_Pos.position;
        }
        private void GeInformationForNeed()
        {
            m_ItemBaseList = MainGameMgr.S.InventoryMgr.GetAllEquipmentForType(m_CurPropType);
        }

        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            PanelPool.S.DisplayPanel();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            m_CurPropType = (PropType)args[0];
            m_CurDisciple = (CharacterItem)args[1];
            GeInformationForNeed();
            switch (m_CurPropType)
            {
                case PropType.Arms:
                    m_Title.text = "选择武器";
                    break;
                case PropType.Armor:
                    m_Title.text = "选择装备";
                    break;
            }
            if (m_ItemBaseList.Count==0)
            {
                switch (m_CurPropType)
                {
                    case PropType.Arms:
                        m_NotAvailable.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Bg36");
                        break;
                    case PropType.Armor:
                        m_NotAvailable.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DiscipleDetailsPanelAtlas, "DiscipleDetails_Bg37");
                        break;
                }
                m_NotAvailable.gameObject.SetActive(true);
            }
            else
            {

                foreach (var item in m_ItemBaseList)
                    CreateWearableLearningItem(item);
            }


            CalculateContainerHeight();
        }

        private void BindAddListenerEvent()
        {
            m_ArrangeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (m_ItemBaseList.Count == 0)
                {
                    return;
                }

                switch (m_SelectedItemBase.PropType)
                {
                    case PropType.Arms:
                        PanelPool.S.AddPromotion(new EquipAmrs(m_CurDisciple.id, m_CurDisciple.atkValue,(ArmsItem)m_SelectedItemBase));
                        MainGameMgr.S.InventoryMgr.AddItem((ArmsItem)m_CurDisciple.GetEquipmentForType(PropType.Arms));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArms(m_SelectedItemBase));
                        break;
                    case PropType.Armor:
                        PanelPool.S.AddPromotion(new EquipAmror(m_CurDisciple.id, m_CurDisciple.atkValue, (ArmorItem)m_SelectedItemBase));
                        MainGameMgr.S.InventoryMgr.AddItem((ArmorItem)m_CurDisciple.GetEquipmentForType(PropType.Armor));
                        MainGameMgr.S.CharacterMgr.AddEquipment(m_CurDisciple.id, new CharacterArmor(m_SelectedItemBase));
                        break;
                    default:
                        break;
                }
                MainGameMgr.S.InventoryMgr.RemoveItem(m_SelectedItemBase);
                EventSystem.S.Send(EventID.OnSelectedEquipSuccess);

                DataAnalysisMgr.S.CustomEvent(DotDefine.students_equip, m_SelectedItemBase.PropType.ToString() + ";" + m_SelectedItemBase.GetSubName().ToString());

                UIMgr.S.ClosePanelAsUIID(UIID.WearableLearningPanel);
            });

            m_ClsoeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

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
        }
    }
}