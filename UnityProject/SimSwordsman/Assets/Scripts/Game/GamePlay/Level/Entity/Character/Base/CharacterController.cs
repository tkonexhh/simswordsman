using Qarth;
using Spine.Unity;
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
        public CollectedObjType CollectObjType { get { return m_CharacterModel.GetCollectedObjType(); } set { m_CharacterModel.SetCollectedObjType(value); } }
        public bool ManualSelectedToCollectObj { get { return m_ManualSelectedToCollectObj; } set { m_ManualSelectedToCollectObj = value; } }

        private bool m_ManualSelectedToCollectObj = false;
        //private CollectedObjType m_CollectedObjType;
        private CharacterStateID m_CurState = CharacterStateID.None;
        private CharacterStateBattle m_StateBattle = null;
        private FightGroup m_FightGroup = null;

        // �ҷ���idΨһ���з�id��Ψһ TODO:���ҷ�Controller�ֿ�
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
                m_CharacterView.SetSkin(m_CharacterModel.GetHeadId());
            }

            m_StateMachine = new CharacterStateMachine(this);
            SetState(initState, m_CharacterModel.GetTargetFacilityType(), m_CharacterModel.GetTargetFacilityStartTime(), m_CharacterModel.GetTargetFacilityIndex());

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
        public int GetDeliverID()
        {
            return m_CharacterModel.GetDeliverID();
        }
        public Vector2 GetPosition()
        {
            return m_CharacterView.transform.position;
        }
        public bool IsOurCharacterCamp()
        {
            return m_CharacterCamp == CharacterCamp.OurCamp;
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
        public void FollowDeliver(Vector2 deliverPos)
        {
            m_CharacterView.FollowDeliver(deliverPos);
        }
        public void RunTo(Vector2 pos, System.Action callback)
        {
            m_CharacterView.RunTo(pos, callback);
        }
        public void StopNavAgent()
        {
            m_CharacterView.StopNavAgent();
        }
        public void Move(Vector2 deltaPos)
        {
            m_CharacterView.Move(deltaPos);
        }

        public void SetPosition(Vector3 targetPos)
        {
            m_CharacterView.SetPosition(targetPos);
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

        public void SetState(CharacterStateID state, FacilityType targetFacilityType = FacilityType.None, string startTime = "", int index = -1)
        {
            if (state != m_CurState)
            {
                m_CurState = state;
                //if (m_CharacterCamp == CharacterCamp.OurCamp)
                //    Debug.LogError("Setstate:" + m_CurState);
                if (m_CharacterCamp == CharacterCamp.OurCamp && m_CurState != CharacterStateID.Battle) //Battle state ������
                {
                    //Debug.LogError("SetstateTODB:" + m_CurState);
                    SetStateToDB(m_CurState, targetFacilityType, startTime, index);

                    //�����Դ��룺����ͷ��û����ʧ��Progress
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
        public void SetDeliverID(int deliverID)
        {
            m_CharacterModel.SetDeliverID(deliverID);
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

        //public void OnExitBattleField()
        //{
        //    m_CharacterView.OnExitBattleField();
        //    SetState(CharacterStateID.Wander);
        //}

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

        public void ChangeBody(CharacterQuality characterQuality, ClanType clanType)
        {
            GameObject go = CharacterLoader.S.GetCharacterGo(m_CharacterModel.CharacterItem.id, characterQuality, m_CharacterModel.CharacterItem.bodyId, clanType);
            if (go == null)
            {
                CharacterLoader.S.LoadCharactersync(m_CharacterModel.CharacterItem.id, characterQuality, m_CharacterModel.CharacterItem.bodyId, clanType, 1, 1);
                go = CharacterLoader.S.GetCharacterGo(m_CharacterModel.CharacterItem.id, characterQuality, m_CharacterModel.CharacterItem.bodyId, clanType);

                if (go == null)
                {
                    Log.e("Load character return null: " + m_CharacterModel.CharacterItem.id + " Quality: " + characterQuality + " clantype: " + clanType);
                    return;
                }
            }

            CharacterView characterView = go.GetComponent<CharacterView>();
            characterView.enabled = false;
            PolyNavAgent polyNavAgent = go.GetComponent<PolyNavAgent>();
            polyNavAgent.enabled = false;

            string animName = m_CharacterView.GetCurRuningAnimName();

            go.transform.SetParent(m_CharacterView.transform);
            go.transform.localPosition = Vector3.zero;

            GameObject.Destroy(m_CharacterView.Body.gameObject);
            GameObject.Destroy(m_CharacterView.HeadPos.gameObject);
            if (m_CharacterView.BoneFollower_Foot != null)
                GameObject.Destroy(m_CharacterView.BoneFollower_Foot.gameObject);
            if (m_CharacterView.Clean_DragSmoke != null)
                GameObject.Destroy(m_CharacterView.Clean_DragSmoke.gameObject);
            BoneFollower[] boneFollower = m_CharacterView.transform.GetComponentsInChildren<BoneFollower>();
            foreach (BoneFollower b in boneFollower)
            {
                GameObject.Destroy(b.gameObject);
            }

            m_CharacterView.Body = go.transform.Find("Body").gameObject;
            m_CharacterView.HeadPos = go.transform.Find("HeadPos").gameObject;
            m_CharacterView.BoneFollower_Foot = null; // Set null for now
            m_CharacterView.Clean_DragSmoke = null; // Set null for now

            m_CharacterView.SetSpineAnim(true);
            //GameObjectPoolMgr.S.Recycle(m_CharacterView.gameObject);

            //m_CharacterView = characterView;
            //m_CharacterView.Init();
            //m_CharacterView.SetController(this);
            m_CharacterView.SetSkin(m_CharacterModel.GetHeadId());
            m_CharacterView.PlayAnim(animName, true, null);
        }
        #endregion

        #region Private
        private void SetStateToDB(CharacterStateID characterStateID, FacilityType targetFacilityType, string startTime, int index)
        {
            m_CharacterModel.SetDataState(characterStateID, targetFacilityType, startTime, index);
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
