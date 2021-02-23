using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Qarth;

namespace GameWish.Game
{
    [System.Serializable]
	public class FightGroup
	{
        public int id;
        private CharacterController m_OurCharacter = null;
        private CharacterController m_EnemyCharacter = null;
        //public List<CharacterController> otherOurCharacterList = new List<CharacterController>();
        //public List<CharacterController> otherEnemyCharacterList = new List<CharacterController>();
        private int m_ArriveCount = 0;
        private int m_AtkEndCount = 0;
        private bool m_EnemyAttack = false;
        private float m_AttackRange = 1.5f;
        private int m_AtkEventIndex = 0;

        private List<float> m_OurHitBackDistance = new List<float>();
        private List<float> m_EnemyHitBackDistance = new List<float>();

        public CharacterController OurCharacter { get => m_OurCharacter; }
        public CharacterController EnemyCharacter { get => m_EnemyCharacter; }

        public FightGroup(int id, CharacterController ourCharacter, CharacterController enemyCharacter)
        {
            this.id = id;
            m_OurCharacter = ourCharacter;
            m_EnemyCharacter = enemyCharacter;

            ourCharacter.SetFightGroup(this);
            ourCharacter.SetFightTarget(m_EnemyCharacter);

            enemyCharacter.SetFightGroup(this);
            enemyCharacter.SetFightTarget(m_OurCharacter);

            RegisterEvents();
        }

        public void Release()
        {
            m_EnemyCharacter.FightGroup = null;
            m_OurCharacter.FightGroup = null;

            UnregisterEvents();
        }

        public void StartFight()
        {
            NextRound();
        }

        private void NextRound()
        {
            m_AtkEventIndex = 0;

            // Who will attack
            if (m_OurCharacter.WillBeDead())
            {
                m_EnemyAttack = true;
            }
            else if (m_EnemyCharacter.WillBeDead())
            {
                m_EnemyAttack = false;
            }
            else
            {
                int random = Random.Range(0, 100);
                if (random > 50)
                {
                    m_EnemyAttack = true;
                }
                else
                {
                    m_EnemyAttack = false;
                }
            }

            string atkAnimName;
            List<float> atkRangeList = null;
            if (m_EnemyAttack)
            {
                m_EnemyCharacter.GetBattleState().SetNextAtkAnimName();
                atkAnimName = m_EnemyCharacter.GetBattleState().NextAtkAnimName;
                atkRangeList = GetAtkRangeList(atkAnimName);
                m_EnemyHitBackDistance = atkRangeList;
            }
            else
            {
                m_OurCharacter.GetBattleState().SetNextAtkAnimName();
                atkAnimName = m_OurCharacter.GetBattleState().NextAtkAnimName;
                atkRangeList = GetAtkRangeList(atkAnimName);
                m_OurHitBackDistance = atkRangeList;
            }
            Log.i("Test--------------, atk anim name is: " + atkAnimName);
            if(string.IsNullOrEmpty(atkAnimName))
            {
                Log.e("Atk anim name is empty: " + atkAnimName);
                return;
            }

            float attackRange = 1f;

            if (atkRangeList != null)
            {
                attackRange = atkRangeList[0];
            }
            // Move to random position
            StartToMove(attackRange);
        }

        private List<float> GetAtkRangeList(string atkAnimName)
        {
            var config = TDKongfuAnimationConfigTable.GetAnimConfig(atkAnimName);
            if (config != null)
            {
                return config.atkRangeList;
            }
            else // Animation not found in Table, return default atk list
            {
                List<float> defaultRangeList = new List<float>();
                defaultRangeList.Add(2f);
                defaultRangeList.Add(0.2f);

                return defaultRangeList;
            }
        }

        private void StartToMove(float attackRange)
        {
            float x1 = Random.Range(MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x, MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x);
            float y = Random.Range(MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.y, MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.y);

            float x2 = Random.Range(0, 100) > 50 ? x1 + attackRange : x1 - attackRange;
            if (x2 < MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x)
            {
                x2 = x1 + attackRange;
            }
            if (x2 > MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x)
            {
                x2 = x1 - attackRange;
            }
            Debug.Log("x1 is: " + x1 + " x2 is: " + x2 + " left x is: " + MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x + " right x is: " + MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x);

            m_EnemyCharacter.GetBattleState().MoveTargetPos = new Vector2(x1, y);
            m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Move);

            m_OurCharacter.GetBattleState().MoveTargetPos = new Vector2(x2, y);
            m_OurCharacter.GetBattleState().SetState(BattleStateID.Move);

        }

        private void StartToFight()
        {
            if (m_EnemyAttack)
            {
                //Debug.LogError("Enemy play attack animation");
                //m_EnemyCharacter.CharacterView.PlayAtkAnim("attack1", OnAtkAnimEnd);
                m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Attack);
            }
            else
            {
                //Debug.LogError("Our play attack animation");
                //m_OurCharacter.CharacterView.PlayAtkAnim("attack", OnAtkAnimEnd);
                m_OurCharacter.GetBattleState().SetState(BattleStateID.Attack);
            }
        }

        private void OnArriveDestination()
        {
            //Debug.LogError("OnArriveDestination");
            m_ArriveCount++;

            if (m_ArriveCount >= 2)
            {
                //Debug.LogError("Face to target pos");
                m_EnemyCharacter.FaceTo(m_OurCharacter.GetPosition().x);
                m_OurCharacter.FaceTo(m_EnemyCharacter.GetPosition().x);

                StartToFight();
                m_ArriveCount = 0;
            }
        }

        private void OnAtkEnd()
        {
            m_AtkEndCount++;

            if (m_AtkEndCount >= 1)
            {
                m_AtkEndCount = 0;

                if (m_EnemyCharacter.IsDead() || m_OurCharacter.IsDead())
                {
                    Debug.LogError("someone is dead");
                    MainGameMgr.S.BattleFieldMgr.OnFightGroupCharacterDead(this);
                }
                else
                {
                    NextRound();
                }
            }
        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnBattleMoveEnd, HandleEvent);
            EventSystem.S.Register(EventID.OnBattleAtkEvent, HandleEvent);
            EventSystem.S.Register(EventID.OnBattleAtkEnd, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnBattleMoveEnd, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnBattleAtkEvent, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnBattleAtkEnd, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnBattleMoveEnd:
                    CharacterController controller1 = (CharacterController)param[0];
                    if (IsCharacterInGroup(controller1))
                    {
                        OnArriveDestination();
                    }
                    break;
                case (int)EventID.OnBattleAtkEnd:
                    CharacterController controller2 = (CharacterController)param[0];
                    if (IsCharacterInGroup(controller2))
                    {
                        OnAtkEnd();
                    }
                    break;
                case (int)EventID.OnBattleAtkEvent:
                    CharacterController controller3 = (CharacterController)param[0];
                    if (controller3 == m_OurCharacter && m_EnemyCharacter.IsDead() == false)
                    {
                        m_AtkEventIndex++;
                        m_AtkEventIndex = Mathf.Clamp(m_AtkEventIndex, 0, m_OurHitBackDistance.Count - 1);
                        m_EnemyCharacter.GetBattleState().HitbackDistance = Mathf.Max(m_OurHitBackDistance[m_AtkEventIndex] - m_OurHitBackDistance[0], 0);
                        m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Attacked);
                    }
                    else if (controller3 == m_EnemyCharacter && m_OurCharacter.IsDead() == false)
                    {
                        m_AtkEventIndex++;
                        m_AtkEventIndex = Mathf.Clamp(m_AtkEventIndex, 0, m_EnemyHitBackDistance.Count - 1);
                        m_OurCharacter.GetBattleState().HitbackDistance = Mathf.Max(m_EnemyHitBackDistance[m_AtkEventIndex] - m_EnemyHitBackDistance[0], 0);
                        m_OurCharacter.GetBattleState().SetState(BattleStateID.Attacked);
                    }
                    break;
            }
        }

        private bool IsCharacterInGroup(CharacterController character)
        {
            return character == m_OurCharacter || character == m_EnemyCharacter;
        }
    }
	
}