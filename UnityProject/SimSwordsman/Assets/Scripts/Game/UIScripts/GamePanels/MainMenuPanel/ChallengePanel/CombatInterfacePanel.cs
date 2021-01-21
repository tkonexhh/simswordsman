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
        private Slider m_LeftBloodStick;
        [SerializeField]
        private Slider m_RightBloodStick;
        [SerializeField]
        private GameObject m_MatchRecordItem;

        private List<BattleTextConfig> m_BattleText = null;
        private List<BattleTextConfig> m_TalkText = null;
        private List<BattleTextConfig> m_EndText = null;
        private List<CharacterController> m_OurCharacterList = null;
        private List<EnemyConfig> m_EnemyCharacterList = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private LogPanel m_logPanel = null;
        private Coroutine m_Coroutine;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
            EventSystem.S.Register(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleFailed, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnCharacterUpgrade, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnKongfuLibraryUpgrade, HandleAddListenerEvent);
            //m_ScrollRect.

        }

        private void StartBattleText()
        {
            //string.Format(m_BattleText[battleTextIndex].BattleWorlds, m_OurCharacterList[characterListIndex], m_EnemyCharacterList[enemyCharacterListIndex]);
            CreateBattleText(m_BattleText);

            int randomSecond = UnityEngine.Random.Range(1, 3);
            m_Coroutine =  StartCoroutine(BattleTextCounDown(randomSecond));
      
        }

        public IEnumerator BattleTextCounDown(int second)
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
        /// 创建文本
        /// </summary>
        /// <param name="battleTexts"></param>
        private void CreateBattleText(List<BattleTextConfig> battleTexts,int type = 0)
        {
            int characterListIndex = UnityEngine.Random.Range(0, m_OurCharacterList.Count);
            int enemyCharacterListIndex = UnityEngine.Random.Range(0, m_EnemyCharacterList.Count);
            int index = UnityEngine.Random.Range(0, battleTexts.Count);
            if (type==0)
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
                if (type == 1)//我方胜利
                {
                    string battleText = ReplaceStr(battleTexts[index].BattleWorlds, 0, GameDataMgr.S.GetClanData().GetClanName());
                    battleText = ReplaceStr(battleText, 1, CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType));
                    CreateBattleText(battleText);
                }
                else//敌人胜利
                {
                    string battleText = ReplaceStr(battleTexts[index].BattleWorlds, 0, CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType));
                    battleText = ReplaceStr(battleText, 1, GameDataMgr.S.GetClanData().GetClanName());
                    CreateBattleText(battleText);
                }
            }
        }

        private string ReplaceStr(string str,int i,string newStr)
        {
            return str.Replace("{"+i+"}", newStr);
        }

        private string GetEnemyName(EnemyConfig enemyConfig)
        {
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
            m_EnemyCharacterList = m_LevelConfigInfo.enemiesList;
        }


        private void CreateBattleText(string cont)
        {
            //StartCoroutine(OnUpdateScroll());
            //m_ScrollRect.normalizedPosition = new Vector2(0, 0);
            //记录
            m_ScrollRect.DoScrollVertical(0, 0.6f);
            
            Transform matchRecordItem = Instantiate(m_MatchRecordItem, m_MatchRecordTra).transform;
            matchRecordItem.GetComponent<Text>().text = cont;
        }
        IEnumerator OnUpdateScroll()
        {
            yield return new WaitForEndOfFrame();
            m_ScrollRect.normalizedPosition = new Vector2(0, 0);

            int randomSecond = UnityEngine.Random.Range(1, 3);
            m_Coroutine = StartCoroutine(BattleTextCounDown(randomSecond));
        }

        public IEnumerator BattleCountdown(int second)
        {
            while (second >= 0)
            {
                if (second <= 5)
                {
                   //TODO
                }

                m_CombatTime.text = SplicingTime(second);
                yield return new WaitForSeconds(1);
                second--;
                if (second==0)
                  EventSystem.S.Send(EventID.OnBattleFailed);
            }
        }
        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }

        private void RefreshCurPanelInfo()
        {
            m_LeftSchoolNameValue.text = GameDataMgr.S.GetClanData().GetClanName();
            m_RightSchoolNameValue.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
            m_MatchNameValue.text = m_LevelConfigInfo.battleName;
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.LogPanel, LogPanelCallback,
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_TITLE),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_CONTENT),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_ACCEPTBTNTXT),
                    CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_LOG_REFUSEBTNTXT));
            });
        }

        private void LogPanelCallback(AbstractPanel obj)
        {
            m_logPanel = obj as LogPanel;
            m_logPanel.OnSuccessBtnEvent += SuccessBtn;
            m_logPanel.OnRefuseBtnEvent += RefuseBtn;
        }

        private void RefuseBtn()
        {
        }

        private void SuccessBtn()
        {
            EventSystem.S.Send(EventID.OnExitBattle);
            HideSelfWithAnim();
            UIMgr.S.OpenPanel(UIID.MainMenuPanel);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurChapterConfigInfo = (ChapterConfigInfo)args[0];
            m_LevelConfigInfo = (LevelConfigInfo)args[1];
            GetInformationForNeed();
            RefreshCurPanelInfo();
            StartCoroutine(BattleCountdown(30));

            StartBattleText();
        }

        protected override void OnClose()
        {
            base.OnClose();
            EventSystem.S.UnRegister(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleFailed, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnCharacterUpgrade, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnKongfuLibraryUpgrade, HandleAddListenerEvent);

            if (m_logPanel!=null)
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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EventSystem.S.Send(EventID.OnBattleSuccessed);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                EventSystem.S.Send(EventID.OnBattleFailed);
            }
        }
        

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnRefreshBattleProgress:
                    m_LeftBloodStick.value = (float)param[0];
                    m_RightBloodStick.value = (float)param[1];
                    break;
                case EventID.OnBattleSuccessed:
                    MainGameMgr.S.ChapterMgr.PassCheckpoint(m_CurChapterConfigInfo.chapterId, m_LevelConfigInfo.level);
                    CreateBattleOverText(true);
                    UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_LevelConfigInfo, true);
                    break;
                case EventID.OnBattleFailed:
                    CreateBattleOverText(false);
                    UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_LevelConfigInfo, false);
                    break;
                case EventID.OnCharacterUpgrade:
                    PanelPool.S.AddPromotion(new DiscipleRiseStage((EventID)key, (int)param[0], (int)param[1]));
                    break;
                case EventID.OnKongfuLibraryUpgrade:
                    PanelPool.S.AddPromotion(new WugongBreakthrough((EventID)key, (int)param[0], (CharacterKongfuDBData)param[1]));
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
    }
}