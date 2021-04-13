using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDHeroTrialConfigTable
    {
        public static Dictionary<ClanType, HeroTrialConfig> heroTrialConfigDic = new Dictionary<ClanType, HeroTrialConfig>();

        static void CompleteRowAdd(TDHeroTrialConfig tdData)
        {
            ClanType clan = (ClanType)tdData.id;
            if (!heroTrialConfigDic.ContainsKey(clan))
            {
                HeroTrialConfig config = new HeroTrialConfig();
                config.clanType = clan;
                config.name = tdData.nameDes;
                config.icon = tdData.jobName;
                config.ordinaryEnemies = ParseEnemy(tdData.ordinaryEnemy);
                config.eliteEnemies = ParseEnemy(tdData.eliteEnemy);
            }
        }

        public static HeroTrialConfig GetConfig(ClanType clanType)
        {
            if (heroTrialConfigDic.ContainsKey(clanType))
            {
                return heroTrialConfigDic[clanType];
            }

            return null;
        }


        private static int[] ParseEnemy(string str)
        {
            string[] idStrArray = str.Split(',');

            int[] ids = new int[idStrArray.Length];

            for (int i = 0; i < idStrArray.Length; i++)
            {
                ids[i] = int.Parse(idStrArray[i]);
            }

            return ids;
        }

    }

    public class HeroTrialConfig
    {
        public ClanType clanType;
        public string name;
        public string icon;
        public int[] ordinaryEnemies;
        public int[] eliteEnemies;
    }
}