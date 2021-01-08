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

        private List<SimGameTask> m_CommonTaskList = null;

        private Dictionary<int, GameObject> m_TaskObjDic = new Dictionary<int, GameObject>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            BindAddListenerEvent();
        }

        private void GetInformationForNeed()
        {
            m_CommonTaskList = MainGameMgr.S.CommonTaskMgr.CurTaskList;
        }

        private void BindAddListenerEvent()
        {
            m_CloaseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            EventSystem.S.Register(EventID.OnTaskManualFinished, HandleAddListenerEvent);
            if (m_CommonTaskList != null)
            {
                for (int i = 0; i < m_CommonTaskList.Count; i++)
                {
                    if (m_CommonTaskList[i].GetCurTaskState() != TaskState.Finished)
                        CreateTask(m_CommonTaskList[i]);
                }
            }

            MainGameMgr.S.CommonTaskMgr.RefreshTask();
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnTaskManualFinished:

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
                    MainGameMgr.S.CommonTaskMgr.ClaimReward(simGameTask.TaskId);
                    if (m_TaskObjDic.ContainsKey(simGameTask.TaskId))
                    {
                        DestroyImmediate(m_TaskObjDic[simGameTask.TaskId]);
                        m_TaskObjDic[simGameTask.TaskId] = null;
                    }

                    //if (simGameTask.CommonTaskItemInfo.triggerType == SimGameTaskTriggerType.Main)
                    //{
                    //    List<int> nextTasks = simGameTask.CommonTaskItemInfo.nextTaskIdList;
                    //    foreach (int taskId in nextTasks)
                    //    {
                    //        MainTaskItemInfo taskInfo = TDMainTaskTable.GetMainTaskItemInfo(taskId);
                    //        MainGameMgr.S.MainTaskMgr.GenerateTask(taskId, taskInfo.taskType, taskInfo.subType, taskInfo.taskTime);
                    //        SimGameTask nextTask = MainGameMgr.S.MainTaskMgr.GetSimGameTask(taskId);
                    //        if (nextTask!=null)
                    //            CreateTask(nextTask);
                    //    }
                    //}
                    break;
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            EventSystem.S.UnRegister(EventID.OnTaskManualFinished, HandleAddListenerEvent);
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
                m_TaskObjDic.Add(simGameTask.TaskId, obj);
            }
        }
    }
}