using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class ArenaSystem : MonoBehaviour, IMgr
    {
        private ArenaData m_ArenaData;

        public void OnInit()
        {
            m_ArenaData = GameDataMgr.S.GetPlayerData().arenaData;
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {

        }

        public void Enter()
        {
            m_ArenaData.Init();
        }

        public void StartLevel(List<CharacterController> owerCharacter, List<HerbType> useHerbs, ArenaCellToSend arg)
        {
            m_ArenaData.AddChallengeCount(-1);
            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ArenaPanel);

            List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();

            var levelData = m_ArenaData.GetArenaEnemyDBByIndex(arg.enemyData.level - 1);
            for (int i = 0; i < levelData.EnemyLst.Count; i++)
            {
                var enemy = levelData.EnemyLst[i];
                enemyConfigs.Add(new CharacterEnemyConfig(enemy.quality, enemy.headId, enemy.bodyId, (int)(arg.recommendAtk / 5)));
            }

            ArenaLevelConfig arenaLevelConfig = new ArenaLevelConfig();
            arenaLevelConfig.level = arg.enemyData.level;
            arenaLevelConfig.enemyConfigs = enemyConfigs;

            EventSystem.S.Send(EventID.OnEnterBattle, enemyConfigs, owerCharacter, useHerbs);
            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, PanelType.Arena, arenaLevelConfig);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengeBattlePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
        }

        public void PassLevel(int level)
        {
            if (m_ArenaData.nowLevel < level)
                return;

            m_ArenaData.SetNowLevel(level);

        }
    }

    public class ArenaLevelConfig
    {
        public int level;

        public List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();

        public void PrepareReward()
        {
            List<RewardBase> rewardLst = new List<RewardBase>() { new ArenaCoinReward(20) };
            rewardLst.ForEach(r => r.AcceptReward());
            EventSystem.S.Send(EventID.OnReciveRewardList, rewardLst);
        }
    }
}