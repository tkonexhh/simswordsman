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
        /// ��ȡ����ӳɱ���
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
        /// ��ȡ�������Ϣ
        /// </summary>
        /// <param name="kungfuType"></param>
        /// <returns></returns>
        public static KungfuConfigInfo GetKungfuConfigInfo(KungfuType kungfuType)
        {
            if (m_KungfuConfigDic.ContainsKey(kungfuType))
                return m_KungfuConfigDic[kungfuType];

            Qarth.Log.e("Kongfu config info not found: " + kungfuType);
            return null;
        }
        /// <summary>
        /// ��ȡ����ͼ�������
        /// </summary>
        /// <param name="kungfuType"></param>
        /// <returns></returns>
        public static string GetIconName(KungfuType kungfuType)
        {
            if (m_KungfuConfigDic.ContainsKey(kungfuType))
                return m_KungfuConfigDic[kungfuType].IconName;
            Qarth.Log.e("Kongfu config info not found: " + kungfuType);
            return CharacterKongfuData.DefaultKungfu;
        }
        /// <summary>
        /// ��ȡ������������
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
        /// ����id KungfuType�ͱ���ID��Ӧ
        /// </summary>
        public KungfuType KungfuType { set; get; }
        public string Name { set; get; }
        public string IconName { set; get; }
        public int Price { set; get; }
        public KungfuQuality KungfuQuality { set; get; }
        public string Desc { set; get; }
        public Dictionary<int, float> AdditionRatioDic = new Dictionary<int, float>();
        public Dictionary<int, int> UpgradeExperienceDic = new Dictionary<int, int>();
        public string AnimName;

        public KungfuConfigInfo(TDKongfuConfig tdData)
        {
            KungfuType = (KungfuType)tdData.id;
            Name = tdData.kongfuName;
            IconName = tdData.iconName;
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