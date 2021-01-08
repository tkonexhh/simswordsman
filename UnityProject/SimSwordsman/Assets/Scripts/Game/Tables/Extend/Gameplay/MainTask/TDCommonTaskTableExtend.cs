using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDCommonTaskTable
    {
        public static Dictionary<int, CommonTaskItemInfo> taskInfoDic = new Dictionary<int, CommonTaskItemInfo>();

        static void CompleteRowAdd(TDCommonTask tdData)
        {
            CommonTaskItemInfo taskInfo = new CommonTaskItemInfo(tdData);

            taskInfoDic.Add(tdData.taskID, taskInfo);
        }

        public static CommonTaskItemInfo GetMainTaskItemInfo(int taskId)
        {
            if (taskInfoDic.ContainsKey(taskId))
            {
                return taskInfoDic[taskId];
            }

            return null;
        }


        //public static List<CommonTaskItemInfo> GetAllCommonTaskByLobbyLevel(int lobbyLevel)
        //{
        //    List<CommonTaskItemInfo> list = new List<CommonTaskItemInfo>();

        //    list = taskInfoDic.Values.Where(i => i.triggerType == SimGameTaskTriggerType.Common && i.needHomeLevel == lobbyLevel).ToList();
        //    return list;
        //}

        //// 反射调用
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

        public static List<TDCommonTask> GetAllTaskByLobbyLevel(int lobbyLevel)
        {
            return m_DataList.Where(i => i.homeLevel == lobbyLevel).ToList();
        }
    }
}