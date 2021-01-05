using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDEnemyConfigTable
    {
        public static Dictionary<int, EnemyInfo> enemyDic = new Dictionary<int, EnemyInfo>();

        static void CompleteRowAdd(TDEnemyConfig tdData)
        {
            EnemyInfo enemyInfo = new EnemyInfo(tdData);
            enemyDic.Add(tdData.id, enemyInfo);
        }

        public static EnemyInfo GetEnemyInfo(int id)
        {
            if (enemyDic.ContainsKey(id))
            {
                return enemyDic[id];
            }

            return null;
        }
    }
}