using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDHerbConfigTable
    {
        public static Dictionary<int, Herb> HerbDic = new Dictionary<int, Herb>();
        
        static void CompleteRowAdd(TDHerbConfig tdData)
        {
            if (!HerbDic.ContainsKey(tdData.id))
                HerbDic.Add(tdData.id, new Herb(tdData));
        }

        /// <summary>
        /// 根据id获取对应的草药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Herb GetHerbForId(int id)
        {
            if (HerbDic.ContainsKey(id))
                return HerbDic[id];
            return null;
        }
    }



    public class Herb
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }

        public Herb(TDHerbConfig tDHerb)
        {
            ID = tDHerb.id;
            Name = tDHerb.name;
            Desc = tDHerb.desc;
        }

        public Herb(Herb herb)
        {
            ID = herb.ID;
            Name = herb.Name;
            Desc = herb.Desc;
        }
    }
}