using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFoodConfigTable
    {
        public static Dictionary<int, List<CostItem>> MakeNeedItemIDsDic = new Dictionary<int, List<CostItem>>();
        static void CompleteRowAdd(TDFoodConfig tdData)
        {
            if (!MakeNeedItemIDsDic.ContainsKey(tdData.id))
            {
                string[] strs = tdData.makeRes.Split(';');
                List<CostItem> infos = new List<CostItem>();
                foreach (var item in strs)
                {
                    string[] str = item.Split('|');
                    infos.Add(new CostItem(int.Parse(str[0]), int.Parse(str[1])));
                }
                MakeNeedItemIDsDic.Add(tdData.id, infos);
            }
        }
    }
}