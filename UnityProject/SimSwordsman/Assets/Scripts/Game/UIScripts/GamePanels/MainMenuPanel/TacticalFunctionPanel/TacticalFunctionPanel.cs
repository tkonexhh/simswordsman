using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class TacticalFunctionPanel : AbstractAnimPanel
    {
        [Header("Upper")]
        [SerializeField]
        private Transform m_Upper;
        [SerializeField]
        private GameObject m_FontPrefabs;

        [Header("Middle Upper")]
        [SerializeField]
        private Image m_TaskImg;
        [SerializeField]
        private Text m_TaskDesc;

        [Header("Middle")]
        [SerializeField]
        private Transform m_DiscipleTra;
        [SerializeField]
        private GameObject m_TacticalFunctionDisciple;

        [Header("MiddleDown")]
        [SerializeField]
        private Transform m_RewardTra;
        [SerializeField]
        private GameObject m_Reward;

        [Header("Down")]
        [SerializeField]
        private Button m_Accept;
        [SerializeField]
        private Button m_Refuse;
        [SerializeField]
        private Button m_BlackExit;
        private SimGameTask m_SimGameTask = null;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private List<CharacterItem> m_CharacterAllItemList;
        private List<TaskReward> m_ItemReward;
        private List<TacticalFunctionDisciple> m_DiscipleList = new List<TacticalFunctionDisciple>();
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<CharacterController> m_SelectedList;
        private List<HerbType> m_PlayerDataHerb = new List<HerbType>();
        public SimGameTask SimGameTask { get { return m_SimGameTask; } }


        #region AbstractAnimPanel
        protected override void OnUIInit()
        {
            base.OnUIInit();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            EventSystem.S.Register(EventID.OnDiscipleButtonOnClick, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            GetInformationForNeed();

            BindAddListenerEvent();

        }



        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_SimGameTask = (SimGameTask)args[0];
            m_CommonTaskItemInfo = m_SimGameTask.CommonTaskItemInfo;
            m_ItemReward = m_CommonTaskItemInfo.GetItemRewards();
            InitPanelInfo();

            for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                CreateDisciple();

            AutoSelectedAndAddTo();
            m_SimGameTask.RecordDiscipleID(m_SelectedDiscipleDic);
            RewardItem();
            m_SelectedList = Transformation(m_SelectedDiscipleDic);

        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnDiscipleButtonOnClick, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
        #endregion
        #region Private
        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnDiscipleButtonOnClick:
                    UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, PanelType.Task, m_CommonTaskItemInfo); ;
                    break;
                case EventID.OnSelectedConfirmEvent:
                    m_SelectedDiscipleDic.Clear();
                    m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)param[0];
                    List<CharacterItem> characterItemList = new List<CharacterItem>();
                    characterItemList.AddRange(m_SelectedDiscipleDic.Values);
                    for (int i = 0; i < m_DiscipleList.Count; i++)
                        m_DiscipleList[i].SelectedDisciple(characterItemList[i]);

                    m_SimGameTask.RecordDiscipleID(m_SelectedDiscipleDic);
                    break;
            }
        }
        private void OpenCallback(AbstractPanel obj)
        {
            ChallengeChooseDisciple challengeChooseDisciple = obj as ChallengeChooseDisciple;
            challengeChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
        }
        private void GetInformationForNeed()
        {
            m_CharacterAllItemList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            CommonUIMethod.BubbleSortForType(m_CharacterAllItemList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
        }
        private void AutoSelectedAndAddTo()
        {
            List<CharacterItem> allCharacterList = new List<CharacterItem>();
            foreach (var item in m_CharacterAllItemList)
                if (item.level >= m_CommonTaskItemInfo.characterLevelRequired)
                    allCharacterList.Add(item);
            if (allCharacterList.Count <= m_CommonTaskItemInfo.GetCharacterAmount())
            {
                for (int i = 0; i < allCharacterList.Count; i++)
                {
                    m_DiscipleList[i].SelectedDisciple(allCharacterList[i]);
                    m_SelectedDiscipleDic.Add(allCharacterList[i].id, allCharacterList[i]);
                }
            }
            else
            {
                for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                {
                    m_SelectedDiscipleDic.Add(allCharacterList[i].id, allCharacterList[i]);
                    m_DiscipleList[i].SelectedDisciple(allCharacterList[i]);
                }
            }
        }
        private List<CharacterController> Transformation(Dictionary<int, CharacterItem> m_SelectedDiscipleDic)
        {
            List<CharacterController> characterController = new List<CharacterController>();
            foreach (var item in m_SelectedDiscipleDic.Values)
                characterController.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            return characterController;
        }
        private void CreateDisciple()
        {
            TacticalFunctionDisciple tacticalFunction = Instantiate(m_TacticalFunctionDisciple, m_DiscipleTra).GetComponent<TacticalFunctionDisciple>();
            tacticalFunction.OnInit(this);
            m_DiscipleList.Add(tacticalFunction);
        }

        private void InitPanelInfo()
        {
            string title = m_CommonTaskItemInfo.title.ToString();
            for (int i = 0; i < title.Length; i++)
                CreateFontPrefabs(title[i].ToString());
            m_TaskImg.sprite = FindSprite("enemy_icon_" + m_CommonTaskItemInfo.iconRes);
            m_TaskDesc.text = m_CommonTaskItemInfo.taskTxt;
        }

        private void RewardItem()
        {
            Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(this, m_SimGameTask.GetTaskReward());
            Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(this, RewardItemType.Exp_Kongfu, m_CommonTaskItemInfo.kongfuReward);
            Instantiate(m_Reward, m_RewardTra).GetComponent<TacticalReward>().RefreshRewardInfo(this, RewardItemType.Exp_Role, m_CommonTaskItemInfo.expReward);
        }

        private void CreateFontPrefabs(string font)
        {
            Instantiate(m_FontPrefabs, m_Upper).GetComponent<FontPrefabs>().SetFontText(font);
        }
        private void BindAddListenerEvent()
        {
            m_Accept.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                SwitchCharacterController();
                ////if (m_SelectedList != null)
                //return;
                if (m_SelectedList.Count < m_CommonTaskItemInfo.GetCharacterAmount())
                {
                    FloatMessage.S.ShowMsg("请选择满弟子 !");
                    return;
                }
                //m_SimGameTask.ExecuteTask(m_SelectedList);
                List<EnemyConfig> enemiesList = new List<EnemyConfig>();
                List<TaskEnemy> taskEnemies = m_SimGameTask.CommonTaskItemInfo.taskEnemies;
                for (int i = 0; i < taskEnemies.Count; i++)
                {
                    enemiesList.Add(new EnemyConfig(taskEnemies[i].enemyId, 1, taskEnemies[i].enemyAtk));
                }
                EventSystem.S.Send(EventID.OnEnterBattle, enemiesList, m_SelectedList, m_PlayerDataHerb);
                UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, PanelType.Task, m_SimGameTask, enemiesList);
                UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
                HideSelfWithAnim();
            });
            m_Refuse.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                DataAnalysisMgr.S.CustomEvent(DotDefine.c_task_refuse, m_SimGameTask.TaskId.ToString());
                //UIMgr.S.OpenPanel(UIID.LogPanel, LogCallBack, "提示", "您确定要放弃任务吗");
                RefuseTask();
                HideSelfWithAnim();
            });
            m_BlackExit.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        private void SwitchCharacterController()
        {
        }

        //private void LogCallBack(AbstractPanel abstractPanel)
        //{
        //    LogPanel logPanel = abstractPanel as LogPanel;
        //    logPanel.OnSuccessBtnEvent += RefuseTask;
        //}

        private void RefuseTask()
        {
            MainGameMgr.S.CommonTaskMgr.RemoveTask(m_SimGameTask.TaskId);
            EventSystem.S.Send(EventID.OnDeleteTaskBtn, m_SimGameTask.TaskId);
        }
        #endregion
    }
}