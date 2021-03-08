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
            animName = animName.ToLower();
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
        public List<string> castSEList = new List<string>();
        public List<string> hitSEList = new List<string>();

        private KongfuAnimStrategy m_AnimStrategy;

        public KongfuAnimConfig(TDKongfuAnimationConfig tdConfig)
        {
            castSEList = Helper.String2ListString(tdConfig.castSE, "|");
            hitSEList = Helper.String2ListString(tdConfig.hitSE, "|");
            if (castSEList.Count == 2)
            {
                m_AnimStrategy = new KongfuAnimStrategy_Direction(castSEList, hitSEList);
            }
            else
                m_AnimStrategy = new KongfuAnimStrategy_NoDirection(castSEList, hitSEList);


        }

        public void ParpareEffectPool()
        {
            m_AnimStrategy.ParpareEffectPool();
        }

        public void ReleaseEffectPool()
        {
            m_AnimStrategy.ReleaseEffectPool();
        }

        public void PlayAttackEffect(Transform transform)
        {
            m_AnimStrategy.PlayAttackEffect(transform);
        }

        public void PlayHurtEffect(Transform transform, float delay)
        {

            m_AnimStrategy.PlayHurtEffect(transform, delay);
        }
    }

    public abstract class KongfuAnimStrategy
    {
        private ResLoader m_Loader;
        protected List<string> castSEList;
        protected List<string> hitSEList;

        public KongfuAnimStrategy(List<string> castSEList, List<string> hitSEList)
        {
            m_Loader = ResLoader.Allocate();
            this.castSEList = castSEList;
            this.hitSEList = hitSEList;
        }

        public void ParpareEffectPool()
        {
            foreach (var castSE in castSEList)
            {
                if (!GameObjectPoolMgr.S.group.HasPool(castSE))
                {
                    var effectPrefab = m_Loader.LoadSync(castSE) as GameObject;
                    GameObjectPoolMgr.S.AddPool(castSE, effectPrefab, 3, 3);
                }
            }

            foreach (var hitSE in hitSEList)
            {
                if (!GameObjectPoolMgr.S.group.HasPool(hitSE))
                {
                    var effectPrefab = m_Loader.LoadSync(hitSE) as GameObject;
                    GameObjectPoolMgr.S.AddPool(hitSE, effectPrefab, 3, 3);
                }
            }
        }

        public void ReleaseEffectPool()
        {
            foreach (var castSE in castSEList)
            {
                GameObjectPoolMgr.S.RemovePool(castSE, true);
            }
            m_Loader.ReleaseAllRes();
            m_Loader = null;
        }

        public abstract void PlayAttackEffect(Transform transform);
        public abstract void PlayHurtEffect(Transform transform, float delay);
    }

    public class KongfuAnimStrategy_Direction : KongfuAnimStrategy
    {
        public KongfuAnimStrategy_Direction(List<string> castSEList, List<string> hitSEList) : base(castSEList, hitSEList)
        {
        }

        public override void PlayAttackEffect(Transform transform)
        {
            if (castSEList.Count > 0)
            {
                float scaleX = transform.localScale.x;
                int index = scaleX < 0 ? 1 : 0;
                var effectGo = GameObjectPoolMgr.S.Allocate(castSEList[index]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();
            }
        }
        public override void PlayHurtEffect(Transform transform, float delay)
        {
            if (hitSEList.Count > 1)
            {
                float scaleX = transform.localScale.x;
                int index = scaleX > 0 ? 1 : 0;
                var effectGo = GameObjectPoolMgr.S.Allocate(hitSEList[index]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();
            }
        }
    }

    public class KongfuAnimStrategy_NoDirection : KongfuAnimStrategy
    {
        public KongfuAnimStrategy_NoDirection(List<string> castSEList, List<string> hitSEList) : base(castSEList, hitSEList)
        {
        }

        public override void PlayAttackEffect(Transform transform)
        {
            if (castSEList.Count > 0)
            {
                var effectGo = GameObjectPoolMgr.S.Allocate(castSEList[0]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();
            }
        }
        public override void PlayHurtEffect(Transform transform, float delay)
        {
            if (hitSEList.Count > 1)
            {
                float scaleX = transform.localScale.x;
                int index = scaleX < 0 ? 1 : 0;
                var effectGo = GameObjectPoolMgr.S.Allocate(hitSEList[index]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();
            }
        }
    }
}