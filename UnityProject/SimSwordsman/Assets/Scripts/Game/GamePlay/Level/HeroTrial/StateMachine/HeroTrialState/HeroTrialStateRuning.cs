using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using DG.Tweening;

namespace GameWish.Game
{
    public class HeroTrialStateRuning : HeroTrialState
    {
        private HeroTrialMgr m_HeroTialMgr = null;

        private CharacterController m_OurCharacter = null;
        private CharacterController m_EnemyCharacter = null;
        private CharacterController m_CachedEnemyCharacter = null;
        private FightGroup m_FightGroup = null;

        private int m_EnemyHP = 100;

        private int m_RoundCount = 0;

        private int m_OrdinaryEnemyKilledCount = 0;
        private int m_OrdinaryEnemyCount = 3;
        private bool m_SpawnOrdinaryEnemy = true;

        private Vector3 m_OurInitPos, m_EnemyInitPos;

        private float m_EnemyCachedTime = 0;
        private float m_EnemyCachedMaxTime = 1f;

        public HeroTrialStateRuning(HeroTrialStateID stateEnum) : base(stateEnum)
        {

        }

        public override void Enter(IHeroTrialStateHander handler)
        {
            m_HeroTialMgr = handler.GetHeroTrialMgr();

            // Reset Data
            m_RoundCount = 0;
            m_OrdinaryEnemyKilledCount = 0;
            m_OrdinaryEnemyCount = 3;
            m_SpawnOrdinaryEnemy = true;
            m_OurInitPos = m_HeroTialMgr.BattleField.GetOurCharacterPos();
            m_EnemyInitPos = m_HeroTialMgr.BattleField.GetEnemyCharacterPos();

            // Spawn Characters
            m_OurCharacter = SpawnOurCharacter(m_HeroTialMgr.DbData.characterId);
            m_OurCharacter.CharacterModel.SetHp(m_OurCharacter.CharacterModel.GetBaseAtkValue());

            int enemyId = GetEnemyId();
            m_EnemyCharacter = SpawnEnemyCharacter(enemyId);

            m_HeroTialMgr.FightGroup = new FightGroup(1, m_OurCharacter, m_EnemyCharacter);
            m_HeroTialMgr.FightGroup.StartFight();

            RegisterEvent();
        }

        public override void Exit(IHeroTrialStateHander handler)
        {
            UnregisterEvent();

            //m_EnemyCharacter.CharacterView.PlayDeadAnim();
            //m_EnemyCharacter.CharacterView.PlayIdleAnim();

            if (m_CachedEnemyCharacter != null)
            {
                ReleaseCachedEnemy();
            }
        }

        public override void Execute(IHeroTrialStateHander handler, float dt)
        {
            m_OurCharacter?.RefreshBattleState();
            m_EnemyCharacter?.RefreshBattleState();

            if (m_CachedEnemyCharacter != null)
            {
                m_EnemyCachedTime += Time.deltaTime;
                if (m_EnemyCachedTime > m_EnemyCachedMaxTime)
                {
                    m_EnemyCachedTime = 0;
                    ReleaseCachedEnemy();
                }
            }
        }

        private void ReleaseCachedEnemy()
        {
            GameObject.Destroy(m_CachedEnemyCharacter.CharacterView.gameObject);
            m_CachedEnemyCharacter = null;
        }

        private CharacterController SpawnEnemyCharacter(int id)
        {
            CharacterController enemy = null;

            EnemyLoader.S.LoadEnemySync(id, (obj) =>
            {
                obj.name = "Character_Enemy" + id;
                obj.transform.parent = m_HeroTialMgr.BattleField.transform;

                CharacterView characterView = obj.GetComponent<CharacterView>();
                CharacterController controller = new CharacterController(id, characterView, CharacterStateID.Battle, CharacterCamp.EnemyCamp);
                controller.OnEnterBattleField(m_EnemyInitPos);
                controller.CharacterModel.SetHp(m_EnemyHP);

                enemy = controller;

            });

            return enemy;
        }

        private CharacterController SpawnOurCharacter(int id)
        {
            CharacterController controller = null;

            CharacterItem characterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(id);

            GameObject go = CharacterLoader.S.GetCharacterGo(id, characterItem.quality, characterItem.bodyId);
            if (go != null)
            {
                go.transform.SetParent(m_HeroTialMgr.BattleField.transform);
                CharacterView characterView = go.GetComponent<CharacterView>();
                controller = new CharacterController(id, characterView, CharacterStateID.Battle);
                controller.OnEnterBattleField(m_OurInitPos);
            }
            else
            {
                Log.e("SpawnCharacterController return null");
            }

            return controller;
        }

        private int GetEnemyId()
        {
            HeroTrialConfig config = TDHeroTrialConfigTable.GetConfig(m_HeroTialMgr.DbData.clanType);
            int[] enemyIds;
            if (m_SpawnOrdinaryEnemy)
            {
                enemyIds = config.ordinaryEnemies;
            }
            else
            {
                enemyIds = config.eliteEnemies;
            }

            int index = UnityEngine.Random.Range(0, enemyIds.Length);

            return enemyIds[index];
        }

        private void RegisterEvent()
        {
            EventSystem.S.Register(EventID.OnOneRoundEnd, HandleEvent);
            EventSystem.S.Register(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void UnregisterEvent()
        {
            EventSystem.S.UnRegister(EventID.OnOneRoundEnd, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnCharacterInFightGroupDead, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnOneRoundEnd:            
                    m_RoundCount++;
                    if (m_RoundCount > 2) // This enemy should be killed
                    {
                        Log.i("Enemy should be dead");

                        m_RoundCount = 0;

                        m_HeroTialMgr.FightGroup.EnemyCharacter.CacheDamage(m_EnemyHP);

                        m_OrdinaryEnemyKilledCount++;

                        m_SpawnOrdinaryEnemy = m_OrdinaryEnemyKilledCount <= m_OrdinaryEnemyCount;
                    }
                    break;

                case (int)EventID.OnCharacterInFightGroupDead:

                    if (m_HeroTialMgr.GetLeftTime() <= 0)
                    {
                        Log.i("Time over, finish trial");
                        m_HeroTialMgr.FinishTrial();
                    }
                    else
                    {
                        Log.i("Spawn a new enemy");
                        m_CachedEnemyCharacter = m_EnemyCharacter;

                        int nextEnemyId = GetEnemyId();
                        m_EnemyCharacter = SpawnEnemyCharacter(nextEnemyId);
                        m_HeroTialMgr.FightGroup.ChangeEnemyCharacter(m_EnemyCharacter);
                        m_HeroTialMgr.FightGroup.StartFight();

                        if (m_SpawnOrdinaryEnemy == false)
                        {
                            m_SpawnOrdinaryEnemy = true;
                            m_OrdinaryEnemyKilledCount = 0;
                        }

                    }
                    break;

            }
        }

    }
}
