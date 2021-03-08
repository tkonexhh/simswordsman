using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class MoneyReward : LevelReward
    {
        private int m_MoneyValue;

        public MoneyReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_MoneyValue = int.Parse(paramStrs[1]);
        }

        public override void ApplyReward(int par)
        {
            GameDataMgr.S.GetPlayerData().AddCoinNum(m_MoneyValue);
            EventSystem.S.Send(EventID.OnChallengeReward, rewardType, -1, m_MoneyValue);
        }

        public override int GetRewardValue()
        {
            return m_MoneyValue;
        }
    }

}