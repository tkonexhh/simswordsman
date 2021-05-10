using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDAvatarTable
    {
        public static Dictionary<int,AvatarConfig> AvatarConfigConfigDic = new Dictionary<int, AvatarConfig>();
        static void CompleteRowAdd(TDAvatar tdData)
        {
            AvatarConfigConfigDic.Add(tdData.id, new AvatarConfig(tdData) );
        }
    
    }

    public enum LevelType
    {
        MainChanllenge,
    }

    public class UnlockingCondition
    {
        public LevelType levelType;
        public int mainLevel;
        public int subLevel;

        public UnlockingCondition(string unlockingCondition)
        {
            string[] unlocking = unlockingCondition.Split(';');
            levelType = EnumUtil.ConvertStringToEnum<LevelType>(unlocking[0]);
            mainLevel = int.Parse(unlocking[1]);
            subLevel = int.Parse(unlocking[2]);
        }
    }
    
    public class AvatarConfig
    {
        public int id;
        public UnlockingCondition unlockingCondition;
        public string headIcon;

        public AvatarConfig(TDAvatar tdData)
        {
            id = tdData.id;
            unlockingCondition = new UnlockingCondition(tdData.unlockingCondition);
            headIcon = tdData.headIcon;
        }
    }
}