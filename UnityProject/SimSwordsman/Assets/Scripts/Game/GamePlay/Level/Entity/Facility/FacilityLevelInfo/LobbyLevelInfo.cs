using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class LobbyLevelInfo : FacilityLevelInfo
	{
        public int commonTaskCount;
        public int workInterval;
        public int workMaxAmount;
        public int workTime;
        public int workPay;
        public int workExp;
        public int maxDailyTask;
        public int PracticeLevelMax;
        public UpgradePreconditions upgradePreconditions;

    }

    public class UpgradePreconditions
    {
        public int DiscipleNumber;
        public int DiscipleLevel;

        public UpgradePreconditions(string str)
        {
            string[] strs = str.Split('|');
            if (strs.Length>=2)
            {
                for (int i = 0; i < strs.Length; i++)
                {
                    DiscipleNumber = int.Parse(strs[1]);
                    DiscipleLevel = int.Parse(strs[0]);
                }
            }
        }
    }


}