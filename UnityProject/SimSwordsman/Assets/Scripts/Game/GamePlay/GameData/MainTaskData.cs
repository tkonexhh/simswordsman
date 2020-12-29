using System;
using Qarth;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
    public class MainTaskData : DataDirtyHandler
    {
        public List<MainTaskItemData> taskList = new List<MainTaskItemData>();

        public void SetDefaultValue()
        {
            taskList.Add(new MainTaskItemData(10010, SimGameTaskType.Collect, (int)CollectedObjType.WuWood, TaskState.NotStart));
        }

        public void Init()
        {

        }

        public void SetTaskFinished(int taskId)
        {
            MainTaskItemData item = GetMainTaskItemData(taskId);
            if (item != null)
            {
                item.taskState = TaskState.Unclaimed;
                SetDataDirty();
            }
        }

        public void AddTask(int taskId, SimGameTaskType taskType, int subType, TaskState taskState)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
            if (mainTaskItem != null)
            {
                Log.e("Task already exists, taskId: " + taskId + " task subId: " + subType);
                return;
            }

            MainTaskItemData item = new MainTaskItemData(taskId, taskType, subType, taskState);
            taskList.Add(item);
        }

        public void OnTaskFinished(int taskId)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
            mainTaskItem.taskState = TaskState.Unclaimed;
            SetDataDirty();
        }

        public void OnTaskRewardClaimed(int taskId)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
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

        public MainTaskItemData GetMainTaskItemData(int taskId)
        {
            foreach (MainTaskItemData item in taskList)
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
            MainTaskItemData task = GetMainTaskItemData(taskId);
            return task != null;
        }

        public void RemoveTask(int taskId)
        {
            MainTaskItemData item = GetMainTaskItemData(taskId);

            if (item != null )
            {
                taskList.Remove(item);
            }

            SetDataDirty();
        }
    }

    [System.Serializable]
    public class MainTaskItemData
    {
        public int taskId = 1;
        public SimGameTaskType taskType;
        public int taskSubType = 1;
        public TaskState taskState;

        public MainTaskItemData()
        {
        }

        public MainTaskItemData(int taskId, SimGameTaskType taskType, int subType, TaskState taskState)
        {
            this.taskId = taskId;
            this.taskType = taskType;
            this.taskSubType = subType;
            this.taskState = taskState;
        }
    }

}