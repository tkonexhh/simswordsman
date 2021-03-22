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
        private int m_AtkEventIndex = 0;

        private List<float> m_OurHitBackDistance = new List<float>();
        private List<float> m_EnemyHitBackDistance = new List<float>();

        public CharacterController OurCharacter { get => m_OurCharacter; }
        public CharacterController EnemyCharacter { get => m_EnemyCharacter; }
        public Dictionary<string, KongfuAnimConfig> m_KongfuAnimMap = new Dictionary<string, KongfuAnimConfig>();
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
            ParpareEffectPool();
        }

        public void Release()
        {
            m_EnemyCharacter.FightGroup = null;
            m_OurCharacter.FightGroup = null;

            UnregisterEvents();
            // ReleaseEffectPool();
        }

        private void ParpareEffectPool()
        {
            var ourkongfus = m_OurCharacter.CharacterModel.GetKongfuTypeList();
            ourkongfus.Add(KungfuType.Attack);
            foreach (var kongfu in ourkongfus)
            {
                var config = TDKongfuAnimationConfigTable.GetAnimConfig((int)kongfu);
                AddKongfuToPool(config);
            }

            var enemyConfig = TDEnemyConfigTable.GetEnemyInfo(m_EnemyCharacter.CharacterModel.Id);
            var enemykongfus = enemyConfig.animNameList;
            foreach (var kongfu in enemykongfus)
            {
                var config = TDKongfuAnimationConfigTable.GetAnimConfig(kongfu);
                AddKongfuToPool(config);
            }
        }

        private void ReleaseEffectPool()
        {
            var enemyConfig = TDEnemyConfigTable.GetEnemyInfo(m_EnemyCharacter.CharacterModel.Id);
            var enemykongfus = enemyConfig.kongfuNameList;
            foreach (var kongfu in enemykongfus)
            {
                var config = TDKongfuAnimationConfigTable.GetAnimConfig(kongfu);
                config?.ReleaseEffectPool();
            }

            m_KongfuAnimMap.Clear();
        }

        private void AddKongfuToPool(KongfuAnimConfig kongfu)
        {
            if (kongfu == null)
                return;
            kongfu.ParpareEffectPool();
            m_KongfuAnimMap.Add(kongfu.animName, kongfu);
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
                //int random = Random.Range(0, 100);
                //if (random > 50)
                //{
                //    m_EnemyAttack = true;
                //}
                //else
                //{
                //    m_EnemyAttack = false;
                //}
                m_EnemyAttack = !m_EnemyAttack;
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
            if (string.IsNullOrEmpty(atkAnimName))
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
            float maxDeltaX = 1;
            float maxDeltaY = 0.5f;

            float x1 = Random.Range(m_OurCharacter.GetPosition().x - maxDeltaX, m_OurCharacter.GetPosition().x + maxDeltaX);
            float y = Random.Range(m_OurCharacter.GetPosition().y - maxDeltaY, m_OurCharacter.GetPosition().y + maxDeltaY);
            x1 = Mathf.Clamp(x1, MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x, MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x);
            y = Mathf.Clamp(y, MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.y, MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.y);

            float x2 = Random.Range(0, 100) > 50 ? x1 + attackRange : x1 - attackRange;
            if (x2 < MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x)
            {
                x2 = x1 + attackRange;
            }
            if (x2 > MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x)
            {
                x2 = x1 - attackRange;
            }
            // Debug.Log("x1 is: " + x1 + " x2 is: " + x2 + " left x is: " + MainGameMgr.S.BattleFieldMgr.BattleAreaLeftBottom.x + " right x is: " + MainGameMgr.S.BattleFieldMgr.BattleAreaRightTop.x);

            m_EnemyCharacter.GetBattleState().MoveTargetPos = new Vector2(x1, y);
            m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Move);

            m_OurCharacter.GetBattleState().MoveTargetPos = new Vector2(x2, y);
            m_OurCharacter.GetBattleState().SetState(BattleStateID.Move);

        }

        private void StartToFight()
        {
            if (m_EnemyAttack)
            {
                //m_EnemyCharacter.CharacterView.PlayAtkAnim("attack1", OnAtkAnimEnd);
                m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Attack);
            }
            else
            {
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
                    //Debug.LogError("someone is dead");
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
            EventSystem.S.Register(EventID.OnBattleAtkStart, HandleEvent);
            EventSystem.S.Register(EventID.OnBattleAtkEnd, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnBattleMoveEnd, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnBattleAtkEvent, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnBattleAtkStart, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnBattleAtkEnd, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnBattleMoveEnd:
                    {
                        CharacterController controller = (CharacterController)param[0];
                        if (IsCharacterInGroup(controller))
                        {
                            OnArriveDestination();
                        }
                    }
                    break;
                case (int)EventID.OnBattleAtkStart:
                    {
                        CharacterController controller = (CharacterController)param[0];
                        string skillname = (string)param[1];
                        KongfuAnimConfig animConfig;
                        m_KongfuAnimMap.TryGetValue(skillname, out animConfig);
                        // Debug.LogError(skillname + "--" + animConfig);
                        animConfig?.PlayAttackEffect(controller.CharacterView.Body.transform);
                        animConfig?.PlayFootEffect(controller.CharacterView.BoneFollower_Foot);
                        animConfig?.PlayAttackSound();

                        CharacterController targetHurtController = null;
                        if (controller == m_OurCharacter)
                        {
                            targetHurtController = m_EnemyCharacter;
                        }
                        else if (controller == m_EnemyCharacter)
                        {
                            targetHurtController = m_OurCharacter;
                        }
                        if (targetHurtController != null)
                        {
                            // animConfig?.PlayHurtEffect(MainGameMgr.S.transform, targetHurtController.CharacterView.transform.position);
                            animConfig?.PlayHurtEffect(targetHurtController.CharacterView.Body.transform, Vector3.zero);
                        }

                    }
                    break;
                case (int)EventID.OnBattleAtkEnd://战斗动画结束
                    {
                        CharacterController controller = (CharacterController)param[0];
                        if (IsCharacterInGroup(controller))
                        {
                            OnAtkEnd();
                        }
                    }
                    break;

                case (int)EventID.OnBattleAtkEvent://战斗伤害事件
                    {
                        CharacterController controller = (CharacterController)param[0];
                        string skillname = (string)param[1];

                        KongfuAnimConfig animConfig;
                        m_KongfuAnimMap.TryGetValue(skillname, out animConfig);
                        //TODO 改为读表获取硬直时间
                        float hurtTime = animConfig == null ? 0.1f : animConfig.hitDelayTime;
                        if (controller == m_OurCharacter && m_EnemyCharacter.IsDead() == false)
                        {
                            m_AtkEventIndex++;
                            m_AtkEventIndex = Mathf.Clamp(m_AtkEventIndex, 0, m_OurHitBackDistance.Count - 1);
                            m_EnemyCharacter.GetBattleState().HitbackDistance = Mathf.Max(m_OurHitBackDistance[m_AtkEventIndex] - m_OurHitBackDistance[0], 0);
                            m_EnemyCharacter.GetBattleState().NextHurtTime = hurtTime;
                            m_EnemyCharacter.GetBattleState().SetState(BattleStateID.Attacked);
                            HitBack(m_EnemyCharacter, m_EnemyCharacter.GetBattleState().HitbackDistance);
                        }
                        else if (controller == m_EnemyCharacter && m_OurCharacter.IsDead() == false)
                        {
                            m_AtkEventIndex++;
                            m_AtkEventIndex = Mathf.Clamp(m_AtkEventIndex, 0, m_EnemyHitBackDistance.Count - 1);
                            m_OurCharacter.GetBattleState().HitbackDistance = Mathf.Max(m_EnemyHitBackDistance[m_AtkEventIndex] - m_EnemyHitBackDistance[0], 0);
                            m_OurCharacter.GetBattleState().NextHurtTime = hurtTime;
                            m_OurCharacter.GetBattleState().SetState(BattleStateID.Attacked);
                            HitBack(m_OurCharacter, m_OurCharacter.GetBattleState().HitbackDistance);
                        }
                    }
                    break;
            }
        }

        private void HitBack(CharacterController controller, float distance)
        {
            controller.CharacterView.transform.DOMoveX(-controller.CharacterView.GetFaceDir() * distance, 0.1f).SetRelative().SetEase(Ease.Linear);
        }

        private bool IsCharacterInGroup(CharacterController character)
        {
            return character == m_OurCharacter || character == m_EnemyCharacter;
        }
    }

}