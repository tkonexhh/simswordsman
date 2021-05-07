using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDArenaEnemyNameTable
    {
        static void CompleteRowAdd(TDArenaEnemyName tdData)
        {

        }

        public static List<TDArenaEnemyName> GetRandomEnemy()
        {
            int randomCount = ArenaDefine.EnemyCount;
            List<TDArenaEnemyName> names = ReservoirSampling(dataList, randomCount);
            return names;
        }

        private static List<T> ReservoirSampling<T>(List<T> list, int m)
        {
            List<T> cache = new List<T>(m);
            for (int i = 0; i < m; i++)
            {
                cache.Add(list[i]);
            }
            int currentIndex;
            for (int i = m; i < list.Count; i++)
            {
                currentIndex = RandomHelper.Range(0, i + 1);
                if (currentIndex < m)
                {
                    cache[currentIndex] = list[i];
                }
            }
            return cache;
        }
    }
}