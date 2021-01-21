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

        private float[] m_OurHitBackDistance = new float[] { 0.3f };
        private float[] m_EnemyHitBackDistance = new float[] { 0.3f, 0.3f, 0.3f, 0.3f };

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
            // Who will attack
            if (m_OurCharacter.IsDead())
            {
                m_EnemyAttack = true;
            }
            else if (m_EnemyCharacter.IsDead())
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
            if (m_EnemyAttack)
            {
                m_EnemyCharacter.GetBattleState().SetNextAtkAnimName();
                atkAnimName = m_EnemyCharacter.GetBattleState().NextAtkAnimName;
            }
            else
            {
                m_OurCharacter.GetBattleState().SetNextAtkAnimName();
                atkAnimName = m_OurCharacter.GetBattleState().NextAtkAnimName;
            }
            Log.e("Test--------------, atk anim name is: " + atkAnimName);
            if(string.IsNullOrEmpty(atkAnimName))
            {
                Log.e("Atk anim name is empty: " + atkAnimName);
                return;
            }

            float attackRange = TDKongfuAnimationConfigTable.GetAnimConfig(atkAnimName).atkRangeList[0];
            attackRange = 1f;
            // Move to random position
            StartToMove(attackRange);
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
            //m_EnemyCharacter.CharacterView.PlayWalkAnim();
            //Vector2 pos1 = new Vector2(x1, y);
            //float distance1 = Vector2.Distance(m_EnemyCharacter.GetPosition(), pos1);
            //m_EnemyCharacter.CharacterView.transform.DOMove(pos1, distance1/m_EnemyCharacter.GetMoveSpeed()).SetEase(Ease.Linear).OnComplete(()=> 
            //{
            //    m_EnemyCharacter.CharacterView.PlayIdleAnim();
            //    OnArriveDestination();
            //});

            m_OurCharacter.GetBattleState().MoveTargetPos = new Vector2(x2, y);
            m_OurCharacter.GetBattleState().SetState(BattleStateID.Move);
            //m_OurCharacter.CharacterView.PlayWalkAnim();
            //Vector2 pos2 = new Vector2(x2, y);
            //float distance2 = Vector2.Distance(m_OurCharacter.GetPosition(), pos2);
            //m_OurCharacter.CharacterView.transform.DOMove(pos2, distance2 / m_OurCharacter.GetMoveSpeed()).SetEase(Ease.Linear).OnComplete(() => 
            //{
            //    m_OurCharacter.CharacterView.PlayIdleAnim();
            //    OnArriveDestination();
            //});
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

        //private void OnAtkAnimEnd()
        //{
        //    Debug.LogError("OnAtkAnimEnd");

        //    if (m_EnemyAttack)
        //    {
        //        m_EnemyCharacter.CharacterView.PlayIdleAnim();
        //        m_OurCharacter.TriggerCachedDamage();
        //    }
        //    else
        //    {
        //        m_EnemyCharacter.TriggerCachedDamage();
        //    }

        //    if (m_OurCharacter.IsDead())
        //    {

        //    }

        //    if (m_EnemyCharacter.IsDead())
        //    {

        //    }

        //    NextRound();
        //}
        //public void AddOtherOurCharacter(CharacterController character)
        //{
        //    if (!otherOurCharacterList.Contains(character))
        //    {
        //        otherOurCharacterList.Add(character);

        //        character.SetFightGroup(this);
        //        character.SetFightTarget(enemyMainCharacter);
        //    }
        //}

        //public void AddOtherEnemyCharacter(CharacterController character)
        //{
        //    if (!otherEnemyCharacterList.Contains(character))
        //    {
        //        otherEnemyCharacterList.Add(character);

        //        character.SetFightGroup(this);
        //        character.SetFightTarget(ourMainCharacter);
        //    }
        //}

        //public void SetOurMainCharacter(CharacterController character)
        //{
        //    ourMainCharacter = character;

        //    if (otherOurCharacterList.Contains(character))
        //    {
        //        otherOurCharacterList.Remove(character);
        //    }

        //    enemyMainCharacter.SetFightTarget(character);
        //    otherEnemyCharacterList.ForEach(i => 
        //    {
        //        i.SetFightTarget(character);
        //    });
        //}

        //public void SetEnemyMainCharacter(CharacterController character)
        //{
        //    enemyMainCharacter = character;

        //    if (otherEnemyCharacterList.Contains(character))
        //    {
        //        otherEnemyCharacterList.Remove(character);
        //    }

        //    ourMainCharacter.SetFightTarget(character);
        //    otherOurCharacterList.ForEach(i =>
        //    {
        //        i.SetFightTarget(character);
        //    });
        //}
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
                    if (controller3 == m_OurCharacter)
                    {
                        int index = 1;
                        m_EnemyCharacter.GetBattleState().HitbackDistance = m_OurHitBackDistance[index - 1];
                        m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Attacked);
                    }
                    else if (controller3 == m_EnemyCharacter)
                    {
                        int index = 1;
                        m_OurCharacter.GetBattleState().HitbackDistance = m_EnemyHitBackDistance[index - 1];
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