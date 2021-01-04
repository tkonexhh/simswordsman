using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskCollectRes : SimGameTask
	{
        public TaskCollectRes(int taskId, string tableName, TaskState taskState, Action<TaskItem> stateChangedCallback) : base(taskId, tableName, taskState, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i => 
            {
                i.CurTask = this;
                i.SetState(CharacterStateID.CollectRes);
                i.SetDataState(CharacterStateID.CollectRes, (CollectedObjType)GetCurSubType());
            });
        }
    }
	
}