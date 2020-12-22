using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BulletinBoardPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_BulletinCont;

        [SerializeField]
        private Button m_CloaseBtn;

        [SerializeField]
        private Transform m_TaskContParent;

        [SerializeField]
        private GameObject m_TaskItem;

        private List<SimGameTask> m_MainTaskList = null;


        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            BindAddListenerEvent();
        }

        private void GetInformationForNeed()
        {
            m_MainTaskList = MainGameMgr.S.MainTaskMgr.CurTaskList;
        }

        private void BindAddListenerEvent()
        {
            m_CloaseBtn.onClick.AddListener(HideSelfWithAnim);

        }

        protected override void OnOpen()
        {
            base.OnOpen();
            if (m_MainTaskList!=null)
            {
                for (int i = 0; i < m_MainTaskList.Count; i++)
                {
                    CreateTask(m_MainTaskList[i]);
                }
            }
        }

        private void TaskCallback(object obj)
        {
            SimGameTask simGameTask = obj as SimGameTask;
            UIMgr.S.OpenPanel(UIID.TaskDetailsPanel, simGameTask);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();

        }

        public void CreateTask(SimGameTask simGameTask)
        {
            if (m_TaskItem != null)
            {
                ItemICom taskItem = Instantiate(m_TaskItem, m_TaskContParent).GetComponent<ItemICom>();
                taskItem.OnInit(simGameTask);
                taskItem.SetButtonEvent(TaskCallback);
            }
        }
    }
}