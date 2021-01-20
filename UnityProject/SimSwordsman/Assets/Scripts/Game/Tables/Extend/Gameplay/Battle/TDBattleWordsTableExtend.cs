using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDBattleWordsTable
    {
        public static Dictionary<int,BattleTextConfig> BattleTextConfigDic = new Dictionary<int,BattleTextConfig>();
        static void CompleteRowAdd(TDBattleWords tdData)
        {
            if (!BattleTextConfigDic.ContainsKey(tdData.id))
                BattleTextConfigDic.Add(tdData.id,new BattleTextConfig (tdData));
        }

        /// <summary>
        /// 根据类型获取相应的文本
        /// </summary>
        /// <param name="battleText"></param>
        /// <returns></returns>
        public static List<BattleTextConfig> GetBattleTextForType(BattleText battleText)
        {
            List<BattleTextConfig> battleTextConfigs = new List<BattleTextConfig>();
            foreach (var item in BattleTextConfigDic.Values)
            {
                if (item.BattleTextType == battleText)
                    battleTextConfigs.Add(item);
            }
            return battleTextConfigs;
        }
    }

    public class BattleTextConfig
    {
        public int ID { set; get; }
        public BattleText BattleTextType { set; get; }
        public string BattleWorlds { set; get; }

        public BattleTextConfig(TDBattleWords tdData)
        {
            ID = tdData.id;
            BattleTextType = EnumUtil.ConvertStringToEnum<BattleText>(tdData.type);
            BattleWorlds = tdData.battleWords;
        }
    }

    public enum BattleText
    {
        Battle,
        Talk,
        End,
    }
}