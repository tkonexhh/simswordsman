using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class DailyTaskItem : IUListItemView
    {
        [SerializeField] private Text m_TxtTitle;
        [SerializeField] private Text m_TxtSubTitle;
        [SerializeField] private Image m_ImgTaskIcon;
        [SerializeField] private Image m_ImgRewardIcon;
        [SerializeField] private Text m_TxtRewardNum;

        [SerializeField, Header("未完成")] private GameObject m_ObjUnComplete;
        [SerializeField, Header("领取")] private GameObject m_ObjComplete;
        [SerializeField] private Button m_BtnComplete;
        [SerializeField, Header("已领取")] private GameObject m_ObjRewarded;


        private Task m_Task;

        private void Awake()
        {
            m_BtnComplete.onClick.AddListener(OnClickComplete);
        }

        public void SetTask(Task task)
        {
            m_Task = task;
            m_TxtTitle.text = task.taskTitle;
            m_TxtSubTitle.text = task.taskSubTitle;
            Refesh();
        }

        private void Refesh()
        {
            m_ObjUnComplete.SetActive(m_Task.taskState == TaskState.Running);
            m_ObjComplete.SetActive(m_Task.taskState == TaskState.Unclaimed);
            m_ObjRewarded.SetActive(m_Task.taskState == TaskState.Finished);
        }

        private void OnClickComplete()
        {
            if (m_Task == null)
                return;

            if (!m_Task.IsComplete())
                return;

            Debug.LogError("GetReward");
            //存档
            GameDataMgr.S.GetPlayerData().taskData.dailyTaskData.AddCompleteID(m_Task.id);
            m_Task.GetReward();
        }
    }

}