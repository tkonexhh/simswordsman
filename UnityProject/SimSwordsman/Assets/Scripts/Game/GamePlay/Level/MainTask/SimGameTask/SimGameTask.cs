using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public abstract class SimGameTask
	{
        protected int m_TaskId;
        protected CommonTaskItemInfo m_TaskDetailInfo = null;
        //protected string m_TaskStartTime = string.Empty;

        public CommonTaskItemInfo CommonTaskItemInfo { get { return m_TaskDetailInfo; } }

        //public string TaskStartTime { get => m_TaskStartTime;}
        public int TaskId { get => m_TaskId; }

        public List<int> CharacterIDs = new List<int>();

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

        //public void SetCurTaskFinished()
        //{
        //     m_TaskDetailInfo.taskState = TaskState.Finished;
        //}

        //public int GetCurSubType()
        //{
        //   return CommonTaskItemInfo.subType;
        //}

        public SimGameTaskType GetCurTaskType()
        {
            return CommonTaskItemInfo.taskType;
        }

        public virtual void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            foreach (var item in selectedCharacters)
                CharacterIDs.Add(item.CharacterId);

            GameDataMgr.S.GetCommonTaskData().OnTaskStarted(TaskId);
        }

        public virtual void OnUpdateTask()
        {

        }

        public void ClaimReward()
        {
            CharacterIDs.Clear();
            // Item reward
            for (int i = 0; i < m_TaskDetailInfo.itemRewards.Count; i++)
            {
                int itemId = m_TaskDetailInfo.GetRewardId(i);
                int count = m_TaskDetailInfo.GetRewardValue(i);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
            }

            // Special reward
            int random = Random.Range(0, 10000);
            if (random < m_TaskDetailInfo.specialRewardRate)
            {
                for (int i = 0; i < m_TaskDetailInfo.specialRewards.Count; i++)
                {
                    int itemId = m_TaskDetailInfo.GetSpecialRewardId(i);
                    int count = m_TaskDetailInfo.GetSpecialRewardValue(i);
                    MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);
                }
            }
        }
    }
	
}