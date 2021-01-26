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

        [SerializeField]
        private Transform m_SelectedTrans;
        [SerializeField]
        private Transform m_UnselectedTrans;
        [SerializeField]
        private Transform m_HerbalMedicineItemTra;

        [SerializeField]
        private GameObject m_DiscipleItem;
        [SerializeField]
        private GameObject m_HerbalMedicineItem;


        [SerializeField]
        private Button m_AutoSelectedBtn;
        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;

        private PanelType m_PanelType;

        private SimGameTask m_CurTaskInfo = null;


        private List<CharacterItem> m_AllCharacterList = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;

        private Dictionary<int, DiscipleItem> m_SelectedDic = new Dictionary<int, DiscipleItem>();
        private List<HerbItem> m_PlayerDataHerbDic = new List<HerbItem>();

        private List<CharacterController> m_SelectedList = new List<CharacterController>();
        private List<HerbType> m_PlayerDataHerb = new List<HerbType>();
        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            InitPanelInfo();

            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_CurTaskInfo = args[1] as SimGameTask;
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = args[1] as ChapterConfigInfo;
                    m_LevelConfigInfo = args[2] as LevelConfigInfo;
                    break;
                default:
                    break;
            }

        }

        private void GetInformationForNeed()
        {
            m_AllCharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            //m_PlayerDataHerbDic = MainGameMgr.S.MedicinalPowderMgr.GetAllHerbs();
            m_PlayerDataHerbDic = MainGameMgr.S.InventoryMgr.GetAllHerbs();
        }

        private void InitPanelInfo()
        {
            if (m_AllCharacterList != null)
                foreach (var item in m_AllCharacterList)
                    CreateDisciple(m_UnselectedTrans, item, AddAllListenerBtn);

            foreach (var item in m_PlayerDataHerbDic)
            {
                CreateHerb(item, AddHerbListenerBtn);
            }
        }

        private void BindAddListenerEvent()
        {
            m_RefuseBtn.onClick.AddListener(() =>
            {
                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
            m_AutoSelectedBtn.onClick.AddListener(()=> 
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
                        EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        break;
                    default:
                        break;
                }
            });
            m_AcceptBtn.onClick.AddListener(() =>
            {
                CloseSelfPanel();
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
                            EventSystem.S.Send(EventID.OnEnterBattle, enemiesList, m_SelectedList, enemiesList);
                            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        }
                        break;
                    case PanelType.Challenge:
                        EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        break;
                    default:
                        break;
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        private void CreateHerb(HerbItem dataHerb, Action<object> action)
        {

            if (m_HerbalMedicineItem == null)
                return;
            Transform herbItem = Instantiate(m_HerbalMedicineItem, m_HerbalMedicineItemTra).transform;
            ItemICom herbItemICom = herbItem.GetComponent<ItemICom>();
            herbItemICom.OnInit(dataHerb);
            herbItemICom.SetButtonEvent(action);
        }

        /// <summary>
        /// 创建弟子
        /// </summary>
        /// <param name="parent">父物体</param>
        /// <param name="characterItem">弟子信息</param>
        /// <param name="action">按钮监听回调</param>
        /// <returns></returns>
        private DiscipleItem CreateDisciple(Transform parent, object characterItem, Action<object> action)
        {
            if (m_DiscipleItem == null)
                return null;
            DiscipleItem discipeItem = Instantiate(m_DiscipleItem, parent).GetComponent<DiscipleItem>();
            ItemICom discipleItem = discipeItem.GetComponent<ItemICom>();
            discipleItem.OnInit(characterItem);
            discipleItem.SetButtonEvent(action);
            return discipeItem;
        }
        /// <summary>
        /// 所有弟子的按钮监听
        /// </summary>
        /// <param name="obj"></param>
        private void AddAllListenerBtn(object obj)
        {
            CharacterItem item = obj as CharacterItem;
            if (!m_SelectedDic.ContainsKey(item.id))
            {
                m_SelectedDic.Add(item.id, CreateDisciple(m_SelectedTrans, obj, AddSelectedListenerBtn));
                m_SelectedList.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            }
        }

        /// <summary>
        /// 已选择弟子的按钮监听
        /// </summary>
        /// <param name="obj"></param>
        private void AddSelectedListenerBtn(object obj)
        {
            CharacterItem item = obj as CharacterItem;
            if (m_SelectedDic.ContainsKey(item.id))
            {
                DiscipleItem discipleItem = m_SelectedDic[item.id];
                m_SelectedDic.Remove(item.id);
                DestroyImmediate(discipleItem.gameObject);
                m_SelectedList.Remove(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            }
        }
        /// <summary>
        /// 草药按钮监听回调
        /// </summary>
        /// <param name="obj"></param>
        private void AddHerbListenerBtn(object obj)
        {
            HerbalMedicineItem item = obj as HerbalMedicineItem;
            item.SetStateSelected();
            if (item.GetHerbStatue())
            {
                if (!m_PlayerDataHerb.Contains((HerbType)item.GetCurHerbId()))
                    m_PlayerDataHerb.Add((HerbType)item.GetCurHerbId());
            }
            else
            {
                if (m_PlayerDataHerb.Contains((HerbType)item.GetCurHerbId()))
                    m_PlayerDataHerb.Remove((HerbType)item.GetCurHerbId());
            }

        }
    }
}