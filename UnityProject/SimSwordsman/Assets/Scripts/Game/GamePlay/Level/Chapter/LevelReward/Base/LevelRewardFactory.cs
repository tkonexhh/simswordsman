using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class LevelRewardFactory
    {
        public static LevelReward SpawnLevelReward(RewardItemType rewardType, string[] paramStrs)
        {
            LevelReward levelReward = null;

            switch (rewardType)
            {
                case RewardItemType.Exp_Role:
                    levelReward = new ExpCharacterReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Exp_Kongfu:
                    levelReward = new ExpKongfuReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Coin:
                    levelReward = new MoneyReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Item:
                    levelReward = new PropItemReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Armor:
                case RewardItemType.Arms:
                    levelReward = new EquipReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Food:
                    levelReward = new FoodReward(rewardType, paramStrs);
                    break;
                case RewardItemType.Kongfu:
                    levelReward = new KongfuItemReward(rewardType, paramStrs);
                    break;
            }

            return levelReward;
        }
    }

}