using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public class MainTaskItemInfo
    {
        public int id;
        public SimGameTaskType taskType;
        public int subType;
        public int time;
        public string title;
        public string desc;
        public List<TaskReward> rewards = new List<TaskReward>();

        public MainTaskItemInfo(int id, SimGameTaskType taskType, int subType)
        {
            this.id = id;
            this.taskType = taskType;
            this.subType = subType;
            this.time = TDMainTaskTable.GetData(id).time;
            this.title = TDMainTaskTable.GetData(id).taskTitle;
            this.desc = TDMainTaskTable.GetData(id).taskDescription;

            ParseReward(TDMainTaskTable.GetData(id).reward);
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
    }

}