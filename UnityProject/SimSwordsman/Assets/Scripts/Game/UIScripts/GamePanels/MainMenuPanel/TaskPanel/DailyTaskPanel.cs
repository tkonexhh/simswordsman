using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class DailyTaskPanel : AbstractAnimPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private IUListView m_ListView;

        private TaskDailyController m_TaskController;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            m_BtnClose.onClick.AddListener(HideSelfWithAnim);
            m_ListView.SetCellRenderer(OnCellRenderer);
            m_TaskController = MainGameMgr.S.TaskMgr.dailyTaskController;
            RegisterEvent(EventID.OnRefeshDailyTaskPanel, Refesh);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            m_TaskController.RefeshTaskLst();
            m_ListView.SetDataCount(m_TaskController.count);
        }

        protected override void OnPanelHideComplete()
        {
            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void OnCellRenderer(Transform root, int index)
        {
            root.GetComponent<DailyTaskItem>().SetTask(this, m_TaskController[index]);
        }

        private void Refesh(int key, params object[] args)
        {
            m_ListView.SetDataCount(m_TaskController.count);
        }
    }

}