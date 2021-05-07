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
        [SerializeField]
        private GameObject m_NotStudy;
        private int m_CurIndex;
        private ItemBase m_SelectedItemBase;
        private bool IsSelected = false;
        private Transform m_Pos;
        private List<ItemBase> m_ItemBaseList = null;
        private CharacterItem m_CharacterItem = null;
        private List<LearnKungfuItem> m_KungfuLearningItemDic = new List<LearnKungfuItem>();
        private const int Rows = 4;
        private const float KungfuHeight = 169.4f;
        private const float BtnHeight = 38f;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
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
        private void CalculateContainerHeight()
        {
            int rows = m_KungfuLearningItemDic.Count / Rows;
            if ((m_KungfuLearningItemDic.Count % Rows) != 0)
                rows += 1;

            float height = KungfuHeight * rows;
            m_SelectedList.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + BtnHeight);
        }

        private void RefreshKungfuInfo(bool isSelected, ItemBase itemBase, Transform transform)
        {
            IsSelected = isSelected;
            ItemBase selected = itemBase;
            m_Pos = transform;
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
            foreach (var item in m_KungfuLearningItemDic)
                item.IsSame(m_SelectedItemBase);
        }

        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnSelectedKungfuEvent, HandleAddListenerEvevt);
            PanelPool.S.DisplayPanel();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            BindAddListenerEvent();
            m_CharacterItem = (CharacterItem)args[0];
            m_CurIndex = (int)args[1];
            GetInformationForNeed();

            if (m_ItemBaseList.Count == 0)
            {
                m_NotStudy.SetActive(true);
                m_ArrangeBtn.gameObject.SetActive(false);
            }

            for (int i = 0; i < m_ItemBaseList.Count; i++)
            {
                CreateKungfu(m_ItemBaseList[i]);
            }
            CalculateContainerHeight();
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

                if (m_SelectedItemBase==null)
                {
                    return;
                }

                float oldCharacterAtkValue = m_CharacterItem.atkValue;

                bool learn = MainGameMgr.S.CharacterMgr.LearnKungfu(m_CharacterItem.id, m_CurIndex, new KungfuItem((KungfuType)m_SelectedItemBase.GetSubName()));

                if (learn)
                {
                    PanelPool.S.AddPromotion(new LearnMartialArts(m_CharacterItem.id, oldCharacterAtkValue, (KungfuItem)m_SelectedItemBase));

                    MainGameMgr.S.InventoryMgr.RemoveItem(m_SelectedItemBase);

                    EventSystem.S.Send(EventID.OnSelectedKungfuSuccess, m_CurIndex, m_CharacterItem.id);

                    m_CharacterItem.CheckKungfuRedPoint();

                    DataAnalysisMgr.S.CustomEvent(DotDefine.students_learn, m_CurIndex.ToString() + ";" + m_SelectedItemBase.GetSubName().ToString());

                    UIMgr.S.ClosePanelAsUIID(UIID.LearnKungfuPanel);

                }
            });
        }
        private void Update()
        {
            //if (IsSelected)
            //    m_ArrangeBtn.transform.position = m_Pos.position;
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
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
            sprites.Add(FindSprite(GetIconName((KungfuType)itemBase.GetSubName())));
            switch (GetKungfuQuality((KungfuType)itemBase.GetSubName()))
            {
                case KungfuQuality.Normal:
                    sprites.Add(FindSprite("Introduction"));
                    break;
                case KungfuQuality.Master:
                    sprites.Add(FindSprite("Advanced"));
                    break;
                case KungfuQuality.Super:
                    sprites.Add(FindSprite("Excellent"));
                    break;
                default:
                    break;
            }
            return sprites;
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }

        private string GetIconName(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
    }
}