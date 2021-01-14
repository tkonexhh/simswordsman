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
            try
            {
                base.ExecuteTask(selectedCharacters);

                // Spawn res
                CollectedObjType collectedObjType = (CollectedObjType)(m_TaskDetailInfo.subType);
                MainGameMgr.S.CommonTaskMgr.SpawnTaskCollectableItem(collectedObjType);

                // Set character state
                selectedCharacters.ForEach(i =>
                {
                    i.SetCurTask(this);
                    i.SetState(CharacterStateID.CollectRes);
                });
            }
            catch (Exception e)
            {
                Qarth.Log.e("Task collect res error: " + e.Message.ToString());
            }

        }
    }
	
}