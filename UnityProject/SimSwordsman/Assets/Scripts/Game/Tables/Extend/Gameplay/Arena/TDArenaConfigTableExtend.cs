using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDArenaConfigTable
    {
        private static List<ArenaConfigInfo> s_ArenaConfigInfos;
        static void CompleteRowAdd(TDArenaConfig tdData)
        {

        }

        private static void Init()
        {
            s_ArenaConfigInfos = new List<ArenaConfigInfo>();
            for (int i = 0; i < m_DataList.Count; i++)
            {
                var config = m_DataList[i];
                var ranges = Helper.String2ListInt(config.range);
                ArenaConfigInfo info = new ArenaConfigInfo();
                info.minLvl = ranges[0];
                info.maxLvl = ranges[1];
                info.reward = config.reward;
                s_ArenaConfigInfos.Add(info);
            }
        }


        public static int GetRewardByRank(int rank)
        {
            if (s_ArenaConfigInfos == null)
            {
                Init();
            }

            for (int i = 0; i < s_ArenaConfigInfos.Count; i++)
            {
                var info = s_ArenaConfigInfos[i];
                if (info.IsWithin(rank))
                {
                    return info.reward;
                }
            }


            return 0;
        }
    }

    public class ArenaConfigInfo
    {
        public int minLvl;
        public int maxLvl;
        public int reward;

        public bool IsWithin(int rank)
        {
            return rank >= minLvl && rank <= maxLvl;
        }
    }
}