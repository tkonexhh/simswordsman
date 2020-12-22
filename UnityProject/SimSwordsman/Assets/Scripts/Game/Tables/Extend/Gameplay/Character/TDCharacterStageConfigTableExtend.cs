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
        public static Dictionary<int, CharacterStageInfo> stageInfoDic = new Dictionary<int, CharacterStageInfo>();

        static void CompleteRowAdd(TDCharacterStageConfig tdData)
        {
            CharacterStageInfo stageInfo = new CharacterStageInfo();
            stageInfo.stage = tdData.stage;
            stageInfo.fromLevel = tdData.fromLevel;
            stageInfo.toLevel = tdData.toLevel;
            stageInfo.baseAtk = float.Parse(tdData.baseAtk);
            stageInfo.growAtk = tdData.growAtk;
            stageInfo.startExp = tdData.startExp;
            stageInfo.growExp = tdData.growExp;
            //TODO:
            //CharacterStageReward reward = EnumUtil.ConvertStringToEnum<CharacterStageReward>(tdData.unlockContent);
            //stageInfo.stageRewards.Add(reward);

            //int stage = stageInfo.stage - 1;
            //if (stage >= 1 && stageInfoDic.ContainsKey(stage))
            //{
            //    stageInfo.stageRewards.AddRange(stageInfoDic[stage].stageRewards);
            //}

            if (!stageInfoDic.ContainsKey(stageInfo.stage))
            {
                stageInfoDic.Add(stageInfo.stage, stageInfo);
            }
        }

        public static CharacterStageInfo GetStageInfo(int stage)
        {
            if (stageInfoDic.ContainsKey(stage))
            {
                return stageInfoDic[stage];
            }

            return null;
        }

        public static int GetStage(int level)
        {
            foreach (var item in stageInfoDic.Values)
            {
                if(level >= item.fromLevel && level <= item.toLevel)
                {
                    return item.stage;
                }
            }

            return -1;
        }

        public static float GetAtk(int stage, int level)
        {
            if (stageInfoDic.ContainsKey(stage))
            {
                float atk = stageInfoDic[stage].baseAtk + (level - stageInfoDic[stage].fromLevel) * stageInfoDic[stage].growAtk;
                return atk;
            }
            else
            {
                return -1;
            }
        }

        public static int GetExpLevelUpNeed(int stage, int level)
        {
            if (stageInfoDic.ContainsKey(stage))
            {
                int exp = stageInfoDic[stage].startExp + (level - stageInfoDic[stage].fromLevel) * stageInfoDic[stage].growExp;
                return exp;
            }
            else
            {
                return -1;
            }
        }
    }

}