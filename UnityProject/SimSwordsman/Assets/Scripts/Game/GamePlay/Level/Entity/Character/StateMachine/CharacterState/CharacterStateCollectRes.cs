using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class CharacterStateCollectRes : CharacterState
    {
        private CharacterController m_Controller = null;

        private TaskPos m_TaskPos = null;

        private bool m_ReachTargetPos = false;
        private float m_Time = 0f;
        private bool m_IsTaskEnd = false;

        private CollectedObjType m_CollectedObjType;

        public CharacterStateCollectRes(CharacterStateID stateEnum) : base(stateEnum)
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

            m_CollectedObjType = (CollectedObjType)m_Controller.CurTask.MainTaskItemInfo.subType;
            Vector3 pos = m_TaskPos.GetTaskPos(m_CollectedObjType);
            m_Controller.MoveTo(pos, OnReachDestination);

            m_ReachTargetPos = false;
            m_IsTaskEnd = false;
        }

        public override void Exit(ICharacterStateHander handler)
        {
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_IsTaskEnd)
                return;

            if (m_ReachTargetPos)
            {
                m_Time += Time.deltaTime;
                if (m_Time > m_Controller.CurTask.MainTaskItemInfo.taskTime)
                {
                    m_Time = 0f;
                    m_IsTaskEnd = true;

                    MainGameMgr.S.MainTaskMgr.SetTaskFinished(m_Controller.CurTask.TaskId);
                    EventSystem.S.Send(EventID.OnTaskManualFinished);
                    //EventSystem.S.Send(EventID.OnTaskFinished);

                    m_Controller.SetCurTask(null);
                    m_Controller.SetState(CharacterStateID.Wander);
                }
            }
        }

        private void OnReachDestination()
        {
            if (m_IsTaskEnd)
                return;

            m_ReachTargetPos = true;

            string animName = GetCollectResAnim();
            m_Controller.CharacterView.PlayAnim(animName, true, null);
        }

        private string GetCollectResAnim()
        {
            string name = string.Empty;
            switch (m_CollectedObjType)
            {
                case CollectedObjType.Bear:
                case CollectedObjType.Boar:
                case CollectedObjType.Chicken:
                case CollectedObjType.Deer:
                case CollectedObjType.Snake:
                    name = "hunting";
                    break;
                case CollectedObjType.Ganoderma:
                case CollectedObjType.SilverWood:
                case CollectedObjType.Vine:
                case CollectedObjType.WuWood:
                    name = "hunting"; //待修改
                    break;
                case CollectedObjType.Fish:
                    name = "hunting"; //待修改
                    break;
                case CollectedObjType.CloudRock:
                case CollectedObjType.QingRock:
                case CollectedObjType.Iron:
                    name = "mining";
                    break;
                case CollectedObjType.Well:
                    name = "hunting"; //待修改
                    break;
            }

            return name;
        }
    }
}
