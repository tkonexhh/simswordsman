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


        public int maxLevel => m_TowerData.maxLevel;


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

        public void EnterTower()
        {
            DataAnalysisMgr.S.CustomEvent(DotDefine.Tower_Enter);
            GameDataMgr.S.GetPlayerData().towerData.Init();
        }

        public void StartLevel(List<CharacterController> owerCharacter, float basicATK)
        {
            m_IsTowerBattle = true;
            DataAnalysisMgr.S.CustomEvent(DotDefine.Tower_Battle_Enter);
            UIMgr.S.ClosePanelAsUIID(UIID.MainMenuPanel);
            UIMgr.S.ClosePanelAsUIID(UIID.TowerPanel);

            int maxLvl = m_TowerData.maxLevel;
            TowerLevelConfig levelConfig = new TowerLevelConfig(maxLvl);

            var enemyPool = m_TowerData.GetLevelConfigByIndex(maxLvl - 1).enemyConfig;
            if (m_TowerData.enemyCharacterLst.Count == 0)
            {
                // Debug.LogError("New Enemy");
                levelConfig.CreateEnemy(enemyPool.enemyIDLst, basicATK);
            }
            else
            {
                // Debug.LogError("Load Enemy");
                levelConfig.SetEnemyFormDB(basicATK);
            }

            // Debug.LogError(levelConfig.enemiesList.Count);
            //????????????
            EventSystem.S.Send(EventID.OnEnterBattle, levelConfig.enemiesList, owerCharacter);
            UIMgr.S.OpenPanel(UIID.CombatInterfacePanel, PanelType.Tower, levelConfig);
        }

        public void PassLevel()
        {
            GameDataMgr.S.GetPlayerData().towerData.AddMaxLevel();
            GameDataMgr.S.GetPlayerData().towerData.ClearEnemyDB();
        }

        private void HandleBattleCharacterHp(List<CharacterController> owrControllers, List<CharacterController> enemyControllers)
        {
            if (!m_IsTowerBattle) return;

            //??????????????????
            owrControllers.ForEach(character =>
            {
                int id = character.CharacterId;
                bool hasID = m_TowerData.HasTowerCharacter(id);
                if (hasID)
                {
                    var towerCharacterDB = m_TowerData.GetTowerCharacterByID(id);
                    if (towerCharacterDB != null)
                    {
                        // Debug.LogError(towerCharacterDB.hpRate);
                        character.CharacterModel.SetHp(character.CharacterModel.GetHp() * towerCharacterDB.hpRate);
                    }
                }
                else
                {
                    m_TowerData.AddTowerCharacter(id);
                }
            });

            //??????????????????
            for (int i = 0; i < enemyControllers.Count; i++)
            {
                double hpRate = m_TowerData.GetEnemyByIndex(i).hpRate;
                // Debug.LogError(i + ":" + hpRate);
                if (hpRate <= 0)
                {
                    //????????????
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

            //??????????????????
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


        #region ??????????????????
        public void BuyShopItem(int index, TowerShopItemInfo itemInfo)
        {
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(itemInfo.reward);
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            itemInfo.reward.AcceptReward();
            GameDataMgr.S.GetPlayerData().towerData.BuyShopData(index);
            DataAnalysisMgr.S.CustomEvent(DotDefine.Tower_Shop_Buy, itemInfo.id);
        }
        #endregion

        #region ??????????????????
        public bool CanAddNewCharacter()
        {
            return GameDataMgr.S.GetPlayerData().towerData.towerCharacterLst.Count < TowerDefine.MAX_CHARACT_NUM;
        }

        #endregion
    }


}