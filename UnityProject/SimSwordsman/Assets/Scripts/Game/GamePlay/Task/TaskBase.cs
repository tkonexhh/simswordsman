using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    // public enum TaskState
    // {
    //     NotStart,
    // }
    public abstract class TaskBase : ITask
    {
        private int m_Now;
        private int m_Target;
        //private TaskState m_TaskState;

        public abstract string GetProgressTxt();
        public abstract void GetReward();

    }



}