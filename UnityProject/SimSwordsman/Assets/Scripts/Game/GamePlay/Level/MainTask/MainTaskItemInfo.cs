using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public enum TaskState
    {
        None,
        /// <summary>
        /// 未开始
        /// </summary>
        NotStart,
        /// <summary>
        /// 待领取
        /// </summary>
        Unclaimed,
        /// <summary>
        /// 已结束
        /// </summary>
        Finished,            
    }

    public class MainTaskItemInfo
    {
        public int id;
        public SimGameTaskTriggerType triggerType;
        public List<int> nextTaskIdList = new List<int>();
        public int needHomeLevel = -1;
        public SimGameTaskType taskType;
        public int subType;
        public int time;
        public string title;
        public string desc;
        public TaskState taskState;
        public List<TaskReward> rewards = new List<TaskReward>();
        public int specialRewardRate = 0;
        public List<TaskReward> specialRewards = new List<TaskReward>();
        public int characterAmount = 1;
        public int characterLevelRequired = 1;

        public MainTaskItemInfo(TDMainTask tDMainTask)
        {
            this.id = tDMainTask.taskID;
            this.triggerType = EnumUtil.ConvertStringToEnum<SimGameTaskTriggerType>(tDMainTask.triggerType);

            taskState = TaskState.None;

            this.time = tDMainTask.time;
            this.title = tDMainTask.taskTitle;
            this.desc = tDMainTask.taskDescription;
            this.needHomeLevel = tDMainTask.homeLevel;
            this.specialRewardRate = tDMainTask.specialRewardRate;
            this.characterAmount = tDMainTask.roleAmount;
            this.characterLevelRequired = tDMainTask.roleLevelRequired;

            ParseReward(tDMainTask.reward);
            ParseNextLevel(tDMainTask.nextTask);
            ParseTaskType(tDMainTask.type);
            ParseSpecialReward(tDMainTask.specialReward);
        }

        public int GetRewardId(int index)
        {
            if (index < 0 || index > rewards.Count - 1)
            {
                Log.e("Reward index out of range");
                return 0;
            }

            TaskReward reward = rewards[index];
            return reward.id;
        }

        public int GetRewardValue(int index)
        {
            if (index < 0 || index > rewards.Count - 1)
            {
                Log.e("Reward index out of range");
                return 0;
            }

            TaskReward reward = rewards[index];
            if (reward.count2 != -1)
            {
                return Random.Range(reward.count1, reward.count2);
            }

            return reward.count1;
        }
        //public MainTaskItemInfo(int id, SimGameTaskType taskType, int subType, TaskState _taskState)
        //{
        //    this.id = id;
        //    this.taskType = taskType;
        //    this.subType = subType;
        //    taskState = _taskState;

        //    TDMainTask tDMain = TDMainTaskTable.GetData(id);
        //    this.time = tDMain.time;
        //    this.title = tDMain.taskTitle;
        //    this.desc = tDMain.taskDescription;
        //    ParseReward(tDMain.reward);
        //    ParseNextLevel(tDMain.nextTask);
        //}

        private void ParseReward(string reward)
        {
            string[] rewardStrs = reward.Split(';');
            foreach (string item in rewardStrs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    TaskReward taskReward = new TaskReward(item);
                    this.rewards.Add(taskReward);
                }
            }
        }

        private void ParseSpecialReward(string reward)
        {
            if (string.IsNullOrEmpty(reward))
                return;

            string[] rewardStrs = reward.Split(';');
            foreach (string item in rewardStrs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    TaskReward taskReward = new TaskReward(item);
                    this.rewards.Add(taskReward);
                }
            }
        }

        private void ParseNextLevel(string nextTask)
        {
            if (!string.IsNullOrEmpty(nextTask))
            {
                string[] strs = nextTask.Split(';');
                foreach (string str in strs)
                {
                    int nextTaskId = int.Parse(str);
                    nextTaskIdList.Add(nextTaskId);
                }
            }
        }

        private void ParseTaskType(string taskTypeStr)
        {
            if (!string.IsNullOrEmpty(taskTypeStr))
            {
                string type = taskTypeStr;
                string[] strs = type.Split('_');

                SimGameTaskType taskType = EnumUtil.ConvertStringToEnum<SimGameTaskType>(strs[0]);
                this.taskType = taskType;

                if (taskType == SimGameTaskType.Collect)
                {
                    CollectedObjType collectedObjType = EnumUtil.ConvertStringToEnum<CollectedObjType>(strs[1]);
                    this.subType = (int)collectedObjType;
                }
            }
        }
    }

}