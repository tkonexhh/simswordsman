using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TaskMgr : MonoBehaviour, IMgr
    {
        private TaskMainController m_MainController;
        private TaskDailyController m_DailyController;

        public TaskMainController mainTaskController => m_MainController;
        public TaskDailyController dailyTaskController => m_DailyController;

        public void OnInit()
        {
            m_MainController = new TaskMainController();
            m_DailyController = new TaskDailyController();
            m_MainController.Init();
            m_DailyController.Init();
        }
        public void OnUpdate() { }
        public void OnDestroyed()
        {
            m_MainController.Destroy();
            m_DailyController.Destroy();
        }
    }

    public abstract class TaskBaseController
    {
        public abstract void Init();
        public abstract void Destroy();
    }

    public class TaskMainController : TaskBaseController
    {
        private Task m_CurTask;


        public Task curTask => m_CurTask;

        public override void Init()
        {
            RefeshCurTask();
        }

        public override void Destroy()
        {

        }

        private void RefeshCurTask()
        {
            int index = GameDataMgr.S.GetPlayerData().taskData.mainTaskData.curIndex;
            var mainTaskConf = TDMainTaskTable.GetData(index);
            if (mainTaskConf != null)
            {
                m_CurTask = new Task(new TaskInfo(mainTaskConf));
            }
        }


        public void FinishCurTask()
        {
            if (m_CurTask == null)
                return;

            m_CurTask.GetReward();
            m_CurTask = null;
            GameDataMgr.S.GetPlayerData().taskData.mainTaskData.FinishMainTask();

            RefeshCurTask();
            EventSystem.S.Send(EventID.OnRefeshMainTask);
        }
    }

    public class TaskDailyController : TaskBaseController
    {
        private List<Task> m_TaskLst = new List<Task>();
        public int count => m_TaskLst.Count;

        public override void Init()
        {
            RefeshTaskLst();
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnRefeshDailyTaskPanel, HandleEvent);
            EventSystem.S.Register(EventID.OnRefeshDailyTask, CheckDailyTask);
        }

        public override void Destroy()
        {
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnRefeshDailyTaskPanel, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnRefeshDailyTask, CheckDailyTask);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                FacilityType facilityType = (FacilityType)args[0];
                if (facilityType == FacilityType.Lobby)
                {
                    RefeshTaskLst();
                }
            }
            else
            {
                RefeshTaskLst();
            }

            FirstCheck();
        }

        public void FirstCheck()
        {
            for (int i = 0; i < m_TaskLst.Count; i++)
            {
                m_TaskLst[i].IsComplete();
                if (m_TaskLst[i].taskState == TaskState.Unclaimed)
                {
                    EventSystem.S.Send(EventID.OnMainMenuDailyTaskRedPoint);
                    break;
                }
            }
        }

        private void CheckDailyTask(int key, params object[] args)
        {
            for (int i = 0; i < m_TaskLst.Count; i++)
            {
                var oldState = m_TaskLst[i].taskState;
                m_TaskLst[i].IsComplete();
                var newState = m_TaskLst[i].taskState;
                if (oldState == TaskState.Running && newState == TaskState.Unclaimed)
                {
                    //完成了一个新任务
                    DataAnalysisMgr.S.CustomEvent(DotDefine.d_task_finish);
                    EventSystem.S.Send(EventID.OnMainMenuDailyTaskRedPoint);
                }
            }
        }

        public void RefeshTaskLst()
        {
            var tasks = TDDailyTaskTable.GetDailyTasksByLvl(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());
            m_TaskLst.Clear();
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = new Task(new TaskInfo(tasks[i]));
                m_TaskLst.Add(task);
            }
            m_TaskLst.Sort((x, y) =>
            {
                if (x.taskState > y.taskState)
                {
                    return 1;
                }
                else if (x.taskState == y.taskState)
                    return 0;
                else
                    return -1;
            });
        }

        public Task this[int index]
        {
            get => m_TaskLst[index];
        }

        public void FinishTask(Task task)
        {
            //存档
            if (GameDataMgr.S.GetPlayerData().taskData.dailyTaskData.AddCompleteID(task.id))
            {
                task.GetReward();
                DataAnalysisMgr.S.CustomEvent(DotDefine.d_task_receive, task.id.ToString());
                EventSystem.S.Send(EventID.OnRefeshDailyTaskPanel);
            }
        }


    }
}