using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class TaskReward
	{
        public TaskRewardType rewardType;
        public int id;
        public int count;

        public TaskReward(string reward)
        {
            Parse(reward);
        }

        private void Parse(string reward)
        {
            string[] strs = reward.Split('|');
            if (strs.Length != 2)
            {
                Debug.LogError("Task reward pattern wrong");
                return;
            }
            rewardType = EnumUtil.ConvertStringToEnum<TaskRewardType>(strs[0]);
            string[] str2 = strs[1].Split('_');
            if (str2.Length == 2)
            {
                id = int.Parse(str2[0]);
                count = int.Parse(str2[1]);
            }
            else
            {
                count = int.Parse(strs[1]);
            }
        }
	}
	
}