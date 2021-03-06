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

        private Vector3 m_LastPos;
        private float m_CheckPosInterval = 1f;
        private float m_CheckPosTime = 0;

        public CharacterStateCollectRes(CharacterStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(ICharacterStateHander handler)
        {
            if (m_Controller == null)
                m_Controller = (CharacterController)handler.GetCharacterController();

            RegisterEvents();

            Qarth.Log.i("Enter State collect res: " + m_Controller.CharacterView.name);

            m_CollectedObjType = m_Controller.CollectObjType;//(CollectedObjType)m_Controller.CurTask.CommonTaskItemInfo.subType;
            m_RawMatItem = null;

            m_LastPos = m_Controller.GetPosition();
            m_ReachTargetPos = false;
            m_IsCollectResEnd = false;



            var warkConf = TDWorkTable.GetWorkConfigItem(m_CollectedObjType);
            if (warkConf == null)//解决因为枚举导致的null问题
            {
                m_Controller.CollectObjType = CollectedObjType.None;
                m_Controller.ReleaseWorkTip();
                m_Controller.SetState(CharacterStateID.Wander);
                return;
            }
            m_Controller.SpawnWorkTipWhenCollectedObj(m_CollectedObjType);
            m_CollectTotalTime = warkConf.workTime;
            //显示对话气泡
            WorldUIPanel.S?.ShowWorkText(m_Controller.CharacterView.transform, warkConf.RandomTalk());

            m_Controller.SetWorkProgressPercent(0);

            m_RawMatItem = MainGameMgr.S.RawMatCollectSystem.GetRawMatItem(m_CollectedObjType);
            if (m_RawMatItem != null)
            {
                m_RawMatItem.OnCharacterSelected(m_Controller);

                MoveToTargetPos();

                m_Controller.ManualSelectedToCollectObj = false;

                m_Time = GameDataMgr.S.GetClanData().GetObjCollectedTime(m_CollectedObjType);
            }
        }

        private void MoveToTargetPos()
        {
            Transform t = m_RawMatItem.GetRandomCollectPos();

            if (m_Controller.ManualSelectedToCollectObj)
            {
                m_Controller.RunTo(t.position, OnReachDestination);
            }
            else
            {
                m_Controller.MoveTo(t.position, OnReachDestination);
            }
        }

        public override void Exit(ICharacterStateHander handler)
        {
            //Qarth.Log.i("Exit State collect res: " + m_Controller.CharacterView.name);

            UnregisterEvents();
        }

        public override void Execute(ICharacterStateHander handler, float dt)
        {
            if (m_IsCollectResEnd)
                return;

            if (m_ReachTargetPos && GuideMgr.S.IsGuideFinish(31))
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

                    //DataAnalysisMgr.S.CustomEvent(DotDefine.work_finish, m_Controller.CollectObjType.ToString());

                    m_Controller.CollectObjType = CollectedObjType.None;

                }
            }

            // Force character move to target pos, if he does not move
            //if (!m_ReachTargetPos)
            //{
            //    m_CheckPosTime += Time.deltaTime;
            //    if (m_CheckPosTime > m_CheckPosInterval)
            //    {
            //        if (m_Controller.CharacterView.NavAgent.enabled == false)
            //        {
            //            m_Controller.CharacterView.NavAgent.enabled = true;
            //        }

            //        m_CheckPosTime = 0;
            //        if (Vector3.Distance(m_LastPos, m_Controller.GetPosition()) < 0.1f)
            //        {
            //            m_LastPos = m_Controller.GetPosition();

            //            MoveToTargetPos();
            //        }
            //    }
            //}
        }

        private void OnReachDestination()
        {
            if (m_IsCollectResEnd)
                return;

            m_Controller.ReleaseWorkTip();

            m_ReachTargetPos = true;

            if (m_RawMatItem != null)
            {
                m_RawMatItem.OnCharacterArriveCollectPos();

                string animName = GetCollectResAnim();

                AudioManager.S.PlayCollectWuwoodOrRockSound(m_CollectedObjType, m_Controller.GetPosition());

                m_Controller.CharacterView.PlayAnim(animName, true, () =>
                {
                });
                m_Controller.FaceTo(m_RawMatItem.transform.position.x);
            }

            m_Controller.SpawnWorkProgressBar();

            if (GuideMgr.S.IsGuideFinish(31) == false)
            {
                EventSystem.S.Send(EventID.OnDiscipleAutoWorkTrigger);
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
            //EventSystem.S.Register(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void UnregisterEvents()
        {
            //EventSystem.S.UnRegister(EventID.OnTaskObjCollected, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            //if (key == (int)EventID.OnTaskObjCollected)
            //{
            //    CollectedObjType taskId = (CollectedObjType)param[0];
            //    if (taskId == m_Controller.CollectObjType)
            //    {
            //        m_IsCollectResEnd = true;

            //        m_Controller.SetState(CharacterStateID.Wander);

            //        m_Controller.CollectObjType = CollectedObjType.None;
            //    }
            //}
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
            GameDataMgr.S.GetPlayerData().recordData.AddJob();
            WorkConfigItem workConfigItem = TDWorkTable.GetWorkConfigItem(m_CollectedObjType);
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            float rewardRatio = workConfigItem.GetRatio(lobbyLevel);
            // Item reward
            for (int i = 0; i < workConfigItem.itemRewards.Count; i++)
            {
                int itemId = workConfigItem.itemRewards[i].id;
                int count = (int)(workConfigItem.itemRewards[i].GetRewardValue() * rewardRatio);
                MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);

                m_Controller.SpawnCollectedObjWorkReward((RawMaterial)itemId, count);
            }

            // Special reward
            if (workConfigItem.IsHaveSpecialReward)
            {
                for (int i = 0; i < workConfigItem.specialRewards.Count; i++)
                {
                    int itemId = workConfigItem.specialRewards[i].id;
                    int count = workConfigItem.specialRewards[i].GetRewardValue();

                    if (count > 0)
                    {
                        MainGameMgr.S.InventoryMgr.AddItem(new PropItem((RawMaterial)itemId), count);

                        m_Controller.SpawnCollectedObjWorkRewardWithDelay((RawMaterial)itemId, count, 1f);
                    }
                }
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
