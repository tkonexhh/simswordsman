using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class KungfuChooseDisciplePanel : AbstractAnimPanel
    {
        [SerializeField] private Button m_CloseBtn;
        [SerializeField] private Transform m_SelectedList;
        [SerializeField] private GameObject m_Disciple;
        [SerializeField] private Button m_ArrangeBtn;

        private const int Rows = 5;
        private const float DiscipleHeight = 153.5f;
        private const float BtnHeight = 38f;

        // private FacilityType m_CurFacilityType;
        // private int m_CurLevel;
        private List<CharacterItem> m_CharacterItem = null;
        private KungfuLibraySlot m_KungfuLibraySlotInfo = null;
        private CharacterItem m_SelectedDisciple = null;
        private List<KungfuLibraryDisciple> m_KungfuLibraryDisciple = new List<KungfuLibraryDisciple>();

        private Transform m_Pos;
        private bool IsSelected = false;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_ArrangeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_KungfuLibraySlotInfo.SelectCharacterItem(m_SelectedDisciple, FacilityType.KongfuLibrary);
                // CharacterController characterController = MainGameMgr.S.CharacterMgr.GetCharacterController(m_SelectedDisciple.id);
                // characterController.SetState(CharacterStateID.Reading, FacilityType.KongfuLibrary, DateTime.Now.ToString(), m_KungfuLibraySlotInfo.Index);
                // m_SelectedDisciple.SetCharacterStateData(CharacterStateID.Reading, FacilityType.KongfuLibrary, DateTime.Now.ToString(), m_Index);
                EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, m_KungfuLibraySlotInfo);
                //TODO ?޸?????
                DataAnalysisMgr.S.CustomEvent(DotDefine.f_copy_book, m_KungfuLibraySlotInfo.Index);

                HideSelfWithAnim();
            });
        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnSelectedEvent, HandAddListenerEvent);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            AudioMgr.S.PlaySound(Define.INTERFACE);
            BindAddListenerEvent();
            EventSystem.S.Register(EventID.OnSelectedEvent, HandAddListenerEvent);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_KungfuLibraySlotInfo = (KungfuLibraySlot)args[0];
            GetInformationForNeed();

            CommonUIMethod.BubbleSortForType(m_CharacterItem, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);

            for (int i = 0; i < m_CharacterItem.Count; i++)
            {
                if (m_CharacterItem[i].IsFreeState() && m_CharacterItem[i].level < Define.CHARACTER_MAX_LEVEL)
                    CreateDisciple(m_CharacterItem[i]);
            }

            CalculateContainerHeight();
        }
        /// <summary>
        /// ?????????߶?
        /// </summary>
        private void CalculateContainerHeight()
        {
            int rows = m_KungfuLibraryDisciple.Count / Rows;
            if ((m_KungfuLibraryDisciple.Count % Rows) != 0)
                rows += 1;

            float height = DiscipleHeight * rows;
            m_SelectedList.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + BtnHeight);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            CharacterItem selected = (CharacterItem)param[1];
            IsSelected = (bool)param[0];
            switch ((EventID)key)
            {
                case EventID.OnSelectedEvent:
                    m_Pos = (Transform)param[2];
                    m_ArrangeBtn.gameObject.SetActive(true);
                    if (m_SelectedDisciple != null && m_SelectedDisciple.id == selected.id)
                    {
                        if (!IsSelected)
                        {
                            m_SelectedDisciple = null;
                            m_ArrangeBtn.gameObject.SetActive(false);
                            return;
                        }
                    }
                    m_SelectedDisciple = selected;
                    foreach (var item in m_KungfuLibraryDisciple)
                        item.IsSame(m_SelectedDisciple);
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            //if (IsSelected)
            //    m_ArrangeBtn.transform.position = m_Pos.position + new Vector3(0, 0.05f, 0);
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
        }

        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject disciple = Instantiate(m_Disciple, m_SelectedList);
            KungfuLibraryDisciple discipleItem = disciple.GetComponent<KungfuLibraryDisciple>();
            discipleItem.OnInit(characterItem, this);
            m_KungfuLibraryDisciple.Add(discipleItem);
        }
    }
}