using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class EnemyInfo
	{
        public int id;
        public List<string> names = new List<string>();
        public ClanType clanType;
        public List<string> kongfuNameList = new List<string> ();
        public List<string> animNameList = new List<string> ();
        public string prefabName;

        public EnemyInfo(TDEnemyConfig tDEnemy)
        {
            this.id = tDEnemy.id;
            this.clanType = EnumUtil.ConvertStringToEnum<ClanType>(tDEnemy.clan);
            ParseKongfuName(tDEnemy.kongfuName);
            ParseAnimName(tDEnemy.animationName);

            this.prefabName = tDEnemy.prefabName;

            ParseName(tDEnemy.name);
        }

        public string GetNameForRandom()
        {
            int random = Random.Range(0, names.Count);
            return names[random];
        }

        private void ParseName(string nameStr)
        {
            string[] nameStrs = nameStr.Split('|');
            foreach (string name in nameStrs)
            {
                names.Add(name);
            }
        }

        private void ParseKongfuName(string kongfuNameStr)
        {
            if (string.IsNullOrEmpty(kongfuNameStr))
                return;

            string[] nameStrs = kongfuNameStr.Split(';');
            foreach (string name in nameStrs)
            {
                kongfuNameList.Add(name);
            }
        }

        private void ParseAnimName(string animNameStr)
        {
            if (string.IsNullOrEmpty(animNameStr))
                return;

            string[] nameStrs = animNameStr.Split(';');
            foreach (string name in nameStrs)
            {
                animNameList.Add(name);
            }
        }
    }
	
}