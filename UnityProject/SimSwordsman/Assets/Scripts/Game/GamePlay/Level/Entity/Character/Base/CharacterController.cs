using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class CharacterController : IEntityController, ICharacterStateHander
    {
        private CharacterModel m_CharacterModel = null;
        private CharacterView m_CharacterView = null;
        private CharacterCamp m_CharacterCamp = CharacterCamp.OurCamp;
        private CharacterController m_FightTarget = null;

        private int m_CharacterId;

        private SimGameTask m_CurTask = null;

        private CharacterStateMachine m_StateMachine;

        public CharacterModel CharacterModel { get => m_CharacterModel; }
        public CharacterView CharacterView { get => m_CharacterView; }
        public int CharacterId { get => m_CharacterId; }
        public CharacterCamp CharacterCamp { get => m_CharacterCamp; }
        public CharacterController FightTarget { get => m_FightTarget; }
        public CharacterStateID CurState { get => m_CurState; }
        public FightGroup FightGroup { get => m_FightGroup; set => m_FightGroup = value; }
        public SimGameTask CurTask { get => m_CurTask; }
        public CollectedObjType CollectObjType { get { return m_CollectedObjType; } set { m_CollectedObjType = value; m_CharacterModel.SetCollectedObjType(value); GameDataMgr.S.GetClanData().SetCharacterCollectedObjType(m_CharacterId, value); } }
        public bool ManualSelectedToCollectObj { get { return m_ManualSelectedToCollectObj; } set { m_ManualSelectedToCollectObj = value; } }

        private bool m_ManualSelectedToCollectObj = false;
        private CollectedObjType m_CollectedObjType;
        private CharacterStateID m_CurState = CharacterStateID.None;
        private CharacterStateBattle m_StateBattle = null;
        private FightGroup m_FightGroup = null;

        // 我方的id唯一，敌方id不唯一 TODO:敌我方Controller分开
        public CharacterController(int id, CharacterView characterView, CharacterStateID initState, CharacterCamp camp = CharacterCamp.OurCamp)
        {
            m_CharacterId = id;
            m_CharacterView = characterView;
            m_CharacterView.ShowBody();
            m_CharacterView.Init();
            m_CharacterView.SetController(this);

            m_CharacterCamp = camp;

            m_CharacterModel = new CharacterModel(id, this);

            if (m_CharacterCamp == CharacterCamp.OurCamp)
            {
                m_CollectedObjType = m_CharacterModel.GetCollectedObjType();

                m_CharacterView.SetSkin(m_CharacterModel.GetHeadId());
            }

            m_StateMachine = new CharacterStateMachine(this);


            SetState(initState, m_CharacterModel.GetTargetFacilityType());

            m_StateBattle = (CharacterStateBattle)GetState(CharacterStateID.Battle);
        }

        #region IEntityController
        public virtual void Init()
        {
        }

        public virtual void Release()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void Update()
        {
            if (m_CurState != CharacterStateID.Battle)
            {
                m_StateMachine.UpdateState(Time.deltaTime);
            }
        }

        public CharacterController GetCharacterController()
        {
            return this;
        }

        #endregion

        #region Public Get
        public Vector2 GetPosition()
        {
            return m_CharacterView.transform.position;
        }

        public bool IsDead()
        {
            return m_CharacterModel.GetHp() <= 0;
        }

        public bool WillBeDead()
        {
            return m_CharacterModel.GetHp() - m_CachedDamage <= 0;
        }

        public float GetAtkRange()
        {
            return 1f;
        }

        public CharacterState GetState(CharacterStateID id)
        {
            CharacterState state = m_StateMachine.GetState(id);
            return state;
        }

        public CharacterStateBattle GetBattleState()
        {
            return m_StateBattle;
        }

        public FightGroup GetFightGroup()
        {
            return m_FightGroup;
        }

        public int GetKongfuCount()
        {
            return m_CharacterModel.GetKongfuCount();
        }

        public float GetMoveSpeed()
        {
            return 1f;
        }

        public int GetCurExp()
        {
            return m_CharacterModel.GetExp();
        }

        public void AddExp(int deltaExp)
        {
            m_CharacterModel.AddExp(deltaExp);
        }

        public int GetExpLevelUpNeed()
        {
            return m_CharacterModel.GetExpLevelUpNeed();
        }

        public void AddKongfuExp(int deltaExp)
        {
            m_CharacterModel.AddKongfuExp(deltaExp);
        }
        #endregion

        #region Public Set

        public void RefreshBattleState()
        {
            if (m_CurState == CharacterStateID.Battle)
            {
                m_StateMachine.UpdateState(Time.deltaTime);
            }
        }

        public void MoveTo(Vector2 pos, System.Action callback)
        {
            m_CharacterView.MoveTo(pos, callback);
        }

        public void RunTo(Vector2 pos, System.Action callback)
        {
            m_CharacterView.RunTo(pos, callback);
        }

        public void Move(Vector2 deltaPos)
        {
            m_CharacterView.Move(deltaPos);
        }

        public void FaceTo(float x)
        {
            if (x > GetPosition().x)
            {
                m_CharacterView.FaceToRight();
            }
            else
            {
                m_CharacterView.FaceToLeft();
            }
        }

        public void SetState(CharacterStateID state, FacilityType targetFacilityType = FacilityType.None)
        {
            if (state != m_CurState)
            {
                m_CurState = state;
                //if (m_CharacterCamp == CharacterCamp.OurCamp)
                //    Debug.LogError("Setstate:" + m_CurState);
                if (m_CharacterCamp == CharacterCamp.OurCamp && m_CurState != CharacterStateID.Battle) //Battle state 不存裆
                {
                    //Debug.LogError("SetstateTODB:" + m_CurState);
                    SetStateToDB(m_CurState, targetFacilityType);

                    //保护性代码：消除头上没有消失的Progress
                    if (state == CharacterStateID.Wander)
                    {
                        CharacterWorkProgressBar[] progressList = m_CharacterView.GetComponentsInChildren<CharacterWorkProgressBar>();
                        foreach (var item in progressList)
                        {
                            if (item != m_CharacterView.WorkProgressBar)
                            {
                                GameObjectPoolMgr.S.Recycle(item.gameObject);
                            }
                        }
                    }
                }

                m_StateMachine.SetCurrentStateByID(state);

                //CollectedObjType collectedObjType = CollectedObjType.None;
                //if (m_CurState == CharacterStateID.CollectRes)
                //{
                //    collectedObjType = (CollectedObjType)m_CurTask.GetCurSubType();
                //}
            }
        }

        public void SetFightTarget(CharacterController target)
        {
            m_FightTarget = target;
        }

        public void SetFightGroup(FightGroup fightGroup)
        {
            m_FightGroup = fightGroup;
        }

        public void OnDamaged(double damage)
        {
            m_CharacterModel.AddHp(-damage);
        }

        private double m_CachedDamage = 0;
        public void CacheDamage(double damage)
        {
            m_CachedDamage += damage;
        }

        public void TriggerCachedDamage()
        {
            OnDamaged(m_CachedDamage);
            m_CachedDamage = 0;
        }

        public void OnEnterBattleField(Vector3 pos)
        {
            m_CharacterView.OnEnterBattleField(pos, m_CharacterCamp);
            SetState(CharacterStateID.Battle);
        }

        public void OnExitBattleField()
        {
            m_CharacterView.OnExitBattleField();
            SetState(CharacterStateID.Wander);
        }

        public void HideBody()
        {
            m_CharacterView.HideBody();
        }

        public void ShowBody()
        {
            m_CharacterView.ShowBody();
        }

        public void SetCurTask(SimGameTask task)
        {
            m_CurTask = task;
            m_CharacterModel.SetCurTask(task);
        }

        public void SpawnTaskRewardBubble()
        {
            if (!GuideMgr.S.IsGuideFinish(16))
            {
                return;
            }

            GameObject go = MainGameMgr.S.CharacterMgr.SpawnTaskRewardBubble();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetTaskRewardBubblePos();
            go.GetComponent<CharacterTaskRewardBubble>().SetController(this);
        }

        public void SpawnWorkTipWhenCollectedObj(CollectedObjType collectedObjType)
        {
            CharacterWorkTip tip = m_CharacterView.GetComponentInChildren<CharacterWorkTip>();
            if (tip != null)
            {
                Log.w("Work tip already added");
                return;
            }

            GameObject go = MainGameMgr.S.CharacterMgr.SpawnWorkTip();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetHeadPos();
            CharacterWorkTip workTip = go.GetComponent<CharacterWorkTip>();
            workTip.OnGotoCollectObj(collectedObjType);
            m_CharacterView.SetWorkTip(workTip);
        }
        public void SpawnWorkTipWhenWorkInFacility(FacilityType facilityType)
        {
            CharacterWorkTip tip = m_CharacterView.GetComponentInChildren<CharacterWorkTip>();
            if (tip != null)
            {
                Log.w("Work tip already added");
                return;
            }

            GameObject go = MainGameMgr.S.CharacterMgr.SpawnWorkTip();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetHeadPos();
            CharacterWorkTip workTip = go.GetComponent<CharacterWorkTip>();
            workTip.OnGotoFacilityWork(facilityType);
            m_CharacterView.SetWorkTip(workTip);
        }

        public void ReleaseWorkTip()
        {
            m_CharacterView.ReleaseWorkTip();
        }

        public void SpawnWorkProgressBar()
        {
            GameObject go = MainGameMgr.S.CharacterMgr.SpawnWorkProgressBar();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetHeadPos();
            CharacterWorkProgressBar progress = go.GetComponent<CharacterWorkProgressBar>();
            m_CharacterView.SetProgressBar(progress);
        }

        public void ReleaseWorkProgressBar()
        {
            m_CharacterView.ReleaseProgressBar();
        }

        public void SetWorkProgressPercent(float percent)
        {
            percent = Mathf.Clamp01(percent);

            m_CharacterView.SetProgressBarPrecent(percent);
        }

        public void HideTaskRewardBubble()
        {
            CharacterTaskRewardBubble bubble = m_CharacterView.GetComponentInChildren<CharacterTaskRewardBubble>();
            if (bubble != null)
            {
                Qarth.GameObjectPoolMgr.S.Recycle(bubble.gameObject);
            }
        }

        public void SpawnFacilityWorkRewardPop(FacilityType facilityType, int count)
        {
            GameObject go = MainGameMgr.S.CharacterMgr.SpawnWorkRewardPop();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetHeadPos();
            CharacterWorkRewardPop workRewardPop = go.GetComponent<CharacterWorkRewardPop>();
            workRewardPop.OnGetFacilityWorkReward(facilityType, (int)FoodBuffSystem.S.Coin(count));
        }

        public void SpawnCollectedObjWorkReward(RawMaterial collectedObjType, int count)
        {
            GameObject go = MainGameMgr.S.CharacterMgr.SpawnWorkRewardPop();
            go.transform.SetParent(m_CharacterView.transform);
            go.transform.position = m_CharacterView.GetHeadPos();
            CharacterWorkRewardPop workRewardPop = go.GetComponent<CharacterWorkRewardPop>();
            workRewardPop.OnGetCollectObjWorkReward(collectedObjType, count);
        }

        public void SpawnCollectedObjWorkRewardWithDelay(RawMaterial collectedObjType, int count, float delay)
        {
            m_CharacterView.GetComponent<MonoBehaviour>().CallWithDelay(() =>
            {
                SpawnCollectedObjWorkReward(collectedObjType, count);
            }, delay);
        }
        #endregion

        #region Private
        private void SetStateToDB(CharacterStateID characterStateID, FacilityType targetFacilityType)
        {
            m_CharacterModel.SetDataState(characterStateID, targetFacilityType);
        }

        //private void SetTaskIfNeed(CharacterStateID initState)
        //{
        //    if (initState == CharacterStateID.CollectRes)
        //    {
        //        int curTaskId = m_CharacterModel.GetCurTaskId();
        //        if (curTaskId != -1)
        //        {
        //            SimGameTask simGameTask = MainGameMgr.S.CommonTaskMgr.GetSimGameTask(curTaskId);
        //            if (simGameTask != null)
        //            {
        //                SetCurTask(simGameTask);
        //            }
        //            else
        //            {
        //                Qarth.Log.e("SpawnTaskIfNeed, MainTaskItemData not found");
        //            }
        //        }
        //        else
        //        {
        //            Qarth.Log.e("SpawnTaskIfNeed, Cur task id is -1");
        //        }
        //    }
        //}
        #endregion
    }

}