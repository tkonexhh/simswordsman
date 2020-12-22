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
        [SerializeField]
        private Text m_ChallengeTitle;
        [SerializeField]
        private Text m_ChallengeCont;

        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_ChallengeTrans;

        [SerializeField]
        private GameObject m_ChallengeTaskItem;

        private List<ChapterConfigInfo> m_CurChapterInfo = null;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            EventSystem.S.Register(EventID.OnCloseParentPanel, HandlingEventListening);

            GetInformationForNeed();

            InitPanelInfo();

            BindAddListenerEvent();
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
            m_ChallengeTitle.text = TDLanguageTable.Get(Define.CHALLENGE_NAME);
            m_ChallengeCont.text = TDLanguageTable.Get(Define.CHALLENGE_DESCRIBE);


            foreach (var item in m_CurChapterInfo)
                CreateChallengeTask(item);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnCloseParentPanel, HandlingEventListening);
            CloseSelfPanel();
        }

        /// <summary>
        /// ������ս����
        /// </summary>
        /// <param name="configInfo"></param>
        private void CreateChallengeTask(ChapterConfigInfo configInfo)
        {
            ItemICom challengeTask = Instantiate(m_ChallengeTaskItem, m_ChallengeTrans).GetComponent<ItemICom>();
            challengeTask.OnInit(configInfo);
        }
    }
}