using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectRes : SimGameTask
	{
        public TaskCollectRes(int taskId, string tableName, TaskState taskState, int taskTime, Action<TaskItem> stateChangedCallback) : base(taskId, tableName, taskState, taskTime, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i => 
            {
                i.SetCurTask(this);
                i.SetState(CharacterStateID.CollectRes);
            });
        }
    }
	
}