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
    public class ChallengeTaskItem : MonoBehaviour, ItemICom
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
        private Slider m_ChallengeSlide;

        [SerializeField]
        private Button m_ChallengeBtn;
        private List<Sprite> m_Sprites;
        private ChapterConfigInfo m_CurChapterConfigInfo = null;
        private ChallengeType m_CurChallengeType = ChallengeType.Unlocked;
        private int m_CurLevel = -1;
        private Sprite GetSprite(string name)
        {
            return m_Sprites.Where(i => i.name.Equals(name)).FirstOrDefault();
        }
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
            m_Sprites = (List<Sprite>)obj[0];
            m_CurChapterConfigInfo = t as ChapterConfigInfo;
            if (m_CurChapterConfigInfo != null)
            {
                m_ChallengeSectsName.text = CommonUIMethod.GetClanName(m_CurChapterConfigInfo.clanType);
                m_ChallengeCont.text = m_CurChapterConfigInfo.desc;
                m_ChallengePhoto.sprite = GetSprite("Gaibang");
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
                    m_CompletedValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED); ;
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_ChallengeSlide.gameObject.SetActive(false);
                    m_CompletedImg.gameObject.SetActive(false);
                    break;
                case ChallengeType.Challenge:
                    m_CompletedValue.text = Define.COMMON_DEFAULT_STR;
                    m_CompletedImg.gameObject.SetActive(false);
                    m_ChallengeBtn.gameObject.SetActive(true);
                    m_ChallengeSlide.value = GetCurProgress();
                    m_ChallengeProgress.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_PROGRESS) +(GetCurProgress() * 100).ToString("f2") + Define.PERCENT;
                    break;
                case ChallengeType.Passed:
                    m_CompletedValue.text = CommonUIMethod.GetStringForTableKey(Define.CHALLENGE_PROGRESS_OVER);
                    m_CompletedImg.gameObject.SetActive(true);
                    m_ChallengeBtn.gameObject.SetActive(false);
                    m_ChallengeSlide.gameObject.SetActive(false);
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