using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using Spine.Unity;

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
                animName = tdData.animationName,
                soundName = tdData.attackSound,
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
        public List<float> atkEffectDelayList = new List<float>();
        public List<string> castSEList = new List<string>();
        public List<string> hitSEList = new List<string>();
        public string footSE;
        public string soundName;

        private KongfuAnimStrategy m_AnimStrategy;

        public KongfuAnimConfig(TDKongfuAnimationConfig tdConfig)
        {
            castSEList = Helper.String2ListString(tdConfig.castSE, "|");
            hitSEList = Helper.String2ListString(tdConfig.hitSE, "|");
            atkEffectDelayList = Helper.String2ListFloat(tdConfig.atkDelay, ";");
            footSE = tdConfig.footSE;
            if (castSEList.Count == 2)
            {
                m_AnimStrategy = new KongfuAnimStrategy_Direction(this);
            }
            else
                m_AnimStrategy = new KongfuAnimStrategy_NoDirection(this);


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
            // Debug.LogError("PlayAttackEffect");
            m_AnimStrategy.PlayAttackEffect(transform);
        }

        public void PlayFootEffect(BoneFollower bone)
        {
            m_AnimStrategy.PlayFootEffect(bone);
        }

        public void PlayAttackSound()
        {

            List<string> soundNameList = Helper.String2ListString(soundName, "|");

            if (soundNameList != null && soundNameList.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, soundNameList.Count);

                string sound = soundNameList[index];

                AudioManager.S.PlayEnemyAttackSound(sound);
            }
        }

        public void PlayHurtEffect(Transform transform, Vector3 pos)
        {
            // Debug.LogError("PlayHurtEffect");
            m_AnimStrategy.PlayHurtEffect(transform, pos);
        }
    }

    public abstract class KongfuAnimStrategy
    {
        private ResLoader m_Loader;
        protected KongfuAnimConfig animConfig;

        public KongfuAnimStrategy(KongfuAnimConfig animConfig)
        {
            m_Loader = ResLoader.Allocate();
            this.animConfig = animConfig;
        }

        public void ParpareEffectPool()
        {
            foreach (var castSE in animConfig.castSEList)
            {
                if (!GameObjectPoolMgr.S.group.HasPool(castSE))
                {
                    var effectPrefab = m_Loader.LoadSync(castSE) as GameObject;
                    GameObjectPoolMgr.S.AddPool(castSE, effectPrefab, 10, 3);
                }
            }

            foreach (var hitSE in animConfig.hitSEList)
            {
                if (!GameObjectPoolMgr.S.group.HasPool(hitSE))
                {
                    var effectPrefab = m_Loader.LoadSync(hitSE) as GameObject;
                    GameObjectPoolMgr.S.AddPool(hitSE, effectPrefab, 15, 3);
                }
            }

            if (!string.IsNullOrEmpty(animConfig.footSE))
            {
                if (!GameObjectPoolMgr.S.group.HasPool(animConfig.footSE))
                {
                    var effectPrefab = m_Loader.LoadSync(animConfig.footSE) as GameObject;
                    GameObjectPoolMgr.S.AddPool(animConfig.footSE, effectPrefab, 5, 3);
                }
            }
        }

        public void ReleaseEffectPool()
        {
            foreach (var castSE in animConfig.castSEList)
            {
                GameObjectPoolMgr.S.RemovePool(castSE, true);
            }
            m_Loader.ReleaseAllRes();
            m_Loader = null;
        }

        public abstract void PlayAttackEffect(Transform transform);
        public void PlayHurtEffect(Transform transform, Vector3 offset)
        {
            for (int i = 0; i < animConfig.atkEffectDelayList.Count; i++)
            {
                AddHurtEffect(transform, offset, animConfig.atkEffectDelayList[i]);
            }
        }

        private void AddHurtEffect(Transform transform, Vector3 offset, float delay)
        {
            Timer.S.Post2Scale(i =>
            {
                if (animConfig.hitSEList.Count > 1)
                {
                    float scaleX = transform.localScale.x;
                    int index = scaleX > 0 ? 1 : 0;
                    var effectGo = GameObjectPoolMgr.S.Allocate(animConfig.hitSEList[index]);
                    // Debug.LogError("Hurt:" + animConfig.hitSEList[index]);
                    effectGo.transform.SetParent(transform);
                    effectGo.transform.ResetTrans();
                    effectGo.transform.localPosition = new Vector3(offset.x, offset.y, 10);
                    var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                    com.StartCD();
                    // effectGo.AddMissingComponent<SortingGroup>();

                    effectGo.AddMissingComponent<CustomShaderFinder>();
                }
            }, delay);
        }

        public void PlayFootEffect(BoneFollower bone)
        {
            if (bone == null)
                return;

            if (string.IsNullOrEmpty(animConfig.footSE))
                return;

            var effectGo = GameObjectPoolMgr.S.Allocate(animConfig.footSE);
            effectGo.transform.SetParent(bone.transform);
            effectGo.transform.ResetTrans();
            var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
            com.StartCD();

            effectGo.AddMissingComponent<CustomShaderFinder>();
        }
    }

    public class KongfuAnimStrategy_Direction : KongfuAnimStrategy
    {
        public KongfuAnimStrategy_Direction(KongfuAnimConfig animConfig) : base(animConfig)
        {
        }

        public override void PlayAttackEffect(Transform transform)
        {
            if (animConfig.castSEList.Count > 0)
            {
                float scaleX = transform.localScale.x;
                int index = scaleX < 0 ? 1 : 0;
                var effectGo = GameObjectPoolMgr.S.Allocate(animConfig.castSEList[index]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();

                effectGo.AddMissingComponent<CustomShaderFinder>();
            }
        }
    }

    public class KongfuAnimStrategy_NoDirection : KongfuAnimStrategy
    {
        public KongfuAnimStrategy_NoDirection(KongfuAnimConfig animConfig) : base(animConfig)
        {
        }

        public override void PlayAttackEffect(Transform transform)
        {
            if (animConfig.castSEList.Count > 0)
            {
                var effectGo = GameObjectPoolMgr.S.Allocate(animConfig.castSEList[0]);
                effectGo.transform.SetParent(transform);
                effectGo.transform.ResetTrans();
                var com = effectGo.AddMissingComponent<ParticleAutoRecycle>();
                com.StartCD();

                effectGo.AddMissingComponent<CustomShaderFinder>();
                {
                }
            }
        }
    }
}