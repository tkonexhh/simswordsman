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
        /// <summary>
        /// 镖局
        /// </summary>
        Deliver,
        /// <summary>
        /// 伏魔塔
        /// </summary>
        Tower,
        /// <summary>
        /// 英雄试炼
        /// </summary>
        HeroTrial,
    }

    public class SendDisciplesPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField] private Button BlackBtn;
        [SerializeField] private Button m_ClsseBtn;
        [SerializeField] private Text m_RecommendedSkillsValue;
        [SerializeField] private Text m_SelectedDiscipleSkillValue;
        [SerializeField] private Image m_StateBg;
        [SerializeField] private Text m_State;
        [Header("Middle")]
        [SerializeField] private Transform m_UnselectedTrans;
        [SerializeField] private GameObject m_DiscipleItem;
        [SerializeField] private GameObject m_DiscipleItemTower;

        [Header("MiddleDown")]
        [SerializeField] private Transform m_HerbalMedicineItemTra;
        [SerializeField] private GameObject m_HerbalMedicineItem;

        [Header("Down")]
        [SerializeField] private Button m_AutoSelectedBtn;
        [SerializeField] private Button m_AcceptBtn;
        [SerializeField] private Button m_RefuseBtn;

        private PanelType m_PanelType;


        private const int MaxDiscipleNumber = 5;

        private List<CharacterItem> m_AllCharacterList = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private TowerPanelChallengeToSelect m_TowerLevelConfig;

        private Dictionary<int, HerbalMedicineItem> m_PlayerDataHerbDic = new Dictionary<int, HerbalMedicineItem>();

        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private List<SendSelectedDisciple> m_ChallengeDiscipleList = new List<SendSelectedDisciple>();

        private List<CharacterController> m_SelectedList = new List<CharacterController>();
        private List<HerbType> m_PlayerDataHerb = new List<HerbType>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            AudioMgr.S.PlaySound(Define.INTERFACE);
            RegisterEvent(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            RegisterEvent(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
            RegisterEvent(EventID.OnSendHerbEvent, HandAddListenerEvent);

            GetInformationForNeed();

            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = args[1] as ChapterConfigInfo;
                    m_LevelConfigInfo = args[2] as LevelConfigInfo;
                    RefeshATK();
                    break;
                case PanelType.Tower:
                    m_TowerLevelConfig = (TowerPanelChallengeToSelect)args[1];
                    RefeshATK();
                    break;
                default:
                    break;
            }
            InitPanelInfo();

            AutoFillDisciples();
        }

        private void AutoFillDisciples()
        {
            if (m_PanelType != PanelType.Challenge)
                return;
            m_SelectedDiscipleDic.Clear();

            CommonUIMethod.BubbleSortForType(m_AllCharacterList, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
            int minCount = Mathf.Min(m_AllCharacterList.Count, MaxDiscipleNumber);
            for (int i = 0; i < m_AllCharacterList.Count; i++)
            {
                if (m_SelectedDiscipleDic.Count < minCount)
                {
                    m_SelectedDiscipleDic.Add(m_AllCharacterList[i].id, m_AllCharacterList[i]);
                    continue;
                }
                break;
            }
            HandConfirmBtnEvent();
            RefeshATK();
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

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedConfirmEvent:
                    m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)param[0];
                    HandConfirmBtnEvent();
                    RefeshATK();
                    break;
                case EventID.OnSendDiscipleDicEvent:
                    var type = (PanelType)param[0];
                    if (type == PanelType.Tower)
                    {
                        UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, type, m_TowerLevelConfig);
                    }
                    else if (type == PanelType.Challenge)
                    {
                        UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, type, m_LevelConfigInfo);
                    }

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

        public void AddDiscipleDicDic(Dictionary<int, CharacterItem> keyValuePairs)
        {
            foreach (var item in keyValuePairs.Values)
                m_SelectedDiscipleDic.Add(item.id, item);
            //RefreshPanelInfo();
            HandConfirmBtnEvent();
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

        private void RefeshATK()
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
            long recommended = 0;
            if (m_PanelType == PanelType.Challenge)
            {
                recommended = m_LevelConfigInfo.recommendAtkValue;
            }
            else if (m_PanelType == PanelType.Tower)
            {
                recommended = m_TowerLevelConfig.recommendATK;
            }
            m_RecommendedSkillsValue.text = CommonUIMethod.GetTenThousandOrMillion(recommended);

            try
            {
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
            catch (DivideByZeroException)
            {
                m_State.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_RELAXED);
                m_StateBg.sprite = SpriteHandler.S.GetSprite(AtlasDefine.SendDisciplePanelAtlas, "SendDisciplePanel_Rleaxed");
            }
        }

        private void GetInformationForNeed()
        {
            m_AllCharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void InitPanelInfo()
        {
            switch (m_PanelType)
            {
                case PanelType.Challenge:
                    if (m_AllCharacterList != null)
                        for (int i = 0; i < MaxDiscipleNumber; i++)
                            CreateDisciple(m_UnselectedTrans);
                    break;
                case PanelType.Tower:
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
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                AutoSelectedDisciple();
                RefeshATK();
            });
            m_AcceptBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                m_SelectedList = Transformation(m_SelectedDiscipleDic);
                switch (m_PanelType)
                {
                    case PanelType.Challenge:
                        if (m_SelectedList.Count != MaxDiscipleNumber)
                        {
                            FloatMessage.S.ShowMsg("请选择满弟子 !");
                            return;
                        }
                        UseHerb();

                        DataAnalysisMgr.S.CustomEvent(DotDefine.level_enter, m_LevelConfigInfo.chapterId.ToString() + ";" + m_LevelConfigInfo.level.ToString());

                        EventSystem.S.Send(EventID.OnEnterBattle, m_LevelConfigInfo.enemiesList, m_SelectedList, m_PlayerDataHerb);
                        UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, m_PanelType, m_CurChapterConfigInfo, m_LevelConfigInfo);
                        UIMgr.S.ClosePanelAsUIID(UIID.ChallengePanel);
                        UIMgr.S.ClosePanelAsUIID(UIID.ChallengeBattlePanel);
                        UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
                        break;
                    case PanelType.Tower:
                        if (m_SelectedList.Count <= 0)
                        {
                            FloatMessage.S.ShowMsg("请先选择弟子！");
                            return;
                        }
                        UseHerb();
                        MainGameMgr.S.TowerSystem.StartLevel(m_SelectedList, m_TowerLevelConfig.basicATK);
                        break;
                    default:
                        break;
                }


                CloseSelfPanel();
            });
        }

        private void UseHerb()//嗑药
        {
            for (int i = 0; i < m_PlayerDataHerb.Count; i++)
            {
                MainGameMgr.S.InventoryMgr.RemoveItem(new HerbItem(m_PlayerDataHerb[i]));
            }
        }

        private void AutoSelectedDisciple()
        {
            m_SelectedDiscipleDic.Clear();

            switch (m_PanelType)
            {
                case PanelType.Challenge:
                    {
                        CommonUIMethod.BubbleSortForType(m_AllCharacterList, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
                        int minCount = Mathf.Min(m_AllCharacterList.Count, MaxDiscipleNumber);
                        for (int i = 0; i < m_AllCharacterList.Count; i++)
                        {
                            if (m_SelectedDiscipleDic.Count < minCount)
                            {
                                m_SelectedDiscipleDic.Add(m_AllCharacterList[i].id, m_AllCharacterList[i]);
                                continue;
                            }
                            break;
                        }
                    }
                    break;
                case PanelType.Tower:
                    {
                        CommonUIMethod.BubbleSortForType(m_AllCharacterList, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
                        int minCount = Mathf.Min(m_AllCharacterList.Count, MaxDiscipleNumber);

                        for (int i = 0; i < m_AllCharacterList.Count; i++)
                        {

                            if (m_AllCharacterList[i].level < TowerDefine.CHARACT_MINLEVEL)
                            {
                                // Debug.LogError(m_AllCharacterList[i].id);
                                continue;
                            }

                            int id = m_AllCharacterList[i].id;
                            var towerCharacterDB = GameDataMgr.S.GetPlayerData().towerData.GetTowerCharacterByID(id);

                            //判断是否超出数量
                            if (towerCharacterDB == null && !MainGameMgr.S.TowerSystem.CanAddNewCharacter())
                            {
                                continue;
                            }

                            //判断死没死
                            if (towerCharacterDB != null && towerCharacterDB.IsDead())
                            {
                                continue;
                            }

                            if (m_SelectedDiscipleDic.Count < minCount)
                            {
                                m_SelectedDiscipleDic.Add(id, m_AllCharacterList[i]);
                                continue;
                            }

                            break;
                        }

                        if (m_SelectedDiscipleDic.Count == 0)
                        {
                            FloatMessage.S.ShowMsg("当前无可用弟子");
                        }
                    }
                    break;
                default:
                    break;
            }

            HandConfirmBtnEvent();
        }

        private List<CharacterController> Transformation(Dictionary<int, CharacterItem> m_SelectedDiscipleDic)
        {
            List<CharacterController> characterController = new List<CharacterController>();
            foreach (var item in m_SelectedDiscipleDic.Values)
                characterController.Add(MainGameMgr.S.CharacterMgr.GetCharacterController(item.id));
            return characterController;
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
            GameObject prefab = m_DiscipleItem;
            if (m_PanelType == PanelType.Tower)
            {
                prefab = m_DiscipleItemTower;
            }
            SendSelectedDisciple discipeItem = Instantiate(prefab, parent).GetComponent<SendSelectedDisciple>();
            discipeItem.Init(m_PanelType, this);
            m_ChallengeDiscipleList.Add(discipeItem);
        }

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
            RefeshATK();
        }
    }
}