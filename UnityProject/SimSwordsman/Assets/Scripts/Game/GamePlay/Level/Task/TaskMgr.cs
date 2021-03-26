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
        public override void Init()
        {

        }

        public override void Destroy()
        {

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
        }

        public override void Destroy()
        {
            EventSystem.S.UnRegister(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnRefeshDailyTaskPanel, HandleEvent);
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
        }

        private void RefeshTaskLst()
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


    }
}