using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDTowerEnemyConfig
    {
        private List<int> m_EnemyIds;
        public void Reset()
        {
            m_EnemyIds = Helper.String2ListInt(enemies, ";");
        }

        public List<EnemyConfig> GetRandomEnemys()
        {
            List<EnemyConfig> enemys = new List<EnemyConfig>();
            EnemyConfig enemyBoss = new EnemyConfig(int.Parse(boss), 1, 200);
            enemys.Add(enemyBoss);
            //随机添加四个敌人
            for (int i = 0; i < 4; i++)
            {
                EnemyConfig enemy = new EnemyConfig(m_EnemyIds[RandomHelper.Range(0, m_EnemyIds.Count)], 1, 200);
                enemys.Add(enemy);
            }
            return enemys;
        }
    }
}