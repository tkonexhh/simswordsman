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
        public int rewardExp = 100;

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
                //TODO 分配战力值
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
            //判断是什么奖励
            // var towerConf = TDTowerConfigTable.GetData(level);
            // if (towerConf == null)
            //     return;

            // if (towerConf.rwardtype.Equals("Fcoin"))
            // {
            //     List<RewardBase> rewardLst = new List<RewardBase>() { new TowerCoinReward(towerConf.fcoinNum) };
            //     rewardLst.ForEach(r => r.AcceptReward());
            //     EventSystem.S.Send(EventID.OnReciveRewardList, rewardLst);
            // }
            // else
            // {
            //     int lobbyLvl = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            //     var conf = TDTowerRewardConfigTable.GetData(lobbyLvl);
            //     if (conf == null)
            //         return;

            //     var reward = conf.GetRandomReward();
            //     List<RewardBase> rewardLst = new List<RewardBase>() { reward };
            //     rewardLst.ForEach(r => r.AcceptReward());
            //     EventSystem.S.Send(EventID.OnReciveRewardList, rewardLst);
            // }

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