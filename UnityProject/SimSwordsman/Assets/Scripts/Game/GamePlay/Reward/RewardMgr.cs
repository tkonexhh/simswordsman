using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum RewardItemType
    {
        Item,//物品
        Armor,//盔甲
        Arms,//武器
        Kongfu,//武功秘籍
        Medicine,//药
        Food,//食物
        Coin,//铜钱
        Exp_Role,//弟子经验
        Exp_Kongfu,//功夫经验
    }
    public class RewardMgr : TSingleton<RewardMgr>
    {
        RewardBase CreateReward(RewardItemType type, int id, int count)
        {
            switch (type)
            {
                case RewardItemType.Item:
                    return new ItemReward(id, count);
                case RewardItemType.Armor:
                    return new ArmorReward(id, count);
                case RewardItemType.Arms:
                    return new ArmsReward(id, count);
                case RewardItemType.Kongfu:
                    return new KongfuReward(id, count);
                case RewardItemType.Medicine:
                    return new MedicineReward(id, count);
                case RewardItemType.Food:
                    return new FoodsReward(count);
                case RewardItemType.Coin:
                    return new CoinReward(count);
                case RewardItemType.Exp_Role:
                    return new Exp_RoleReward(id, count);
                case RewardItemType.Exp_Kongfu:
                    return new Exp_KongfuRweard(id, count);
                default:
                    Log.e("无此奖励类型");
                    return null;
            }
        }
        /// <summary>
        /// 获取奖励
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public RewardBase GetRewardBase(RewardItemType type, int id, int count = 1)
        {
            return CreateReward(type, id, count);
        }

        /// <summary>
        /// Param格式为RewardItemType的string|id|数量
        /// </summary>
        /// <param name="param">RewardItemType,id,count</param>
        /// <returns></returns>
        public RewardBase GetRewardBase(string param)
        {
            RewardItemType rewardItemType;
            string[] sp = param.Split('|');
            if (!Enum.TryParse(sp[0], out rewardItemType))
                return null;
            switch (rewardItemType)
            {
                case RewardItemType.Item:
                case RewardItemType.Armor:
                case RewardItemType.Arms:
                case RewardItemType.Kongfu:
                case RewardItemType.Medicine:
                    return CreateReward(rewardItemType, int.Parse(sp[1]), GetRewardCount(sp[2]));
                case RewardItemType.Food:
                case RewardItemType.Coin:
                    return CreateReward(rewardItemType, 0, int.Parse(sp[1]));
                case RewardItemType.Exp_Role:
                case RewardItemType.Exp_Kongfu:
                    return CreateReward(rewardItemType, 0, GetRewardCount(sp[1]));
                default:
                    break;
            }
            return null;
        }
        string[] temp;
        int GetRewardCount(string param)
        {
            if (param.Contains("_"))
            {
                temp = param.Split('_');
                return RandomHelper.Range(int.Parse(temp[0]), int.Parse(temp[1]) + 1);
            }
            else
            {
                return int.Parse(param);
            }
        }

        // public RewardBase GetRewardBase(string type, int id, int count)
        // {
        //     RewardItemType rewardItemType;
        //     if (!Enum.TryParse(type, out rewardItemType))
        //         return null;
        //     return CreateReward(rewardItemType, id, count);
        // }


        //public RewardBase AcceptRewardByConfig(string type, int id, int count)
        //{
        //    RewardBase reward = GetRewardBase(type, id, count);
        //    reward.AcceptReward();
        //    return reward;
        //}

    }
}