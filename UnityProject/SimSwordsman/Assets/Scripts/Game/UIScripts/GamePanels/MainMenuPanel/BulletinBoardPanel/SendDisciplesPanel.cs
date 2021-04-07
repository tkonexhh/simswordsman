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
        private Button BlackBtn;
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
        private CommonTaskItemInfo m_CommonTaskItemInfo = null;

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
            AudioMgr.S.PlaySound(Define.INTERFACE);
            EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendHerbEvent, HandAddListenerEvent);

            GetInformationForNeed();

            BindAddListenerEvent();
        }

        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

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
                    if (m_PanelType == PanelType.Challenge)
                        RefreshDisicipleSkill();
                    else
                    {
                        EventSystem.S.Send(EventID.OnBulletinSelectedConfirmEvent, m_SelectedDiscipleDic, m_CommonTaskItemInfo);
                        m_CurTaskInfo.RecordDiscipleID(m_SelectedDiscipleDic);
                    }
                    break;
                case EventID.OnSendDiscipleDicEvent:
                    UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, m_LevelConfigInfo, (PanelType)param[0], m_CommonTaskItemInfo);
                    break;
                case EventID.OnSendHerbEvent:
                    HandHerbEvent((bool)param[0], (HerbItem)param[1]);
                    break;
                default:
                    break;
            }
        }
        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            foreach (var item in keyValuePairs.Values)
                m_SelectedDiscipleDic.Add(item.id, item);
            //RefreshPanelInfo();
            HandConfirmBtnEvent();
        }

        private void OpenCallback(AbstractPanel obj)
        {
            ChallengeChooseDisciple challengeChooseDisciple = obj as ChallengeChooseDisciple;
            challengeChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
        }

        private void HandConfirmBtnEvent()
        {
            int i = 0;
            foreach (var item in m_SelectedDiscipleDic.Values)
            {
                m_ChallengeDiscipleList[i].RefreshSelectedDisciple(item);
                i++;
            }
            for (int j = m_SelectedDiscipleDic.Values.Count; j < m_ChallengeDiscipleList.Count; j++)
                m_ChallengeDiscipleList[j].RefreshSelectedDisciple(null);
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_CurTaskInfo = args[1] as SimGameTask;
                    m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
                    HandConfirmBtnEvent();
                    m_AcceptText.text = "����ս��";
                    RefreshFixedInfo();
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = args[1] as ChapterConfigInfo;
                    m_LevelConfigInfo = args[2] as LevelConfigInfo;
                    m_AcceptText.text = "��ʼս��";
                    CommonUIMethod.GetStrForColor("#405787", m_LevelConfigInfo.recommendAtkValue.ToString());
                    m_RecommendedSkillsValue.text = CommonUIMethod.GetTenThousandOrMillion(m_LevelConfigInfo.recommendAtkValue);
                    RefreshDisicipleSkill();
                    break;
                default:
                    break;
            }
            InitPanelInfo();
        }
        private void RefreshFixedInfo()
        {
            m_SelectedDiscipleSkillTitle.gameObject.SetActive(false);
            m_SelectedDiscipleSkillValue.gameObject.SetActive(false);
            m_StateBg.gameObject.SetActive(false);
            m_State.gameObject.SetActive(false);
            m_RecommendedSkillsTitle.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NEEDLEVEL);
            m_RecommendedSkillsValue.text = CommonUIMethod.GetGrade(m_CommonTaskItemInfo.characterLevelRequired);
        }
        private void RefreshDisicipleSkill()
        {
            float atkValue = 0;
            foreach (var item in m_SelectedDiscipleDic.Values)
                atkValue += item.atkValue;

            int selected = (int)atkValue;
            for (int i = 0; i < m_PlayerDataHerb.Count; i++)
            {
                HerbConfig herbConfig = TDHerbConfigTable.GetHerbById((int)m_PlayerDataHerb[i]);
                float addition = herbConfig.PowerRatio;
                selected = (int)(selected * addition);
            }
            //if (m_PlayerDataHerb.Count>=1)
            //{
            //    HerbConfig herbConfig = TDHerbConfigTable.GetHerbById((int)m_PlayerDataHerb[0]);
            //    float addition = herbConfig.PowerRatio;
            //    selected = (int)(herb ? (int)atkValue * (addition) : atkValue);
            //}
            m_SelectedDiscipleSkillValue.text = CommonUIMethod.GetStrForColor("#A35953", CommonUIMethod.GetTenThousandOrMillion((long)selected));
            long recommended = m_LevelConfigInfo.recommendAtkValue;
            float result = (float)selected / recommended;

            if (result < 0.75)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_DANGER);
                m_StateBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.SendDisciplePanelAtlas, "SendDisciplePanel_Danger");
            }
            else if (result > 1.1f)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
                m_StateBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.SendDisciplePanelAtlas, "SendDisciplePanel_Rleaxed");
            }
            else
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_AUTIOUS);
                m_StateBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.SendDisciplePanelAtlas, "SendDisciplePanel_Autions");
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
        }

        private void InitPanelInfo()
        {
            switch (m_PanelType)
            {
                case PanelType.Task:
                    for (int i = 0; i < m_CurTaskInfo.CommonTaskItemInfo.GetCharacterAmount(); i++)
                        CreateDisciple(m_UnselectedTrans);
                    break;
                case PanelType.Challenge:
                    if (m_AllCharacterList != null)
                        for (int i = 0; i < MaxDiscipleNumber; i++)
                            CreateDisciple(m_UnselectedTrans);
                    break;
                default:
                    break;
            }
            for (int i = (int)HerbType.ChiDanZhuangQiWan; i <= (int)HerbType.HuanHunDan; i++)
            {
                CreateHerb(i);
            }
        }

        private void BindAddListenerEvent()
        {
            m_ClsseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            BlackBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });

            m_RefuseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_AutoSelectedBtn.onClick.AddListener(() =>
            {
                ///ֻ����ս��
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                AutoSelectedDisciple();
                if (m_PanelType == PanelType.Challenge)
                    RefreshDisicipleSkill();
                else
                {
                    EventSystem.S.Send(EventID.OnBulletinSelectedConfirmEvent, m_SelectedDiscipleDic, m_CommonTaskItemInfo);
                    m_CurTaskInfo.RecordDiscipleID(m_SelectedDiscipleDic);
                }
            });
            m_AcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_SelectedList = Transformation(m_SelectedDiscipleDic);
                switch (m_PanelType)
                {
                    case PanelType.Task:
                        if (m_SelectedList.Count != m_CommonTaskItemInfo.GetCharacterAmount())
                        {
                            FloatMessage.S.ShowMsg("��ѡ�������� !");
                            return;
                        }
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
                            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_PanelType, m_CurTaskInfo, enemiesList);
                        }
                        for (int i = 0; i < m_PlayerDataHerb.Count; i++)
                        {
                            MainGameMgr.S.InventoryMgr.RemoveItem(new HerbItem(m_PlayerDataHerb[i]));
                        }
                        UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);
                        UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
                        break;
                    case PanelType.Challenge:
                        if (m_SelectedList.Count != MaxDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("��ѡ�������� !");
                            return;
                        }
                        for (int i = 0; i < m_PlayerDataHerb.Count; i++)
                        {
                            MainGameMgr.S.InventoryMgr.RemoveItem(new HerbItem(m_PlayerDataHerb[i]));
                        }

                        DataAnalysisMgr.S.CustomEvent(DotDefine.level_enter, m_LevelConfigInfo.chapterId.ToString() + ";" + m_LevelConfigInfo.level.ToString());

                        EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_PanelType, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        UIMgr.S.ClosePanelAsUIID(UIID.ChallengePanel);
                        UIMgr.S.ClosePanelAsUIID(UIID.ChallengeBattlePanel);
                        UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
                        break;
                    default:
                        break;
                }


                CloseSelfPanel();
            });
        }

        private void AutoSelectedDisciple()
        {
            //TODO  ���յ���ս���Ӹߵ�������
            m_SelectedDiscipleDic.Clear();

            switch (m_PanelType)
            {
                case PanelType.Task:
                    CommonUIMethod.BubbleSortForType(m_AllCharacterList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
                    List<CharacterItem> alCharacterList = new List<CharacterItem>();
                    foreach (var item in m_AllCharacterList)
                        if (item.level >= m_CommonTaskItemInfo.characterLevelRequired)
                            alCharacterList.Add(item);
                    if (alCharacterList.Count >= m_CommonTaskItemInfo.GetCharacterAmount())
                    {
                        for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                            m_SelectedDiscipleDic.Add(alCharacterList[i].id, alCharacterList[i]);
                    }
                    else
                    {
                        for (int i = 0; i < alCharacterList.Count; i++)
                            m_SelectedDiscipleDic.Add(alCharacterList[i].id, alCharacterList[i]);
                    }
                    break;
                case PanelType.Challenge:
                    CommonUIMethod.BubbleSortForType(m_AllCharacterList, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
                    if (m_AllCharacterList.Count >= MaxDiscipleNumber)
                    {
                        for (int i = 0; i < MaxDiscipleNumber; i++)
                            m_SelectedDiscipleDic.Add(m_AllCharacterList[i].id, m_AllCharacterList[i]);
                    }
                    else
                    {
                        for (int i = 0; i < m_AllCharacterList.Count; i++)
                            m_SelectedDiscipleDic.Add(m_AllCharacterList[i].id, m_AllCharacterList[i]);
                    }
                    break;
                default:
                    break;
            }

            //MainGameMgr.S.CharacterMgr.GetAllCharacterList
            //m_SelectedDiscipleDic
            HandConfirmBtnEvent();
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

            CloseSelfPanel();
        }

        private void CreateHerb(int herbID)
        {
            if (m_HerbalMedicineItem == null)
                return;
            HerbalMedicineItem herbItem = Instantiate(m_HerbalMedicineItem, m_HerbalMedicineItemTra).GetComponent<HerbalMedicineItem>();
            herbItem.OnInit(herbID, this);
            m_PlayerDataHerbDic.Add(herbID, herbItem);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="parent">������</param>
        /// <param name="characterItem">������Ϣ</param>
        /// <param name="action">��ť�����ص�</param>
        /// <returns></returns>
        private void CreateDisciple(Transform parent)
        {
            SendSelectedDisciple discipeItem = Instantiate(m_DiscipleItem, parent).GetComponent<SendSelectedDisciple>();
            discipeItem.OnInit(m_PanelType, this);
            m_ChallengeDiscipleList.Add(discipeItem);
        }

        /// <summary>
        /// ��ҩ��ť�����ص�
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
                //ȡ��
                if (m_PlayerDataHerb.Contains(herbItem.HerbID))
                    m_PlayerDataHerb.Remove(herbItem.HerbID);
            }
            RefreshDisicipleSkill();
        }
    }
}