using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class TaskBattle : SimGameTask
    {
        public TaskBattle(int taskId, string tableName, TaskState taskState, Action<TaskItem> stateChangedCallback) : base(taskId, tableName, taskState, stateChangedCallback)
        {
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            selectedCharacters.ForEach(i =>
            {
                i.SetState(CharacterStateID.GoOutside);
                i.SetDataState(CharacterStateID.GoOutside, (RawMaterial)GetCurSubType());
            });
        }
    }

}