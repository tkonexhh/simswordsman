using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDKongfuStageConfigTable
    {
        private static Dictionary<int, KungfuWeightConfig> KungfuWeight = new Dictionary<int, KungfuWeightConfig>();
        static void CompleteRowAdd(TDKongfuStageConfig tdData)
        {
            if (!KungfuWeight.ContainsKey(tdData.level))
                KungfuWeight.Add(tdData.level, new KungfuWeightConfig(tdData.level, tdData.weight));
        }

        /// <summary>
        /// 获取权重信息
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static KungfuWeightConfig GetKungfuweight(int level)
        {
            if (KungfuWeight.ContainsKey(level))
                return KungfuWeight[level];
            return null;
        }

    }


    public class KungfuWeightConfig
    {
        public int Level { set; get; }
        public int Weight { set; get; }

        public KungfuWeightConfig(int level, int weight)
        {
            Level = level;
            Weight = weight;
        }
    }
}