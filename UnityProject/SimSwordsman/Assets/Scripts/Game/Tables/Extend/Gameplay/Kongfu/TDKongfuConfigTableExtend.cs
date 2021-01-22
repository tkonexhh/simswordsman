using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDKongfuConfigTable
    {
        public static Dictionary<KongfuType, KungfuConfigInfo> m_KungfuConfigDic = new Dictionary<KongfuType, KungfuConfigInfo>();
        static void CompleteRowAdd(TDKongfuConfig tdData)
        {
            if (!m_KungfuConfigDic.ContainsKey((KongfuType)tdData.id))
            {
                m_KungfuConfigDic.Add((KongfuType)tdData.id, new KungfuConfigInfo(tdData)); ;
            }
        }

        /// <summary>
        /// 获取功夫加成比例
        /// </summary>
        /// <param name="kungfuType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetAddition(KongfuType kungfuType,int level)
        {
            if (m_KungfuConfigDic.ContainsKey(kungfuType))
                return m_KungfuConfigDic[kungfuType].GetAddition(level);
            return 0;
        }
        /// <summary>
        /// 获取功夫的信息
        /// </summary>
        /// <param name="kungfuType"></param>
        /// <returns></returns>
        public static KungfuConfigInfo GetKungfuConfigInfo(KongfuType kungfuType)
        {
            if (m_KungfuConfigDic.ContainsKey(kungfuType))
                return m_KungfuConfigDic[kungfuType];

            Qarth.Log.e("Kongfu config info not found: " + kungfuType);
            return null;
        }
        /// <summary>
        /// 获取功夫升级经验
        /// </summary>
        /// <param name="characterKongfu"></param>
        /// <returns></returns>
        public static int GetKungfuUpgradeInfo(CharacterKongfuDBData characterKongfu)
        {

            if (m_KungfuConfigDic.ContainsKey(characterKongfu.kongfuType))
                return m_KungfuConfigDic[characterKongfu.kongfuType].GetKungfuUpgradeInfo(characterKongfu);
            return 0;
        }
    }

    public enum KungfuQuality
    {
        Normal,
        Super,
        Master,
    }

    public class KungfuConfigInfo
    {
        /// <summary>
        /// 功夫id KungfuType和表中ID对应
        /// </summary>
        public KongfuType KungfuType { set; get; }
        public string Name { set; get; }
        public int Price { set; get; }
        public KungfuQuality KungfuQuality { set; get; }
        public string Desc { set; get; }
        public Dictionary<int, float> AdditionRatioDic = new Dictionary<int, float>();
        public Dictionary<int, int> UpgradeExperienceDic = new Dictionary<int, int>();
        public string AnimName;

        public KungfuConfigInfo(TDKongfuConfig tdData)
        {
            KungfuType = (KongfuType)tdData.id;
            Name = tdData.kongfuName;
            Desc = tdData.desc;
            KungfuQuality = EnumUtil.ConvertStringToEnum<KungfuQuality>(tdData.quality);
            AnimName = tdData.animationName;

            string[] additionStr = tdData.atkRate.Split(';');
            for (int i = 0; i < additionStr.Length; i++)
            {
                AdditionRatioDic.Add(i+1,float.Parse(additionStr[i]));
            }

            string[] UpgradeStr = tdData.upgradeExp.Split(';');
            for (int i = 0; i < UpgradeStr.Length; i++)
            {
                UpgradeExperienceDic.Add(i+1, int.Parse(UpgradeStr[i]));
            }
            Price = tdData.price;
        }

        public  int GetKungfuUpgradeInfo(CharacterKongfuDBData characterKongfu)
        {
            if (UpgradeExperienceDic.ContainsKey(characterKongfu.level))
                return UpgradeExperienceDic[characterKongfu.level];
            return 0;
        }

        public float GetAddition(int levle)
        {
            if (levle<= AdditionRatioDic.Count)
                return AdditionRatioDic[levle];
            return 0;
        }
    }
}