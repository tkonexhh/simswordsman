using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDVisitorRewardConfigTable
    {
        public static Dictionary<int, List<int>> rewardIDByMainLevelDic = new Dictionary<int, List<int>>();
        static void CompleteRowAdd(TDVisitorRewardConfig tdData)
        {
            List<int> list;
            if (rewardIDByMainLevelDic.TryGetValue(tdData.lobbyLevel, out list))
            {
                list.Add(tdData.id);
            }
            else
            {
                list = new List<int>();
                list.Add(tdData.id);
                rewardIDByMainLevelDic.Add(tdData.lobbyLevel, list);
            }
        }
    }
}