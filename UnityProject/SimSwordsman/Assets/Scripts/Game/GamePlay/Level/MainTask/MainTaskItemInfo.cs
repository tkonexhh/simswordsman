using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;
using System;

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
        /// 正在执行
        /// </summary>
        Running,
        /// <summary>
        /// 已完成，待领取
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
        public int taskTime;
        public string title;
        public string desc;
        public TaskState taskState;
        public List<TaskReward> rewards = new List<TaskReward>();
        public int specialRewardRate = 0;
        public List<TaskReward> specialRewards = new List<TaskReward>();
        public int characterAmount = 1;
        public int characterLevelRequired = 1;
        public List<TaskEnemy> taskEnemies = new List<TaskEnemy>();

        public MainTaskItemInfo(TDMainTask tDMainTask)
        {
            try
            {
                this.id = tDMainTask.taskID;
                this.triggerType = EnumUtil.ConvertStringToEnum<SimGameTaskTriggerType>(tDMainTask.triggerType);

                taskState = TaskState.None;

                this.taskTime = tDMainTask.time;
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
                ParseSpecialReward(tDMainTask.specialReward);
                ParseTaskEnemy(tDMainTask.enemy);
            }
            catch (Exception e)
            {
                Log.e("Parse task error: " + e.Message);
            }
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
                return UnityEngine.Random.Range(reward.count1, reward.count2);
            }

            return reward.count1;
        }

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

        private void ParseTaskEnemy(string enemyStr)
        {
            if (string.IsNullOrEmpty(enemyStr))
                return;

            string[] enemyStrs = enemyStr.Split(';');
            foreach (string item in enemyStrs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] strs = item.Split('|');
                    this.taskEnemies.Add(new TaskEnemy(int.Parse(strs[0]), int.Parse(strs[1])));
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

    public class TaskEnemy
    {
        public int enemyId;
        public int enemyAtk;

        public TaskEnemy(int id, int atk)
        {
            enemyId = id;
            enemyAtk = atk;
        }
    }
}