using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public abstract class TaskBase : ITask
    {
        private TaskState m_TaskState;
        protected TaskInfo m_TaskInfo;

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

    public class Task : TaskBase//, IComparable<Task>
    {
        public Task(TaskInfo info) : base(info) { }

        public int CompareTo(Task other)
        {
            //返回值：1 -> 大于、 0 -> 等于、 -1 -> 小于
            if (other.taskState > other.taskState)
            {
                Debug.LogError(other);
                return 1;
            }
            else if (other.taskState == other.taskState)
                return 0;
            else
                return -1;
        }

        public void FocusTask()
        {
            var targetTransform = m_TaskInfo.taskTransform;
            if (targetTransform == null)
                return;

            WorldUIPanel.S.ShowHandTips(targetTransform);
            if (targetTransform.gameObject.layer != LayerDefine.LAYER_UI)
            {
                MainGameMgr.S.MainCamera.LookAtTarget(targetTransform);
            }

        }

    }




}