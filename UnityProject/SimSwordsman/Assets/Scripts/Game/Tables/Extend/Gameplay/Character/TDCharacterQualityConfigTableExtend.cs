using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterQualityConfigTable
    {
        public static Dictionary<CharacterQuality, CharacterQualityConfigInfo> characterQualityConfigDic = new Dictionary<CharacterQuality, CharacterQualityConfigInfo>();

        static void CompleteRowAdd(TDCharacterQualityConfig tdData)
        {
            CharacterQualityConfigInfo info = new CharacterQualityConfigInfo();
            info.quality = EnumUtil.ConvertStringToEnum<CharacterQuality>(tdData.quality);
            info.maxLevel = tdData.maxLevel;

            string[] levelStrs = tdData.kongfuNeedLevel.Split('|');
            for (int i = 0; i < levelStrs.Length; i++)
            {
                info.learnKonfuNeedLevelList.Add(int.Parse(levelStrs[i]));
            }

            if (!characterQualityConfigDic.ContainsKey(info.quality))
            {
                characterQualityConfigDic.Add(info.quality, info);
            }
        }

        public static CharacterQualityConfigInfo GetQualityConfigInfo(CharacterQuality quality)
        {
            if (characterQualityConfigDic.ContainsKey(quality))
            {
                return characterQualityConfigDic[quality];
            }

            return null;
        }
    }

    public class CharacterQualityConfigInfo
    {
        public CharacterQuality quality;
        public int maxLevel;
        public int kongfuSlot;
        public List<int> learnKonfuNeedLevelList = new List<int>();
    }
}