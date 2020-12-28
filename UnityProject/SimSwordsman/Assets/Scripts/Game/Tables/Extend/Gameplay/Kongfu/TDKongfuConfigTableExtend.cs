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
        public static Dictionary<KungfuType, KungfuConfigInfo> m_KungfuConfigDic = new Dictionary<KungfuType, KungfuConfigInfo>();
        static void CompleteRowAdd(TDKongfuConfig tdData)
        {
            if (!m_KungfuConfigDic.ContainsKey((KungfuType)tdData.id))
            {
                m_KungfuConfigDic.Add((KungfuType)tdData.id, new KungfuConfigInfo(tdData)); ;
            }
        }

        /// <summary>
        /// 获取功夫加成比例
        /// </summary>
        /// <param name="kungfuType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetAddition(KungfuType kungfuType,int level)
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
        public static KungfuConfigInfo GetKungfuConfigInfo(KungfuType kungfuType)
        {
            if (m_KungfuConfigDic.ContainsKey(kungfuType))
                return m_KungfuConfigDic[kungfuType];
            return null;
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
        public KungfuType KungfuType { set; get; }
        public string Name { set; get; }
        public KungfuQuality KungfuQuality { set; get; }
        public string Desc { set; get; }
        public Dictionary<int, float> AdditionRatioDic = new Dictionary<int, float>();
        public Dictionary<int, float> UpgradeExperienceDic = new Dictionary<int, float>();

        public KungfuConfigInfo(TDKongfuConfig tdData)
        {
            KungfuType = (KungfuType)tdData.id;
            Name = tdData.name;
            Desc = tdData.desc;
            KungfuQuality = EnumUtil.ConvertStringToEnum<KungfuQuality>(tdData.quality);

            string[] additionStr = tdData.addition.Split('|');
            for (int i = 0; i < additionStr.Length; i++)
            {
                AdditionRatioDic.Add(i+1,float.Parse(additionStr[i]));
            }

            string[] UpgradeStr = tdData.addition.Split('|');
            for (int i = 0; i < UpgradeStr.Length; i++)
            {
                UpgradeExperienceDic.Add(i+1, float.Parse(additionStr[i]));
            }
        }

        public float GetAddition(int levle)
        {
            if (levle<= AdditionRatioDic.Count)
                return AdditionRatioDic[levle];
            return 0;
        }
    }
}