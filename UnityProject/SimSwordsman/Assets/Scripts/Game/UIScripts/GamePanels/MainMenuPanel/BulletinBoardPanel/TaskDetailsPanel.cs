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
        private Text m_EffectValue;
        [SerializeField]
        private Text m_SelectedEffectValue;
        [SerializeField]
        private Text m_Statue;



        [SerializeField]
        private Button m_AcceptBtn;
        [SerializeField]
        private Button m_RefuseBtn;

        [SerializeField]
        private Transform m_SelectedTra;
        [SerializeField]
        private GameObject m_DiscipleItem;

        private SimGameTask m_CurTaskInfo;

        // Start is called before the first frame update
        protected override void OnUIInit()
        {
            base.OnUIInit();
            AudioMgr.S.PlaySound(Define.INTERFACE);
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
            m_CurTaskInfo = args[0] as SimGameTask;

            for (int i = 0; i < m_CurTaskInfo.CommonTaskItemInfo.itemRewards.Count; i++)
            {
                m_RewardValueOne.text += m_CurTaskInfo.CommonTaskItemInfo.GetRewardValue(0).ToString();
            }
            m_TaskCont.text = m_CurTaskInfo.CommonTaskItemInfo.desc;
        }

        private void RefeshPanelInfo()
        {
            switch (m_CurTaskInfo.CommonTaskItemInfo.taskState)
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
            m_RefuseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel,PanelType.Task, m_CurTaskInfo);
                UIMgr.S.ClosePanelAsUIID(UIID.BulletinBoardPanel);
                OnPanelHideComplete();
            });
            m_AcceptBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

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
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
	
}