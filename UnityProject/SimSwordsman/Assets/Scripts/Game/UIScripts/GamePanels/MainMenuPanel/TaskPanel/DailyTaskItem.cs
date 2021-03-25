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

        public void SetTask(Task task)
        {
            m_TxtTitle.text = task.taskTitle;
            m_TxtSubTitle.text = task.taskSubTitle;
        }
    }

}