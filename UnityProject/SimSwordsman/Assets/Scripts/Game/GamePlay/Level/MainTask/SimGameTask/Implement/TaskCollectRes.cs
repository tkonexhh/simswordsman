using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectRes : SimGameTask
	{
        public TaskCollectRes(int taskId, string tableName, TaskState taskState, int taskTime) : base(taskId, tableName, taskState, taskTime)
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