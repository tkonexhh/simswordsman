using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    //public class CharacterStateGoOutsideForTaskBattle : CharacterState
    //{
    //    private CharacterController m_Controller = null;

    //    private TaskPos m_TaskPos = null;
    //    private float m_Time = 0;
    //    private bool m_HasArrived = false;

    //    public CharacterStateGoOutsideForTaskBattle(CharacterStateID stateEnum) : base(stateEnum)
    //    {

    //    }

    //    public override void Enter(ICharacterStateHander handler)
    //    {
    //        if(m_Controller == null)
    //            m_Controller = (CharacterController)handler.GetCharacterController();

    //        if (m_TaskPos == null)
    //        {
    //            m_TaskPos = GameObject.FindObjectOfType<TaskPos>();
    //        }

    //        m_HasArrived = false;

    //        m_Time = MainGameMgr.S.CommonTaskMgr.GetTaskExecutedTime(m_Controller.CurTask.TaskId);
    //        if (m_Time > 0)
    //        {
    //            if (m_Time >= m_Controller.CurTask.CommonTaskItemInfo.taskTime)
    //            {
    //                m_HasArrived = true;
    //            }

    //            m_Controller.HideBody();
    //        }
    //        else
    //        {
    //            Vector3 pos = m_TaskPos.GetDoorPos();
    //            m_Controller.MoveTo(pos, OnReachDestination);
    //        }
    //    }

    //    public override void Exit(ICharacterStateHander handler)
    //    {
    //    }

    //    public override void Execute(ICharacterStateHander handler, float dt)
    //    {
    //        if (m_HasArrived == false)
    //        {
    //            m_Time += Time.deltaTime;

    //            if (m_Time > m_Controller.CurTask.CommonTaskItemInfo.taskTime)
    //            {
    //                m_Time = 0f;

    //                m_HasArrived = true;

    //                MainGameMgr.S.CommonTaskMgr.SetTaskExcutedTime(m_Controller.CurTask.TaskId, m_Controller.CurTask.CommonTaskItemInfo.taskTime);
    //            }
    //            else
    //            {
    //                MainGameMgr.S.CommonTaskMgr.SetTaskExcutedTime(m_Controller.CurTask.TaskId, (int)m_Time);
    //            }
    //        }
    //    }

    //    private void OnReachDestination()
    //    {
    //        m_Controller.HideBody();
    //    }
    //}
}
