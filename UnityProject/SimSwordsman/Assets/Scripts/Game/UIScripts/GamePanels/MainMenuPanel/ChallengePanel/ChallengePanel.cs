using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ChallengePanel : AbstractAnimPanel
    {
        [SerializeField] private Text m_ChallengeCont;

        [SerializeField] private Button m_CloseBtn;

        [SerializeField] private Transform m_ChallengeTrans;

        [SerializeField] private GameObject m_ChallengeTaskItem;
        [SerializeField] private ScrollRect m_SrollView;
        private List<ChapterConfigInfo> m_CurChapterInfo = null;
        private List<ChallengeTaskItem> m_ChallengeTaskItemList = new List<ChallengeTaskItem>();
        protected override void OnUIInit()
        {
            base.OnUIInit();

            EventSystem.S.Register(EventID.OnCloseParentPanel, HandlingEventListening);
            BindAddListenerEvent();
            GetInformationForNeed();

            InitPanelInfo();

        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_SrollView.verticalNormalizedPosition = 1;
            int isOpened = PlayerPrefs.GetInt(Define.Is_Enter_Challenge_Panel, -1);
            if (isOpened == -1)
            {
                PlayerPrefs.SetInt(Define.Is_Enter_Challenge_Panel, 1);
            }

            if (args.Length == 0)
                return;

            foreach (var item in m_ChallengeTaskItemList)
            {
                if (item.GetClanType() == (ClanType)args[0])
                {
                    item.ClickBtn();
                }
            }
        }

        private void HandlingEventListening(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnCloseParentPanel:
                    HideSelfWithAnim();
                    break;
                default:
                    break;
            }
        }
        private void GetInformationForNeed()
        {
            m_CurChapterInfo = MainGameMgr.S.ChapterMgr.GetAllChapterInfo();
        }

        private void InitPanelInfo()
        {
            m_ChallengeCont.text = TDLanguageTable.Get(Define.CHALLENGE_DESCRIBE);

            foreach (var item in m_CurChapterInfo)
                CreateChallengeTask(item);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                UIMgr.S.OpenPanel(UIID.MainMenuPanel);
                HideSelfWithAnim();
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();

            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandlingEventListening);
        }
        /// <summary>
        /// ������ս����
        /// </summary>
        /// <param name="configInfo"></param>
        private void CreateChallengeTask(ChapterConfigInfo configInfo)
        {
            ChallengeTaskItem challengeTask = Instantiate(m_ChallengeTaskItem, m_ChallengeTrans).GetComponent<ChallengeTaskItem>();
            challengeTask.OnInit(configInfo, this);
            m_ChallengeTaskItemList.Add(challengeTask);
        }
    }
}