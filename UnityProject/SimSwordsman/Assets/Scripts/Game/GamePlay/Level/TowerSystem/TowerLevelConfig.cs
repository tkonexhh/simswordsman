using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TowerLevelConfig
    {
        public int level;
        // public int enemyPoolID;
        public List<EnemyConfig> enemiesList = new List<EnemyConfig>();
        public int rewardExp = 100;

        public TowerLevelConfig(int level)
        {
            this.level = level;
        }

        public void CreateEnemy(int enemyPoolID)
        {
            var enemyConfig = TDTowerEnemyConfigTable.GetData(enemyPoolID);
            enemiesList = enemyConfig.GetRandomEnemys();
            GameDataMgr.S.GetPlayerData().towerData.SetEnemy(enemiesList);
        }

        public void SetEnemyFormDB()
        {
            enemiesList = GameDataMgr.S.GetPlayerData().towerData.GetEnemys();
        }

        public void PrepareReward()
        {
            int lobbyLvl = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            var conf = TDTowerRewardConfigTable.GetData(lobbyLvl);
            if (conf == null)
                return;

            var reward = conf.GetRandomReward();
            List<RewardBase> rewardLst = new List<RewardBase>() { reward };
            rewardLst.ForEach(r => r.AcceptReward());
            EventSystem.S.Send(EventID.OnReciveRewardList, rewardLst);
        }
    }

}