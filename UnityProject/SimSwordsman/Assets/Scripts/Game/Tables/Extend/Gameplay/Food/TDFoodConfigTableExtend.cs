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
        public static Dictionary<int, List<FoodItemMakeNeedResInfo>> FoodItemMakeNeedResInfoDis = new Dictionary<int, List<FoodItemMakeNeedResInfo>>();
        static void CompleteRowAdd(TDFoodConfig tdData)
        {
            if (!FoodItemMakeNeedResInfoDis.ContainsKey(tdData.id))
            {
                string[] strs = tdData.makeRes.Split(';');
                List<FoodItemMakeNeedResInfo> infos = new List<FoodItemMakeNeedResInfo>();
                foreach (var item in strs)
                {
                    string[] str = item.Split('|');
                    infos.Add(new FoodItemMakeNeedResInfo() { ItemId = int.Parse(str[0]), Count = int.Parse(str[1]) });
                }
                FoodItemMakeNeedResInfoDis.Add(tdData.id, infos);
            }
        }
    }
    public struct FoodItemMakeNeedResInfo
    {
        public int ItemId;
        public int Count;
    }
}