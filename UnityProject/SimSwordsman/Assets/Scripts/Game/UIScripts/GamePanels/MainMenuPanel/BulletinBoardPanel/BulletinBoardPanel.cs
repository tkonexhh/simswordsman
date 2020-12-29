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

        private Dictionary<int, GameObject> m_TaskObjDic = new Dictionary<int, GameObject>();

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
            EventSystem.S.Register(EventID.OnTaskFinished, HandleAddListenerEvent);
            if (m_MainTaskList != null)
            {
                for (int i = 0; i < m_MainTaskList.Count; i++)
                {
                    if (m_MainTaskList[i].GetCurTaskState() != TaskState.Finished)
                        CreateTask(m_MainTaskList[i]);
                }
            }

            MainGameMgr.S.MainTaskMgr.RefreshTask();
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnTaskFinished:

                    break;
                default:
                    break;
            }
        }

        private void TaskCallback(object obj)
        {
            SimGameTask simGameTask = obj as SimGameTask;

            switch (simGameTask.GetCurTaskState())
            {
                case TaskState.NotStart:
                    UIMgr.S.OpenPanel(UIID.TaskDetailsPanel, simGameTask);
                    break;
                case TaskState.Unclaimed:
                    MainGameMgr.S.MainTaskMgr.ClaimReward(simGameTask.GetId());
                    if (m_TaskObjDic.ContainsKey(simGameTask.GetId()))
                    {
                        DestroyImmediate(m_TaskObjDic[simGameTask.GetId()]);
                        m_TaskObjDic[simGameTask.GetId()] = null;
                    }
                    break;
            }


        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnTaskFinished, HandleAddListenerEvent);
            CloseSelfPanel();
        }

        public void CreateTask(SimGameTask simGameTask)
        {
            if (m_TaskItem != null)
            {
                GameObject obj = Instantiate(m_TaskItem, m_TaskContParent);
                ItemICom taskItem = obj.GetComponent<ItemICom>();
                taskItem.OnInit(simGameTask);
                taskItem.SetButtonEvent(TaskCallback);
                m_TaskObjDic.Add(simGameTask.GetId(), obj);
            }
        }
    }
}