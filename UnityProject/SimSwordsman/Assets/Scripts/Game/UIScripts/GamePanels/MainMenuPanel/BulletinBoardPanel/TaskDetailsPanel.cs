using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class TaskDetailsPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_TaskCont;
        [SerializeField]
        private Text m_RewardValueOne;
        [SerializeField]
        private Text m_RewardValueTwo;


        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;

        private SimGameTask m_CurTaskInfo;

        // Start is called before the first frame update
        protected override void OnUIInit()
        {
            base.OnUIInit();

            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurTaskInfo = args[0] as SimGameTask;

            for (int i = 0; i < m_CurTaskInfo.MainTaskItemInfo.rewards.Count; i++)
            {
                m_RewardValueOne.text += m_CurTaskInfo.MainTaskItemInfo.rewards[0].count.ToString();
            }
            m_TaskCont.text = m_CurTaskInfo.MainTaskItemInfo.desc;
        }

        private void RefeshPanelInfo()
        {
            switch (m_CurTaskInfo.MainTaskItemInfo.taskState)
            {
                case TaskState.NotStart:

                    break;
                case TaskState.Unclaimed:
                    break;
                case TaskState.Finished:
                    break;
                default:
                    break;
            }
        }

        private void BindAddListenerEvent()
        {
            m_RefuseBtn.onClick.AddListener(HideSelfWithAnim);
            m_AcceptBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel,PanelType.Task, m_CurTaskInfo);
                UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);
                OnPanelHideComplete();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
	
}