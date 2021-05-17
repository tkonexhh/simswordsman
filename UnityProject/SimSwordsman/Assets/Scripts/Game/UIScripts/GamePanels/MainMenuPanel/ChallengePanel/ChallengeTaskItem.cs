using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class ChallengeTaskItem : MonoBehaviour
    {
        [SerializeField]
        private Text m_ChallengeSectsName;
        [SerializeField]
        private Text m_ChallengeCont;
        [SerializeField]
        private Text m_ChallengeProgress;

        [SerializeField]
        private Image m_ChallengePhoto;
        [SerializeField]
        private Image m_CompletedImg;
        [SerializeField]
        private Text m_CompletedValue;

        [SerializeField]
        private Image m_ChallengeSlide;   
        [SerializeField]
        private Image m_Background;

        [SerializeField]
        private Button m_ChallengeBtn;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private ChallengeType m_CurChallengeType = ChallengeType.Unlocked;
        private int m_CurLevel = -1;
        private ChallengePanel m_ChallengePanel;

        public ClanType GetClanType()
        {
            return m_CurChapterConfigInfo.clanType;
        }

        // Start is called before the first frame update
        void Start()
        {
            BindAddListenerEvent();
        }
        void OnDestroy()
        {

        }


        public void ClickBtn()
        {
            UIMgr.S.OpenPanel(UIID.ChallengeBattlePanel, m_CurChapterConfigInfo);
        }

        private void BindAddListenerEvent()
        {
            m_ChallengeBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.ChallengeBattlePanel, m_CurChapterConfigInfo);
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

        public void OnInit<T>(T t, ChallengePanel challengePanel )
        {
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnUnlockNewChapter, HandlingListeningEvents);
            m_ChallengePanel = challengePanel;
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

            m_CurLevel = MainGameMgr.S.ChapterMgr.GetLevelProgressNumber(m_CurChapterConfigInfo.chapterId);
            if (m_CurLevel == -1)
            {
                UpdataPanelInfo();
                return;
            }

            if (m_CurLevel == MainGameMgr.S.ChapterMgr.GetChapterNumber(m_CurChapterConfigInfo.chapterId))
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
            return (float)m_CurLevel / MainGameMgr.S.ChapterMgr.GetChapterNumber(m_CurChapterConfigInfo.chapterId);
        }

        /// <summary>
        /// 更新面板信息
        /// </summary>
        private void UpdataPanelInfo()
        {
            switch (m_CurChallengeType)
            {
                case ChallengeType.Unlocked:
                    m_CompletedValue.text = "通过" + CommonUIMethod.GetClanName((ClanType)m_CurChapterConfigInfo.unlockPrecondition.chapter)
                        + CommonUIMethod.GetStrForColor("#9C4B45", " 挑战" + m_CurChapterConfigInfo.unlockPrecondition.level) + " 后解锁";
                    m_ChallengePhoto.sprite = m_ChallengePanel.FindSprite("Challenge_Bignotunlock");
                    m_ChallengeProgress.text = Define.COMMON_DEFAULT_STR;
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_ChallengeSlide.gameObject.SetActive(false);
                    m_Background.gameObject.SetActive(false);
                    m_CompletedImg.gameObject.SetActive(false);
                    break;
                case ChallengeType.Challenge:
                    m_CompletedValue.text = Define.COMMON_DEFAULT_STR;
                    m_ChallengePhoto.sprite = m_ChallengePanel.FindSprite("Challenge_Big" + m_CurChapterConfigInfo.clanType.ToString().ToLower());
                    m_CompletedImg.gameObject.SetActive(false);
                    m_ChallengeBtn.gameObject.SetActive(true);
                    m_ChallengeSlide.fillAmount = GetCurProgress();
                    m_ChallengeSlide.gameObject.SetActive(true);
                    m_Background.gameObject.SetActive(true);
                    m_ChallengeProgress.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_PROGRESS) + (GetCurProgress() * 100).ToString("f2") + Define.PERCENT;
                    break;
                case ChallengeType.Passed:
                    //m_CompletedValue.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_PROGRESS_OVER);
                    m_CompletedValue.gameObject.SetActive(false);
                    m_ChallengeProgress.text = Define.COMMON_DEFAULT_STR;
                    m_ChallengePhoto.sprite = m_ChallengePanel.FindSprite("Challenge_Big" + m_CurChapterConfigInfo.clanType.ToString().ToLower());
                    m_CompletedImg.gameObject.SetActive(true);
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_Background.gameObject.SetActive(false);
                    m_ChallengeSlide.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }

            if (PlatformHelper.isTestMode)
            {
                m_ChallengeBtn.gameObject.SetActive(true);
            }
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }
    }
}