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

        public void StartLevel(List<CharacterController> owerCharacter, List<HerbType> useHerbs)
        {
            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ArenaPanel);

            List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();
            enemyConfigs.Add(new EnemyConfig(101, 3, 500));
            EventSystem.S.Send(EventID.OnEnterBattle, enemyConfigs, owerCharacter, useHerbs);
            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, PanelType.Arena, enemyConfigs);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.ChallengeBattlePanel);
            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
        }
    }

}