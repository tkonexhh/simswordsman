using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateFishing : CharacterState
    {
        private CharacterController m_Controller = null;

        private TaskPos m_TaskPos = null;

        private float m_Time = 0f;
        private bool m_IsTaskEnd = false;

        public CharacterStateFishing(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            if (m_TaskPos == null)
            {
                m_TaskPos = GameObject.FindObjectOfType<TaskPos>();
            }

            Vector3 pos = m_TaskPos.GetTaskPos(SimGameTaskType.Fish);
            m_Controller.MoveTo(pos, OnReachDestination);

            m_IsTaskEnd = false;
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_IsTaskEnd)
                return;

            m_Time += Time.deltaTime;
            if (m_Time > m_Controller.CurTask.MainTaskItemInfo.time)
            {
                m_Time = 0f;
                m_IsTaskEnd = true;

                GameDataMgr.S.GetMainTaskData().SetTaskFinished(m_Controller.CurTask.TaskId);
                EventSystem.S.Send(EventID.OnTaskManualFinished);
                EventSystem.S.Send(EventID.OnTaskFinished);

                m_Controller.CurTask = null;
                m_Controller.SetState(CharacterStateID.Wander);
            }
        }

        private void OnReachDestination()
        {
            if (m_IsTaskEnd)
                return;

            m_Controller.CharacterView.PlayAnim("mining", true, null);
        }
    }
}
