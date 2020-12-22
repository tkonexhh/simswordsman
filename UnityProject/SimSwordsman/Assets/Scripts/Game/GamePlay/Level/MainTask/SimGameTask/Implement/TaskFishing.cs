using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskFishing : SimGameTask
	{
        public TaskFishing(int taskId, SimGameTaskType taskType, int subType, string tableName, Action<TaskItem> stateChangedCallback) : base(taskId, taskType, subType, tableName, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i => 
            {
                i.CurTask = this;
                i.SetState(CharacterStateID.Fishing);
            });
        }
    }
	
}