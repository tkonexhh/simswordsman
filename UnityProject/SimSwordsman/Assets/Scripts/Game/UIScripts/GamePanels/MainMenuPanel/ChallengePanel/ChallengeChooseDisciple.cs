using Qarth;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ChallengeChooseDisciple : AbstractAnimPanel
    {
        [SerializeField] private Button m_CloseBtn;
        [Header("Bottom")]
        [SerializeField] private Text m_RecommendedSkillsTitle;
        [SerializeField] private Text m_RecommendedSkillsValue;
        [SerializeField] private Text m_SelectedDiscipleSkillTitle;
        [SerializeField] private Text m_SelectedDiscipleSkillValue;
        [SerializeField] private Text m_State;
        [SerializeField] private Image m_StateBg;
        [SerializeField] private Transform m_Cont;
        [SerializeField] private GameObject m_ChallengePanelDisciple;
        [SerializeField] private GameObject m_ChallengePanelDisciple_Tower;
        [Header("Top")]
        [SerializeField] private Transform m_Bottom;
        [SerializeField] private GameObject m_ChallengeSelectedDisciple;
        [SerializeField] private GameObject m_ChallengeSelectedDisciple_Tower;
        [SerializeField] private Button m_ConfirmBtn;
        [SerializeField] private Text m_ConfirmText;


        private LevelConfigInfo m_LevelConfigInfo = null;
        private TowerPanelChallengeToSelect m_TowerLevelConfig;

        private PanelType m_PanelType;
        private const int ChallengeSelectedDiscipleNumber = 5;

        private List<CharacterItem> m_AllDiscipleList;
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<ChallengeSelectedDisciple> m_SelectedDiscipleObjList = new List<ChallengeSelectedDisciple>();
        private Dictionary<int, ChallengePanelDisciple> m_DiscipleObjDic = new Dictionary<int, ChallengePanelDisciple>();

        private const int DeliverDiscipleNumber = 4;
        private SingleDeliverDetailData m_SingleDeliverDetailData;

        private const int HeroTrialDiscipleNumber = 1;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            RegisterEvent(EventID.OnSelectedEvent, HandAddListenerEvent);

            AudioMgr.S.PlaySound(Define.INTERFACE);
            GetInformationForNeed();
            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Deliver:
                    m_SingleDeliverDetailData = args[1] as SingleDeliverDetailData;
                    CommonUIMethod.BubbleSortForType(m_AllDiscipleList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        if (m_AllDiscipleList[i].IsFreeState())
                            CreateDisciple(m_AllDiscipleList[i]);
                    for (int i = 0; i < DeliverDiscipleNumber; i++)
                        CreateSelectedDisciple();
                    RefreshFixedInfo(PanelType.Deliver);
                    m_ConfirmText.text = "出发";
                    break;
                case PanelType.Challenge:
                    m_LevelConfigInfo = args[1] as LevelConfigInfo;
                    CommonUIMethod.BubbleSortForType(m_AllDiscipleList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        CreateDisciple(m_AllDiscipleList[i]);

                    for (int i = 0; i < ChallengeSelectedDiscipleNumber; i++)
                        CreateSelectedDisciple();

                    RefreshDisicipleSkill();
                    break;
                case PanelType.HeroTrial:
                    //CommonUIMethod.BubbleSortForType(m_AllDiscipleList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromBigToSmall);
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        if (m_AllDiscipleList[0].quality == CharacterQuality.Perfect || m_AllDiscipleList[0].level >= 200)
                            CreateDisciple(m_AllDiscipleList[i]);
                    for (int i = 0; i < HeroTrialDiscipleNumber; i++)
                        CreateSelectedDisciple();
                    RefreshFixedInfo(PanelType.HeroTrial);
                    m_ConfirmText.text = "出发";
                    break;
                case PanelType.Tower:
                    for (int i = m_AllDiscipleList.Count - 1; i >= 0; i--)
                    {
                        //移除等级低于要求的弟子
                        if (m_AllDiscipleList[i].level < TowerDefine.CHARACT_MINLEVEL)
                        {
                            m_AllDiscipleList.RemoveAt(i);
                        }
                    }
                    CommonUIMethod.BubbleSortForType(m_AllDiscipleList, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);

                    //接下来排序
                    var towerData = GameDataMgr.S.GetPlayerData().towerData;
                    m_AllDiscipleList.Sort((x, y) =>
                    {
                        var charactX = towerData.GetTowerCharacterByID(x.id);
                        var charactY = towerData.GetTowerCharacterByID(x.id);
                        if (charactX == null && charactY == null)
                        {
                            return 0;
                        }

                        // if (charactX == null || charactY == null)
                        // {
                        //     bool? isDeadX = charactX?.IsDead();
                        //     bool? isDeadY = charactY?.IsDead();
                        //     return isDeadX > isDeadY;
                        // }

                        return 0;
                    });


                    m_TowerLevelConfig = (TowerPanelChallengeToSelect)args[1];
                    for (int i = 0; i < m_AllDiscipleList.Count; i++)
                        CreateDisciple(m_AllDiscipleList[i]);
                    for (int i = 0; i < ChallengeSelectedDiscipleNumber; i++)
                        CreateSelectedDisciple();

                    RefreshDisicipleSkill();
                    break;
                default:
                    break;
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
            m_AllDiscipleList.Clear();
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
                switch (m_PanelType)
                {
                    case PanelType.Challenge:
                        if (m_SelectedDiscipleDic.Count >= ChallengeSelectedDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                            return;
                        }
                        break;
                    case PanelType.Deliver:
                        if (m_SelectedDiscipleDic.Count >= DeliverDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                            return;
                        }
                        break;
                    case PanelType.HeroTrial:
                        if (m_SelectedDiscipleDic.Count >= HeroTrialDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                            return;
                        }
                        break;
                    case PanelType.Tower:
                        if (m_SelectedDiscipleDic.Count >= ChallengeSelectedDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("选择人数已满，请重新选择");
                            return;
                        }
                        break;
                    default:
                        break;
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
                case PanelType.Challenge:
                case PanelType.Tower:
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
            m_SelectedDiscipleSkillValue.text = CommonUIMethod.GetStrForColor("#A35953", CommonUIMethod.GetTenThousandOrMillion((long)atkValue));

            int selected = (int)atkValue;
            long recommended = 0;
            if (m_PanelType == PanelType.Challenge)
            {
                recommended = m_LevelConfigInfo.recommendAtkValue;
            }
            else if (m_PanelType == PanelType.Tower)
            {
                recommended = m_TowerLevelConfig.recommendATK;
            }
            m_RecommendedSkillsValue.text = CommonUIMethod.GetStrForColor("#405787", CommonUIMethod.GetTenThousandOrMillion(recommended));

            try
            {
                float result = selected / recommended;
                if (result < 0.75)
                    m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_DANGER);
                else if (result > 1.1f)
                    m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
                else
                    m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_AUTIOUS);
            }
            catch (DivideByZeroException)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
            }


        }
        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            if (keyValuePairs != null)
            {
                foreach (var item in keyValuePairs.Values)
                    EventSystem.S.Send(EventID.OnSelectedEvent, item, true);

                RefreshPanelInfo();
            }
        }
        private void RefreshPanelInfo()
        {
            foreach (var item in m_SelectedDiscipleDic.Values)
            {
                if (m_DiscipleObjDic.ContainsKey(item.id))
                    m_DiscipleObjDic[item.id].SetItemState(true);
            }
        }

        private void RefreshFixedInfo(PanelType panelType)
        {
            m_SelectedDiscipleSkillTitle.gameObject.SetActive(false);
            m_SelectedDiscipleSkillValue.gameObject.SetActive(false);
            m_StateBg.gameObject.SetActive(false);
            m_State.gameObject.SetActive(false);
            if (panelType == PanelType.Deliver || panelType == PanelType.HeroTrial)
            {
                m_RecommendedSkillsTitle.gameObject.SetActive(false);
                m_RecommendedSkillsValue.gameObject.SetActive(false);
            }
            else
            {
                m_RecommendedSkillsTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NEEDLEVEL);
                // m_RecommendedSkillsValue.text = CommonUIMethod.GetGrade(m_CommonTaskItemInfo.characterLevelRequired);
            }
        }
        private void GetInformationForNeed()
        {
            m_AllDiscipleList = new List<CharacterItem>(MainGameMgr.S.CharacterMgr.GetAllCharacterList());
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_ConfirmBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                switch (m_PanelType)
                {
                    case PanelType.Challenge:
                        if (m_SelectedDiscipleDic.Count != ChallengeSelectedDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("人数不足5人，请选满");
                            return;
                        }
                        break;
                    case PanelType.Deliver:
                        if (m_SelectedDiscipleDic.Count != DeliverDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("人数不足4人，请选满");
                            return;
                        }
                        break;
                    case PanelType.HeroTrial:
                        if (m_SelectedDiscipleDic.Count != HeroTrialDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("人数不足1人，请选满");
                            return;
                        }
                        break;
                    case PanelType.Tower:
                        break;
                    default:
                        break;
                }

                EventSystem.S.Send(EventID.OnSelectedConfirmEvent, m_SelectedDiscipleDic, m_SingleDeliverDetailData?.DeliverID);
                HideSelfWithAnim();
            });
        }
        /// <summary>
        /// 创建所有弟子
        /// </summary>
        /// <param name="characterItem"></param>
        private void CreateDisciple(CharacterItem characterItem)
        {
            GameObject prefab = m_ChallengePanelDisciple;
            if (m_PanelType == PanelType.Tower)
            {
                prefab = m_ChallengePanelDisciple_Tower;
            }
            GameObject obj = Instantiate(prefab, m_Cont);
            ChallengePanelDisciple itemICom = obj.GetComponent<ChallengePanelDisciple>();
            itemICom.Init(characterItem, this);
            m_DiscipleObjDic.Add(characterItem.id, itemICom);
        }


        /// <summary>
        /// 创建已选择的弟子
        /// </summary>
        private void CreateSelectedDisciple()
        {
            GameObject prefab = m_ChallengeSelectedDisciple;
            if (m_PanelType == PanelType.Tower)
            {
                prefab = m_ChallengeSelectedDisciple_Tower;
            }
            GameObject obj = Instantiate(prefab, m_Bottom);
            ChallengeSelectedDisciple itemICom = obj.GetComponent<ChallengeSelectedDisciple>();
            itemICom.Init(this);
            m_SelectedDiscipleObjList.Add(itemICom);
        }
    }
}