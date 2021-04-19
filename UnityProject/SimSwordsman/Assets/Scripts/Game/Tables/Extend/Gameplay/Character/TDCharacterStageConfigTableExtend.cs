using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterStageConfigTable
    {
        public static Dictionary<CharacterQuality, CharacterStageInfo> stageInfoDic = new Dictionary<CharacterQuality, CharacterStageInfo>();

        static void CompleteRowAdd(TDCharacterStageConfig tdData)
        {
            CharacterQuality quality = EnumUtil.ConvertStringToEnum<CharacterQuality>(tdData.quality);
            if (!stageInfoDic.ContainsKey(quality))
                stageInfoDic.Add(quality, new CharacterStageInfo(tdData));
            else
                stageInfoDic[quality].AddCharacterStageInfo(tdData);
        }

        //public static CharacterStageInfoItem GetStageInfo(CharacterQuality quality, int stage)
        //{
        //    if (stageInfoDic.ContainsKey(quality))
        //        return stageInfoDic[quality].GetCharacterStageInfoItem(stage);
        //    return null;
        //}

        public static int GetStage(CharacterQuality quality, int level)
        {
            if (quality == CharacterQuality.Hero)
                quality = CharacterQuality.Perfect;

            if (stageInfoDic.ContainsKey(quality))
            {
                foreach (var item in stageInfoDic[quality].GetCharacterStageInfoItems().Values)
                {
                    if (level >= item.FromLevel && level <= item.ToLevel)
                    {
                        return item.Stage;
                    }
                }
            }
            return -1;
        }

        public static float GetAtk(CharacterQuality quality, int stage, int level)
        {
            if (quality == CharacterQuality.Hero)
                quality = CharacterQuality.Perfect;

            if (stageInfoDic.ContainsKey(quality))
            {
                CharacterStageInfoItem characterStageInfoItem = stageInfoDic[quality].GetCharacterStageInfoItem(stage);
                if (characterStageInfoItem != null)
                {
                    float atk = characterStageInfoItem.BaseAtk + (level - characterStageInfoItem.FromLevel) * characterStageInfoItem.GrowAtk;
                    return atk;
                }
            }
            return -1;
        }

        public static int GetExpLevelUpNeed(CharacterItem character)
        {
            CharacterQuality characterQuality = character.quality;
            if (characterQuality == CharacterQuality.Hero)
                characterQuality = CharacterQuality.Perfect;

            if (stageInfoDic.ContainsKey(characterQuality))
            {
                CharacterStageInfoItem characterStageInfoItem = stageInfoDic[characterQuality].GetCharacterStageInfoItem(character.stage);
                if (characterStageInfoItem != null)
                {
                    int exp = characterStageInfoItem.StartExp + (character.level - characterStageInfoItem.FromLevel) * characterStageInfoItem.GrowExp;
                    return exp;
                }
            }
            else
            {
                return 1;
            }
            return 1;
        }

        public static UnlockContentConfigInfo GetUnlockForStage(CharacterQuality characterQuality, int stage)
        {
            if (characterQuality == CharacterQuality.Hero)
                characterQuality = CharacterQuality.Perfect;

            if (stageInfoDic.ContainsKey(characterQuality))
            {
                foreach (var item in stageInfoDic[characterQuality].GetCharacterStageInfoItems().Values)
                    if (item.Stage == stage)
                        return item.UnlockContentInfo;
            }
            return null;
        }

        //public static CharacterStageInfoItem GetUnlockContent(CharacterQuality characterQuality, int level)
        //{
        //    if (stageInfoDic.ContainsKey(characterQuality))
        //        return stageInfoDic[characterQuality].GetUnlockContent(level);
        //    return null;
        //}

        public static int GetUnlockConfigInfo(UnlockContent unlockContent, int index = 0)
        {
            foreach (var item in stageInfoDic[CharacterQuality.Good].GetCharacterStageInfoItems().Values)
                if (item.IsHaveUnlockContentInfo(unlockContent, index))
                    return item.FromLevel;
            return -1;
        }
    }
}