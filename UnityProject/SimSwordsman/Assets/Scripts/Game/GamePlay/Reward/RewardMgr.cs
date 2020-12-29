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
                    return new ItemReward(type, id, count);
                case RewardItemType.Armor:
                    return new ArmorReward(type, id, count);
                case RewardItemType.Arms:
                    return new ArmsReward(type, id, count);
                case RewardItemType.Kongfu:
                    return new KongfuReward(type, id, count);
                case RewardItemType.Medicine:
                    return new MedicineReward(type, id, count);
                case RewardItemType.Food:
                    return new FoodsReward(type, id, count);
                case RewardItemType.Coin:
                    return new CoinReward(type, id, count);
                case RewardItemType.Exp_Role:
                    return new Exp_RoleReward(type, id, count);
                case RewardItemType.Exp_Kongfu:
                    return new Exp_KongfuRweard(type, id, count);
                default:
                    Log.e("无此奖励类型");
                    return null;
            }
        }

        RewardBase CreateReward(string param)
        {
            RewardItemType rewardItemType;
            string[] sp = param.Split(',');
            if (!Enum.TryParse(sp[0], out rewardItemType)) return null;
            return CreateReward(rewardItemType, int.Parse(sp[1]), int.Parse(sp[2]));
        }


        public RewardBase GetRewardBase(string type, int id, int count)
        {
            RewardItemType rewardItemType;
            if (!Enum.TryParse(type, out rewardItemType))
                return null;
            return CreateReward(rewardItemType, id, count);
        }

        //public RewardBase AcceptRewardByConfig(string type, int id, int count)
        //{
        //    RewardBase reward = GetRewardBase(type, id, count);
        //    reward.AcceptReward();
        //    return reward;
        //}

    }
}