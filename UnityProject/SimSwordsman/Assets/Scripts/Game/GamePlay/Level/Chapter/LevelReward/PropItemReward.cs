using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class PropItemReward : LevelReward
    {
        private int m_ItemId;
        private int m_Number;

        public PropItemReward(RewardItemType rewardType, string[] paramStrs) : base(rewardType, paramStrs)
        {
            m_ItemId = int.Parse(paramStrs[1]);
            m_Number = int.Parse(paramStrs[2]);
        }

        public override void ApplyReward(int par)
        {
            //MainGameMgr.S.InventoryMgr.p
        }

        public override int GetRewardValue()
        {
            return m_Number;
        }
    }

}