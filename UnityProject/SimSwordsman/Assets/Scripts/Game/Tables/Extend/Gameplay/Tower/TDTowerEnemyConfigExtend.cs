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

        public List<int> enemyIDs => m_EnemyIds;

        public void Reset()
        {
            m_EnemyIds = Helper.String2ListInt(enemies, ";");
        }
    }
}