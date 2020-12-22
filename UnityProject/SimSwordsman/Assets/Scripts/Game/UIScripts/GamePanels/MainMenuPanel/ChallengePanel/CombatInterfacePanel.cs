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
        private Transform m_MatchRecordTra;
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Slider m_LeftBloodStick;
        [SerializeField]
        private Slider m_RightBloodStick;
        [SerializeField]
        private GameObject m_MatchRecordItem;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private LevelConfigInfo m_LevelConfigInfo = null;
        private LogPanel m_logPanel = null;
        private LogPanel m_successPanel = null;
        private LogPanel m_failPanel = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnBattleFailed, HandleAddListenerEvent);

            for (int i = 0; i < 5; i++)
            {
                Transform matchRecordItem = Instantiate(m_MatchRecordItem, m_MatchRecordTra).transform;
                matchRecordItem.GetComponent<Text>().text = "测试内容";
            }

            BindAddListenerEvent();
        }

        public IEnumerator BattleCountdown(int second)
        {
            while (second >= 0)
            {
                if (second<=5)
                {
                    //m_CombatTime.color = Color.red;
                    //m_CombatTime.Alpha
                }

                m_CombatTime.text = SplicingTime(second);
                yield return new WaitForSeconds(1);
                second--;
            }
        }
        /// <summary>
        /// 拼接时间
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public string SplicingTime(int second)
        {
            if (second.ToString().Length > 1)
                return "00:" + second;
            else
                return "00:0" + second;
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
                    CommonUIMethod.GetStringForTableKey(Define.Challenge_LOG_TITLE),
                    CommonUIMethod.GetStringForTableKey(Define.Challenge_LOG_CONTENT),
                    CommonUIMethod.GetStringForTableKey(Define.Challenge_LOG_ACCEPTBTNTXT),
                    CommonUIMethod.GetStringForTableKey(Define.Challenge_LOG_REFUSEBTNTXT));
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
            CloseSelfPanel();
            UIMgr.S.OpenPanel(UIID.MainMenuPanel);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurChapterConfigInfo = (ChapterConfigInfo)args[0];
            m_LevelConfigInfo = (LevelConfigInfo)args[1];
            RefreshCurPanelInfo();
            StartCoroutine(BattleCountdown(7));
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnRefreshBattleProgress, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleSuccessed, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBattleFailed, HandleAddListenerEvent);


            m_logPanel.OnSuccessBtnEvent -= SuccessBtn;
            m_logPanel.OnRefuseBtnEvent -= RefuseBtn;
            m_successPanel.OnSuccessBtnEvent -= BattleSuccessConfirm;
            m_successPanel.OnRefuseBtnEvent -= BattleSuccessCancle;
            m_failPanel.OnSuccessBtnEvent -= BattleFailConfirm;
            m_failPanel.OnRefuseBtnEvent -= BattleFailCancle;
            CloseSelfPanel();
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
                    //UIMgr.S.OpenPanel(UIID.LogPanel, BattleSucessCallback, "挑战结果", "恭喜您挑战成功", "确定", "取消");
                    UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_LevelConfigInfo, true);
                    break;
                case EventID.OnBattleFailed:
                    UIMgr.S.OpenPanel(UIID.CombatSettlementPanel, m_LevelConfigInfo, false);
                    //UIMgr.S.OpenPanel(UIID.LogPanel, BattleFailCallback, "挑战结果", "对不起，您挑战失败!", "确定", "取消");
                    break;
                default:
                    break;
            }
        }

        private void BattleSucessCallback(AbstractPanel obj)
        {
            m_successPanel = obj as LogPanel;
            m_successPanel.OnSuccessBtnEvent += BattleSuccessConfirm;
            m_successPanel.OnRefuseBtnEvent += BattleSuccessCancle;
        }

        private void BattleFailCallback(AbstractPanel obj)
        {
            m_failPanel = obj as LogPanel;
            m_failPanel.OnSuccessBtnEvent += BattleFailConfirm;
            m_failPanel.OnRefuseBtnEvent += BattleFailCancle;
        }

        private void BattleSuccessConfirm()
        {
            SuccessBtn();
        }
        private void BattleSuccessCancle()
        {

        }

        private void BattleFailConfirm()
        {
            SuccessBtn();
        }
        private void BattleFailCancle()
        {

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
    }
}