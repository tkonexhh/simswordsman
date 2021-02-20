using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class CommonTaskData : DataDirtyHandler
    {
        public List<CommonTaskItemData> taskList = new List<CommonTaskItemData>();
        public string lastRefreshTime;

        public void SetDefaultValue()
        {
            lastRefreshTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Local).ToLongDateString();
            //int id = 10010;
            //int time = TDMainTaskTable.GetMainTaskItemInfo(id).taskTime;
            //taskList.Add(new CommonTaskItemData(id, SimGameTaskType.Collect, (int)CollectedObjType.WuWood, TaskState.NotStart, time));
        }

        public void Init()
        {

        }

        public void SetRecordChracterID(int taskId,List<int> IDList)
        {
            CommonTaskItemData item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                item.SetRecordChracterID(IDList);
            }
        }

        public void SetTaskFinished(int taskId)
        {
            CommonTaskItemData item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                item.taskState = TaskState.Unclaimed;
                SetDataDirty();
            }
        }

        public void SetTaskExecutedTime(int taskId, int time)
        {
            CommonTaskItemData item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                item.SetTaskExecutedTime(time);
                SetDataDirty();
            }
        }

        public int GetTaskExecutedTime(int taskId)
        {
            CommonTaskItemData item = GetCommonTaskItemData(taskId);
            if (item != null)
            {
                return item.executedTime;
            }

            return 0;
        }

        public void AddTask(int taskId, SimGameTaskType taskType, int subType, TaskState taskState)
        {
            CommonTaskItemData mainTaskItem = GetCommonTaskItemData(taskId);
            if (mainTaskItem != null)
            {
                Log.e("Task already exists, taskId: " + taskId + " task subId: " + subType);
                return;
            }

            int time = TDCommonTaskTable.GetMainTaskItemInfo(taskId).taskTime;
            CommonTaskItemData item = new CommonTaskItemData(taskId, taskType, subType, taskState, time);
            taskList.Add(item);
        }

        public void OnTaskStarted(int taskId)
        {
            CommonTaskItemData mainTaskItem = GetCommonTaskItemData(taskId);
            mainTaskItem.taskState = TaskState.Running;
            SetDataDirty();
            EventSystem.S.Send(EventID.OnCommonTaskStart, taskId);
        }

        //public void OnTaskFinished(int taskId)
        //{
        //    CommonTaskItemData mainTaskItem = GetCommonTaskItemData(taskId);
        //    mainTaskItem.taskState = TaskState.Unclaimed;
        //    SetDataDirty();
        //}

        public void OnTaskRewardClaimed(int taskId)
        {
            CommonTaskItemData mainTaskItem = GetCommonTaskItemData(taskId);
            if (mainTaskItem != null)
            {
                taskList.Remove(mainTaskItem);
                // mainTaskItem.isRewardClaimed = true;
            }
            else
            {
                Log.e("OnTaskRewardClaimed, task not found in list : " + taskId);
            }

            SetDataDirty();
        }

        public CommonTaskItemData GetCommonTaskItemData(int taskId)
        {
            foreach (CommonTaskItemData item in taskList)
            {
                if (item.taskId == taskId)
                {
                    return item;
                }
            }

            return null;
        }

        public bool IsTaskExist(int taskId)
        {
            CommonTaskItemData task = GetCommonTaskItemData(taskId);
            return task != null;
        }

        public void RemoveTask(int taskId)
        {
            CommonTaskItemData item = GetCommonTaskItemData(taskId);

            if (item != null )
            {
                taskList.Remove(item);
            }

            SetDataDirty();
        }
    }

    [System.Serializable]
    public class CommonTaskItemData
    {
        public int taskId = 1;
        public SimGameTaskType taskType;
        public int taskSubType = 1;
        public TaskState taskState;
        public int taskTime; //总的时间
        public int executedTime; //已执行时间
        public List<int> recordCharacterID = new List<int>();
        public CommonTaskItemData()
        {
        }

        public CommonTaskItemData(int taskId, SimGameTaskType taskType, int subType, TaskState taskState, int taskTime)
        {
            this.taskId = taskId;
            this.taskType = taskType;
            this.taskSubType = subType;
            this.taskState = taskState;
            this.taskTime = taskTime;
        }

        public void SetTaskExecutedTime(int time)
        {
            this.executedTime = time;
        }

        public void SetRecordChracterID(List<int> iDList)
        {
            recordCharacterID = iDList;
        }
    }

}