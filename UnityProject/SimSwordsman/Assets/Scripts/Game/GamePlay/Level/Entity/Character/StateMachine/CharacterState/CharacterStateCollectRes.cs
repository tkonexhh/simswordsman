using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;
using System.Linq;

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

            m_Controller.SpawnWorkTipWhenCollectedObj(m_CollectedObjType);

            m_CollectTotalTime = TDWorkTable.GetWorkConfigItem(m_CollectedObjType).workTime;

            m_ReachTargetPos = false;
            m_IsCollectResEnd = false;
            //m_IsTaskCollectableItemFound = false;

            RegisterEvents();

            m_RawMatItem = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(m_CollectedObjType);
            if (m_RawMatItem != null)
            {
                m_RawMatItem.SetCharacterSelected(true);
                m_RawMatItem.HideBubble();

                Transform t = m_RawMatItem.GetRandomCollectPos();

                if (m_Controller.ManualSelectedToCollectObj)
                {
                    m_Controller.RunTo(t.position, OnReachDestination);
                }
                else
                {
                    m_Controller.MoveTo(t.position, OnReachDestination);
                }
                m_Controller.ManualSelectedToCollectObj = false;

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

            if (m_ReachTargetPos && GuideMgr.S.IsGuideFinish(8))
            {
                m_Time += Time.deltaTime;

                GameDataMgr.S.GetClanData().SetObjCollectedTime(m_CollectedObjType, (int)m_Time);

                m_Controller.SetWorkProgressPercent(m_Time / m_CollectTotalTime);

                if (m_Time > m_CollectTotalTime)
                {
                    m_Time = 0f;
                    m_IsCollectResEnd = true;

                    ClaimReward();

                    EventSystem.S.Send(EventID.OnTaskObjCollected, m_Controller.CollectObjType);
    
                    GameDataMgr.S.GetClanData().SetObjCollectedTime(m_CollectedObjType, 0);

                    m_Controller.ReleaseWorkProgressBar();

                    m_Controller.SetState(CharacterStateID.Wander);

                    m_Controller.CollectObjType = CollectedObjType.None;
                }
            }
        }

        private void OnReachDestination()
        {
            if (m_IsCollectResEnd)
                return;

            m_Controller.ReleaseWorkTip();

            m_ReachTargetPos = true;

            if (m_RawMatItem != null)
            {
                string animName = GetCollectResAnim();
                m_Controller.CharacterView.PlayAnim(animName, true, null);
                m_Controller.FaceTo(m_RawMatItem.transform.position.x);
            }

            m_Controller.SpawnWorkProgressBar();

            if (GuideMgr.S.IsGuideFinish(8) == false) {
                EventSystem.S.Send(EventID.OnGuideClickTaskDetailsTrigger1);
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

        //private void MoveToBulletinBoard()
        //{
        //    Vector3 bulletinBoardPos = MainGameMgr.S.FacilityMgr.GetDoorPos(FacilityType.BulletinBoard);
        //    Vector2 pos = new Vector2(bulletinBoardPos.x, bulletinBoardPos.y) + UnityEngine.Random.insideUnitCircle * 0.3f;

        //    m_Controller.MoveTo(pos, ()=> 
        //    {
        //        if (m_Controller.CurTask != null)
        //        {
        //            MainGameMgr.S.CommonTaskMgr.SetTaskFinished(m_Controller.CurTask.TaskId, TaskState.Finished);
        //        }
        //        m_Controller.CharacterView.PlayIdleAnim();
        //        m_Controller.SpawnTaskRewardBubble();
        //    });
        //}

        private void ClaimReward()
        {
            WorkConfigItem workConfigItem = TDWorkTable.GetWorkConfigItem(m_CollectedObjType);
            // Item reward
            for (int i = 0; i < workConfigItem.itemRewards.Count; i++)
            {
                int itemId = workConfigItem.itemRewards[i].id;
                int count = workConfigItem.itemRewards[i].GetRewardValue();
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);

                m_Controller.SpawnCollectedObjWorkReward((RawMaterial)itemId, count);
            }

            // Special reward
            for (int i = 0; i < workConfigItem.specialRewards.Count; i++)
            {
                int itemId = workConfigItem.specialRewards[i].id;
                int count = workConfigItem.specialRewards[i].GetRewardValue();
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);

                m_Controller.SpawnCollectedObjWorkReward((RawMaterial)itemId, count);
            }

            // Add exp
            List<CharacterController> characters = MainGameMgr.S.CharacterMgr.CharacterControllerList.Where(i => i.CollectObjType == m_CollectedObjType).ToList();
            LobbyLevelInfo lobbyLevelInfo = (LobbyLevelInfo)TDFacilityLobbyTable.GetLevelInfo(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            characters.ForEach(i =>
            {
                i.AddExp(lobbyLevelInfo.workExp);
            });

        }
    }
}
