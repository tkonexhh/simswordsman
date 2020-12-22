using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskBattle : SimGameTask
	{
        public TaskBattle(int taskId, SimGameTaskType taskType, int subType, string tableName, Action<TaskItem> stateChangedCallback) : base(taskId, taskType, subType, tableName, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i => i.SetState(CharacterStateID.Fighting));
        }
    }
	
}