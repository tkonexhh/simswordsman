using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public abstract class TaskBase : ITask
    {
        private TaskState m_TaskState;
        private TaskInfo m_TaskInfo;

        public string taskTitle => m_TaskInfo.taskTitle;
        public string taskSubTitle => m_TaskInfo.taskSubTitle;
        public int id => m_TaskInfo.id;
        public TaskState taskState => m_TaskState;
        public RewardBase reward => m_TaskInfo.Reward;
        public string taskIcon => m_TaskInfo.taskIcon;

        public TaskBase(TaskInfo info)
        {
            m_TaskInfo = info;
            if (GameDataMgr.S.GetPlayerData().taskData.dailyTaskData.HasCompleteID(id))
            {
                m_TaskState = TaskState.Finished;
            }
            else if (IsComplete())
            {
                m_TaskState = TaskState.Unclaimed;
            }
            else
            {
                m_TaskState = TaskState.Running;
            }

        }

        public bool IsComplete()
        {
            if (m_TaskState == TaskState.Finished)
                return false;


            bool isComplete = m_TaskInfo.IsComplete();
            if (isComplete)
                m_TaskState = TaskState.Unclaimed;
            return isComplete;
        }



        public void GetReward()
        {
            if (m_TaskState != TaskState.Unclaimed)
                return;

            if (m_TaskInfo.Reward == null)
                return;

            m_TaskState = TaskState.Finished;
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(reward);
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            m_TaskInfo.Reward.AcceptReward();
        }

    }

    public class Task : TaskBase
    {
        public Task(TaskInfo info) : base(info) { }
    }




}