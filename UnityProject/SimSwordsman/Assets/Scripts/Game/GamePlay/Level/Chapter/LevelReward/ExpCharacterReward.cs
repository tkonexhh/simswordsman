using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class ExpCharacterReward : LevelReward
    {
        private int m_ExpValue;

        public ExpCharacterReward(LevelRewardType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ExpValue = int.Parse(paramStrs[1]);
        }

        public override void ApplyReward()
        {
            MainGameMgr.S.BattleFieldMgr.OurCharacterList.ForEach(i => 
            {
                i.AddExp(m_ExpValue);
            });
        }
    }


}