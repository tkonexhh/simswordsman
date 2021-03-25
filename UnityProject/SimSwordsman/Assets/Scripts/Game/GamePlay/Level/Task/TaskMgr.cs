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
        public void OnDestroyed() { }
    }

    public abstract class TaskBaseController
    {

        public abstract void Init();
    }

    public class TaskMainController : TaskBaseController
    {
        public override void Init()
        {

        }
    }

    public class TaskDailyController : TaskBaseController
    {
        private List<Task> m_TaskLst = new List<Task>();
        public int count => m_TaskLst.Count;

        public override void Init()
        {
            var tasks = TDDailyTaskTable.dataList;
            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = new Task(new TaskInfo(tasks[i]));
                m_TaskLst.Add(task);
            }
        }

        public Task this[int index]
        {
            get => m_TaskLst[index];
        }


    }
}