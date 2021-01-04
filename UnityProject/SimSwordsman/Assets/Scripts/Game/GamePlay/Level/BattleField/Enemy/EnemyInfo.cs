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
        public string kongfuName;
        public string animName;
        public string prefabName;

        public EnemyInfo(TDEnemyConfig tDEnemy)
        {
            this.id = tDEnemy.id;
            this.clanType = EnumUtil.ConvertStringToEnum<ClanType>(tDEnemy.clan);
            this.kongfuName = tDEnemy.kongfuName;
            this.animName = tDEnemy.animationName;
            this.prefabName = tDEnemy.prefabName;

            ParseName(tDEnemy.name);
        }

        private void ParseName(string nameStr)
        {
            string[] nameStrs = nameStr.Split('|');
            foreach (string name in nameStrs)
            {
                names.Add(name);
            }
        }
	}
	
}