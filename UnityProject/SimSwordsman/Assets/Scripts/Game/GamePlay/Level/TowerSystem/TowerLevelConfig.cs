using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TowerLevelConfig
    {
        public int level;

        public List<EnemyConfig> enemiesList = new List<EnemyConfig>();
        public int rewardExp = 0;

        public TowerLevelConfig(int level)
        {
            this.level = level;
        }

        public void CreateEnemy(List<int> enemys, float basicATK)
        {
            enemiesList = CreateEnemysByConfig(enemys, basicATK);
            GameDataMgr.S.GetPlayerData().towerData.SetEnemy(enemiesList);
        }

        private List<EnemyConfig> CreateEnemysByConfig(List<int> enemyIDs, float basicATK)
        {
            List<EnemyConfig> enemys = new List<EnemyConfig>();

            for (int i = 0; i < enemyIDs.Count; i++)
            {
                EnemyConfig enemy = new EnemyConfig(enemyIDs[i], 1, (int)(i == 0 ? basicATK * 1.5f : basicATK));
                enemys.Add(enemy);
            }
            return enemys;
        }

        public void SetEnemyFormDB(float basicATK)
        {
            enemiesList = GameDataMgr.S.GetPlayerData().towerData.GetEnemys(basicATK);
        }

        public void PrepareReward()
        {

            //从初始数据里面取出数据
            var levelConfigDB = GameDataMgr.S.GetPlayerData().towerData.GetLevelConfigByIndex(level - 1);
            if (!string.IsNullOrEmpty(levelConfigDB.reward))
            {
                var reward = RewardMgr.S.GetRewardBase(levelConfigDB.reward);
                List<RewardBase> rewardLst = new List<RewardBase>() { reward };
                rewardLst.ForEach(r => r.AcceptReward());
                EventSystem.S.Send(EventID.OnReciveRewardList, rewardLst);
            }
        }
    }

}