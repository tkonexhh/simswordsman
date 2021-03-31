using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BulletinBoardPanel : AbstractAnimPanel
    {
        [Header("Top")]
        [SerializeField]
        private Button m_CloseBtn;

        [Header("Middle")]
        [SerializeField]
        private Text m_BulletinCont;

        [Header("Bottom")]
        [SerializeField]
        private Transform m_TaskContParent;
        [SerializeField]
        private GameObject m_BulletinBoardtem;
        [SerializeField]
        private GameObject m_NoTaskImg;

        private List<SimGameTask> m_CommonTaskList = null;

        private Dictionary<int, GameObject> m_TaskObjDic = new Dictionary<int, GameObject>();
        protected override void OnUIInit()
        {
            base.OnUIInit();
            GetInformationForNeed();

            BindAddListenerEvent();
        }

        private void RefreshDiscipleInfo()
        {
            throw new NotImplementedException();
        }

        private void GetInformationForNeed()
        {
            m_CommonTaskList = MainGameMgr.S.CommonTaskMgr.CurTaskList;
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            EventSystem.S.Register(EventID.OnTaskManualFinished, HandleAddListenerEvent);

            MainGameMgr.S.CommonTaskMgr.RefreshTask();

            if (m_CommonTaskList != null)
            {
                for (int i = 0; i < m_CommonTaskList.Count; i++)
                {
                    if (m_CommonTaskList[i].GetCurTaskState() != TaskState.Finished)
                        CreateTask(m_CommonTaskList[i]);
                }
            }

            if (MainGameMgr.S.CommonTaskMgr.IsTodayMissionAllClear() && m_CommonTaskList.Count == 0)
            {
                m_NoTaskImg.SetActive(true);
            }
            else
            {
                m_NoTaskImg.SetActive(false);
            }
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnTaskManualFinished:

                    break;
                default:
                    break;
            }
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();

            CloseSelfPanel();
        }

        protected override void OnClose()
        {
            base.OnClose();

            EventSystem.S.UnRegister(EventID.OnTaskManualFinished, HandleAddListenerEvent);
        }

        public void CreateTask(SimGameTask simGameTask)
        {
            List<TaskReward>  taskRewards = simGameTask.CommonTaskItemInfo.GetItemRewards();

            GameObject obj = Instantiate(m_BulletinBoardtem, m_TaskContParent);
            ItemICom taskItem = obj.GetComponent<ItemICom>();
            taskItem.OnInit(simGameTask,null, this);
            m_TaskObjDic.Add(simGameTask.TaskId, obj);      
        }

        public string GetStrForItemID(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }
    }
}