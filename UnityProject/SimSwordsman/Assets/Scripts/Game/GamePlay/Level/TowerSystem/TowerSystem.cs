using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TowerSystem : MonoBehaviour, IMgr
    {
        private TowerData m_TowerData;
        private bool m_IsTowerBattle = false;

        public void OnInit()
        {
            MainGameMgr.S.BattleFieldMgr.onSpawnOwerCharacterComplete += HandleBattleCharacterHp;
            MainGameMgr.S.BattleFieldMgr.onBattleExit += OnBattleExit;
            m_TowerData = GameDataMgr.S.GetPlayerData().towerData;
        }

        public void OnUpdate()
        {

        }

        public void OnDestroyed()
        {
            MainGameMgr.S.BattleFieldMgr.onSpawnOwerCharacterComplete -= HandleBattleCharacterHp;
            MainGameMgr.S.BattleFieldMgr.onBattleExit -= OnBattleExit;
        }

        public void StartLevel()
        {
            m_IsTowerBattle = true;

            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.TowerPanel);

            int maxLvl = GameDataMgr.S.GetPlayerData().towerData.maxLevel;
            TowerLevelConfig levelConfig = new TowerLevelConfig(maxLvl);

            int enemyPoolID = GameDataMgr.S.GetPlayerData().towerData.GetEnemyPoolIDByIndex(maxLvl);
            if (m_TowerData.enemyCharacterLst.Count == 0)
            {
                // Debug.LogError("New Enemy");
                levelConfig.CreateEnemy(enemyPoolID);
            }
            else
            {
                // Debug.LogError("Load Enemy");
                levelConfig.SetEnemyFormDB();
            }

            List<CharacterController> owrCharacter = new List<CharacterController>();
            for (int i = 0; i < 5; i++)
            {
                var character = MainGameMgr.S.CharacterMgr.GetCharacterController(i);
                if (character != null)
                    owrCharacter.Add(character);
            }

            //随机敌人
            EventSystem.S.Send(EventID.OnEnterBattle, levelConfig.enemiesList, owrCharacter);
            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, PanelType.Tower, levelConfig);
        }

        public void PassLevel()
        {
            GameDataMgr.S.GetPlayerData().towerData.AddMaxLevel();
            GameDataMgr.S.GetPlayerData().towerData.ClearEnemyDB();
        }

        public void GetLevelReward(int level)
        {
            var conf = TDTowerConfigTable.GetData(level);
            if (conf == null)
                return;

            int coin = conf.fcoinNum;
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(new TowerCoinReward(coin));
            rewards.ForEach(r => r.AcceptReward());
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            //得到伏魔币奖励
            GameDataMgr.S.GetPlayerData().towerData.GetReward(level);
        }


        private void HandleBattleCharacterHp(List<CharacterController> owrControllers, List<CharacterController> enemyControllers)
        {
            if (!m_IsTowerBattle) return;

            //处理己方血量
            owrControllers.ForEach(character =>
            {
                int id = character.CharacterId;
                bool hasID = m_TowerData.HasTowerCharacter(id);
                if (hasID)
                {
                    var towerCharacterDB = m_TowerData.GetTowerCharacterByID(id);
                    if (towerCharacterDB != null)
                    {
                        // character.CharacterModel.SetHp(character.CharacterModel.GetHp() * towerCharacterDB.hpRate);
                    }
                }
                else
                {
                    m_TowerData.AddTowerCharacter(id);
                }
            });

            //处理敌人血量
            for (int i = 0; i < enemyControllers.Count; i++)
            {
                double hpRate = m_TowerData.GetEnemyByIndex(i).hpRate;
                // Debug.LogError(i + ":" + hpRate);
                if (hpRate <= 0)
                {
                    //移除敌人
                    enemyControllers[i].GetBattleState().SetState(BattleStateID.Dead);
                    enemyControllers[i].CharacterModel.SetHp(0);
                    enemyControllers[i].CharacterView.gameObject.SetActive(false);
                }
                else
                {
                    enemyControllers[i].CharacterModel.SetHp(enemyControllers[i].CharacterModel.GetHp() * hpRate);
                }
            }
        }

        private void OnBattleExit(List<CharacterController> owrControllers, List<CharacterController> enemyControllers)
        {
            if (!m_IsTowerBattle) return;
            m_IsTowerBattle = false;

            //保存人物血量
            owrControllers.ForEach(character =>
            {
                int id = character.CharacterId;
                var towerCharacterDB = m_TowerData.GetTowerCharacterByID(id);
                if (towerCharacterDB != null)
                {
                    double rate = character.CharacterModel.GetHp() / character.CharacterModel.GetMaxHp();
                    m_TowerData.SetHpRate(id, rate);
                }
            });

            for (int i = 0; i < enemyControllers.Count; i++)
            {
                double rate = enemyControllers[i].CharacterModel.GetHp() / enemyControllers[i].CharacterModel.GetMaxHp();
                // Debug.LogError("Exit" + i + ":" + rate);
                m_TowerData.SetEnemyHpRate(i, rate);
            }
        }
    }


}