using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDKongfuAnimationConfigTable
    {
        public static Dictionary<string, KongfuAnimConfig> kongfuAnimConfigDic = new Dictionary<string, KongfuAnimConfig>();

        static void CompleteRowAdd(TDKongfuAnimationConfig tdData)
        {
            KongfuAnimConfig config = new KongfuAnimConfig();
            config.id = tdData.id;
            config.animName = tdData.animationName;
            ParseAtkRange(ref config, tdData.atkRange);

            kongfuAnimConfigDic.Add(tdData.animationName, config);
        }

        public static KongfuAnimConfig GetAnimConfig(string animName)
        {
            if (kongfuAnimConfigDic.ContainsKey(animName))
            {
                return kongfuAnimConfigDic[animName];
            }

            //Log.e("KongfuAnimConfig not found: " + animName);

            return null;
        }

        private static void ParseAtkRange(ref KongfuAnimConfig config, string str)
        {
            string[] strs = str.Split(';');
            for (int i = 0; i < strs.Length; i++)
            {
                if (!string.IsNullOrEmpty(strs[i]))
                {
                    float value = float.Parse(strs[i]);
                    config.atkRangeList.Add(value);
                }
            }
        }
    }

    public class KongfuAnimConfig
    {
        public int id;
        public string animName;
        public List<float> atkRangeList = new List<float>();
    }
}