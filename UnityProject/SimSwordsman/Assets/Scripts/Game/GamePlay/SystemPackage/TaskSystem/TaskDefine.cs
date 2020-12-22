using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameWish.Game
{
	public class TaskDefine
	{
        // 注意：checker类的名字命名规则：相应的ConditionType后面加Checker,
        // 比如表里面Condition配置为LevelReach,则相应的checker类名为LevelReachChecker

        // 注意：rewardHandler类的名字命名规则：相应的RewardType后面加RewardHandler,
        // 比如表里面Reward配置为Coin,则相应的RewardHandler类名为CoinRewardHandler

        // 注意：需要TDMainTaskTable类里面需要GetConditionType(int taskId) GetConditionValue(int taskId) 
        //GetEvent(int taskId) GetRewardType(int taskId) 和GetRewardValue(int taskId)的方法

        public const string GET_CONDITION_TYPE = "GetConditionType";
        public const string GET_CONDITION_VALUE = "GetConditionValue";
        public const string GET_EVENT = "GetEvent";

        public const string GET_REWARD = "GetReward";
        //public const string GET_REWARD_VALUE = "GetRewardValue";
    }
	
}