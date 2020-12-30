using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskReward
	{
        public TaskRewardType rewardType;
        public int id;
        public int count1 = -1;
        public int count2 = -1;

        public TaskReward(string reward)
        {
            Parse(reward);
        }

        private void Parse(string reward)
        {
            string[] strs = reward.Split('|');

            rewardType = EnumUtil.ConvertStringToEnum<TaskRewardType>(strs[0]);

            if (strs.Length == 2) // Coin
            {
                //id = int.Parse(strs[0]);
                count1 = int.Parse(strs[1]);
                return;
            }
            if (strs.Length == 3) // Item
            {
                id = int.Parse(strs[1]);
                string[] countStrs = strs[2].Split('_');
                if (countStrs.Length == 1)
                {
                    count1 = int.Parse(countStrs[0]);
                }
                else if (countStrs.Length == 2)
                {
                    count1 = int.Parse(countStrs[0]);
                    count2 = int.Parse(countStrs[0]);
                }
                return;
            }

        }
	}
	
}