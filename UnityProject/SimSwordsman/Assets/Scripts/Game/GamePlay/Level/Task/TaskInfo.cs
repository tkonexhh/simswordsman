using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class TaskInfo
    {
        private string m_TaskTitle;
        private RewardBase m_Reward;
        private TaskHandler m_TaskHandler;
        private int m_Target;
        private int m_ID;
        private string m_TaskIcon;

        public int id => m_ID;
        public RewardBase Reward => m_Reward;
        public string taskTitle => m_TaskTitle;
        public string taskSubTitle => string.Format(m_TaskHandler.taskSubTitle + "({1}/{0})", m_Target, Mathf.Min(m_Target, m_TaskHandler.count));
        public string taskIcon => m_TaskIcon;


        //-----------Static Block----------//
        public delegate TaskHandler CreateTaskHandlerDelegate(int? value);
        private static Dictionary<string, CreateTaskHandlerDelegate> taskHandlerMap = new Dictionary<string, CreateTaskHandlerDelegate>();
        static TaskInfo()
        {
            taskHandlerMap.Add("Main_BuildLivableRoom", (value) => { return new TaskHandler_BuildLivableRoom(value.Value); });
            taskHandlerMap.Add("Main_OwnStudents", (value) => { return new TaskHandler_OwnStudents(); });
            taskHandlerMap.Add("Main_BuildPracticeField", (value) => { return new TaskHandler_BuildPracticeField(value.Value); });
            taskHandlerMap.Add("Main_BuildLobby", (value) => { return new TaskHandler_BuildLobby(); });
            taskHandlerMap.Add("Main_Chanllenge", (value) => { return new TaskHandler_Chanllenge(value.Value); });
            taskHandlerMap.Add("Main_BuildLibrary", (value) => { return new TaskHandler_BuildLibrary(value.Value); });
            taskHandlerMap.Add("Main_BuildForgeHouse", (value) => { return new TaskHandler_BuildForgeHouse(value.Value); });
            taskHandlerMap.Add("Main_BuildBaicaohu", (value) => { return new TaskHandler_BuildBaicaohu(value.Value); });
            //Daily
            taskHandlerMap.Add("Daily_Food", (value) => { return new TaskHandler_DailyFood(); });
            taskHandlerMap.Add("Daily_Visitor", (value) => { return new TaskHandler_DailyVisitor(); });
            taskHandlerMap.Add("Daily_Recruit", (value) => { return new TaskHandler_DailyRecruit(); });
            taskHandlerMap.Add("Daily_Job", (value) => { return new TaskHandler_DailyJob(); });
            taskHandlerMap.Add("Daily_Practice", (value) => { return new TaskHandler_DailyPractice(); });
            taskHandlerMap.Add("Daily_Copy", (value) => { return new TaskHandler_DailyCopy(); });
            taskHandlerMap.Add("Daily_Cook", (value) => { return new TaskHandler_DailyCook(); });
            taskHandlerMap.Add("Daily_Collect", (value) => { return new TaskHandler_DailyCollect(); });
            taskHandlerMap.Add("Daily_Chanllenge", (value) => { return new TaskHandler_DailyChanllenge(); });
            taskHandlerMap.Add("Daily_Forge", (value) => { return new TaskHandler_DailyForge(); });
            taskHandlerMap.Add("Daily_Medicine", (value) => { return new TaskHandler_DailyMedicine(); });
        }
        private static TaskHandler CreateTaskHandler(string taskType)
        {
            var taskInfos = Helper.String2ListString(taskType, "|");
            string taskMainStr = taskInfos[0];
            int? subID = null;
            if (taskInfos.Count > 1)
            {
                subID = int.Parse(taskInfos[1]);
            }
            CreateTaskHandlerDelegate createTaskHandlerDelegate;
            taskHandlerMap.TryGetValue(taskMainStr, out createTaskHandlerDelegate);
            if (createTaskHandlerDelegate != null)
            {
                if (!subID.HasValue)
                    subID = 1;
                return createTaskHandlerDelegate(subID);
            }

            throw new ArgumentException(taskType);
        }
        //-----------Static Block----------//


        public TaskInfo(TDMainTask mainTask)
        {
            m_ID = mainTask.taskID;
            m_Reward = RewardMgr.S.GetRewardBase(mainTask.reward);
            m_Target = mainTask.countP;
            m_TaskHandler = CreateTaskHandler(mainTask.type);
            m_TaskTitle = m_TaskHandler.taskTitle;
        }

        public TaskInfo(TDDailyTask dailyTask)
        {
            m_ID = dailyTask.taskID;
            m_Reward = RewardMgr.S.GetRewardBase(dailyTask.reward);
            m_Target = dailyTask.taskCount;
            m_TaskHandler = CreateTaskHandler(dailyTask.type);
            m_TaskTitle = dailyTask.taskTitle;
            m_TaskIcon = dailyTask.iconRes;
        }

        public bool IsComplete()
        {
            return m_TaskHandler.count >= m_Target;
        }



    }



}