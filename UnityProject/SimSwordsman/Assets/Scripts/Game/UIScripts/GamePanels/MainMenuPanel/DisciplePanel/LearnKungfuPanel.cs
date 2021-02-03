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
        private Button m_ArrangeBtn;
        [SerializeField]
        private Transform m_SelectedList;
        [SerializeField]
        private GameObject m_Kungfu;
        private int m_CurIndex;
        private ItemBase m_SelectedItemBase;
        private bool IsSelected = false;
        private Transform m_Pos;
        private List<ItemBase> m_ItemBaseList = null;
        private CharacterItem m_CharacterItem = null;
        private List<LearnKungfuItem> m_KungfuLearningItemDic = new List<LearnKungfuItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectedKungfuEvent, HandleAddListenerEvevt);
        }

        private void HandleAddListenerEvevt(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedKungfuEvent:
                    RefreshKungfuInfo((bool)param[0], (ItemBase)param[1], (Transform)param[2]);
                    break;
                default:
                    break;
            }
        }

        private void RefreshKungfuInfo(bool isSelected, ItemBase itemBase, Transform transform)
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
            foreach (var item in m_KungfuLearningItemDic)
                item.IsSame(m_SelectedItemBase);
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedKungfuEvent, HandleAddListenerEvevt);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            BindAddListenerEvent();
            m_CharacterItem = (CharacterItem)args[0];
            m_CurIndex = (int)args[1];
            GetInformationForNeed();

            for (int i = 0; i < m_ItemBaseList.Count; i++)
            {
                CreateKungfu(m_ItemBaseList[i]);
            }
        }

        private void GetInformationForNeed()
        {
            m_ItemBaseList = MainGameMgr.S.InventoryMgr.GetAllEquipmentForType(PropType.Kungfu);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_ArrangeBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                MainGameMgr.S.CharacterMgr.LearnKungfu(m_CharacterItem.id, m_CurIndex, new KungfuItem((KongfuType)m_SelectedItemBase.GetSubName()));
                MainGameMgr.S.InventoryMgr.RemoveItem(m_SelectedItemBase);
                EventSystem.S.Send(EventID.OnSelectedKungfuSuccess, m_CurIndex);
                UIMgr.S.ClosePanelAsUIID(UIID.LearnKungfuPanel);
            });
        }
        private void Update()
        {
            if (IsSelected)
                m_ArrangeBtn.transform.position = m_Pos.position;
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }

        private void CreateKungfu(ItemBase itemBase)
        {
            GameObject kungfu = Instantiate(m_Kungfu, m_SelectedList);
            LearnKungfuItem kungfuItem = kungfu.GetComponent<LearnKungfuItem>();

            kungfuItem.OnInit(itemBase,null, m_CharacterItem, GetSprite(itemBase));
            m_KungfuLearningItemDic.Add(kungfuItem);
        }

        private List<Sprite> GetSprite(ItemBase itemBase)
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(FindSprite(GetIconName((KongfuType)itemBase.GetSubName())));
            switch (GetKungfuQuality((KongfuType)itemBase.GetSubName()))
            {
                case KungfuQuality.Normal:
                    sprites.Add(FindSprite("Introduction"));
                    break;
                case KungfuQuality.Super:
                    sprites.Add(FindSprite("Advanced"));
                    break;
                case KungfuQuality.Master:
                    sprites.Add(FindSprite("Excellent"));
                    break;
                default:
                    break;
            }
            return sprites;
        }
        private KungfuQuality GetKungfuQuality(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }

        private string GetIconName(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
    }
}