using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

namespace GameWish.Game
{
    public class CommonTaskMgr : MonoBehaviour, IMgr
    {
        private string m_MainTaskTableName = "TDCommonTaskTable";

        private CommonTaskData m_CommonTaskData;

        private List<SimGameTask> m_CurTaskList = new List<SimGameTask>();

        private float m_CommonTaskRefreshInterval = 0.1f; // 5分钟刷新一次
        private int m_CommonTaskCount = 2;

        private DateTime m_LastRefreshCommonTaskTime = DateTime.Now;

        private TaskPos m_TaskPos;

        private Dictionary<CollectedObjType, TaskCollectableItem> m_CollectedObjDic = new Dictionary<CollectedObjType, TaskCollectableItem>();
        private Dictionary<CollectedObjType, AddressableGameObjectLoader> m_TaskObjLoaderDic = new Dictionary<CollectedObjType, AddressableGameObjectLoader>();

        public List<SimGameTask> CurTaskList { get => m_CurTaskList; }

        #region IMgr
        public void OnInit()
        {
            m_TaskPos = GameObject.FindObjectOfType<TaskPos>();

            m_CommonTaskData = GameDataMgr.S.GetCommonTaskData();

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
            m_CommonTaskCount = TDFacilityLobbyTable.GetLevelInfo(lobbyLevel).commonTaskCount;
            //InitTaskList();

            m_LastRefreshCommonTaskTime = DateTime.Parse(m_CommonTaskData.lastRefreshTime);

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
            RefreshCommonTask();
        }

        public SimGameTask GetSimGameTask(int taskID)
        {
            SimGameTask simGameTask = m_CurTaskList.Where(i => i.TaskId == taskID).FirstOrDefault();
            if (simGameTask != null)
                return simGameTask;
            return null;
        }

        public void RemoveTask(int taskId)
        {
            SimGameTask taskItem = m_CurTaskList.FirstOrDefault(i => i.TaskId == taskId);
            if (taskItem != null)
            {
                m_CurTaskList.Remove(taskItem);
            }

            GameDataMgr.S.GetCommonTaskData().RemoveTask(taskId);
        }

        public void SetTaskFinished(int taskId)
        {
            SimGameTask item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                item.CommonTaskItemInfo.taskState = TaskState.Unclaimed;
                GameDataMgr.S.GetCommonTaskData().SetTaskFinished(taskId);
                EventSystem.S.Send(EventID.OnCommonTaskFinish, taskId);
            }
        }

        public void SetTaskExcutedTime(int taskId, int time)
        {
            SimGameTask item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                GameDataMgr.S.GetCommonTaskData().SetTaskExecutedTime(taskId, time);
            }
        }

        public int GetTaskExecutedTime(int taskId)
        {
            SimGameTask item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                return GameDataMgr.S.GetCommonTaskData().GetTaskExecutedTime(taskId);
            }

            return 0;
        }
        /// <summary>
        /// 完成任务后领取奖励
        /// </summary>
        public void ClaimReward(int taskId)
        {
            GameDataMgr.S.GetCommonTaskData().OnTaskRewardClaimed(taskId);

            SimGameTask taskItem = m_CurTaskList.FirstOrDefault(i => i.TaskId == taskId );
            if (taskItem != null)
            {
                taskItem.ClaimReward(true);
                m_CurTaskList.Remove(taskItem);
            }
        }

        public void SpawnTaskCollectableItem(CollectedObjType collectedObjType)
        {
            string prefabName = GetPrefabName(collectedObjType);

            if (string.IsNullOrEmpty(prefabName))
                return;

            try
            {
                AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
                loader.InstantiateAsync(prefabName, (go) =>
                {
                    go.transform.position = m_TaskPos.GetTaskPos(collectedObjType);
                    if (!m_CollectedObjDic.ContainsKey(collectedObjType))
                    {
                        TaskCollectableItem item = go.GetComponent<TaskCollectableItem>();
                        m_CollectedObjDic.Add(collectedObjType, item);
                        m_TaskObjLoaderDic.Add(collectedObjType, loader);
                    }
                    else
                    {
                        Log.e("Task obj has been created before: " + collectedObjType);
                    }
                });
            }
            catch (Exception e)
            {
                Log.e("SpawnTaskCollectableItem error: " + prefabName + " " + e.Message.ToString() + " " + e.StackTrace);
            }
        }

        public void RemoveTaskCollectableItem(CollectedObjType collectedObjType)
        {
            if (m_CollectedObjDic.ContainsKey(collectedObjType))
            {
                m_TaskObjLoaderDic[collectedObjType].Release();
                m_TaskObjLoaderDic.Remove(collectedObjType);

                m_CollectedObjDic.Remove(collectedObjType);
            }
        }

        public TaskCollectableItem GetTaskCollectableItem(CollectedObjType collectedObjType)
        {
            if (m_CollectedObjDic.ContainsKey(collectedObjType))
            {
                return m_CollectedObjDic[collectedObjType];
            }

            return null;
        }

        public static bool IsNotNeedToSpawnTaskItem(CollectedObjType collectedObjType)
        {
            return collectedObjType == CollectedObjType.Well || collectedObjType == CollectedObjType.Fish;
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


        public void InitTaskList()
        {
            m_CommonTaskData.taskList.ForEach(i => 
            {
                //int leftTime = Mathf.Max(0, i.taskTime - i.executedTime);
                SimGameTask task = AddTask(i.taskId, i.taskType, i.taskState, i.taskTime);

                // 如果数据库中该task正在执行
                if (i.taskState == TaskState.Running)
                {
                    List<CharacterController> characters = MainGameMgr.S.CharacterMgr.GetAllCharacterInTask(i.taskId);
                    task.ExecuteTask(characters);
                }
            });

            RefreshTask();
        }

        private void RefreshCommonTask()
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(m_LastRefreshCommonTaskTime.Ticks);

            if (timeSpan.TotalMinutes > m_CommonTaskRefreshInterval)
            {
                m_LastRefreshCommonTaskTime = DateTime.Now;

                int curCommonTaskCount = m_CurTaskList.Where(i => i.CommonTaskItemInfo.triggerType == SimGameTaskTriggerType.Common).ToList().Count;
                if (curCommonTaskCount < m_CommonTaskCount)
                {
                    int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
                    if (GameDataMgr.S.GetPlayerData().isGuideStart == false)
                    {
                        lobbyLevel = 0;
                    }
                    List<CommonTaskItemInfo> allCommonTask = TDCommonTaskTable.GetAllCommonTaskByLobbyLevel(lobbyLevel);

                    for (int i = 0; i < m_CommonTaskCount - curCommonTaskCount; i++)
                    {
                        int randomIndex = UnityEngine.Random.Range(0, allCommonTask.Count);
                        CommonTaskItemInfo task = allCommonTask[randomIndex];

                        if (!IsTaskExist(task.id))
                        {
                            GenerateTask(task.id, task.taskType, task.subType, task.taskTime);
                            allCommonTask.Remove(task);
                        }
                    }
                }
            }
        }

        public void GenerateTask(int taskId, SimGameTaskType taskType, int subType, int taskTime)
        {
            AddTask(taskId, taskType, TaskState.NotStart, taskTime);

            m_CommonTaskData.AddTask(taskId, taskType, subType, TaskState.NotStart);
        }

        public SimGameTask GetCommonTaskItemData(int taskId)
        {
            foreach (SimGameTask item in m_CurTaskList)
            {
                if (item.CommonTaskItemInfo.id == taskId)
                {
                    return item;
                }
            }

            return null;
        }

        private SimGameTask AddTask(int taskId, SimGameTaskType taskType, TaskState taskState, int taskTime)
        {
            SimGameTask simGameTask = SimGameTaskFactory.SpawnTask(taskId, taskType, taskState, taskTime);
            m_CurTaskList.Add(simGameTask);

            return simGameTask;
        }

        private bool IsTaskExist(int taskId)
        {
            return m_CurTaskList.Any(i => i.TaskId == taskId);
        }

        private string GetPrefabName(CollectedObjType collectedObjType)
        {
            if (IsNotNeedToSpawnTaskItem(collectedObjType))
            {
                return string.Empty;
            }

            return collectedObjType.ToString();
        }
        #endregion

    }

}