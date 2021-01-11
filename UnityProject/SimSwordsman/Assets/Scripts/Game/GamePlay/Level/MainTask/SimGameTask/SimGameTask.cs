using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public abstract class SimGameTask
	{
        protected int m_TaskId;
        protected CommonTaskItemInfo m_TaskDetailInfo = null;
        protected string m_TaskStartTime = string.Empty;

        public CommonTaskItemInfo CommonTaskItemInfo { get { return m_TaskDetailInfo; } }

        public string TaskStartTime { get => m_TaskStartTime;}
        public int TaskId { get => m_TaskId; }

        public SimGameTask(int taskId, string tableName, TaskState taskState, int taskTime)
        {
            m_TaskId = taskId;

            m_TaskDetailInfo = TDCommonTaskTable.GetMainTaskItemInfo(taskId);
            if (m_TaskDetailInfo == null)
            {
                Debug.LogError("Task info not found, id: " + taskId);
            }

            m_TaskDetailInfo.taskState = taskState;
            m_TaskDetailInfo.taskTime = taskTime;
        }

        public TaskState GetCurTaskState()
        {
            return m_TaskDetailInfo.taskState;
        }

        public void SetCurTaskFinished()
        {
             m_TaskDetailInfo.taskState = TaskState.Finished;
        }

        public int GetCurSubType()
        {
           return CommonTaskItemInfo.subType;
        }

        public SimGameTaskType GetCurTaskType()
        {
            return CommonTaskItemInfo.taskType;
        }

        public abstract void ExecuteTask(List<CharacterController> selectedCharacters);

        public void ClaimReward()
        {
            // Item reward
            for (int i = 0; i < m_TaskDetailInfo.itemRewards.Count; i++)
            {
                int itemId = m_TaskDetailInfo.GetRewardId(i);
                int count = m_TaskDetailInfo.GetRewardValue(i);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
            }

            // TODO:Exp reward

            // TODO:Kongfu reward
        }
    }
	
}