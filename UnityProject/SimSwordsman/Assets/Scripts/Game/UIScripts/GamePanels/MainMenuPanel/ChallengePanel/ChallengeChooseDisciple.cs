using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
using System;

namespace GameWish.Game
{
	public class ChallengeChooseDisciple : AbstractAnimPanel
	{

        [Header("Bottom")]
        [SerializeField]
        private Text m_ChoiceDiscipleTitle;
        [SerializeField]
        private Text m_RecommendedSkillsTitle;
        [SerializeField]
        private Text m_RecommendedSkillsValue;
        [SerializeField]
        private Text m_SelectedDiscipleSkillTitle;
        [SerializeField]
        private Text m_SelectedDiscipleSkillValue;
        [SerializeField]
        private Text m_State;
        [SerializeField]
        private Image m_StateBg;
        [SerializeField]
        private Transform m_Cont;
        [SerializeField]
        private GameObject m_ChallengePanelDisciple;
        [Header("Top")]
        [SerializeField]
        private Transform m_Bottom;
        [SerializeField]
        private GameObject m_ChallengeSelectedDisciple;
        [SerializeField]
        private Button m_ConfirmBtn;
        [SerializeField]
        private Text m_ConfirmText;

        private const int ChallengeSelectedDiscipleNumber = 5;

        private List<CharacterItem> m_AllDiscipleList;
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<ChallengeSelectedDisciple> m_SelectedDiscipleObjList = new List<ChallengeSelectedDisciple>();
        private Dictionary<int, ChallengePanelDisciple> m_DiscipleObjDic = new Dictionary<int, ChallengePanelDisciple>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectedDiscipleEvent, HandAddListenerEvent);

            BindAddListenerEvent();

            GetInformationForNeed();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedDiscipleEvent, HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedDiscipleEvent:
                    HandSelectedDiscipleEvent((CharacterItem)param[0], (bool)param[1]);
                    break;
                default:
                    break;
            }
        }

        private void HandSelectedDiscipleEvent(CharacterItem characterItem, bool seleted)
        {
            //ѡ��
            if (seleted)
            {
                if (m_SelectedDiscipleDic.Count >= ChallengeSelectedDiscipleNumber)
                {
                    FloatMessage.S.ShowMsg("ѡ������������������ѡ��");
                    return;
                }

                if (!m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Add(characterItem.id, characterItem);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelelctedState.NotSelected)
                        {
                            item.SetSelectedDisciple(characterItem, seleted);
                            break;
                        }
                    }
                }

                foreach (var item in m_DiscipleObjDic.Values)
                {
                    if (item.IsHavaSameDisciple(characterItem))
                        item.SetItemState(true);
                }
            }
            //ѡ��ȡ��
            else
            {
                if (m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Remove(characterItem.id);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelelctedState.Selected && item.IsHavaSame(characterItem))
                        {
                            item.SetSelectedDisciple(null, seleted);
                            break;
                        }
                    }
                }

                foreach (var item in m_DiscipleObjDic.Values)
                {
                    if (item.IsHavaSameDisciple(characterItem))
                        item.SetItemState(false);
                }
            }
        }
        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            foreach (var item in keyValuePairs.Values)
                EventSystem.S.Send(EventID.OnSelectedDiscipleEvent, item, true);

            RefreshPanelInfo();
        }
        private void RefreshPanelInfo()
        {
            foreach (var item in m_SelectedDiscipleDic.Values)
            {
                if (m_DiscipleObjDic.ContainsKey(item.id))
                    m_DiscipleObjDic[item.id].SetItemState(true);
            }
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            for (int i = 0; i < m_AllDiscipleList.Count; i++)
            {
                if ( m_AllDiscipleList[i].characterStateId == CharacterStateID.Wander)
                    CreateDisciple(m_AllDiscipleList[i]);
            }

            for (int i = 0; i < ChallengeSelectedDiscipleNumber; i++)
                CreateSelectedDisciple();

        }
        private void GetInformationForNeed()
        {
            m_AllDiscipleList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void BindAddListenerEvent()
        {
            m_ConfirmBtn.onClick.AddListener(()=> {
                if (m_SelectedDiscipleDic.Count!= ChallengeSelectedDiscipleNumber)
                {
                    //FloatMessage.S.ShowMsg("�����������ˣ���ѡ��");
                    //return;
                }
                EventSystem.S.Send(EventID.OnSelectedConfirmEvent, m_SelectedDiscipleDic);
                HideSelfWithAnim();
            });
        }
        /// <summary>
        /// �������е���
        /// </summary>
        /// <param name="characterItem"></param>
        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject obj = Instantiate(m_ChallengePanelDisciple, m_Cont);
            ChallengePanelDisciple itemICom = obj.GetComponent<ChallengePanelDisciple>();
            itemICom.OnInit(characterItem);
            m_DiscipleObjDic.Add(characterItem.id, itemICom);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }

        /// <summary>
        /// ������ѡ��ĵ���
        /// </summary>
        private void CreateSelectedDisciple()
        {
            GameObject obj = Instantiate(m_ChallengeSelectedDisciple, m_Bottom);
            ChallengeSelectedDisciple itemICom = obj.GetComponent<ChallengeSelectedDisciple>();
            itemICom.OnInit(this);
            m_SelectedDiscipleObjList.Add(itemICom);
        }
    }
}