using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public abstract class SimGameTask : TaskItem
	{
        protected MainTaskItemInfo m_TaskDetailInfo = null;

        public MainTaskItemInfo MainTaskItemInfo { get { return m_TaskDetailInfo; } }

        public SimGameTask(int taskId, SimGameTaskType taskType, int subTaskType, string tableName,TaskState taskState, System.Action<TaskItem> stateChangedCallback) : base(taskId, tableName, stateChangedCallback)
        {
            m_TaskDetailInfo = new MainTaskItemInfo(taskId, taskType, subTaskType, taskState);
        }

        public TaskState GetCurTaskState()
        {
            return m_TaskDetailInfo.taskState;
        }

        public void SetCurTaskFinished()
        {
             m_TaskDetailInfo.taskState = TaskState.Finished;
        }

        public abstract void ExecuteTask(List<CharacterController> selectedCharacters);

    }
	
}