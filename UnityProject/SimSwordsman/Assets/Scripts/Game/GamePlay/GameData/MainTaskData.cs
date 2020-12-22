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

        }

        public void Init()
        {

        }

        public void SetTaskFinished(int taskId)
        {
            MainTaskItemData item = GetMainTaskItemData(taskId);
            if (item != null)
            {
                item.isFinished = true;

                SetDataDirty();
            }
        }

        public void AddTask(int taskId, SimGameTaskType taskType, int subType)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
            if (mainTaskItem != null)
            {
                Log.e("Task already exists, taskId: " + taskId + " task subId: " + subType);
                return;
            }

            MainTaskItemData item = new MainTaskItemData(taskId, taskType, subType);
            taskList.Add(item);
        }

        public void OnTaskFinished(int taskId)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
            mainTaskItem.isFinished = true;

            SetDataDirty();
        }

        public void OnTaskRewardClaimed(int taskId)
        {
            MainTaskItemData mainTaskItem = GetMainTaskItemData(taskId);
            if (mainTaskItem != null)
            {
                mainTaskItem.isRewardClaimed = true;
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
    }

    [System.Serializable]
    public class MainTaskItemData
    {
        public int taskId = 1;
        public SimGameTaskType taskType;
        public int taskSubType = 1;
        public bool isFinished = false;
        public bool isRewardClaimed = false;

        public MainTaskItemData()
        {
        }

        public MainTaskItemData(int taskId, SimGameTaskType taskType, int subType)
        {
            this.taskId = taskId;
            this.taskType = taskType;
            this.taskSubType = subType;
        }
    }

}