using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ExpKongfuReward : LevelReward
    {
        private int m_ExpValue;

        public ExpKongfuReward(LevelRewardType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ExpValue = int.Parse(paramStrs[1]);
        }

        public override void ApplyReward()
        {
            MainGameMgr.S.BattleFieldMgr.OurCharacterList.ForEach(i => 
            {
                int kongfuCount = Mathf.Max( i.GetKongfuCount(), 1);
                i.AddKongfuExp(m_ExpValue/kongfuCount);
            });
        }
    }
}