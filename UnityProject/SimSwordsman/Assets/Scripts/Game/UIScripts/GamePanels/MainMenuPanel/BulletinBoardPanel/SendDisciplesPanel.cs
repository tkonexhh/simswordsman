using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{

    public enum PanelType
    {
        Task,
        Challenge,
    }

    public class SendDisciplesPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_ClsseBtn;
        [SerializeField]
        private Text m_SendDisciplesTitle;
        [SerializeField]
        private Text m_RecommendedSkillsTitle;
        [SerializeField]
        private Text m_RecommendedSkillsValue;
        [SerializeField]
        private Text m_SelectedDiscipleSkillTitle;
        [SerializeField]
        private Text m_SelectedDiscipleSkillValue;
        [SerializeField]
        private Image m_StateBg;
        [SerializeField]
        private Text m_State;
        [Header("Middle")]
        [SerializeField]
        private Transform m_UnselectedTrans;
        [SerializeField]
        private GameObject m_DiscipleItem;

        [Header("MiddleDown")]
        [SerializeField]
        private Text m_HerbTitle;
        [SerializeField]
        private Transform m_HerbalMedicineItemTra;
        [SerializeField]
        private GameObject m_HerbalMedicineItem;

        [Header("Down")]

        [SerializeField]
        private Button m_AutoSelectedBtn;
        [SerializeField]
        private Text m_AutoSelectedText;
        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Text m_AcceptText;
        [SerializeField]
        private Button m_RefuseBtn;
        [SerializeField]
        private Text m_RefuseText;

        private PanelType m_PanelType;

        private SimGameTask m_CurTaskInfo = null;

        private const int MaxDiscipleNumber = 5;

        private List<CharacterItem> m_AllCharacterList = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;

        private Dictionary<int, DiscipleItem> m_SelectedDic = new Dictionary<int, DiscipleItem>();
        private Dictionary<int, HerbalMedicineItem> m_PlayerDataHerbDic = new Dictionary<int, HerbalMedicineItem>();

        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<SendSelectedDisciple> m_ChallengeDiscipleList = new List<SendSelectedDisciple>();
      
        private List<CharacterController> m_SelectedList = new List<CharacterController>();
        private List<HerbType> m_PlayerDataHerb = new List<HerbType>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendHerbEvent, HandAddListenerEvent);

            GetInformationForNeed();

            BindAddListenerEvent();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSendHerbEvent, HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedConfirmEvent:
                    m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)param[0];
                    HandConfirmBtnEvent();
                    break;
                case EventID.OnSendDiscipleDicEvent:
                    UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback);
                    break;     
                case EventID.OnSendHerbEvent:
                    HandHerbEvent((bool)param[0], (HerbItem)param[1]);
                    break;
                default:
                    break;
            }
        }

        private void OpenCallback(AbstractPanel obj)
        {
            ChallengeChooseDisciple challengeChooseDisciple = obj as ChallengeChooseDisciple;
            challengeChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
        }

        private void HandConfirmBtnEvent()
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                case PanelType.Challenge:
                    int i = 0;
                    foreach (var item in m_SelectedDiscipleDic.Values)
                    {
                        m_ChallengeDiscipleList[i].RefreshSelectedDisciple(item);
                        i++;
                    }
                    for (int j = m_SelectedDiscipleDic.Values.Count; j < m_ChallengeDiscipleList.Count; j++)
                        m_ChallengeDiscipleList[j].RefreshSelectedDisciple(null);
                    break;
                default:
                    break;
            }
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            m_PanelType = (PanelType)args[0];
            InitPanelInfo();
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_CurTaskInfo = args[1] as SimGameTask;
                    m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)args[2];
                    HandConfirmBtnEvent();
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = args[1] as ChapterConfigInfo;
                    m_LevelConfigInfo = args[2] as LevelConfigInfo;
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

            int selected = int.Parse(m_SelectedDiscipleSkillValue.text);
            int recommended = int.Parse(m_RecommendedSkillsValue.text);
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
        private void GetInformationForNeed()
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                    break;
                case PanelType.Challenge:
                    break;
                default:
                    break;
            }
            m_AllCharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            //m_PlayerDataHerbDic = MainGameMgr.S.MedicinalPowderMgr.GetAllHerbs();
            //m_PlayerDataHerbDic = MainGameMgr.S.InventoryMgr.GetAllHerbs();
        }

        private void InitPanelInfo()
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                case PanelType.Challenge:
                    if (m_AllCharacterList != null)
                        for (int i = 0; i < MaxDiscipleNumber; i++)
                            CreateDisciple(m_UnselectedTrans);
                    break;
                default:
                    break;
            }
            for (int i = (int)HerbType.ChiDanZhuangQiWan; i <=  (int)HerbType.HuanHunDan; i++)
            {
                CreateHerb(i);
            }
        }

        private void BindAddListenerEvent()
        {
            m_ClsseBtn.onClick.AddListener(() =>
            {
                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });

            m_RefuseBtn.onClick.AddListener(() =>
            {
                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
            m_AutoSelectedBtn.onClick.AddListener(() =>
            {
                CloseSelfPanel();
                switch (m_PanelType)
                {
                    case PanelType.Task:
                        if (m_SelectedList.Count == 0)
                        {
                            foreach (var item in m_AllCharacterList)
                            {
                                if (item.IsFreeState())
                                    m_SelectedList.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
                            }
                        }
                        if (m_CurTaskInfo.GetCurTaskType() != SimGameTaskType.Battle)
                        {
                            m_CurTaskInfo.ExecuteTask(m_SelectedList);
                        }

                        if (m_CurTaskInfo.GetCurTaskType() == SimGameTaskType.Battle)
                        {
                            List<EnemyConfig> enemiesList = new List<EnemyConfig>();
                            List<TaskEnemy> taskEnemies = m_CurTaskInfo.CommonTaskItemInfo.taskEnemies;
                            for (int i = 0; i < taskEnemies.Count; i++)
                            {
                                enemiesList.Add(new EnemyConfig(taskEnemies[i].enemyId, 1, taskEnemies[i].enemyAtk));
                            }
                            EventSystem.S.Send(EventID.OnEnterBattle, enemiesList, m_SelectedList, enemiesList);
                            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        }
                        break;
                    case PanelType.Challenge:
                        AutoSelectedDisciple();
                        //EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        //UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        break;
                    default:
                        break;
                }
            });
            m_AcceptBtn.onClick.AddListener(() =>
            {
                CloseSelfPanel();
                m_SelectedList = Transformation(m_SelectedDiscipleDic);
                switch (m_PanelType)
                {
                    case PanelType.Task:
                        m_CurTaskInfo.ExecuteTask(m_SelectedList);
                        if (m_CurTaskInfo.GetCurTaskType() == SimGameTaskType.Battle)
                        {
                            List<EnemyConfig> enemiesList = new List<EnemyConfig>();
                            List<TaskEnemy> taskEnemies = m_CurTaskInfo.CommonTaskItemInfo.taskEnemies;
                            for (int i = 0; i < taskEnemies.Count; i++)
                            {
                                enemiesList.Add(new EnemyConfig(taskEnemies[i].enemyId, 1, taskEnemies[i].enemyAtk));
                            }
                            EventSystem.S.Send(EventID.OnEnterBattle, enemiesList, m_SelectedList, m_PlayerDataHerb);
                            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_PanelType,m_CurTaskInfo, enemiesList);
                        }
                        break;
                    case PanelType.Challenge:
                        if (m_SelectedList.Count!= MaxDiscipleNumber)
                        {
                            //FloatMessage.S.ShowMsg("请选择满弟子 !");
                            //return;
                        }
                        EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_PanelType, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        break;
                    default:
                        break;
                }
            });
        }

        private void AutoSelectedDisciple()
        {
            //TODO  按照弟子战力从高到底排序
            //MainGameMgr.S.CharacterMgr.GetAllCharacterList
            //m_SelectedDiscipleDic

        }

        private List<CharacterController> Transformation(Dictionary<int, CharacterItem> m_SelectedDiscipleDic)
        {
            List<CharacterController> characterController = new List<CharacterController>();
            foreach (var item in m_SelectedDiscipleDic.Values)
                characterController.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            return characterController;
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseDependPanel(EngineUI.MaskPanel);
            CloseSelfPanel();
        }

        private void CreateHerb(int herbID)
        {
            if (m_HerbalMedicineItem == null)
                return;
            HerbalMedicineItem herbItem = Instantiate(m_HerbalMedicineItem, m_HerbalMedicineItemTra).GetComponent<HerbalMedicineItem>();
            herbItem.OnInit(herbID);
            m_PlayerDataHerbDic.Add(herbID, herbItem);
        }

        /// <summary>
        /// 创建弟子
        /// </summary>
        /// <param name="parent">父物体</param>
        /// <param name="characterItem">弟子信息</param>
        /// <param name="action">按钮监听回调</param>
        /// <returns></returns>
        private void CreateDisciple(Transform parent)
        {
            SendSelectedDisciple discipeItem = Instantiate(m_DiscipleItem, parent).GetComponent<SendSelectedDisciple>();
            discipeItem.OnInit(m_PanelType);
            m_ChallengeDiscipleList.Add(discipeItem);
        }

        /// <summary>
        /// 草药按钮监听回调
        /// </summary>
        /// <param name="obj"></param>
        private void HandHerbEvent(bool obj, HerbItem herbItem)
        {
            if (herbItem == null)
                return;
            if (obj)
            {
                if (!m_PlayerDataHerb.Contains(herbItem.HerbID))
                    m_PlayerDataHerb.Add(herbItem.HerbID);
            }
            else
            {
                if (m_PlayerDataHerb.Contains(herbItem.HerbID))
                    m_PlayerDataHerb.Remove(herbItem.HerbID);
            }
        }
    }
}