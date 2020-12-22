using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDCharacterConfigTable
    {
        public static Dictionary<CharacterQuality, CharacterConfigInfo> characterConfigInfoDic = new Dictionary<CharacterQuality, CharacterConfigInfo>();

        static void CompleteRowAdd(TDCharacterConfig tdData)
        {
            CharacterQuality quality = (CharacterQuality)Enum.Parse(typeof(CharacterQuality), tdData.quality, true);
            switch (quality)
            {
                case CharacterQuality.Normal:
                    if (characterConfigInfoDic.ContainsKey(quality))
                        characterConfigInfoDic[quality].AddCharacterConfig(tdData);
                    else
                        characterConfigInfoDic.Add(quality, new CharacterConfigInfo(tdData));
                    break;
                case CharacterQuality.Good:
                    if (characterConfigInfoDic.ContainsKey(quality))
                        characterConfigInfoDic[quality].AddCharacterConfig(tdData);
                    else
                        characterConfigInfoDic.Add(quality, new CharacterConfigInfo(tdData));
                    break;
                case CharacterQuality.Perfect:
                    if (characterConfigInfoDic.ContainsKey(quality))
                        characterConfigInfoDic[quality].AddCharacterConfig(tdData);
                    else
                        characterConfigInfoDic.Add(quality, new CharacterConfigInfo(tdData));
                    break;
                default:  
                    break;
            }
        }

        public static string GetRandomDesc(CharacterQuality quality)
        {
            if (characterConfigInfoDic.ContainsKey(quality))
                return characterConfigInfoDic[quality].GetRandomDesc();
            else
            {
                Log.w("Currently not included");
                return null;
            }  
        }
    }

    public class CharacterConfigInfo
    {
        private List<TDCharacterConfig> m_tDCharacterConfigs = new List<TDCharacterConfig>();

        public void AddCharacterConfig(TDCharacterConfig tDCharacterConfig)
        {
            if (!m_tDCharacterConfigs.Contains(tDCharacterConfig))
            {
                m_tDCharacterConfigs.Add(tDCharacterConfig);
            }
        }

        public CharacterConfigInfo(TDCharacterConfig tDCharacterConfig)
        {
            if (!m_tDCharacterConfigs.Contains(tDCharacterConfig))
            {
                m_tDCharacterConfigs.Add(tDCharacterConfig);
            }
        }

        public string GetRandomDesc()
        {
            int index = UnityEngine.Random.Range(0, m_tDCharacterConfigs.Count);
            return m_tDCharacterConfigs[index].desc;
        }
    }
}