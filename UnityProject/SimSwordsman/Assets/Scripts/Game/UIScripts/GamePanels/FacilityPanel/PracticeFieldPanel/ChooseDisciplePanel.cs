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
        [SerializeField] private Button m_CloseBtn;
        [SerializeField] private Transform m_SelectedList;
        [SerializeField] private GameObject m_Disciple;
        [SerializeField] private Button m_ArrangeBtn;
        private FacilityType m_CurFacilityType;
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

            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
            m_ArrangeBtn.onClick.AddListener(() =>
            {
                if (m_SelectedDisciple == null)
                    return;
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_PracticeFieldInfo.SelectCharacterItem(m_SelectedDisciple, m_CurFacilityType);
                EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, m_PracticeFieldInfo);

                DataAnalysisMgr.S.CustomEvent(DotDefine.f_practice, m_PracticeFieldInfo.Index.ToString());

                HideSelfWithAnim();
            });
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            RegisterEvent(EventID.OnSelectedEvent, HandAddListenerEvent);

            AudioMgr.S.PlaySound(Define.INTERFACE);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_PracticeFieldInfo = (PracticeField)args[0];
            m_CurFacilityType = (FacilityType)args[1];
            GetInformationForNeed();

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            int maxLevel = TDFacilityLobbyTable.GetPracticeLevelMax(lobbyLevel);

            //CommonUIMethod.BubbleSortForType(m_CharacterItem, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
            List<CharacterItem> characterItems = new List<CharacterItem>();
            QuickStartAddDisciple(CharacterQuality.Hero, characterItems, m_CharacterItem.Count, maxLevel);
            for (int i = 0; i < characterItems.Count; i++)
            {
                CreateDisciple(characterItems[i]);
                //if (m_CharacterItem[i].IsFreeState() && m_CharacterItem[i].level < maxLevel)
            }
            CalculateContainerHeight();
        }
        public void QuickStartAddDisciple(CharacterQuality quality, List<CharacterItem> characterItems, int surplus,int maxLevel)
        {
            if (characterItems.Count == m_CharacterItem.Count)
                return;

            List<CharacterItem> normalList = MainGameMgr.S.CharacterMgr.GetCharacterForQuality(quality);
            CommonUIMethod.BubbleSortForType(normalList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
            if (normalList.Count >= surplus)
            {
                int number = 0;
                for (int i = 0; i < normalList.Count; i++)
                {
                    if (number == surplus)
                        break;
                    if ( normalList[i].IsFreeState() && normalList[i].level < maxLevel)
                    {
                        number++;
                        characterItems.Add( normalList[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < normalList.Count; i++)
                    if ( normalList[i].IsFreeState() && normalList[i].level < maxLevel)
                    {
                        characterItems.Add(normalList[i]);
                    }
            }
            surplus = surplus - characterItems.Count;
            if ((int)quality > 0)
            {
                QuickStartAddDisciple(quality - 1, characterItems, surplus, maxLevel);
            }
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();
            CloseDependPanel(EngineUI.MaskPanel);
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
            //if (IsSelected)
            //    m_ArrangeBtn.transform.position = m_Pos.position;
        }

        private void GetInformationForNeed()
        {
            m_CharacterItem = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }




        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject disciple = Instantiate(m_Disciple, m_SelectedList);
            PracticeDisciple discipleItem = disciple.GetComponent<PracticeDisciple>();

            discipleItem.OnInit(characterItem, this);
            m_PracticeDisciple.Add(discipleItem);
        }
    }
}