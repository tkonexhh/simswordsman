using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class SimGameTaskFactory
	{
        private static string m_MainTaskTableName = "TDMainTaskTable";

        public static SimGameTask SpawnTask(int taskId, SimGameTaskType taskType, TaskState taskState, int taskTime, List<int> recordCharacterID)
        {
            SimGameTask task = null;

            switch (taskType)
            {
                //case SimGameTaskType.Collect:
                //    //CollectedObjType collectedObjType = (CollectedObjType)subType;
                //    task = new TaskCollectRes(taskId, m_MainTaskTableName, taskState, taskTime);
                //    break;
                case SimGameTaskType.Battle:
                    task = new TaskBattle(taskId, m_MainTaskTableName, taskState, taskTime, recordCharacterID);
                    break;
            }
            return task;
        }
	}
	
}