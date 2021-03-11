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
        [SerializeField]
        private Button m_ArrangeBtn;
        private FacilityType m_CurFacilityType;
        private int m_CurLevel;
        private List<CharacterItem> m_CharacterItem = null;
        private CharacterItem m_SelectedDisciple = null;
        private PracticeField m_PracticeFieldInfo = null;
        private List<PracticeDisciple> m_PracticeDisciple = new List<PracticeDisciple>();
        private Transform m_Pos;
        private bool IsSelected = false;


        private const int Rows = 5;
        private const float DiscipleHeight = 156f;
        private const float BtnHeight = 38f;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            EventSystem.S.Register(EventID.OnSelectedEvent, HandAddListenerEvent);

            m_ArrangeBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_PracticeFieldInfo.SetCharacterItem(m_SelectedDisciple, SlotState.Practice, m_CurFacilityType);
                EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, m_PracticeFieldInfo);
                HideSelfWithAnim();
            });
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedEvent, HandAddListenerEvent);

        }
        private void CalculateContainerHeight()
        {
            int rows = m_PracticeDisciple.Count / Rows;
            if ((m_PracticeDisciple.Count % Rows) != 0)
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
                    foreach (var item in m_PracticeDisciple)
                        item.IsSame(m_SelectedDisciple);
                    break;
                default:
                    break;
            }
        }
        private void Update()
        {
            if (IsSelected)
                m_ArrangeBtn.transform.position = m_Pos.position;
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PracticeFieldInfo = (PracticeField)args[0];
            m_CurFacilityType = (FacilityType)args[1];
            m_CurLevel = (int)args[2];
            GetInformationForNeed();

            BindAddListenerEvent();

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            int maxLevel = TDFacilityLobbyTable.GetPracticeLevelMax(lobbyLevel);
            CommonUIMethod.BubbleSortForType(m_CharacterItem, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
            for (int i = 0; i < m_CharacterItem.Count; i++)
            {
                if (m_CharacterItem[i].IsFreeState() && m_CharacterItem[i].level< maxLevel)
                    CreateDisciple(m_CharacterItem[i]);
            }
            CalculateContainerHeight();
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
            PracticeDisciple discipleItem = disciple.GetComponent<PracticeDisciple>();

            discipleItem.OnInit(characterItem,this);
            m_PracticeDisciple.Add(discipleItem);
        }
    }
}