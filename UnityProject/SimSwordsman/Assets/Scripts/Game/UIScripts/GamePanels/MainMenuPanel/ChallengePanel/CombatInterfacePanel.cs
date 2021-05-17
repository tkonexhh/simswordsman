using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Application;

namespace GameWish.Game
{
    public class CombatInterfacePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_LeftSchoolNameValue;
        [SerializeField]
        private Text m_RightSchoolNameValue;
        [SerializeField]
        private Text m_MatchNameValue;
        [SerializeField]
        private Text m_CombatTime;
        [SerializeField]
        private ScrollRect m_ScrollRect;
        [SerializeField]
        private Transform m_MatchRecordTra;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Image m_LeftBloodStickImg;
        [SerializeField]
        private Image m_RightBloodStickImg;
        [SerializeField]
        private GameObject m_MatchRecordItem;
        [SerializeField]
        private Button m_Speed1Btn;
        [SerializeField]
        private Button m_Speed2Btn;
        [SerializeField]
        private Transform m_Top;
        [SerializeField]
        private Transform m_Bottom;
        //[SerializeField]
        //private Button m_Speed4Btn;
        private int m_CurTimeScale = 1;

        private List<BattleTextConfig> m_BattleText = null;
        private List<BattleTextConfig> m_TalkText = null;
        private List<BattleTextConfig> m_EndText = null;
        private List<CharacterController> m_OurCharacterList = null;
        private List<EnemyConfig> m_EnemyCharacterList = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private LogPanel m_logPanel = null;
        private Coroutine m_Coroutine;
        private PanelType m_PanelType;
        private SimGameTask m_CurTaskInfo = null;

        //TODO 重构 目前一种类型添加将来需要很多无关变量

        //Tower
        private TowerLevelConfig m_TowerLevelConfig = null;
        private ArenaLevelConfig m_ArenaLevelConfig = null;

        private static bool isSpeedUp = false;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
            EventSystem.S.Register(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleFailed, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnCharacterUpgrade, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnKongfuLibraryUpgrade, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleSecondEvent, HandleAddListenerEvent);
            //m_ScrollRect.
        }

        protected override void OnOpen()
        {
            base.OnOpen();

            if (isSpeedUp)
            {
                SetTimeScale(2);
            }
            else
            {
                SetTimeScale(1);
            }
        }

        private void StartBattleText()
        {
            //string.Format(m_BattleText[battleTextIndex].BattleWorlds, m_OurCharacterList[characterListIndex], m_EnemyCharacterList[enemyCharacterListIndex]);
            CreateBattleText(m_BattleText);

            int randomSecond = UnityEngine.Random.Range(1, 3);
            m_Coroutine = StartCoroutine(BattleTextCounDown(randomSecond));

        }

        private IEnumerator BattleTextCounDown(int second)
        {
            while (second >= 0)
            {
                yield return new WaitForSeconds(1);
                second--;
            }

            int random = UnityEngine.Random.Range(0, 10);
            if (random <= 6)
                CreateBattleText(m_BattleText);
            else
                CreateBattleText(m_TalkText);
        }

        /// <summary>
        /// �����ı�
        /// </summary>
        /// <param name="battleTexts"></param>
        private void CreateBattleText(List<BattleTextConfig> battleTexts, int type = 0)
        {
            try
            {
                int characterListIndex = UnityEngine.Random.Range(0, m_OurCharacterList.Count);
                int enemyCharacterListIndex = UnityEngine.Random.Range(0, m_EnemyCharacterList.Count);
                int index = UnityEngine.Random.Range(0, battleTexts.Count);

                if (index >= battleTexts.Count)
                {
                    Debug.LogError("---index = "+ index+ "----battleTexts.Count"+ battleTexts.Count);
                }
                else  
                {
                    if (type == 0)
                    {
                        int random = UnityEngine.Random.Range(0, 2);
                        if (random == 0)
                            CreateBattleText(string.Format(battleTexts[index].BattleWorlds, GetDiscipleName(m_OurCharacterList[characterListIndex]), GetEnemyName(m_EnemyCharacterList[enemyCharacterListIndex])));
                        else
                            CreateBattleText(string.Format(battleTexts[index].BattleWorlds, GetEnemyName(m_EnemyCharacterList[enemyCharacterListIndex]), GetDiscipleName(m_OurCharacterList[characterListIndex])));

                        int randomSecond = UnityEngine.Random.Range(1, 3);
                        m_Coroutine = StartCoroutine(BattleTextCounDown(randomSecond));
                    }
                    else
                    {
                        if (type == 1)//�ҷ�ʤ��
                        {
                            string battleText = ReplaceStr(battleTexts[index].BattleWorlds, 0, GameDataMgr.S.GetClanData().GetClanName());
                            switch (m_PanelType)
                            {
                                case PanelType.Task:
                                    battleText = ReplaceStr(battleText, 1, m_CurTaskInfo.CommonTaskItemInfo.title);
                                    break;
                                case PanelType.Challenge:
                                    battleText = ReplaceStr(battleText, 1, CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType));
                                    break;
                                case PanelType.Tower:
                                    break;
                                default:
                                    break;
                            }
                            CreateBattleText(battleText);
                        }
                        else//����ʤ��
                        {
                            string battleText = string.Empty;
                            switch (m_PanelType)
                            {
                                case PanelType.Task:
                                    battleText = ReplaceStr(battleTexts[index].BattleWorlds, 0, m_CurTaskInfo.CommonTaskItemInfo.title);
                                    battleText = ReplaceStr(battleText, 1, GameDataMgr.S.GetClanData().GetClanName());
                                    CreateBattleText(battleText);
                                    break;
                                case PanelType.Challenge:
                                    battleText = ReplaceStr(battleTexts[index].BattleWorlds, 0, CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType));
                                    battleText = ReplaceStr(battleText, 1, GameDataMgr.S.GetClanData().GetClanName());
                                    CreateBattleText(battleText);
                                    break;
                                case PanelType.Tower:

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("e:" + e);
            }


        }

        private string ReplaceStr(string str, int i, string newStr)
        {
            return str.Replace("{" + i + "}", newStr);
        }

        private string GetEnemyName(EnemyConfig enemyConfig)
        {
            if (enemyConfig is CharacterEnemyConfig)
                return "敌人";
            else
                return MainGameMgr.S.BattleFieldMgr.GetEnemyInfo(enemyConfig.ConfigId).GetNameForRandom();
        }

        private string GetDiscipleName(CharacterController characterController)
        {
            return MainGameMgr.S.CharacterMgr.GetCharacterItem(characterController.CharacterId).name;
        }

        private void GetInformationForNeed()
        {
            m_BattleText = TDBattleWordsTable.GetBattleTextForType(BattleText.Battle);
            m_TalkText = TDBattleWordsTable.GetBattleTextForType(BattleText.Talk);
            m_EndText = TDBattleWordsTable.GetBattleTextForType(BattleText.End);
            m_OurCharacterList = MainGameMgr.S.BattleFieldMgr.OurCharacterList;
        }


        private void CreateBattleText(string cont)
        {
            //m_ScrollRect.normalizedPosition = new Vector2(0, 0);
            //��¼
            m_ScrollRect.DoScrollVertical(0, 0.6f);

            Transform matchRecordItem = Instantiate(m_MatchRecordItem, m_MatchRecordTra).transform;
            matchRecordItem.GetComponent<Text>().text = cont;
        }



        private void RefreshCurPanelInfo()
        {
            m_LeftSchoolNameValue.text = GameDataMgr.S.GetClanData().GetClanName();
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_RightSchoolNameValue.text = m_CurTaskInfo.CommonTaskItemInfo.enemyClan;
                    m_MatchNameValue.text = m_CurTaskInfo.CommonTaskItemInfo.title;
                    break;
                case PanelType.Challenge:
                    m_RightSchoolNameValue.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
                    m_MatchNameValue.text = m_LevelConfigInfo.battleName;
                    MainGameMgr.S.BattleFieldMgr.BattleField.ChangeBgSpriteRender(m_CurChapterConfigInfo.clanType);
                    break;
                case PanelType.Tower:
                    m_RightSchoolNameValue.text = "伏魔塔";
                    m_MatchNameValue.text = "伏魔塔" + m_TowerLevelConfig.level;
                    MainGameMgr.S.BattleFieldMgr.BattleField.ChangeBgSpriteRenderToTower();
                    break;
                case PanelType.Arena:
                    m_RightSchoolNameValue.text = "竞技场";
                    m_MatchNameValue.text = "竞技场" + m_ArenaLevelConfig.level;
                    break;
                default:
                    break;
            }

        }
        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.LogPanel, LogPanelCallback,
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_TITLE),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_CONTENT),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_ACCEPTBTNTXT),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_REFUSEBTNTXT));

                Time.timeScale = 0;
            });

            m_Speed1Btn.onClick.AddListener(() =>
            {
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
                if (lobbyLevel < 3)
                {
                    FloatMessage.S.ShowMsg("讲武堂3级后，可加速战斗!");
                    return;
                }

                SetTimeScale(2, true);
            });

            m_Speed2Btn.onClick.AddListener(() =>
            {
                SetTimeScale(1, true);
            });

            //m_Speed4Btn.onClick.AddListener(() =>
            //{
            //    Time.timeScale = 4;
            //});
        }
        private void LogPanelCallback(AbstractPanel obj)
        {

            m_logPanel = obj as LogPanel;
            m_logPanel.OnSuccessBtnEvent += SuccessBtn;
            m_logPanel.OnRefuseBtnEvent += RefuseBtn;
        }
        private void RefuseBtn()
        {
            Time.timeScale = 1;
        }
        private void SuccessBtn()
        {
            Time.timeScale = 1;
            MainGameMgr.S.BattleFieldMgr.StopBattleCoroutine();
            EventSystem.S.Send(EventID.OnExitBattle);
            HideSelfWithAnim();
            switch (m_PanelType)
            {
                case PanelType.Task:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                    break;
                case PanelType.Challenge:
                    DataAnalysisMgr.S.CustomEvent(DotDefine.level_quit, m_LevelConfigInfo.chapterId.ToString() + ";" + m_LevelConfigInfo.level.ToString());
                    OpenParentChallenge();
                    break;
                case PanelType.Tower:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                    UIMgr.S.OpenPanel(UIID.TowerPanel);
                    break;
                case PanelType.Arena:
                    UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                    UIMgr.S.OpenPanel(UIID.ArenaPanel);
                    break;
                default:
                    break;
            }
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_PanelType = (PanelType)args[0];
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_CurTaskInfo = (SimGameTask)args[1];
                    m_EnemyCharacterList = (List<EnemyConfig>)args[2];
                    break;
                case PanelType.Challenge:
                    m_CurChapterConfigInfo = (ChapterConfigInfo)args[1];
                    m_LevelConfigInfo = (LevelConfigInfo)args[2];
                    m_EnemyCharacterList = m_LevelConfigInfo.enemiesList;
                    break;
                case PanelType.Tower:
                    m_TowerLevelConfig = (TowerLevelConfig)args[1];
                    m_EnemyCharacterList = m_TowerLevelConfig.enemiesList;
                    break;
                case PanelType.Arena:
                    m_ArenaLevelConfig = (ArenaLevelConfig)args[1];
                    m_EnemyCharacterList = m_ArenaLevelConfig.enemyConfigs;
                    break;
                default:
                    break;
            }

            GetInformationForNeed();
            RefreshCurPanelInfo();
            //注意
            //Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector2(39.99f, 4.0351f));
            //Debug.LogError("##21" + screenPoint);

            //// 再将屏幕坐标转换成UGUI坐标
            //Vector2 localPoint;
            //Canvas canvas = GameObject.FindGameObjectWithTag("Target").GetComponent<Canvas>() ;
            //Debug.LogError("##21" + canvas.name);

            //if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, screenPoint, Camera.main, out localPoint))
            //{
            //    Debug.LogError("##1" + localPoint);

            //}
            //Vector2 vector2;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent.transform, new Vector2(39.99f, 4.0351f),Camera.main,out vector2);

            //Debug.LogError("##1"+ vector2);
            //#if UNITY_IOS

            //#endif
            m_Top.localPosition = new Vector3(0, 333);
            m_Bottom.localPosition = new Vector3(0, -277);

#if UNITY_IOS
            m_Bottom.localPosition = new Vector3(0, -250);
#endif 
            StartBattleText();

        }

        protected override void OnClose()
        {
            base.OnClose();

            Time.timeScale = 1;

            EventSystem.S.UnRegister(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleFailed, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnCharacterUpgrade, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnKongfuLibraryUpgrade, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleSecondEvent, HandleAddListenerEvent);

            if (m_logPanel != null)
            {
                m_logPanel.OnSuccessBtnEvent -= SuccessBtn;
                m_logPanel.OnRefuseBtnEvent -= RefuseBtn;
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        /// <summary>
        /// ��֮ǰѡ��Ľ���
        /// </summary>
        private void OpenParentChallenge()
        {
            UIMgr.S.OpenPanel(UIID.ChallengePanel, m_CurChapterConfigInfo.clanType);
        }
        private void ChallengePanelCallback(AbstractPanel obj)
        {
        }

        private void ReduceHPWithAni(float endValue, Image hpSlider, float duration = .5f)
        {
            DG.Tweening.DOTween.To(() => hpSlider.fillAmount, (x) =>
            {
                hpSlider.fillAmount = x;
            }, endValue, duration);
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshBattleProgress:
                    //m_LeftBloodStick.value = (float)param[0];
                    //m_RightBloodStick.value = (float)param[1];

                    float leftBloodEndValue = (float)param[0];
                    ReduceHPWithAni(leftBloodEndValue, m_LeftBloodStickImg);

                    float rightBloodEndValue = (float)param[1];
                    ReduceHPWithAni(rightBloodEndValue, m_RightBloodStickImg);
                    break;
                case EventID.OnBattleSuccessed:
                    SetTimeScale(1);
                    switch (m_PanelType)
                    {
                        case PanelType.Task:
                            MainGameMgr.S.CommonTaskMgr.SetTaskFinished(m_CurTaskInfo.TaskId, TaskState.Finished);
                            MainGameMgr.S.CommonTaskMgr.ClaimReward(m_CurTaskInfo.TaskId);
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_CurTaskInfo, true);
                            break;
                        case PanelType.Challenge:
                            DataAnalysisMgr.S.CustomEvent(DotDefine.level_end_win, m_LevelConfigInfo.chapterId.ToString() + ";" + m_LevelConfigInfo.level.ToString());
                            MainGameMgr.S.ChapterMgr.PassCheckpoint(m_CurChapterConfigInfo.chapterId, m_LevelConfigInfo.level);
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_CurChapterConfigInfo, m_LevelConfigInfo, true);
                            EventSystem.S.Send(EventID.OnMainMenuChallenging);
                            break;
                        case PanelType.Tower:
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_TowerLevelConfig, true);
                            break;
                        case PanelType.Arena:
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_ArenaLevelConfig, true);
                            break;
                        default:
                            break;
                    }
                    CreateBattleOverText(true);
                    break;
                case EventID.OnBattleFailed:
                    SetTimeScale(1);
                    switch (m_PanelType)
                    {
                        case PanelType.Task:
                            MainGameMgr.S.CommonTaskMgr.RemoveTask(m_CurTaskInfo.TaskId);
                            //MainGameMgr.S.CommonTaskMgr.SetTaskFinished(m_CurTaskInfo.TaskId, TaskState.NotStart);
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_CurTaskInfo, false);
                            break;
                        case PanelType.Challenge:
                            DataAnalysisMgr.S.CustomEvent(DotDefine.level_end_fail, m_LevelConfigInfo.chapterId.ToString() + ";" + m_LevelConfigInfo.level.ToString());
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_CurChapterConfigInfo, m_LevelConfigInfo, false);
                            EventSystem.S.Send(EventID.OnMainMenuChallenging);
                            break;
                        case PanelType.Tower:
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_TowerLevelConfig, false);
                            break;
                        case PanelType.Arena:
                            UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_PanelType, m_ArenaLevelConfig, false);
                            break;
                        default:
                            break;
                    }
                    CreateBattleOverText(false);
                    break;
                case EventID.OnCharacterUpgrade:
                    PanelPool.S.AddPromotion(new DiscipleRiseStage((int)param[0], (int)param[1], (float)param[2]));
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    PanelPool.S.AddPromotion(new WugongBreakthrough((int)param[0], (CharacterKongfuDBData)param[1], (float)param[2]));
                    break;
                case EventID.OnBattleSecondEvent:
                    m_CombatTime.text = (string)param[0];
                    break;
                default:
                    break;
            }
        }

        private void CreateBattleOverText(bool result)
        {
            StopAllCoroutines();
            if (result)
                CreateBattleText(m_EndText, 1);
            else
                CreateBattleText(m_EndText, 2);
        }

        private void SetTimeScale(int timeScale, bool setSpeedUp = false)
        {
            m_CurTimeScale = timeScale;
            Time.timeScale = timeScale;

            if (timeScale == 1)
            {
                m_Speed1Btn.gameObject.SetActive(true);
                m_Speed2Btn.gameObject.SetActive(false);

                if (setSpeedUp)
                    isSpeedUp = false;
            }

            if (timeScale == 2)
            {
                m_Speed1Btn.gameObject.SetActive(false);
                m_Speed2Btn.gameObject.SetActive(true);

                if (setSpeedUp)
                    isSpeedUp = true;
            }
        }
    }
}