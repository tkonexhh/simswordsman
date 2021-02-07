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
        [SerializeField]
        private Button m_CloseBtn;

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
        private LevelConfigInfo m_LevelConfigInfo = null;
        private const int ChallengeSelectedDiscipleNumber = 5;

        private List<CharacterItem> m_AllDiscipleList;
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<ChallengeSelectedDisciple> m_SelectedDiscipleObjList = new List<ChallengeSelectedDisciple>();
        private Dictionary<int, ChallengePanelDisciple> m_DiscipleObjDic = new Dictionary<int, ChallengePanelDisciple>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectedEvent, HandAddListenerEvent);
            AudioMgr.S.PlaySound(Define.INTERFACE);
            BindAddListenerEvent();

            GetInformationForNeed();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedEvent, HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedEvent:
                    HandSelectedDiscipleEvent((CharacterItem)param[0], (bool)param[1]);
                    break;
                default:
                    break;
            }
        }

        private void HandSelectedDiscipleEvent(CharacterItem characterItem, bool seleted)
        {
            //选中
            if (seleted)
            {
                if (m_SelectedDiscipleDic.Count >= ChallengeSelectedDiscipleNumber)
                {
                    FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                    return;
                }

                if (!m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Add(characterItem.id, characterItem);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelectedState.NotSelected)
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
            //选中取消
            else
            {
                if (m_SelectedDiscipleDic.ContainsKey(characterItem.id))
                {
                    m_SelectedDiscipleDic.Remove(characterItem.id);
                    foreach (var item in m_SelectedDiscipleObjList)
                    {
                        if (item.GetSelelctedState() == SelectedState.Selected && item.IsHavaSame(characterItem))
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
            RefreshDisicipleSkill();
        }
        private void RefreshDisicipleSkill()
        {
            float atkValue = 0;
            foreach (var item in m_SelectedDiscipleDic.Values)
                atkValue += item.atkValue;
            m_SelectedDiscipleSkillValue.text = CommonUIMethod.GetStrForColor("#A35953", atkValue.ToString());

            int selected = (int)atkValue;
            int recommended = m_LevelConfigInfo.recommendAtkValue;
            float result = selected / recommended;
            if (result < 0.75)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
            }
            else if (result > 1.1f)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_DANGER);
                //m_StateBg.text = CommonUIMethod.GetStrForColor("#A35953", Define.BULLETINBOARD_DANGER);
            }
            else
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_AUTIOUS);
            }
        }
        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            foreach (var item in keyValuePairs.Values)
                EventSystem.S.Send(EventID.OnSelectedEvent, item, true);

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
            m_LevelConfigInfo = args[0] as LevelConfigInfo;


            m_RecommendedSkillsValue.text = m_LevelConfigInfo.recommendAtkValue.ToString();

            for (int i = 0; i < m_AllDiscipleList.Count; i++)
            {
                if ( m_AllDiscipleList[i].IsFreeState())
                    CreateDisciple(m_AllDiscipleList[i]);
            }

            for (int i = 0; i < ChallengeSelectedDiscipleNumber; i++)
                CreateSelectedDisciple();
            RefreshDisicipleSkill();

        }
        private void GetInformationForNeed()
        {
            m_AllDiscipleList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_ConfirmBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (m_SelectedDiscipleDic.Count!= ChallengeSelectedDiscipleNumber)
                {
                    //FloatMessage.S.ShowMsg("人数不足五人，请选满");
                    //return;
                }
                EventSystem.S.Send(EventID.OnSelectedConfirmEvent, m_SelectedDiscipleDic);
                HideSelfWithAnim();
            });
        }
        /// <summary>
        /// 创建所有弟子
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
        /// 创建已选择的弟子
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