using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskHunting : SimGameTask
	{
        public TaskHunting(int taskId, SimGameTaskType taskType, int subType, string tableName,TaskState taskState, Action<TaskItem> stateChangedCallback) : base(taskId, taskType, subType, tableName, taskState, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i =>
            {
                i.CurTask = this;
                i.SetState(CharacterStateID.Hunting);
            });
        }
    }
	
}