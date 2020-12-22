using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class LevelRewardFactory
	{
        public static LevelReward SpawnLevelReward(LevelRewardType rewardType, string[] paramStrs)
        {
            LevelReward levelReward = null;

            switch (rewardType)
            {
                case LevelRewardType.ExpCharacter:
                    levelReward = new ExpCharacterReward(rewardType, paramStrs);
                    break;
                case LevelRewardType.ExpKongfu:
                    levelReward = new ExpKongfuReward(rewardType, paramStrs);
                    break;
                case LevelRewardType.Money:
                    levelReward = new MoneyReward(rewardType, paramStrs);
                    break;
                case LevelRewardType.PropItem:
                    levelReward = new PropItemReward(rewardType, paramStrs);
                    break;
                case LevelRewardType.Equip:
                    levelReward = new EquipReward(rewardType, paramStrs);
                    break;
                case LevelRewardType.Food:
                    levelReward = new FoodReward(rewardType, paramStrs);
                    break;
            }

            return levelReward;
        }
	}
	
}