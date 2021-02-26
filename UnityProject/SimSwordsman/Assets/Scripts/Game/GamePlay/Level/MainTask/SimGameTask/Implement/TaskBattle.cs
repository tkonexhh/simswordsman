using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class TaskBattle : SimGameTask
    {
        public TaskBattle(int taskId, string tableName, TaskState taskState, int taskTime, List<int> recordCharacterID) : base(taskId, tableName, taskState, taskTime, recordCharacterID)
        {
          
        }

        public override void ExecuteTask(List<CharacterController> selectedCharacters)
        {
            base.ExecuteTask(selectedCharacters);

            selectedCharacters.ForEach(i =>
            {
                i.SetCurTask(this);
                //i.SetState(CharacterStateID.GoOutsideForTaskBattle);
            });
        }
    }

}