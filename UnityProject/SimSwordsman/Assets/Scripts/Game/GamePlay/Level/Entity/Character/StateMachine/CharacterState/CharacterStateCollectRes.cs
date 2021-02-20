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

        //private TaskPos m_TaskPos = null;

        private bool m_ReachTargetPos = false;
        private float m_Time = 0f;
        private bool m_IsCollectResEnd = false;

        private CollectedObjType m_CollectedObjType;
        //private TaskCollectableItem m_TaskCollectableItem = null;
        //private bool m_IsTaskCollectableItemFound = false;
        private RawMatItem m_RawMatItem = null;
        private float m_CollectTotalTime;

        public CharacterStateCollectRes(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if(m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            m_CollectedObjType = m_Controller.CollectObjType;//(CollectedObjType)m_Controller.CurTask.CommonTaskItemInfo.subType;
            m_RawMatItem = null;
            m_CollectTotalTime = TDWorkTable.GetWorkConfigItem(m_CollectedObjType).workTime;

            m_ReachTargetPos = false;
            m_IsCollectResEnd = false;
            //m_IsTaskCollectableItemFound = false;

            RegisterEvents();

            m_RawMatItem = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(m_CollectedObjType);
            if (m_RawMatItem != null)
            {
                Transform t = m_RawMatItem.GetRandomCollectPos();

                m_Controller.MoveTo(t.position, OnReachDestination);

                m_Time = GameDataMgr.S.GetClanData().GetObjCollectedTime(m_CollectedObjType);
            }
        }

        public override void Exit(ICharacterStateHander handler)
        {
            UnregisterEvents();
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            //if (!CommonTaskMgr.IsNotNeedToSpawnTaskItem(m_CollectedObjType))
            //{
            //    if (m_IsTaskCollectableItemFound == false)
            //    {
            //        m_TaskCollectableItem = MainGameMgr.S.CommonTaskMgr.GetTaskCollectableItem(m_CollectedObjType);
            //        if (m_TaskCollectableItem != null)
            //        {
            //            m_IsTaskCollectableItemFound = true;

            //            //Vector2 randomDelta = UnityEngine.Random.insideUnitCircle;
            //            //Vector3 pos = m_TaskCollectableItem.transform.position + new Vector3(randomDelta.x, randomDelta.y, 0);
            //            Transform t = m_TaskCollectableItem.GetRandomCollectPos();
            //            m_TaskCollectableItem.OnCollectPosTaken(t);
            //            m_Controller.MoveTo(t.position, OnReachDestination);

            //            m_Time = MainGameMgr.S.CommonTaskMgr.GetTaskExecutedTime(m_Controller.CurTask.TaskId);
            //        }
            //    }

            //    if (m_TaskCollectableItem == null)
            //        return;
            //}
            //else
            //{
            //    if (m_IsTaskCollectableItemFound == false)
            //    {
            //        m_IsTaskCollectableItemFound = true;

            //        m_Controller.MoveTo(GameObject.FindObjectOfType<TaskPos>().GetTaskPos(m_CollectedObjType), OnReachDestination);
            //    }
            //}

            //if (m_IsTaskEnd)
            //    return;

            //if (m_ReachTargetPos)
            //{
            //    m_Time += Time.deltaTime;

            //    MainGameMgr.S.CommonTaskMgr.SetTaskExcutedTime(m_Controller.CurTask.TaskId, (int)m_Time);
            //    EventSystem.S.Send(EventID.OnArriveCollectResPos, m_Controller.CurTask);

            //    if (m_Time > m_Controller.CurTask.CommonTaskItemInfo.taskTime)
            //    {
            //        m_Time = 0f;
            //        m_IsTaskEnd = true;


            //        m_TaskCollectableItem?.OnEndCollected();
            //        //MainGameMgr.S.CommonTaskMgr.SetTaskFinished(m_Controller.CurTask.TaskId);
            //        EventSystem.S.Send(EventID.OnTaskObjCollected, m_Controller.CurTask.TaskId);
            //    }
            //}

            if (m_IsCollectResEnd)
                return;

            if (m_ReachTargetPos)
            {
                m_Time += Time.deltaTime;

                GameDataMgr.S.GetClanData().SetObjCollectedTime(m_CollectedObjType, (int)m_Time);

                if (m_Time > m_CollectTotalTime)
                {
                    m_Time = 0f;
                    m_IsCollectResEnd = true;

                    EventSystem.S.Send(EventID.OnTaskObjCollected, m_Controller.CollectObjType);
                    //TODO: Get reward


                    m_Controller.SetState(CharacterStateID.Wander);

                    m_Controller.CollectObjType = CollectedObjType.None;
                }
            }
        }

        private void OnReachDestination()
        {
            if (m_IsCollectResEnd)
                return;

            m_ReachTargetPos = true;

            if (m_RawMatItem != null)
            {
                string animName = GetCollectResAnim();
                m_Controller.CharacterView.PlayAnim(animName, true, null);
                m_Controller.FaceTo(m_RawMatItem.transform.position.x);
            }

        }

        private string GetCollectResAnim()
        {
            string name = string.Empty;
            switch (m_CollectedObjType)
            {
                case CollectedObjType.Bear:
                case CollectedObjType.Boar:
                case CollectedObjType.Deer:
                case CollectedObjType.Snake:
                    name = "hunting";
                    break;
                case CollectedObjType.SilverWood:
                case CollectedObjType.WuWood:
                    name = "lumbering";
                    break;
                case CollectedObjType.Chicken:
                case CollectedObjType.Ganoderma:
                case CollectedObjType.Vine:
                    name = "collection";
                    break;
                case CollectedObjType.Fish:
                    name = "fishing";
                    break;
                case CollectedObjType.CloudRock:
                case CollectedObjType.QingRock:
                case CollectedObjType.Iron:
                    name = "mining";
                    break;
                case CollectedObjType.Well:
                    name = "collection_well_water";
                    break;
            }

            return name;
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            if (key == (int)EventID.OnTaskObjCollected)
            {
                CollectedObjType taskId = (CollectedObjType)param[0];
                if (taskId == m_Controller.CollectObjType)
                {
                    m_IsCollectResEnd = true;

                    m_Controller.SetState(CharacterStateID.Wander);

                    m_Controller.CollectObjType = CollectedObjType.None;
                }
            }
        }

        private void MoveToBulletinBoard()
        {
            Vector3 bulletinBoardPos = MainGameMgr.S.FacilityMgr.GetDoorPos(FacilityType.BulletinBoard);
            Vector2 pos = new Vector2(bulletinBoardPos.x, bulletinBoardPos.y) + UnityEngine.Random.insideUnitCircle * 0.3f;

            m_Controller.MoveTo(pos, ()=> 
            {
                if (m_Controller.CurTask != null)
                {
                    MainGameMgr.S.CommonTaskMgr.SetTaskFinished(m_Controller.CurTask.TaskId);
                }
                m_Controller.CharacterView.PlayIdleAnim();
                m_Controller.SpawnTaskRewardBubble();
            });
        }
    }
}
