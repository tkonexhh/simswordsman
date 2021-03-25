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
        public override void Init()
        {

        }
    }

}