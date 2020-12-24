using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class SimGameTaskFactory
	{
        private static string m_MainTaskTableName = "TDMainTaskTable";

        public static SimGameTask SpawnTask(int taskId, SimGameTaskType taskType, int subType,TaskState taskState)
        {
            SimGameTask task = null;

            switch (taskType)
            {
                case SimGameTaskType.Fish:
                    task = new TaskFishing(taskId, SimGameTaskType.Fish, 1, m_MainTaskTableName, taskState, MainGameMgr.S.MainTaskMgr.OnTaskStateChanged);
                    break;
                case SimGameTaskType.Hunt:
                    task = new TaskHunting(taskId, SimGameTaskType.Fish, subType, m_MainTaskTableName, taskState, MainGameMgr.S.MainTaskMgr.OnTaskStateChanged);
                    break;
                case SimGameTaskType.Battle:
                    task = new TaskBattle(taskId, SimGameTaskType.Fish, subType, m_MainTaskTableName, taskState, MainGameMgr.S.MainTaskMgr.OnTaskStateChanged);
                    break;
            }

            return task;
        }
	}
	
}