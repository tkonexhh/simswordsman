using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public abstract class TaskBase : ITask
    {
        private TaskState m_TaskState;
        private TaskInfo m_TaskInfo;

        public string taskTitle => m_TaskInfo.taskTitle;
        public string taskSubTitle => m_TaskInfo.taskSubTitle;

        public TaskBase(TaskInfo info)
        {
            m_TaskInfo = info;
        }

        public bool IsComplete()
        {
            if (m_TaskState == TaskState.Finished)
                return false;


            return m_TaskInfo.IsComplete();
        }


        public void GetReward()
        {
            if (m_TaskState != TaskState.Unclaimed)
                return;

            if (m_TaskInfo.Reward == null)
                return;

            m_TaskState = TaskState.Finished;
            m_TaskInfo.Reward.AcceptReward();
        }

    }

    public class Task : TaskBase
    {
        public Task(TaskInfo info) : base(info) { }
    }




}