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

        public List<SimGameTask> CurTaskList { get => m_CurTaskList; }

        #region IMgr
        public void OnInit()
        {
            m_MainTaskData = GameDataMgr.S.GetMainTaskData();

            InitTaskList();
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
        /// ��UI����ʱ����
        /// </summary>
        public void RefreshTask()
        {
            RefreshDailyTask();
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
        /// ����������ȡ����
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
                AddTask(i.taskId, i.taskType, i.taskState);
            });

            RefreshDailyTask();
        }

        private void RefreshDailyTask()
        {
            DateTime lasPlayTime = OfflineRewardMgr.GetLastPlayDate(GameDataMgr.S.GetPlayerData().lastPlayTime);
            if (DateTime.Now.Day != lasPlayTime.Day && DateTime.Now.Hour >= 6) // 6��ˢ��
            {
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);

                m_MainTaskData.RemoveDailyTaskByLobbyLevel(lobbyLevel);

                List<MainTaskItemInfo> allDailyTask = TDMainTaskTable.GetAllDailyTaskByLobbyLevel(lobbyLevel);
                foreach (MainTaskItemInfo item in allDailyTask)
                {
                    if (!m_MainTaskData.IsTaskExist(item.id))
                    {
                        GenerateTask(item.id, item.taskType, item.subType);
                    }
                }
            }
        }

        private void GenerateTask(int taskId, SimGameTaskType taskType, int subType)
        {
            AddTask(taskId, taskType);

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

        private void AddTask(int taskId, SimGameTaskType taskType, TaskState taskState = TaskState.NotStart)
        {
            SimGameTask simGameTask = SimGameTaskFactory.SpawnTask(taskId, taskType, taskState);
            m_CurTaskList.Add(simGameTask);
        }

        #endregion

    }

}