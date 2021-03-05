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
            KongfuAnimConfig config = new KongfuAnimConfig(tdData)
            {
                id = tdData.id,
                animName = tdData.animationName
            };
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

        public static KongfuAnimConfig GetAnimConfig(int id)
        {
            var config = GetData(id);
            return GetAnimConfig(config.animationName);
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

        private KongfuAnimStrategy m_AnimStrategy;

        public KongfuAnimConfig(TDKongfuAnimationConfig tdConfig)
        {

        }

        public void ParpareEffectPool()
        {
            m_AnimStrategy.ParpareEffectPool();
        }

        public void PlayAttackEffect()
        {
            m_AnimStrategy.PlayAttackEffect();
        }

        public void PlayHurtEffect()
        {
            m_AnimStrategy.PlayHurtEffect();
        }
    }

    public abstract class KongfuAnimStrategy
    {
        public abstract void ParpareEffectPool();
        public abstract void PlayAttackEffect();
        public abstract void PlayHurtEffect();
    }

    public class KongfuAnimStrategy_Direction : KongfuAnimStrategy
    {
        public override void ParpareEffectPool() { }
        public override void PlayAttackEffect() { }
        public override void PlayHurtEffect() { }
    }

    public class KongfuAnimStrategy_NoDirection : KongfuAnimStrategy
    {
        public override void ParpareEffectPool() { }
        public override void PlayAttackEffect() { }
        public override void PlayHurtEffect() { }
    }
}