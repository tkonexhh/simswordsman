using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDHeroTrialConfigTable
    {
        public static List<HeroTrialConfig> heroTrialConfigDic = new List<HeroTrialConfig>();

        static void CompleteRowAdd(TDHeroTrialConfig tdData)
        {
            int id = tdData.id;
            {
                HeroTrialConfig config = new HeroTrialConfig();
                config.id = id;
                config.clanType = EnumUtil.ConvertStringToEnum<ClanType>(tdData.clan);
                config.name = tdData.nameDes;
                config.icon = tdData.jobName;
                config.ordinaryEnemies = ParseEnemy(tdData.ordinaryEnemy);
                config.eliteEnemies = ParseEnemy(tdData.eliteEnemy);

                heroTrialConfigDic.Add(config);
            }
        }

        public static HeroTrialConfig GetConfig(ClanType clanType)
        {
            HeroTrialConfig config = heroTrialConfigDic.FirstOrDefault(i => i.clanType == clanType);

            return config;
        }

        public static HeroTrialConfig GetConfig(int id)
        {
            HeroTrialConfig config = heroTrialConfigDic.FirstOrDefault(i => i.id == id);

            return config;
        }


        private static int[] ParseEnemy(string str)
        {
            string[] idStrArray = str.Split(';');

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
        public int id;
        public ClanType clanType;
        public string name;
        public string icon;
        public int[] ordinaryEnemies;
        public int[] eliteEnemies;
    }
}