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
        private CharacterController m_EnemyController = null;

        private FightGroup m_FightGroup = null;

        private int m_RoundCount = 0;

        private int m_OrdinaryEnemyKilledCount = 0;
        private int m_OrdinaryEnemyCount = 3;
        private bool m_ShouldSpawnOrdinaryEnemy = true;

        private Vector3 m_OurInitPos, m_EnemyInitPos;

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
            m_ShouldSpawnOrdinaryEnemy = true;

            // Spawn Characters
            CharacterController ourCharacter = SpawnOurCharacter(m_HeroTialMgr.DbData.characterId);
            ourCharacter.CharacterModel.SetHp(ourCharacter.CharacterModel.GetBaseAtkValue());

            int enemyId = GetEnemyId(m_RoundCount);
            CharacterController enemyCharacter = SpawnEnemyCharacter(enemyId);
            enemyCharacter.CharacterModel.SetHp(100);

            m_HeroTialMgr.FightGroup = new FightGroup(1, ourCharacter, enemyCharacter);
            m_HeroTialMgr.FightGroup.StartFight();

            RegisterEvent();
        }

        public override void Exit(IHeroTrialStateHander handler)
        {
            UnregisterEvent();
        }

        public override void Execute(IHeroTrialStateHander handler, float dt)
        {

            
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
                CharacterView characterView = go.GetComponent<CharacterView>();
                controller = new CharacterController(id, characterView, CharacterStateID.Battle);
            }
            else
            {
                Log.e("SpawnCharacterController return null");
            }

            return controller;
        }

        private int GetEnemyId(int roundCount)
        {
            HeroTrialConfig config = TDHeroTrialConfigTable.GetConfig(m_HeroTialMgr.DbData.clanType);
            int[] enemyIds;
            if (m_ShouldSpawnOrdinaryEnemy)
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
        }

        private void UnregisterEvent()
        {
            EventSystem.S.UnRegister(EventID.OnOneRoundEnd, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            if (key == (int)EventID.OnOneRoundEnd)
            {
                m_RoundCount++;

                if (m_RoundCount > 2) // This enemy should be killed
                {
                    m_RoundCount = 0;

                    m_HeroTialMgr.FightGroup.EnemyCharacter.CharacterModel.SetHp(0);

                    m_OrdinaryEnemyKilledCount++;

                    m_ShouldSpawnOrdinaryEnemy = m_OrdinaryEnemyKilledCount <= m_OrdinaryEnemyCount;
                }
            }
        }

    }
}
