using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDItemConfigTable
    {
        public static Dictionary<int, PropConfigInfo> propConfigInfoDic = new Dictionary<int, PropConfigInfo>();

        static void CompleteRowAdd(TDItemConfig tdData)
        {
            if (!propConfigInfoDic.ContainsKey(tdData.id))
            {
                PropConfigInfo propConfigInfo = new PropConfigInfo();
                propConfigInfo.id = tdData.id;
                propConfigInfo.name = tdData.name;
                propConfigInfo.iconName = tdData.iconName;
                propConfigInfo.desc = tdData.desc;
                propConfigInfo.price = tdData.price;

                propConfigInfoDic.Add(tdData.id, propConfigInfo);
            }
        }

        public static PropConfigInfo GetPropConfigInfo(int id)
        {
            if (propConfigInfoDic.ContainsKey(id))
            {
                return propConfigInfoDic[id];
            }
            return null;
        }

        public static string GetIconName(int id)
        {
            if (propConfigInfoDic.ContainsKey(id))
                return propConfigInfoDic[id].iconName;
            return string.Empty;
        }
    }

    public class PropConfigInfo
    {
        public int id;
        public string name;
        public string desc;
        public int price;
        public string iconName;
    }
}