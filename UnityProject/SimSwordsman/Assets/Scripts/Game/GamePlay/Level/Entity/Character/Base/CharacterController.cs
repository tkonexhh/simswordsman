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
        public CharacterCamp CharacterCamp { get => m_CharacterCamp;}
        public CharacterController FightTarget { get => m_FightTarget;}
        public CharacterStateID CurState { get => m_CurState;}
        public FightGroup FightGroup { get => m_FightGroup; set => m_FightGroup = value; }
        public SimGameTask CurTask { get => m_CurTask; set => m_CurTask = value; }

        private CharacterStateID m_CurState = CharacterStateID.None;
        private CharacterStateBattle m_StateBattle = null;
        private FightGroup m_FightGroup = null;

        public CharacterController(int id, CharacterView characterView, CharacterStateID initState, CharacterCamp camp = CharacterCamp.OurCamp)
        {
            m_CharacterId = id;
            m_CharacterView = characterView;
            m_CharacterView.Init();
            m_CharacterView.SetController(this);

            m_CharacterCamp = camp;

            m_CharacterModel = new CharacterModel(id, this);

            m_StateMachine = new CharacterStateMachine(this);
            SetState(initState);

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
            return m_CharacterModel.Hp <= 0;
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

        public void SetState(CharacterStateID state, CollectedObjType _subState = CollectedObjType.None)
        {
            m_CharacterModel.SetDataState(new CharacterStateData(state, _subState));

            if (state != m_CurState)
            {
                m_CurState = state;
                m_StateMachine.SetCurrentStateByID(state);
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

        public void OnDamaged(float damage)
        {
            m_CharacterModel.AddHp(-damage);
        }

        private float m_CachedDamage = 0;
        public void CacheDamage(float damage)
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

        #endregion
    }

}