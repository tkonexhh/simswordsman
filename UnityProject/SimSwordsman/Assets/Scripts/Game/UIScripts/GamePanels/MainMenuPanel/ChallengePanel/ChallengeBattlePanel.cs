using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ChallengeBattlePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_ChallengeBattleTitle;

        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_CheckpointTrans;

        [SerializeField]
        private GameObject m_CheckpointItem;

        private List<Button> m_CheckpointBtns = null;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;

        private Dictionary<int, Button> m_LevelBtnDic = new Dictionary<int, Button>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnCloseParentPanel, HandlingListeningEvents);
            m_CurChapterConfigInfo = args[0] as ChapterConfigInfo;

            InitPanelInfo();
        }

        /// <summary>
        /// 处理事件机函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void HandlingListeningEvents(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnChanllengeSuccess:
                    int  level = (int)param[0];
                    if (m_LevelBtnDic.ContainsKey(level))
                    {
                        Button levelBtn = m_LevelBtnDic[level];
                        m_LevelBtnDic.Remove(level);
                        DestroyImmediate(levelBtn.gameObject);
                    }
                    break;
                case EventID.OnCloseParentPanel:
                    HideSelfWithAnim();
                    break;
                default:
                    break;
            }
        }

        private void InitPanelInfo()
        {
            //m_LevelConfigInfo = MainGameMgr.S.ChapterMgr.GetLevelInfo(m_CurChapterConfigInfo.chapterId);
            m_ChallengeBattleTitle.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
            int CurLevel = MainGameMgr.S.ChapterMgr.GetLevelProgress(m_CurChapterConfigInfo.chapterId);
            for (int i = 1; i <= m_CurChapterConfigInfo.chapterCount; i++)
            {
                if (i < CurLevel)
                    continue;

                Transform chapterItem = Instantiate(m_CheckpointItem, m_CheckpointTrans).transform;
                chapterItem.GetComponentInChildren<Text>().text = CommonUIMethod.GetChallengeTitle(m_CurChapterConfigInfo, TDLevelConfigTable.GetLevelId(m_CurChapterConfigInfo.chapterId, i));
                Button challengeBtn =  chapterItem.GetComponent<Button>();
                if (!m_LevelBtnDic.ContainsKey(i))
                    m_LevelBtnDic.Add(i, challengeBtn);
                challengeBtn.onClick.AddListener(() =>
                {
                    string[] name = chapterItem.GetComponentInChildren<Text>().text.Split('-');
                    int levelId = int.Parse(name[1]);
                    UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel, m_CurChapterConfigInfo, levelId);
                });
            }

        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=>{
                HideSelfWithAnim();
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
            });
            foreach (var item in m_CheckpointBtns)
            {
                item.onClick.AddListener(() =>
                {
                    UIMgr.S.OpenPanel(UIID.IdentifyChallengesPanel);
                });
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandlingListeningEvents);
            CloseSelfPanel();
        }
    }

}