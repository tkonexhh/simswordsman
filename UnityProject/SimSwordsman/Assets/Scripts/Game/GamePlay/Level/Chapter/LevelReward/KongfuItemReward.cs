using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class KongfuItemReward : LevelReward
    {
        private int m_KongfuItemID;
        private int m_KongfuItemNumber;

        public KongfuItemReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_KongfuItemID = int.Parse(paramStrs[1]);
            m_KongfuItemID = int.Parse(paramStrs[2]);
        }

        public override void ApplyReward(int par)
        {
            MainGameMgr.S.InventoryMgr.AddItem(new KungfuItem((KungfuType)m_KongfuItemID), m_KongfuItemNumber);
            EventSystem.S.Send(EventID.OnChallengeReward, rewardType, m_KongfuItemID, m_KongfuItemNumber);

        }

        public override int GetRewardValue()
        {
            return m_KongfuItemNumber;
        }
    }
	
}