using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDMainTaskTable
    {
        //public static Dictionary<int, TaskDetailInfo> taskInfoDic = new Dictionary<int, TaskDetailInfo>();

        static void CompleteRowAdd(TDMainTask tdData)
        {
            //TaskDetailInfo taskDetail = new TaskDetailInfo();
            //taskDetail.taskId = tdData.taskID;
            //taskDetail.taskTitle = tdData.taskTitle;
            //taskDetail.taskDesc = tdData.taskDescription;
            //taskDetail.lobbyLevel = tdData.homeLevel;

            //taskInfoDic.Add(tdData.taskID, taskDetail);
        }

        public static string GetConditionType(int taskId)
        {
            return GetData(taskId).conditionType;
        }

        public static string GetConditionValue(int taskId)
        {
            return GetData(taskId).conditionValue;
        }

        public static string GetEvent(int taskId)
        {
            return GetData(taskId).taskEvent;
        }

        public static string GetReward(int taskId)
        {
            return GetData(taskId).reward;
        }

        public static List<TDMainTask> GetAllTaskByLobbyLevel(int lobbyLevel)
        {
            return m_DataList.Where(i => i.homeLevel == lobbyLevel).ToList();
        }

        //public static TaskDetailInfo GetTaskDetailInfo(int taskId)
        //{
        //    if (taskInfoDic.ContainsKey(taskId))
        //    {
        //        return taskInfoDic[taskId];
        //    }

        //    Log.e("Task id not found: " + taskId);
        //    return null;
        //}
    }
}