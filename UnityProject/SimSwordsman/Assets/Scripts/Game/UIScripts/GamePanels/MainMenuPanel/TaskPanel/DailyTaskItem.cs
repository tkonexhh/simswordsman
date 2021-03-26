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
        private DailyTaskPanel m_Panel;

        private void Awake()
        {
            m_BtnComplete.onClick.AddListener(OnClickComplete);
        }

        public void SetTask(DailyTaskPanel panel, Task task)
        {
            m_Task = task;
            if (m_Panel == null)
                m_Panel = panel;
            m_TxtTitle.text = task.taskTitle;
            m_TxtSubTitle.text = task.taskSubTitle;
            m_ImgRewardIcon.sprite = m_Panel.FindSprite(task.reward.SpriteName());
            m_TxtRewardNum.text = task.reward.Count.ToString();
            Refesh();
        }

        private void Refesh()
        {
            m_Task.IsComplete();
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


            //存档
            if (GameDataMgr.S.GetPlayerData().taskData.dailyTaskData.AddCompleteID(m_Task.id))
                m_Task.GetReward();
            Refesh();
        }
    }

}