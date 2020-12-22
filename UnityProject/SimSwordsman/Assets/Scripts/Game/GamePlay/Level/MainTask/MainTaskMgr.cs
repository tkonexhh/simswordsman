using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public class MainTaskMgr : MonoBehaviour, IMgr
    {
        private string m_MainTaskTableName = "TDMainTaskTable";

        private MainTaskData m_MainTaskData;

        private List<SimGameTask> m_CurTaskList = new List<SimGameTask>();

        //private List<MainTaskItemInfo> m_CurTaskInfoList = new List<MainTaskItemInfo>();

        private List<IMainTaskObserver> m_MainTaskObserverList = new List<IMainTaskObserver>();

        public List<SimGameTask> CurTaskList { get => m_CurTaskList; }

        #region IMgr
        public void OnInit()
        {
            m_MainTaskData = GameDataMgr.S.GetMainTaskData();

            InitTaskList();
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

        }

        #endregion

        #region Public 

        public List<MainTaskItemInfo> GetCurTaskItemInfoList()
        {
            List<MainTaskItemInfo> list = new List<MainTaskItemInfo>();
            m_CurTaskList.ForEach( i => list.Add(i.MainTaskItemInfo));

            return list;
        }
        /// <summary>
        /// 完成任务后领取奖励
        /// </summary>
        public void ClaimReward(MainTaskItemData item)
        {
            GameDataMgr.S.GetMainTaskData().OnTaskRewardClaimed(item.taskId);

            SimGameTask taskItem = m_CurTaskList.FirstOrDefault(i => i.GetId() == item.taskId );
            if (taskItem != null)
            {
                taskItem.ClaimReward();
                m_CurTaskList.Remove(taskItem);

                //MainTaskItemInfo itemInfo = m_CurTaskInfoList.FirstOrDefault(i => i.id == taskItem.GetId() && i.subId == taskItem.GetSubId());
                //if (itemInfo != null)
                //{
                //    m_CurTaskInfoList.Remove(itemInfo);
                //}
            }
        }

        public void AddMainTaskObserver(IMainTaskObserver ob)
        {
            if (!m_MainTaskObserverList.Contains(ob))
            {
                m_MainTaskObserverList.Add(ob);
            }
        }

        public void RemoveMainTaskObserver(IMainTaskObserver ob)
        {
            if (m_MainTaskObserverList.Contains(ob))
            {
                m_MainTaskObserverList.Remove(ob);
            }
        }

        public void OnTaskStateChanged(TaskItem taskItem)
        {
            NotifyObserver(taskItem);

            bool isFinished = taskItem.IsFinished();

            if (isFinished)
            {
                GameDataMgr.S.GetMainTaskData().OnTaskFinished(taskItem.GetId());
            }
        }

        #endregion

        #region Private

        private void InitTaskList()
        {
            if (m_MainTaskData.taskList.Count == 0)
            {
                int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);
                List<TDMainTask> allTask = TDMainTaskTable.GetAllTaskByLobbyLevel(lobbyLevel);
                for (int i = 0; i < allTask.Count; i++)
                {
                    int taskId = allTask[i].taskID;

                    GenerateTask(taskId);
                }
            }
            else
            {
                m_MainTaskData.taskList.ForEach(i => 
                {
                    AddTask(i.taskId, i.taskType, i.taskSubType);
                });

            }
        }

        private void GenerateTask(int taskId)
        {
            string type = TDMainTaskTable.GetData(taskId).type;
            string[] strs = type.Split('_');

            SimGameTaskType taskType = EnumUtil.ConvertStringToEnum<SimGameTaskType>(strs[0]);
            int subType = 1;
            if (taskType == SimGameTaskType.Hunt)
            {
                HuntTaskSubType huntTaskSubType = EnumUtil.ConvertStringToEnum<HuntTaskSubType>(strs[1]);
                subType = (int)huntTaskSubType;
            }

            AddTask(taskId, taskType, subType);

            m_MainTaskData.AddTask(taskId, taskType, subType);
        }

        private void NotifyObserver(TaskItem task)
        {
            foreach (IMainTaskObserver ob in m_MainTaskObserverList)
            {
                ob.OnTaskStateChanged(task);
            }
        }

        private void AddTask(int taskId, SimGameTaskType taskType, int subType)
        {
            //TaskItem task = new TaskItem(id, subId, m_MainTaskTableName, OnTaskStateChanged);
            SimGameTask simGameTask = SimGameTaskFactory.SpawnTask(taskId, taskType, subType);
            m_CurTaskList.Add(simGameTask);

            //MainTaskItemInfo itemInfo = new MainTaskItemInfo(task.GetId(), task.GetSubId());
            //m_CurTaskInfoList.Add(itemInfo);
        }

        #endregion

    }

}