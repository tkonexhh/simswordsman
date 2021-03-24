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
        public static Dictionary<int, MainTaskItemInfo> taskInfoDic = new Dictionary<int, MainTaskItemInfo>();

        static void CompleteRowAdd(TDMainTask tdData)
        {
            MainTaskItemInfo taskInfo = new MainTaskItemInfo(tdData);
            
            taskInfoDic.Add(tdData.taskID, taskInfo);
        }

        public static MainTaskItemInfo GetMainTaskItemInfo(int taskId)
        {
            if (taskInfoDic.ContainsKey(taskId))
            {
                return taskInfoDic[taskId];
            }

            return null;
        }

        //public static List<MainTaskItemInfo> GetAllDailyTaskByLobbyLevel(int lobbyLevel)
        //{
        //    List<MainTaskItemInfo> list = new List<MainTaskItemInfo>();

        //    list = taskInfoDic.Values.Where(i => i.triggerType == SimGameTaskTriggerType.Daily && i.needHomeLevel == lobbyLevel).ToList();
        //    return list;
        //}

        //public static List<MainTaskItemInfo> GetAllCommonTaskByLobbyLevel(int lobbyLevel)
        //{
        //    List<MainTaskItemInfo> list = new List<MainTaskItemInfo>();

        //    list = taskInfoDic.Values.Where(i => i.triggerType == SimGameTaskTriggerType.Common && i.needHomeLevel == lobbyLevel).ToList();
        //    return list;
        //}

        // 反射调用
        //public static string GetConditionType(int taskId)
        //{
        //    return GetData(taskId).conditionType;
        //}

        //// 反射调用
        //public static string GetConditionValue(int taskId)
        //{
        //    return GetData(taskId).conditionValue;
        //}

        //// 反射调用
        //public static string GetEvent(int taskId)
        //{
        //    return GetData(taskId).taskEvent;
        //}

        //// 反射调用
        //public static string GetReward(int taskId)
        //{
        //    return GetData(taskId).reward;
        //}

        //public static List<TDMainTask> GetAllTaskByLobbyLevel(int lobbyLevel)
        //{
        //    return m_DataList.Where(i => i.homeLevel == lobbyLevel).ToList();
        //}

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