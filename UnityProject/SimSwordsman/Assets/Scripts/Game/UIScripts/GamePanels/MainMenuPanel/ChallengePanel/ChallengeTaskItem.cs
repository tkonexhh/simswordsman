using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum ChallengeType
    {
        Unlocked,       //未解锁
        Challenge,      //可挑战
        Passed,         //已完成
    }
    public class ChallengeTaskItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Text m_ChallengeSectsName;
        [SerializeField]
        private Text m_ChallengeCont;
        [SerializeField]
        private Text m_ChallengeProgress;
        [SerializeField]
        private Text m_ChallengeState;

        [SerializeField]
        private Image m_ChallengePhoto;

        [SerializeField]
        private Slider m_ChallengeSlide;

        [SerializeField]
        private Button m_ChallengeBtn;

        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private ChallengeType m_CurChallengeType = ChallengeType.Unlocked;
        private int m_CurLevel = -1;

        // Start is called before the first frame update
        void Start()
        {
            BindAddListenerEvent();
        }

        private void BindAddListenerEvent()
        {
            m_ChallengeBtn.onClick.AddListener(() =>
            {
                UIMgr.S.OpenPanel(UIID.ChallengeBattlePanel, m_CurChapterConfigInfo);
                EventSystem.S.Send(EventID.OnCloseParentPanel);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.UnRegister(EventID.OnUnlockNewChapter, HandlingListeningEvents);
        }

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnUnlockNewChapter, HandlingListeningEvents);   
            m_CurChapterConfigInfo = t as ChapterConfigInfo;
            if (m_CurChapterConfigInfo != null)
            {
                m_ChallengeSectsName.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
                m_ChallengeCont.text = m_CurChapterConfigInfo.desc;

                RefreshPanelInfo();
            }
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
                    RefreshPanelInfo();
                    break;
                case EventID.OnUnlockNewChapter:
                    int chapterId = (int)param[0];
                    MainGameMgr.S.ChapterMgr.AddNewCheckpoint(chapterId);
                    RefreshPanelInfo();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 刷新面板所需信息
        /// </summary>
        private void RefreshPanelInfo()
        {
            bool isUnlock = MainGameMgr.S.ChapterMgr.JudgeChapterIsUnlock(m_CurChapterConfigInfo.chapterId);
            if (isUnlock)
                MainGameMgr.S.ChapterMgr.AddNewCheckpoint(m_CurChapterConfigInfo.chapterId);     

            m_CurLevel = MainGameMgr.S.ChapterMgr.GetLevelProgress(m_CurChapterConfigInfo.chapterId) - 1;
            if (m_CurLevel == -1)
            {
                UpdataPanelInfo();
                return;
            }

            if (m_CurLevel == m_CurChapterConfigInfo.chapterCount)
            {
                m_CurChallengeType = ChallengeType.Passed;
                UpdataPanelInfo();
                return;
            }

            m_CurChallengeType = ChallengeType.Challenge;

            UpdataPanelInfo();
        }
        /// <summary>
        /// 获取当前关卡进度
        /// </summary>
        /// <returns></returns>
        private float GetCurProgress()
        {
            return (float)m_CurLevel / m_CurChapterConfigInfo.chapterCount;
        }

        /// <summary>
        /// 更新面板信息
        /// </summary>
        private void UpdataPanelInfo()
        {
            switch (m_CurChallengeType)
            {
                case ChallengeType.Unlocked:
                    m_ChallengeState.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_STATUE_UNLOCKED);
                    m_ChallengeState.gameObject.SetActive(true);
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_ChallengeSlide.value = 0;
                    m_ChallengeProgress.text = "0%";
                    break;
                case ChallengeType.Challenge:
                    m_ChallengeState.text = "";
                    m_ChallengeState.gameObject.SetActive(false);
                    m_ChallengeBtn.gameObject.SetActive(true);
                    m_ChallengeSlide.value = GetCurProgress();
                    m_ChallengeProgress.text = (GetCurProgress() * 100).ToString() + "%";
                    break;
                case ChallengeType.Passed:
                    m_ChallengeState.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_STATUE_COMPLETED);
                    m_ChallengeState.gameObject.SetActive(true);
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_ChallengeSlide.value = 1;
                    m_ChallengeProgress.text = "100%";
                    break;
                default:
                    break;
            }
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }
    }
}