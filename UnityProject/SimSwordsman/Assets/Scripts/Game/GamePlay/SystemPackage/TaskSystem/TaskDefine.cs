using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class TaskDefine
	{
        // ע�⣺checker�����������������Ӧ��ConditionType�����Checker,
        // ���������Condition����ΪLevelReach,����Ӧ��checker����ΪLevelReachChecker

        // ע�⣺rewardHandler�����������������Ӧ��RewardType�����RewardHandler,
        // ���������Reward����ΪCoin,����Ӧ��RewardHandler����ΪCoinRewardHandler

        // ע�⣺��ҪTDMainTaskTable��������ҪGetConditionType(int taskId) GetConditionValue(int taskId) 
        //GetEvent(int taskId) GetRewardType(int taskId) ��GetRewardValue(int taskId)�ķ���

        public const string GET_CONDITION_TYPE = "GetConditionType";
        public const string GET_CONDITION_VALUE = "GetConditionValue";
        public const string GET_EVENT = "GetEvent";

        public const string GET_REWARD = "GetReward";
        //public const string GET_REWARD_VALUE = "GetRewardValue";
    }
	
}