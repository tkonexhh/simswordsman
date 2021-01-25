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
        public static Dictionary<int, HerbConfig> HerbDic = new Dictionary<int, HerbConfig>();
        public static Dictionary<int, List<CostItem>> MakeNeedItemIDsDic = new Dictionary<int, List<CostItem>>();

        static void CompleteRowAdd(TDHerbConfig tdData)
        {
            if (!HerbDic.ContainsKey(tdData.id))
                HerbDic.Add(tdData.id, new HerbConfig(tdData));

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

        /// <summary>
        /// 根据id获取对应的草药
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static HerbConfig GetHerbForId(int id)
        {
            if (HerbDic.ContainsKey(id))
                return HerbDic[id];
            return null;
        }
    }



    public class HerbConfig
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public int Price { set; get; }
        public string Icon { set; get; }

        public HerbConfig(TDHerbConfig tDHerb)
        {
            ID = tDHerb.id;
            Name = tDHerb.name;
            Desc = tDHerb.desc;
            Price = tDHerb.price;
            Icon = tDHerb.icon;
        }

        //public HerbConfig(HerbConfig herb)
        //{
        //    ID = herb.ID;
        //    Name = herb.Name;
        //    Desc = herb.Desc;
        //    Price = herb.Price;
        //    Icon = herb.Icon;
        //}
    }
}