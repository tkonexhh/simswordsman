using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ExpKongfuReward : LevelReward
    {
        private int m_ExpValue;

        public ExpKongfuReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ExpValue = int.Parse(paramStrs[1]);
        }

        public override void ApplyReward(int par)
        {
            MainGameMgr.S.BattleFieldMgr.OurCharacterList.ForEach(i => 
            {
                i.CharacterModel.DistributionKungfuExp(m_ExpValue/ par);
            });
        }
    }
}