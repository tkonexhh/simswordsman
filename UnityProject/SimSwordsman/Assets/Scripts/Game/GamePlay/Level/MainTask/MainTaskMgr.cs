using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class MainTaskMgr : MonoBehaviour, IMgr
    {
        private string m_MainTaskTableName = "TDMainTaskTable";

        private MainTaskData m_MainTaskData;

        private List<SimGameTask> m_CurTaskList = new List<SimGameTask>();

        //private List<MainTaskItemInfo> m_CurTaskInfoList = new List<MainTaskItemInfo>();

        private List<IMainTaskObserver> m_MainTaskObserverList = new List<IMainTaskObserver>();

        private int m_DailyTaskCount = 2;
        private float m_CommonTaskRefreshInterval = 0.1f; // 5分钟刷新一次
        private int m_CommonTaskCount = 3;

        private DateTime m_LastRefreshCommonTaskTime = DateTime.Now;

        public List<SimGameTask> CurTaskList { get => m_CurTaskList; }

        #region IMgr
        public void OnInit()
        {
            m_MainTaskData = GameDataMgr.S.GetMainTaskData();

            InitTaskList();

            m_LastRefreshCommonTaskTime = DateTime.Now;
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

        }

        #endregion

        #region Public 

        /// <summary>
        /// 打开UI界面时调用
        /// </summary>
        public void RefreshTask()
        {
            RefreshDailyTask();
            RefreshCommonTask();
        }

        public SimGameTask GetSimGameTask(int taskID)
        {
            SimGameTask simGameTask = m_CurTaskList.Where(i => i.GetId() == taskID).FirstOrDefault();
            if (simGameTask != null)
                return simGameTask;
            return null;
        }

        public void SetTaskFinished(int taskId)
        {
            SimGameTask item = GetMainTaskItemData(taskId);
            if (item != null)
            {
                item.MainTaskItemInfo.taskState = TaskState.Unclaimed;
                GameDataMgr.S.GetMainTaskData().SetTaskFinished(taskId);
            }
        }

        /// <summary>
        /// 完成任务后领取奖励
        /// </summary>
        public void ClaimReward(int taskId)
        {
            GameDataMgr.S.GetMainTaskData().OnTaskRewardClaimed(taskId);

            SimGameTask taskItem = m_CurTaskList.FirstOrDefault(i => i.GetId() == taskId );
            if (taskItem != null)
            {
                taskItem.ClaimReward();
                m_CurTaskList.Remove(taskItem);

                //MainTaskItemInfo itemInfo = m_CurTaskInfoList.FirstOrDefault(i => i.id == taskItem.GetId() && i.subId == taskItem.GetSubId());
                //if (itemInfo != null)
                //{
                //    m_CurTaskInfoList.Remove(itemInfo);
                //}
            }
        }

        public void AddMainTaskObserver(IMainTaskObserver ob)
        {
            if (!m_MainTaskObserverList.Contains(ob))
            {
                m_MainTaskObserverList.Add(ob);
            }
        }

        public void RemoveMainTaskObserver(IMainTaskObserver ob)
        {
            if (m_MainTaskObserverList.Contains(ob))
            {
                m_MainTaskObserverList.Remove(ob);
            }
        }

        public void OnTaskStateChanged(TaskItem taskItem)
        {
            NotifyObserver(taskItem);

            bool isFinished = taskItem.IsFinished();

            if (isFinished)
            {
                GameDataMgr.S.GetMainTaskData().OnTaskFinished(taskItem.GetId());
            }
        }

        #endregion

        #region Private

        private void RegisterEvents()
        {
            //EventSystem.S.Register(EngineEventID.OnDateUpdate, HandleEvent);
        }

        private void UnregisterEvents()
        {
            //EventSystem.S.UnRegister(EventID.OnStartUnlockFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnStartUpgradeFacility:

                    break;

            }
        }


        private void InitTaskList()
        {
            m_MainTaskData.taskList.ForEach(i => 
            {
                AddTask(i.taskId, i.taskType, i.taskState, i.taskTime);
            });

            RefreshTask();
        }

        private void RefreshDailyTask()
        {
            DateTime lasPlayTime = OfflineRewardMgr.GetLastPlayDate(GameDataMgr.S.GetPlayerData().lastPlayTime);
            if (DateTime.Now.Day != lasPlayTime.Day && DateTime.Now.Hour >= 6) // 6点刷新
            {
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);

                RemoveDailyTaskByLobbyLevel(lobbyLevel);

                List<MainTaskItemInfo> allDailyTask = TDMainTaskTable.GetAllDailyTaskByLobbyLevel(lobbyLevel);
                foreach (MainTaskItemInfo item in allDailyTask)
                {
                    if (!m_MainTaskData.IsTaskExist(item.id))
                    {
                        GenerateTask(item.id, item.taskType, item.subType, item.taskTime);
                    }
                }
            }
        }

        private void RemoveDailyTaskByLobbyLevel(int lobbyLevel)
        {
            for (int i = m_CurTaskList.Count - 1; i >= 0; i--)
            {
                if (m_CurTaskList[i].MainTaskItemInfo.triggerType == SimGameTaskTriggerType.Daily && m_CurTaskList[i].MainTaskItemInfo.needHomeLevel != lobbyLevel)
                {
                    m_MainTaskData.RemoveTask(m_CurTaskList[i].TaskId);

                    m_CurTaskList.RemoveAt(i);
                }
            }
        }

        private void RefreshCommonTask()
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(m_LastRefreshCommonTaskTime.Ticks);

            if (timeSpan.TotalMinutes > m_CommonTaskRefreshInterval)
            {
                m_LastRefreshCommonTaskTime = DateTime.Now;

                int curCommonTaskCount = m_CurTaskList.Where(i => i.MainTaskItemInfo.triggerType == SimGameTaskTriggerType.Common).ToList().Count;
                if (curCommonTaskCount < m_CommonTaskCount)
                {
                    int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
                    List<MainTaskItemInfo> allCommonTask = TDMainTaskTable.GetAllCommonTaskByLobbyLevel(lobbyLevel);

                    for (int i = 0; i < m_CommonTaskCount - curCommonTaskCount; i++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, allCommonTask.Count);
                        MainTaskItemInfo task = allCommonTask[randomIndex];
                        GenerateTask(task.id, task.taskType, task.subType, task.taskTime);

                        allCommonTask.Remove(task);
                    }
                }
            }
        }

        public void GenerateTask(int taskId, SimGameTaskType taskType, int subType, int taskTime)
        {
            AddTask(taskId, taskType, TaskState.NotStart, taskTime);

            m_MainTaskData.AddTask(taskId, taskType, subType, TaskState.NotStart);
        }

        public SimGameTask GetMainTaskItemData(int taskId)
        {
            foreach (SimGameTask item in m_CurTaskList)
            {
                if (item.MainTaskItemInfo.id == taskId)
                {
                    return item;
                }
            }

            return null;
        }

        private void NotifyObserver(TaskItem task)
        {
            foreach (IMainTaskObserver ob in m_MainTaskObserverList)
            {
                ob.OnTaskStateChanged(task);
            }
        }

        private void AddTask(int taskId, SimGameTaskType taskType, TaskState taskState, int taskTime)
        {
            SimGameTask simGameTask = SimGameTaskFactory.SpawnTask(taskId, taskType, taskState, taskTime);
            m_CurTaskList.Add(simGameTask);
        }

        #endregion

    }

}