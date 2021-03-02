using Qarth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private CommonTaskItemInfo m_CommonTaskItemInfo = null;
        private PanelType m_PanelType;
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
                if (m_PanelType == PanelType.Challenge)
                {
                    if (m_SelectedDiscipleDic.Count >= ChallengeSelectedDiscipleNumber)
                    {
                        FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                        return;
                    }
                }
                else
                {
                    if (m_SelectedDiscipleDic.Count >= m_CommonTaskItemInfo.GetCharacterAmount())
                    {
                        FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                        return;
                    }
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
            switch (m_PanelType)
            {
                case PanelType.Task:
                    break;
                case PanelType.Challenge:
                    RefreshDisicipleSkill();
                    break;
                default:
                    break;
            }
        }
        private void RefreshDisicipleSkill()
        {
            float atkValue = 0;
            foreach (var item in m_SelectedDiscipleDic.Values)
                atkValue += item.atkValue;
            m_SelectedDiscipleSkillValue.text = CommonUIMethod.GetStrForColor("#A35953", atkValue.ToString());

            int selected = (int)atkValue;
            long recommended = m_LevelConfigInfo.recommendAtkValue;
            float result = selected / recommended;
            if (result < 0.75)
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_DANGER);
            else if (result > 1.1f) 
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
            else
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_AUTIOUS);
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
            m_PanelType = (PanelType)args[1];
            m_CommonTaskItemInfo = args[2] as CommonTaskItemInfo;

            CommonUIMethod.BubbleSortForType(m_AllDiscipleList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);

            switch (m_PanelType)
            {
                case PanelType.Task:
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        if (m_AllDiscipleList[i].level >= m_CommonTaskItemInfo.characterLevelRequired)
                            CreateDisciple(m_AllDiscipleList[i]);

                    for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                        CreateSelectedDisciple();
                    RefreshFixedInfo();
                    break;
                case PanelType.Challenge:
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        CreateDisciple(m_AllDiscipleList[i]);

                    for (int i = 0; i < ChallengeSelectedDiscipleNumber; i++)
                        CreateSelectedDisciple();
                    m_RecommendedSkillsValue.text = CommonUIMethod.GetStrForColor("#405787", m_LevelConfigInfo.recommendAtkValue.ToString());
                    RefreshDisicipleSkill();
                    break;
                default:
                    break;
            }
        }
        private void RefreshFixedInfo()
        {
            m_SelectedDiscipleSkillTitle.gameObject.SetActive(false);
            m_SelectedDiscipleSkillValue.gameObject.SetActive(false);
            m_StateBg.gameObject.SetActive(false);
            m_State.gameObject.SetActive(false);
            m_RecommendedSkillsTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NEEDLEVEL);
            m_RecommendedSkillsValue.text = CommonUIMethod.GetGrade(m_CommonTaskItemInfo.characterLevelRequired);
            //m_SelectedDiscipleSkillTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_SELECTEDDISCIPLEYSKILLS);
            //m_RecommendedSkillsValue.text = 
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
                    FloatMessage.S.ShowMsg("人数不足五人，请选满");
                    return;
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
            itemICom.OnInit(characterItem,this);
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